using AutoMapper;
using NEWS.Entities.Dto;
using NEWS.Entities.MySqlEntities;

namespace NEWS.Services.AutoMappings
{
    public class MappingConfigProfile : Profile
    {
        public MappingConfigProfile()
        {
            CreateMap<Post, PostDto>()
                //.ForMember(x => x.CategoryId, y => y.MapFrom(_ => _.PostCategories.FirstOrDefault().Id))
                //.ForMember(x => x.PostCategories, y => y.Ignore())
                .ForMember(x => x.UserEmail, y => y.MapFrom(_ => _.User.Email))
                .ForMember(x => x.UserFirstName, y => y.MapFrom(_ => _.User.FirstName))
                .ForMember(x => x.UserLastName, y => y.MapFrom(_ => _.User.LastName))
                .AfterMap((x, y) => {
                    y.Categories = x.PostCategories.Select(_ => new Category
                    {
                        Id = _.CategoryId,
                        Name = _.Category?.Name
                    }).ToList();
                }
                )
                .ReverseMap();
        }
    }
}
