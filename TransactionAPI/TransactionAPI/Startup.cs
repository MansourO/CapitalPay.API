using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Shared.Interfaces;
using Shared.Models;
using Shared.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TransactionAPI.Data.Repository.v1;
using TransactionAPI.Infrastructure.AutoMapper;
using TransactionAPI.Messaging.Receive.Options.v1;
using TransactionAPI.Messaging.Receive.Receiver.v1;
using TransactionAPI.Service.v1.Command;
using TransactionAPI.Service.v1.Query;
using TransactionAPI.Service.v1.Services;
using TransactionAPI.Validators.v1;

namespace TransactionAPI
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
            services.AddHealthChecks();
            services.AddOptions();

            var serviceClientSettingsConfig = Configuration.GetSection("RabbitMq");
            var serviceClientSettings = serviceClientSettingsConfig.Get<RabbitMqConfiguration>();
            services.Configure<RabbitMqConfiguration>(serviceClientSettingsConfig);

            services.AddApplicationInsightsTelemetry();

            services.AddMvc(options => options.EnableEndpointRouting = false)
             .SetCompatibilityVersion(CompatibilityVersion.Latest);

            // use repository project for db context and migrations
            services.AddDbContext<Data.Database.CapitalTransactionContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("TransactionDBConnection"), x => x.MigrationsAssembly("TransactionAPI.Data")));

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddCors(options =>
            {
                options.AddPolicy("CorsList", builder =>
                {
                    //App:CorsOrigins in appsettings.json can contain more than one address with splitted by comma.
                    builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            Configuration.GetSection("App:CorsOrigins").Value
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .ToArray()
                        )
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .SetIsOriginAllowed(origin => true)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            services.AddMvc().AddFluentValidation();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Order Api",
                    Description = "A simple API to create or pay orders",
                    Contact = new OpenApiContact
                    {
                        Name = "TopSoftNAtion",
                        Email = "topsoftnation@gmail.com",
                        Url = new Uri("https://www.google.com/")
                    }
                });
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var actionExecutingContext =
                        actionContext as ActionExecutingContext;

                    if (actionContext.ModelState.ErrorCount > 0
                        && actionExecutingContext?.ActionArguments.Count == actionContext.ActionDescriptor.Parameters.Count)
                    {
                        return new UnprocessableEntityObjectResult(actionContext.ModelState);
                    }

                    return new BadRequestObjectResult(actionContext.ModelState);
                };
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            services.AddTransient<IValidator<PostTransaction>, PostTransactionValidator>();

            services.AddScoped<DbContext, Data.Database.CapitalTransactionContext>();

            services.AddSingleton<ITransactionService, TransactionService>();
            services.AddScoped<IRequestHandler<CreateTransactionCommand, Transaction>, CreateTransactionCommandHandler>();
            services.AddTransient<IRequestHandler<UpdateTransactionCommand>, UpdateTransactionCommandHandler>();
            services.AddTransient<IRequestHandler<GetTransactionByIdQuery, Transaction>, GetTransactionByIdQueryHandler>();

            services.AddScoped<IRequestHandler<GetTransactionsQuery, IEnumerable<Transaction>>, GetTransactionsHandler>();

            services.AddScoped(typeof(ITransactionRepository), typeof(TransactionRepository));
            if (serviceClientSettings.Enabled)
            {
                services.AddHostedService<GetTransactionsReceiver>();
                services.AddHostedService<CreateTransactionReceiver>();
            }

            services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(ITransactionService).Assembly);
            services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(GetTransactionsQuery).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts(options => options.MaxAge(365).IncludeSubdomains().Preload());
            }

            // security headers
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXfo(options => options.Deny());
            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(options => options.NoReferrer());

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Capital Pay API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseCors("CorsList"); //Enable CORS!

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}