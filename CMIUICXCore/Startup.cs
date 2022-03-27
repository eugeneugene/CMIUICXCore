using CMIUICXCore.Data;
using CMIUICXCore.MiddleWare;
using CMIUICXCore.Services;
using CMIUICXCore.Services.HttpClients;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SBCommon.HttpClients.New;
using System;
using System.IO;

namespace CMIUICXCore
{
    internal class Startup
    {
        private IConfiguration Configuration { get; }
        private IHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = services.AddControllers();

            var info = new OpenApiInfo
            {
                Version = "1.0",
                Title = "Интеграция ЦМИУ и АТС Commend",
            };
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", info);
                var filePath = Path.Combine(AppContext.BaseDirectory, "CMIUICXCore.xml");
                options.IncludeXmlComments(filePath);
            });

            // Db Context
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DataContext"));
                options.EnableSensitiveDataLogging(Environment.IsDevelopment());
            });

            // HTTP Services
            services.AddHttpClient(CmiuHttpClientProvider.HttpClientName, (provider, client) => CmiuHttpClient.TimeoutSetter(provider, client));

            // Services
            services.AddTransient<ICmiuHttpClient, CmiuHttpClient>();
            services.AddTransient(typeof(HttpRequestFactory<>));
            services.AddTransient<CmiuHttpClientProvider>();
            services.AddTransient<CmiuUriProvider>();
            services.AddSingleton<AsyncTcpClient>();

            // Hosted services
            services.AddHostedService<IcxHandlingService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseRequestResponseLogging();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json"; ;
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Интеграция с оборудованием Scheidt&Bachmann");
                c.RoutePrefix = "swagger";
            });

            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
