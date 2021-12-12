using Confluent.Kafka;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegramma.Sender.Api.Sender;
using Telegramma.Sender.Core.ExceptionHandling;
using Telegramma.Sender.Core.Validation;

namespace Telegramma.Sender
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
            services.AddGrpc(x => x.Interceptors.Add<ExceptionHandlingInterceptor>());
            services.AddMediatR(typeof(Startup).Assembly);
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.Scan(scan => scan.FromAssemblies(typeof(Startup).Assembly)
                .AddClasses(c => c.AssignableTo(typeof(IExceptionHandler)))
                .AsImplementedInterfaces()
            );
            services.AddValidatorsFromAssembly(typeof(Startup).Assembly);

            services.AddScoped(_ => new ProducerConfig
            {
                BootstrapServers = Configuration.GetConnectionString("Kafka"),
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<SenderV1Controller>();

                endpoints.MapGet("/",
                    async context =>
                    {
                        await context.Response.WriteAsync("Use GRPC");
                    });
            });

        }
    }
}
