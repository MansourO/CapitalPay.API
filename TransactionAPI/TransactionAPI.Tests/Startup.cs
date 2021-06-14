using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Interfaces;
using Shared.Models;
using Shared.Models.ViewModels;
using System.Collections.Generic;
using System.Reflection;
using TransactionAPI.Data.Repository.v1;
using TransactionAPI.Messaging.Receive.Options.v1;
using TransactionAPI.Messaging.Receive.Receiver.v1;

using TransactionAPI.Service.v1.Command;
using TransactionAPI.Service.v1.Query;
using TransactionAPI.Service.v1.Services;
using TransactionAPI.Validators.v1;

namespace TransactionAPI.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
        .SetBasePath(System.IO.Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false, true)
        .Build();

            var serviceClientSettingsConfig = configuration.GetSection("RabbitMq");
            var serviceClientSettings = serviceClientSettingsConfig.Get<RabbitMqConfiguration>();
            services.Configure<RabbitMqConfiguration>(serviceClientSettingsConfig);

            services.AddMvc(options => options.EnableEndpointRouting = false)
             .SetCompatibilityVersion(CompatibilityVersion.Latest);

            // use repository project for db context and migrations
            //services.AddDbContext<Data.Database.CapitalTransactionContext>(options =>
            //   options.UseSqlServer(configuration.GetConnectionString("TransactionDBConnection"), x => x.MigrationsAssembly("TransactionAPI.Data")));
            services.AddDbContext<Data.Database.CapitalTransactionContext>(options => options.UseInMemoryDatabase(databaseName: "MockDb"));
            services.AddAutoMapper(typeof(Startup));

            services.AddMvc().AddFluentValidation();
            //services.AddControllersWithViews();

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

            services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(ITransactionService).Assembly);

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            services.AddTransient<IValidator<PostTransaction>, PostTransactionValidator>();

      

            //services.AddTransient<IRequestHandler<UpdateTransactionCommand>, UpdateTransactionCommandHandler>();

            services.AddTransient<IRequestHandler<CreateTransactionCommand, Transaction>, CreateTransactionCommandHandler>();

            services.AddTransient<IRequestHandler<GetTransactionByIdQuery, Transaction>, GetTransactionByIdQueryHandler>();


            services.AddTransient<IRequestHandler<GetTransactionsQuery, IEnumerable<Transaction>>, GetTransactionsHandler>();

            services.AddTransient<ITransactionService, TransactionService>();

            if (serviceClientSettings.Enabled)
            {
                services.AddHostedService<GetTransactionsReceiver>();
                services.AddHostedService<CreateTransactionReceiver>();
            }
        }
    }
}