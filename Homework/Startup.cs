using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homework.Auth;
using Homework.Events;
using Homework.Friends;
using Homework.Persistence;
using Homework.Updates;
using Homework.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Homework
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/auth/login");
                });

            services.AddControllersWithViews();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddOptions<MySqlOptions>().Bind(Configuration.GetSection("MySql"));
            services.AddSingleton<MySqlDb>();

            services.AddScoped<CurrentUserManager>();
            services.AddSingleton<IPasswordManager, HashingMySqlPasswordManager>();

            services.AddSingleton<IUserRepository, MySqlUserRepository>();
            services.AddSingleton<UserCreator>();

            services.AddSingleton<IFriendOfferRepository, MySqlFriendOfferRepository>();
            services.AddSingleton<IFriendLinkRepository, MySqlFriendLinkRepository>();
            services.AddSingleton<FriendManager>();
            
            services.AddMemoryCache();

            services.AddSingleton<MySqlUpdatesRepository>();
            services.AddSingleton<IUpdatesRepository>(sp => new UpdatesRepositoryCachingProxy(
                sp.GetRequiredService<MySqlUpdatesRepository>(), sp.GetRequiredService<IFriendLinkRepository>(),
                sp.GetRequiredService<IMemoryCache>()));

            services.AddScoped<ExceptionHandlingMiddleware>();

            services.AddSingleton<KafkaProducer>();
            services.AddSingleton<KafkaConsumer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler();
            //}
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
