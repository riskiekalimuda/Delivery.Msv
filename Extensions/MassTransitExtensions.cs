using Delivery.Msv.Consumers;
using Delivery.Msv.Models;
using MassTransit;
using MessageMQCommon.MQ.Names;
using MessageMQCommon.Parameters;

namespace Delivery.Msv.Extensions
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddCustomMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMqSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMQParameter>() ?? new RabbitMQParameter() ;
            services.AddMassTransit(x =>
            {
                x.AddEntityFrameworkOutbox<DeliveryMsvDbContext>(o =>
                {
                    o.UsePostgres();
                    o.UseBusOutbox();
                    o.QueryDelay = TimeSpan.FromSeconds(10); 
                }); 

                x.AddConsumersFromNamespaceContaining<DeliveryOrderApprovedConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings.Host, rabbitMqSettings.VirtualHost, h =>
                    {
                        h.Username(rabbitMqSettings.Username);
                        h.Password(rabbitMqSettings.Password);
                    });
                    cfg.ReceiveEndpoint(QueueNames.OrderQueue.ApproveOrderQueue, e =>
                    {
                        e.Durable = true;   
                        e.UseMessageRetry(r => r.Interval(20,10)); 
                        e.ConfigureConsumer<DeliveryOrderApprovedConsumer>(context);
                    });
                });
            });
            return services;
        }
    }
}
