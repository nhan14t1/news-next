using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NEWS.Entities.Constants;
using NEWS.Entities.Extensions;
using NEWS.Entities.Models.Dto;
using NEWS.Entities.Models.Responses;
using NEWS.Entities.Models.ViewModels;
using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Repositories;
using NEWS.Entities.Services;
using NEWS.Entities.UnitOfWorks;
using NEWS.Entities.Exceptions;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace NEWS.Services.Services
{
    public class PostService : BaseService<Post>, IPostService
    {
        private IUserRepository _userRepository;
        private IFileManagementRepository _fileManagementRepository;
        private IMapper _mapper;
        private ILogger<PostService> _logger;
        private ITagRepository _tagRepository;

        public PostService(IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IFileManagementRepository fileManagementRepository,
            IMapper mapper,
            ILogger<PostService> logger,
            ITagRepository tagRepository)
            : base(unitOfWork)
        {
            _userRepository = userRepository;
            _fileManagementRepository = fileManagementRepository;
            _mapper = mapper;
            _logger = logger;
            _tagRepository = tagRepository;
        }

        private async Task<bool> IsSlugExisted(string slug, int exceptPostId = 0)
        {
            var post = await _repository.GetAll(_ => _.Slug == slug && (exceptPostId == 0 || _.Id != exceptPostId))
                .FirstOrDefaultAsync();
            
            return post != null;
        }

        private void SaveNewTags(List<Tag> tags)
        {
            try
            {
                // Add new tags
                tags.Where(_ => _.Id == 0).ToList().ForEach(tag => {
                    tag.LowerText = tag.Text.ToLower();
                    _tagRepository.Add(tag);
                });
            }
            catch (Exception ex)
            {
                // Error maybe happens when user type tag fastly.
                _logger.LogError(ex, $"SaveNewTags error: {ex.Message} - {ex.StackTrace}");
                throw new BusinessException("Có lỗi xảy ra khi lưu Nhãn, hãy thử xóa và nhập lại nhãn");
            }
        }

        public async Task<PostDto> AddAsync(PostVM request, string email, FileManagement thumbnail)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            var newPost = new Post
            {
                Id = request.Id,
                Title = request.Title ?? "",
                IntroText = request.IntroText ?? "",
                Slug = !string.IsNullOrWhiteSpace(request.Slug) ? request.Slug.Trim().ToLower() : "",
                Content = request.Content ?? "",
                UserId = user.Id,
                Status = request.Status,
                CreatedDate = DateTime.Now.ToTimeStamp()
            };

            if (!string.IsNullOrWhiteSpace(newPost.Slug) && await IsSlugExisted(newPost.Slug)) {
                throw new BusinessException("Slug đã tồn tại, đổi một slug khác");
            }

            var postCategories = request.CategoryIds != null
                    ? request.CategoryIds.Select(categoryId => new PostCategory
                    {
                        CategoryId = categoryId
                    }).ToList() : new List<PostCategory>();

            // Flag image to use. The image isn't used will be removed by the job
            var images = request.ImageUrls != null
                ? await _fileManagementRepository.GetAll(_ => !_.IsUsed && request.ImageUrls.Contains(_.Name)).ToListAsync()
            : new List<FileManagement>();

            // Add new tags
            SaveNewTags(request.Tags);

            try
            {
                await _unitOfWork.BeginTransaction();

                if (thumbnail != null)
                {
                    newPost.ThumbnailId = thumbnail.Id;

                    thumbnail.IsUsed = true;
                    _unitOfWork.DbContext.Update(thumbnail);
                }

                _unitOfWork.DbContext.Add(newPost);
                await _unitOfWork.SaveChangesAsync();

                foreach (var tag in request.Tags)
                {
                    var postTag = new PostTag
                    {
                        TagId = tag.Id,
                        PostId = newPost.Id
                    };

                    _unitOfWork.DbContext.Add(postTag);
                }

                foreach (var postCategory in postCategories)
                {
                    postCategory.PostId = newPost.Id;
                    _unitOfWork.DbContext.Add(postCategory);
                }

                foreach (var image in images)
                {
                    image.IsUsed = true;
                    _unitOfWork.DbContext.Update(image);
                }

                await _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollBack();
                // Delete thumbnail on File explore
                throw;
            }

            return _mapper.Map<PostDto>(newPost); ;
        }

        public async Task<PostDto> UpdateAsync(PostVM request, FileManagement thumbnail)
        {
            var post = await _repository.GetAll(_ => _.Id == request.Id && !_.IsDeleted)
                .Include(_ => _.Thumbnail)
                .Include(_ => _.PostCategories)
                .Include(_ => _.PostTags)
                .FirstOrDefaultAsync();

            post.Title = request.Title;
            post.Slug = request.Slug;
            post.IntroText = request.IntroText;
            post.Content = request.Content;
            post.UpdatedDate = DateTime.Now.ToTimeStamp();
            post.Status = request.Status;

            if (!string.IsNullOrWhiteSpace(post.Slug) && await IsSlugExisted(post.Slug, post.Id))
            {
                throw new BusinessException("Slug đã tồn tại, đổi một slug khác");
            }

            var newCategories = request.CategoryIds != null
                    ? request.CategoryIds.Select(categoryId => new PostCategory
                    {
                        CategoryId = categoryId,
                        PostId = post.Id
                    }).ToList() : new List<PostCategory>();

            var oldCategories = post.PostCategories;

            var categoriesToAdd = newCategories.Where(_ => !oldCategories.Any(x => x.CategoryId == _.CategoryId)).ToList();
            var categoriesToDelete = oldCategories.Where(_ => !newCategories.Any(x => x.CategoryId == _.CategoryId)).ToList();

            // Flag image to use. The image isn't used will be removed by the job
            var images = request.ImageUrls != null
                ? await _fileManagementRepository.GetAll(_ => !_.IsUsed && request.ImageUrls.Contains(_.Name)).ToListAsync()
            : new List<FileManagement>();

            // Add new tags
            SaveNewTags(request.Tags);

            var postTagsToAdd = request.Tags.Where(tag => !post.PostTags.Any(postTag => tag.Id == postTag.TagId))
                .Select(tag => new PostTag {
                    TagId = tag.Id,
                    PostId = post.Id
                }).ToList();

            var postTagsToDelete = post.PostTags.Where(postTag => !request.Tags.Any(tag => postTag.TagId == tag.Id)).ToList();

            try
            {
                await _unitOfWork.BeginTransaction();

                if (thumbnail != null)
                {
                    var previousThumbnail = post.Thumbnail;

                    post.ThumbnailId = thumbnail.Id;
                    thumbnail.IsUsed = true;
                    _unitOfWork.DbContext.Update(thumbnail);

                    // Delete previous thumbnail
                    if (previousThumbnail != null)
                    {
                        previousThumbnail.IsUsed = false;
                        _unitOfWork.DbContext.Update(previousThumbnail);
                    }
                }

                _unitOfWork.DbContext.Update(post);

                if (categoriesToAdd.Any())
                {
                    _unitOfWork.DbContext.AddRange(categoriesToAdd);
                }

                if (categoriesToDelete.Any())
                {
                    _unitOfWork.DbContext.RemoveRange(categoriesToDelete);
                }

                if (postTagsToAdd.Any())
                {
                    _unitOfWork.DbContext.AddRange(postTagsToAdd);
                }

                if (postTagsToDelete.Any())
                {
                    _unitOfWork.DbContext.RemoveRange(postTagsToDelete);
                }

                await _unitOfWork.SaveChangesAsync();

                foreach (var image in images)
                {
                    image.IsUsed = true;
                    _unitOfWork.DbContext.Update(image);
                }

                await _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollBack();
                // Delete thumbnail on File explore
                throw;
            }

            return _mapper.Map<PostDto>(post);
        }

        public async Task<List<PostDto>> GetAllAsync()
        {
            var rs = await _repository.GetAll(_ => !_.IsDeleted)
                .Include(_ => _.User)
                .Include(_ => _.PostCategories)
                .ThenInclude(_ => _.Category)
                .Include(_ => _.PostTags)
                .ThenInclude(_ => _.Tag)
                .OrderByDescending(_ => _.Id)
                .ToListAsync();

            return _mapper.Map<List<PostDto>>(rs);
        }

        public async Task<HomePageResult> GetHomePageData()
        {
            var vietNamPosts = await _repository.GetAll(_ => !_.IsDeleted && _.Status == (int)PostStatus.Active
                && _.PostCategories.Any(x => x.CategoryId == (int)AppCategory.VietNam))
                .Include(_ => _.Thumbnail)
                .AsNoTracking()
                .OrderByDescending(_ => _.Id)
                .Take(6)
                .ToListAsync();
            var globalPosts = await _repository.GetAll(_ => !_.IsDeleted && _.Status == (int)PostStatus.Active
                && _.PostCategories.Any(x => x.CategoryId == (int)AppCategory.Global))
                .Include(_ => _.Thumbnail)
                .AsNoTracking()
                .OrderByDescending(_ => _.Id)
                .Take(6)
                .ToListAsync();

            var topPosts = await _repository.GetAll(_ => !_.IsDeleted && _.Status == (int)PostStatus.Active)
                .Include(_ => _.Thumbnail)
                .AsNoTracking()
                .OrderByDescending(_ => _.Views)
                .Take(8)
                .ToListAsync();

            // Save API size
            vietNamPosts.ForEach(_ => _.Content = "");
            globalPosts.ForEach(_ => _.Content = "");
            topPosts.ForEach(_ => _.Content = "");

            return new HomePageResult
            {
                VietNamPosts = _mapper.Map<List<PostDto>>(vietNamPosts),
                GlobalPosts = _mapper.Map<List<PostDto>>(globalPosts),
                TopPosts = _mapper.Map<List<PostDto>>(topPosts)
            };
        }

        public async Task<PostDto> GetBySlugAsync(string slug, bool isPreview = false)
        {
            slug = slug.ToLower();
            var post = await _repository.GetAll(_ => _.Slug == slug && (isPreview || _.Status == (int)PostStatus.Active) && !_.IsDeleted)
                .Include(_ => _.Thumbnail)
                .Include(_ => _.PostTags).ThenInclude(_ => _.Tag)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (post == null)
            {
                throw new BusinessException("Bài viết không tồn tại");
            }

            return _mapper.Map<PostDto>(post);
        }

        public async Task<PostDto> GetByIdAsync(int id)
        {
            var post = await _repository.GetAll(_ => _.Id == id && !_.IsDeleted)
                .Include(_ => _.Thumbnail)
                .Include(_ => _.PostCategories).ThenInclude(_ => _.Category)
                .Include(_ => _.PostTags).ThenInclude(_ => _.Tag)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (post == null)
            {
                throw new BusinessException("Bài viết không tồn tại");
            }

            return _mapper.Map<PostDto>(post);
        }

        public async Task DeleteAsync(int id)
        {
            var post = await _repository.GetAll(_ => _.Id == id && !_.IsDeleted).FirstOrDefaultAsync();

            if (post == null)
            {
                throw new BusinessException("Bài viết không tồn tại");
            }

            post.IsDeleted = true;
            post.Status = (int)PostStatus.Deleted;
            post.UpdatedDate = DateTime.Now.ToTimeStamp();
            await _repository.UpdateAsync(post);
        }

        public async Task<List<PostDto>> GetPostMap()
        {
            var posts = await _repository.GetAll(_ => !_.IsDeleted && _.Status == (int)PostStatus.Active)
                .Include(_ => _.Thumbnail)
                .AsNoTracking()
                .OrderByDescending(_ => _.UpdatedDate)
                .Take(200)
                .Select(_ => new PostDto
                {
                    Id = _.Id,
                    Slug = _.Slug,
                    UpdatedDate = _.UpdatedDate,
                    Title = _.Title,
                    IntroText = _.IntroText,
                    ThumbnailFileName = _.Thumbnail != null ? _.Thumbnail.Name : ""
                })
                .ToListAsync();

            return posts;
        }

        public async Task UpdateViews(int postId)
        {
            var post = await _repository.GetAll(_ => _.Id == postId && !_.IsDeleted)
                .FirstOrDefaultAsync();
            post.Views += 1;
            await _repository.UpdateAsync(post);
        }

        public async Task<List<PostDto>> GetRelatedByTagIdsAsync(List<int> tagIds, int excludePostId = 0)
        {
            if (!tagIds.Any())
            {
                return new List<PostDto>();
            }

            var posts = await _repository.GetAll(post => post.PostTags.Any(postTag => tagIds.Contains(postTag.TagId))
                && !post.IsDeleted && post.Status == (int)PostStatus.Active && post.Id != excludePostId)
                .Include(_ => _.Thumbnail)
                .AsNoTracking()
                .OrderByDescending(post => post.Id)
                .Take(8)
                .ToListAsync();

            // Reduce API size
            posts.ForEach(_ => _.Content = "");

            return _mapper.Map<List<PostDto>>(posts);
        }
    }
}
