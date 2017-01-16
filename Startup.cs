
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IdentityServer4.Validation;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity.MongoDB;
using Microsoft.AspNetCore.Identity;
using cinnamon.api.Models;

namespace cinnamon.api
{
    public partial class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddOptions();
            services.Configure<AppOptions>(Configuration);
            services.AddTransient<Models.Context>();

            var builder = services.AddIdentityServer();
            builder.AddTemporarySigningCredential();
            builder.AddInMemoryPersistedGrants();
            builder.AddInMemoryApiResources(Config.GetApiResources());
            builder.AddInMemoryClients(Config.GetClients());
            builder.AddAspNetIdentity<Models.User>();
            builder.Services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            builder.Services.AddTransient<IProfileService, ProfileService>();

            services.AddIdentityWithMongoStores(Configuration.GetConnectionString("DefaultConnection"))
                .AddDefaultTokenProviders();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseIdentity();
            app.UseIdentityServer();

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = "http://localhost:5000",
                RequireHttpsMetadata = false,

                ApiName = "api1"
            });

            app.UseMvc();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            Models.Context.Init();
        }
    }
}
