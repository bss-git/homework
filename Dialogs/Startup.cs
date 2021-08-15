using Auth;
using Consul;
using Dialogs.Application;
using Dialogs.Persistence;
using Dialogs.Persistence.Kafka;
using Dialogs.Persistence.Mysql;
using Dialogs.Persistense;
using Dialogs.ServiceDiscovery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

            services.AddOptions<KafkaOptions>().Bind(Configuration.GetSection("Kafka"));
            services.AddSingleton<KafkaProducer<string>>();

            services.AddOptions<DialogsMySqlOptions>().Bind(Configuration.GetSection("DialogsMySql"));
            services.AddSingleton<DialogsShardSelector>();
            services.AddSingleton<IDialogsRepository, MySqlDialogsRepository>();

            services.AddJaegerTracing(Configuration.GetSection("Jaeger").Get<JaegerConfig>());

            services.AddOptions<ConsulOptions>().Bind(Configuration.GetSection("Consul"));

            services.AddHealthChecks();
            services.AddHostedService<OutboxEventSender>();
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

            app.UseHealthChecks("/health");

            UseConsul(app);
        }

        private static void UseConsul(IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetService<IOptions<ConsulOptions>>();
            var client = new ConsulClient(new ConsulClientConfiguration() { Address = new Uri(options.Value.Address) });
            var hostname = Environment.MachineName;
            var consulServiceID = $"dialogs:{hostname}";
            client.Agent.ServiceRegister(new AgentServiceRegistration
            {
                Name = "dialogs",
                ID = consulServiceID,
                Address = hostname,
                Port = 5001,
                Check = new AgentServiceCheck()
                {
                    Name = "http health",
                    Method = "GET",
                    Interval = TimeSpan.FromSeconds(5),
                    Timeout = TimeSpan.FromSeconds(5),
                    HTTP = $"http://{hostname}:5001/health"
                }
            }).Wait();
        }
    }
}
