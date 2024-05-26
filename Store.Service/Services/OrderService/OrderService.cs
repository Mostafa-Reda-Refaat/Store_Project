using AutoMapper;
using Store.Data.Entities;
using Store.Data.Entities.OrderEntities;
using Store.Repository.Interfaces;
using Store.Repository.Specification.OrderSpec;
using Store.Service.Services.BasketService;
using Store.Service.Services.BasketService.Dtos;
using Store.Service.Services.OrderService.Dtos;
using Store.Service.Services.PaymentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketService _basketService;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitOfWork, IBasketService basketService, IMapper mapper, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _basketService = basketService;
            _mapper = mapper;
            _paymentService = paymentService;
        }
        public async Task<OrderResultDto> CreateOrderAsync(OrderDto input)
        {
            //Get Basket
            var basket = await _basketService.GetBasketAsync(input.BasketId);

            if (basket is null)
                throw new Exception("Basket Not Exist");

            //Fill OrderItems from BasketItems
            var orderItems = new List<OrderItemDto>();

            foreach(var basketItem in basket.BasketItems)
            {
                var productItem = await _unitOfWork.ProductRepository.GetByIdAsync(basketItem.ProductId);

                if (productItem is null)
                    throw new Exception($"Product With Id : {basketItem.ProductId} Not Exist");

                var itemOrdered = new ProductItemOrdered
                {
                    ProductItemId = productItem.Id,
                    ProductName = productItem.Name,
                    PictureUrl = productItem.PictureUrl
                };

                var orderItem = new OrderItem
                {
                    Price = productItem.Price,
                    Quantity = basketItem.Quantity,
                    ItemOrdered = itemOrdered
                };

                var mappedOrderItem = _mapper.Map<OrderItemDto>(orderItem);

                orderItems.Add(mappedOrderItem);
            }

            //Get Delivery Method
            var deliveryMethod = await _unitOfWork.DeliveryMethodRepository.GetByIdAsync(input.DeliveryMethodId);

            if (deliveryMethod is null)
                throw new Exception("Delivery Method Not Provided");

            //Calculate Subtotal
            var subTotal = orderItems.Sum(item => item.Quantity * item.Price);

            // To Do => Check If Order Exist
            var specs = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);

            var existingOrder = await _unitOfWork.OrderRepository.GetWithSpecificationByIdAsync(specs);

            var basketDto = new CustomerBasketDto();

            if (existingOrder != null)
            {
                _unitOfWork.OrderRepository.Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntentForExistingOrder(basket);
            }
            else
            {
                basketDto = await _paymentService.CreateOrUpdatePaymentIntentForNewOrder(basket.Id);
            }

            //Create Order
            var mappedShippingAddress = _mapper.Map<ShippingAddress>(input.ShippingAddress);

            var mappedOrderItems = _mapper.Map<List<OrderItem>>(orderItems);

            var order = new Order
            {
                BuyerEmail = input.BuyerEmail,
                ShippingAddress = mappedShippingAddress,
                DeliveryMethodId = deliveryMethod.Id,
                OrderItems = mappedOrderItems,
                SubTotal = subTotal,
                BasketId = basket.Id,
                PaymentIntentId = basketDto.PaymentIntentId
            };

            await _unitOfWork.OrderRepository.AddAsync(order);

            _unitOfWork.Complete();

            var mappedOrder = _mapper.Map<OrderResultDto>(order);

            return mappedOrder;
        }

        public async Task<IReadOnlyList<OrderResultDto>> GetAllOrdersForUserAsync(string buyerEmail)
        {
            var specs = new OrderWithItemsSpecification(buyerEmail);

            var orders = await _unitOfWork.OrderRepository.GetAllWithSpecificationAsync(specs);

            if (orders is { Count: <= 0 })
                throw new Exception("You Do Not Have Any Orders Yet");

            var mappedOrders = _mapper.Map<List<OrderResultDto>>(orders);

            return mappedOrders;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodsAsync()
            => await _unitOfWork.DeliveryMethodRepository.GetAllAsync();

        public async Task<OrderResultDto> GetOrderByIdAsync(Guid id, string buyerEmail)
        {
            var specs = new OrderWithItemsSpecification(id, buyerEmail);

            var order = await _unitOfWork.OrderRepository.GetWithSpecificationByIdAsync(specs);

            if (order is null)
                throw new Exception($"There Is No Order With Id : {id}");

            var mappedOrder = _mapper.Map<OrderResultDto>(order);

            return mappedOrder;
        }
    }
}
