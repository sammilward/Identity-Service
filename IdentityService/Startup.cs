using IdentityServer4.Services;
using IdentityService.Data;
using IdentityService.Interfaces;
using IdentityService.Wrappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;

namespace IdentityService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer(options => { options.IssuerUri = "http://identityservice"; }).AddDeveloperSigningCredential()
                .AddInMemoryClients(IdentityServerConfig.GetClients())
                .AddInMemoryApiResources(IdentityServerConfig.GetApis())
                .AddAspNetIdentity<ApplicationUser>();

            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            services.AddTransient<UserManager<ApplicationUser>>();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddSingleton<IConfigurationWrapper, ConfigurationWrapper>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var subDirectory = $"/{Configuration.GetValue<string>("SubDirectory")}";
            app.UseStaticFiles(subDirectory);
            app.UsePathBase(subDirectory);

            app.UseMetricServer("/metrics");

            app.UseRequestMiddleware();

            app.UseMvcWithDefaultRoute();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseIdentityServer();

            SeedDatabase.Initialize(app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);

            app.UseMvc();
        }
    }
}