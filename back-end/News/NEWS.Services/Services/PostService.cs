using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NEWS.Entities.Constants;
using NEWS.Entities.Dto;
using NEWS.Entities.Extensions;
using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Repositories;
using NEWS.Entities.Responses;
using NEWS.Entities.Services;
using NEWS.Entities.UnitOfWorks;
using NEWS.Entities.ViewModels;

namespace NEWS.Services.Services
{
    public class PostService : BaseService<Post>, IPostService
    {
        private IUserRepository _userRepository;
        private IMapper _mapper;

        public PostService(IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IMapper mapper)
            : base(unitOfWork)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Post> AddAsync(PostVM request, string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            var newPost = new Post
            {
                Id = request.Id,
                Title = request.Title ?? "",
                IntroText = request.IntroText ?? "",
                Slug = request.Slug ?? "",
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
            try
            {
                await _unitOfWork.BeginTransaction();
                _unitOfWork.DbContext.Add(newPost);
                await _unitOfWork.SaveChangesAsync();

                foreach (var postCategory in postCategories)
                {
                    postCategory.PostId = newPost.Id;
                    _unitOfWork.DbContext.Add(postCategory);
                }

                await _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollBack();
                throw;
            }

            var a = _userRepository.GetAll(_ => true).Include(_ => _.UserRoles).ThenInclude(x=> x.Role).ToList();
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
                .OrderByDescending(_ => _.Id)
                .Take(3)
                .ToListAsync();
            var globalPosts = await _repository.GetAll(_ => _.Status == (int)PostStatus.Active
                && _.PostCategories.Any(x => x.CategoryId == (int)AppCategory.Global))
                .OrderByDescending(_ => _.Id)
                .Take(3)
                .ToListAsync();
            
            var lastMonth = DateTime.Now.ToTimeStamp() - AppConst.MILISECOND_OF_DATE * 60;
            var topPosts = await _repository.GetAll(_ => _.Status == (int)PostStatus.Active && _.CreatedDate >= lastMonth)
                .OrderByDescending(_ => _.Id)
                .Take(8)
                .ToListAsync();

            return new HomePageResult
            {
                VietNamPosts = _mapper.Map<List<PostDto>>(vietNamPosts),
                GlobalPosts = _mapper.Map<List<PostDto>>(globalPosts),
                TopPosts = _mapper.Map<List<PostDto>>(topPosts)
            };
        }
    }
}
