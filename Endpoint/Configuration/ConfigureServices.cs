using Application.Files;
using Application.Interfaces.Context;
using Endpoint.Consumer;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Endpoint.Configuration
{
    public static class ConfigureServices
    {
        public static void ConfigureAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDataBaseContext, DataBaseContext>();
            
            services.AddTransient<IUploadFileServices, UploadFileServices>();
            services.AddMassTransit(x =>
            {
                x.AddConsumer<ProcessUploadedItemConsumer>();
                x.AddConsumer<SaveToDatabaseConsumer>();
                x.AddConsumer<SuccessConsumer>();
                x.AddConsumer<UploadFileQueue1Consumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.ReceiveEndpoint("uplodeandsaveDB-file-queue1",
                      ep => { ep.ConfigureConsumer<UploadFileQueue1Consumer>(context); });
                    cfg.ReceiveEndpoint("process-file-queue2",
                        ep => { ep.ConfigureConsumer<ProcessUploadedItemConsumer>(context); });

                    cfg.ReceiveEndpoint("save-db-queue3", ep => { ep.ConfigureConsumer<SaveToDatabaseConsumer>(context); });

                    cfg.ReceiveEndpoint("success-queue",
                        ep => { ep.ConfigureConsumer<SuccessConsumer>(context); });
                });
            });

        }

    }
}
