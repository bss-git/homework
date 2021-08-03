using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Auth;
using Homework.Auth;
using Homework.Dialogs;
using Homework.Dialogs.Application;
using Homework.Events;
using Homework.Events.RabbitMQ;
using Homework.Friends;
using Homework.Persistence;
using Homework.Persistence.Tarantool;
using Homework.Updates;
using Homework.Updates.SignalR;
using Homework.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Tracing;

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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = JwtAuthOptions.Key,
                    ValidateIssuerSigningKey = true,
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["beareridentity"];
                        return Task.CompletedTask;
                    }
                };
            });

            var redis = Configuration.GetSection("Redis").Get<RedisConfig>();
            services.AddSignalR();//.AddStackExchangeRedis($"{redis.Host}:{redis.Port}");

            services.AddControllersWithViews();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddOptions<MySqlOptions>().Bind(Configuration.GetSection("MySql"));
            services.AddSingleton<MySqlDb>();

            services.AddScoped<CurrentUserManager>();
            services.AddSingleton<IUserIdProvider, ClaimsUserIdProvider>();
            services.AddSingleton<IPasswordManager, HashingMySqlPasswordManager>();

            services.AddOptions<TaratoolOptions>().Bind(Configuration.GetSection("Tarantool"));
            services.AddSingleton<TarantoolDb>();
            services.AddSingleton<MySqlUserRepository>();
            services.AddSingleton<IUserRepository, TarantoolUserRepository>();
            services.AddSingleton<UserCreator>();

            services.AddSingleton<IFriendOfferRepository, MySqlFriendOfferRepository>();
            services.AddSingleton<IFriendLinkRepository, MySqlFriendLinkRepository>();
            services.AddSingleton<FriendManager>();
            
            services.AddMemoryCache();


            services.AddOptions<RabbitOptions>().Bind(Configuration.GetSection("RabbitMQ"));
            services.AddSingleton<RabbitChannelFactory>();
            services.AddOptions<KafkaOptions>().Bind(Configuration.GetSection("Kafka"));
            services.AddSingleton<KafkaProducer>();
            services.AddSingleton<KafkaConsumer>();
            services.AddSingleton<MySqlUpdatesRepository>();
            services.AddSingleton<UpdatesRepositoryCachingProxy>();
            services.AddSingleton<IUpdatesRepository, UpdateMessageDispatchingDecorator>();
            services.AddSingleton<UpdatesMessageBus>();
            services.AddSingleton<UpdatesHubEventPublisher>();

            services.AddOptions<DialogsOptions>().Bind(Configuration.GetSection("Dialogs"));
            services.AddHttpClient<IDialogsRepository, ServiceDialogsRepository>();

            services.AddScoped<ExceptionHandlingMiddleware>();

            services.AddJaegerTracing(Configuration.GetSection("Jaeger").Get<JaegerConfig>());
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

            app.UseStatusCodePages(async context => {
                var request = context.HttpContext.Request;
                var response = context.HttpContext.Response;

                if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    response.Redirect("/auth/login");
                };
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<RequestTracingMiddleware>();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<UpdatesHub>("/api/updates/hub");
            });

        }
    }
}
