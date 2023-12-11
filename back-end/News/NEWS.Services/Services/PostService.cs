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

namespace NEWS.Services.Services
{
    public class PostService : BaseService<Post>, IPostService
    {
        private IUserRepository _userRepository;
        private IFileManagementRepository _fileManagementRepository;
        private IMapper _mapper;
        private ILogger<PostService> _logger;

        public PostService(IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IFileManagementRepository fileManagementRepository,
            IMapper mapper,
            ILogger<PostService> logger)
            : base(unitOfWork)
        {
            _userRepository = userRepository;
            _fileManagementRepository = fileManagementRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Post> AddAsync(PostVM request, string email, FileManagement thumbnail)
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

            var postCategories = request.CategoryIds != null
                    ? request.CategoryIds.Select(categoryId => new PostCategory
                    {
                        CategoryId = categoryId
                    }).ToList() : new List<PostCategory>();

            // Flag image to use. The image isn't used will be removed by the job
            var images = request.ImageUrls != null
                ? await _fileManagementRepository.GetAll(_ => !_.IsUsed && request.ImageUrls.Contains(_.Name)).ToListAsync()
            : new List<FileManagement>();

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

            newPost.PostCategories = postCategories.Select(_ => new PostCategory {
                Id = _.Id,
                PostId = _.PostId,
                CategoryId= _.CategoryId
            }).ToList();
            return newPost;
        }

        public async Task<List<PostDto>> GetAllAsync()
        {
            var rs = await _repository.GetAll(_ => true)
                .Include(_ => _.User)
                .Include(_ => _.PostCategories)
                .ThenInclude(_ => _.Category)
                .OrderByDescending(_ => _.Id)
                .ToListAsync();

            return _mapper.Map<List<PostDto>>(rs);
        }

        public async Task<HomePageResult> GetHomePageData()
        {
            var vietNamPosts = await _repository.GetAll(_ => _.Status == (int)PostStatus.Active
                && _.PostCategories.Any(x => x.CategoryId == (int)AppCategory.VietNam))
                .Include(_ => _.Thumbnail)
                .AsNoTracking()
                .OrderByDescending(_ => _.Id)
                .Take(6)
                .ToListAsync();
            var globalPosts = await _repository.GetAll(_ => _.Status == (int)PostStatus.Active
                && _.PostCategories.Any(x => x.CategoryId == (int)AppCategory.Global))
                .Include(_ => _.Thumbnail)
                .AsNoTracking()
                .OrderByDescending(_ => _.Id)
                .Take(6)
                .ToListAsync();
            
            var lastMonth = DateTime.Now.ToTimeStamp() - AppConst.MILISECOND_OF_DATE * 60;
            var topPosts = await _repository.GetAll(_ => _.Status == (int)PostStatus.Active && _.CreatedDate >= lastMonth)
                .Include(_ => _.Thumbnail)
                .AsNoTracking()
                .OrderByDescending(_ => _.Id)
                .Take(8)
                .ToListAsync();

            throw new BusinessException("Test");

            return new HomePageResult
            {
                VietNamPosts = _mapper.Map<List<PostDto>>(vietNamPosts),
                GlobalPosts = _mapper.Map<List<PostDto>>(globalPosts),
                TopPosts = _mapper.Map<List<PostDto>>(topPosts)
            };
        }

        public async Task<PostDto> GetBySlug(string slug)
        {
            slug = slug.ToLower();
            var post = await _repository.GetAll(_ => _.Slug == slug && _.Status == (int)PostStatus.Active)
                .Include(_ => _.Thumbnail)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return _mapper.Map<PostDto>(post);
        }
    }
}
