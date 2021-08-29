using Auth;
using Dialogs.Application;
using Dialogs.Persistence;
using Dialogs.Persistence.Mysql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracing;

namespace Dialogs
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
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            ValidateLifetime = true,
                            IssuerSigningKey = JwtAuthOptions.Key,
                            ValidateIssuerSigningKey = true,
                        };
                    });

            services.AddControllers();

            services.AddSingleton<MySqlDb>();

            services.AddOptions<DialogsMySqlOptions>().Bind(Configuration.GetSection("DialogsMySql"));
            services.AddSingleton<DialogsShardSelector>();
            services.AddSingleton<IDialogsRepository, MySqlDialogsRepository>();

            services.AddJaegerTracing(Configuration.GetSection("Jaeger").Get<JaegerConfig>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(builder => builder.WithOrigins("http://kany.ga", "http://localhost:5000")
                            .AllowAnyHeader()
                            .AllowAnyMethod());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<RequestTracingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
