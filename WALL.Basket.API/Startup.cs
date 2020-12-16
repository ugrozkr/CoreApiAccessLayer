using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using Wall.Service.DependencyResolver;
using WALL.Basket.API.Services;
using Wall.Integration.MessagingBus;
using RabbitMQ.Client;
using Microsoft.Extensions.Logging;
using Wall.Service.Extensions;
using Wall.Service.DataAccessLayer.DbContexts;
using Wall.Service.Mapper;
using Wall.Integration.Messages;

namespace WALL.Basket.API
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
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<RabbitMQPersistentConnection>>();
                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBusConnection"],
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(Configuration["EventBusUserName"]))
                {
                    factory.UserName = Configuration["EventBusUserName"];
                }

                if (!string.IsNullOrEmpty(Configuration["EventBusPassword"]))
                {
                    factory.Password = Configuration["EventBusPassword"];
                }
                return new RabbitMQPersistentConnection(factory, logger, 5);
            });

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new BasketServiceProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddDbContext<CatalogDbContext>(
            options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient
            );
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.LoadDependencies(Configuration["DI:Path"], Configuration["DI:Basket:Dll"]);
            services.LoadDependencies(Configuration["DI:Path"], Configuration["DI:BasketLine:Dll"]);
            services.LoadDependencies(Configuration["DI:Path"], Configuration["DI:BasketChangeEvent:Dll"]);
            services.LoadDependencies(Configuration["DI:Path"], Configuration["DI:Event:Dll"]);
            services.AddHttpClient<IEventCatalogService, EventCatalogService>(c => c.BaseAddress = new Uri(Configuration["ApiConfigs:EventCatalog:Uri"]));
            services.AddHttpClient<IDiscountService, DiscountService>(c => c.BaseAddress = new Uri(Configuration["ApiConfigs:Discount:Uri"]))
                .AddPolicyHandler(PolicyBuilderExtensions.GetRetryPolicy())
                .AddPolicyHandler(PolicyBuilderExtensions.GetCircuitBreakerPolicy());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket Catalog API", Version = "v1" });
            });
            services.AddMvc();
            services.AddOptions();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket API V1");

            });
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<ProductPriceChangedIntegrationEvent, ProductPriceChangedIntegrationEventHandler>();
        }
    }
}
