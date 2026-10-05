using Delivery.Msv.Services;
using MassTransit;
using MessageMQCommon.MQ.Messages.OrderMsv;
using System.Runtime.CompilerServices;

namespace Delivery.Msv.Consumers
{
    public class DeliveryOrderApprovedConsumer : IConsumer<ApproveOrderMessage>
    {
        private readonly ILogger<DeliveryOrderApprovedConsumer> _logger;
        private readonly DeliveryService _deliveryService;
        public DeliveryOrderApprovedConsumer(ILogger<DeliveryOrderApprovedConsumer> logger, DeliveryService deliveryService)
        {
            _logger = logger;
            _deliveryService = deliveryService;
        }
        public async Task Consume(ConsumeContext<ApproveOrderMessage> context)
        {
            var message = context.Message;
            var result = await _deliveryService.GetCustomeDataAsync(message);
            if (result.IsSuccess)
            {
                _logger.LogInformation($"Request data for order with ID: {message.Id} has been successfully processed.");
            }
            else
            {
                _logger.LogError($"Failed to process request data for order with ID: {message.Id}.");
            }
        }
    }
}
