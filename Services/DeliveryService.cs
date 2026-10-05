using AutoMapper;
using Delivery.Msv.Models;
using MassTransit;
using MessageMQCommon.MQ.Messages.DeliveryMsv;
using MessageMQCommon.MQ.Messages.OrderMsv;
using MessageMQCommon.MQ.Names;
using MessageMQCommon.Respones;

namespace Delivery.Msv.Services
{
    public class DeliveryService
    {
        private readonly ILogger<DeliveryService> _logger;
        private readonly IMapper _mapper;
        private readonly DeliveryMsvDbContext _dbContext;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        public DeliveryService(ILogger<DeliveryService> logger, IMapper mapper, DeliveryMsvDbContext dbContext, ISendEndpointProvider sendEndpointProvider)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
            _sendEndpointProvider = sendEndpointProvider;   
        }

        public async Task<ServiceResult<TrxDelivery>> InsertAsync(ApproveOrderMessage approveOrderMessage)
        {
            try
            {
                var delivery = _mapper.Map<TrxDelivery>(approveOrderMessage);
                await _dbContext.TrxDeliveries.AddAsync(delivery);
                await _dbContext.SaveChangesAsync();
                return new ServiceResult<TrxDelivery>(true) { Data = delivery, IsSuccess = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting delivery record.");
                return new ServiceResult<TrxDelivery>(false) { IsSuccess = false };
            }
        }
        public async Task<ServiceResult<ApproveOrderMessage>> GetCustomeDataAsync(ApproveOrderMessage approveOrderMessage)
        {
            try
            {
                DeliveryMessage deliveryMessage = new DeliveryMessage
                {
                    ApprovedOrder = approveOrderMessage
                };
                var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{QueueNames.DeliveryQueue.DeliveryRequestDataCustomerQueue}"));
                await sendEndpoint.Send(deliveryMessage);
                await _dbContext.SaveChangesAsync();
                return new ServiceResult<ApproveOrderMessage>(true) { Data = approveOrderMessage, IsSuccess = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting delivery record.");
                return new ServiceResult<ApproveOrderMessage>(false) { IsSuccess = false };
            }
        }
    }
}
