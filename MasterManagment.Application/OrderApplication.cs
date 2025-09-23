using System;
using System.Collections.Generic;
using System.Linq;
using MasterManagement.Domain.OrderAgg;
using MasterManagment.Application.Contracts.Order;

namespace MasterManagment.Application
{
    public class OrderApplication : IOrderApplication
    {
        private readonly IOrderRepository _orderRepository;

        public OrderApplication(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public long PlaceOrder(CreateOrderCommand command)
        {
            var order = new Order(command.AccountId, command.PaymentMethod, command.TotalAmount,
                                  command.DiscountAmount, command.PayAmount);

            foreach (var item in command.Items)
            {
            
                order.AddItem(new OrderItem(item.ProductId, item.Count, item.UnitPrice, item.DiscountRate, item.ProductName));
            }

            _orderRepository.Create(order);
            _orderRepository.SaveChanges();

            return order.Id;
        }

        public void Edit(EditOrderCommand command)
        {
            var order = _orderRepository.Get(command.Id);
            if (order == null)
                throw new Exception("Order not found.");

            order.Edit(command.PaymentMethod, command.TotalAmount, command.DiscountAmount, command.PayAmount);
            order.Items.Clear();

            foreach (var item in command.Items)
            {
               
                order.AddItem(new OrderItem(item.ProductId, item.Count, item.UnitPrice, item.DiscountRate, item.ProductName));
            }

            _orderRepository.Update(order);
            _orderRepository.SaveChanges();
        }

        public void Cancel(long id)
        {
            var order = _orderRepository.Get(id);
            if (order == null)
                throw new Exception("Order not found.");

            order.Cancel();
            _orderRepository.Update(order);
            _orderRepository.SaveChanges();
        }

        public double GetAmountBy(long id)
        {
            var order = _orderRepository.Get(id);
            if (order == null)
                throw new Exception("Order not found.");

            return order.PayAmount;
        }

        public List<OrderItemViewModel> GetItems(long orderId)
        {
            var order = _orderRepository.Get(orderId);
            if (order == null)
                throw new Exception("Order not found.");

            return order.Items.Select(i => new OrderItemViewModel
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,  
                Count = i.Count,
                UnitPrice = i.UnitPrice,
                DiscountRate = i.DiscountRate
            }).ToList();
        }

        public List<OrderViewModel> Search(OrderSearchCriteria searchModel)
        {
            var query = _orderRepository.GetAll().AsQueryable();

            if (searchModel.AccountId != 0)
                query = query.Where(o => o.AccountId == searchModel.AccountId);

            if (searchModel.IsCanceled.HasValue)
                query = query.Where(o => o.IsCanceled == searchModel.IsCanceled.Value);

            if (searchModel.IsPaid.HasValue)
                query = query.Where(o => o.IsPaid == searchModel.IsPaid.Value);

            var orders = query.ToList();

            return orders.Select(o => new OrderViewModel
            {
                Id = o.Id,
                AccountName = "",
                PaymentMethod = o.PaymentMethod,
                TotalAmount = o.TotalAmount,
                IsPaid = o.IsPaid,
                IsCanceled = o.IsCanceled,
                IssueTrackingNo = o.IssueTrackingNo
            }).ToList();
        }
    }
}
