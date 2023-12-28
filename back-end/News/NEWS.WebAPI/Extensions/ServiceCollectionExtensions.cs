using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NEWS.Entities.DataContext;
using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Repositories;
using NEWS.Entities.Services;
using NEWS.Entities.UnitOfWorks;
using NEWS.Repositories;
using NEWS.Repositories.Repositories;
using NEWS.Services.Services;

namespace NEWS.WebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure DbContext with Scoped lifetime  
            services.AddDbContext<NewsContext>(options =>
            {
                options.UseLazyLoadingProxies(false)
                    .UseMySql(configuration.GetConnectionString("Mysql"), ServerVersion.Parse("8.0.32-mysql"));
                //options.UseMySql(ServerVersion.AutoDetect(configuration.GetConnectionString("Mysql")));
            }
            );

            services.AddScoped<Func<NewsContext>>((provider) => () => provider.GetService<NewsContext>());
            services.AddScoped<DbFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped<DbContext, NewsContext>()
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IRoleRepository, RoleRepository>()
                .AddScoped<IUserRoleRepository, UserRoleRepository>()
                .AddScoped<IPostRepository, PostRepository>()
                .AddScoped<ICategoryRepository, CategoryRepository>()
                .AddScoped<IPostCategoryRepository, PostCategoryRepository>()
                .AddScoped<IFileManagementRepository, FileManagementRepository>()
                .AddScoped<IUserTokenRepository, UserTokenRepository>()
                ;
        }

        /// <summary>
        /// Add instances of in-use services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddScoped<IUserService, UserService>()
                .AddScoped<IRoleService, RoleService>()
                .AddScoped<IUserRoleService, UserRoleService>()
                .AddScoped<IPostService, PostService>()
                .AddScoped<ICategoryService, CategoryService>()
                .AddScoped<IPostCategoryService, PostCategoryService>()
                .AddScoped<IFileManagementService, FileManagementService>()
                .AddScoped<IUserTokenService, UserTokenService>()
                ;
        }
    }
}
