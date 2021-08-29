using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserCounters.Domain;
using UserCounters.IncomingEvents;
using UserCounters.Persistence;
using UserCounters.Persistence.Kafka;
using UserCounters.Persistense;
using UserCounters.Persistense.Redis;
using UserCounters.UserCounters;

namespace UserCounters
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
            services.AddControllers();

            services.AddOptions<RedisOptions>().Bind(Configuration.GetSection("Redis"));
            services.AddOptions<KafkaOptions>().Bind(Configuration.GetSection("Kafka"));
            services.AddSingleton<EventHandlerFactory>();
            services.AddSingleton<RedisDb>();
            services.AddSingleton<ICountersRepository, RedisCountersRepository>();

            services.AddHostedService<EventHandlingService>();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
