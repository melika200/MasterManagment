using System;
using _01_FrameWork.Domain;

namespace MasterManagement.Domain.OrderAgg
{
    public class OrderItem : EntityBase
    {
        public long ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Count { get; private set; }
        public double UnitPrice { get; private set; }
        public int DiscountRate { get; private set; }
        public long OrderId { get; private set; }
        public Order Order { get; private set; }

        public OrderItem(long productId, int count, double unitPrice, int discountRate, string productName)
        {
            ProductId = productId;
            Count = count;
            UnitPrice = unitPrice;
            DiscountRate = discountRate;
            ProductName = productName;
        }

        public double GetTotalPrice()
        {
            return UnitPrice * Count;
        }

        public double GetDiscountAmount()
        {
            return GetTotalPrice() * DiscountRate / 100.0;
        }
        public void SetOrderId(long orderId)
        {
            OrderId = orderId;
        }

    }
}