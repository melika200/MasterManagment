using System;
using System.Collections.Generic;
using System.Linq;
using _01_FrameWork.Domain;
using MasterManagement.Domain.CartAgg;

namespace MasterManagement.Domain.OrderAgg
{
    public class Order : EntityBase
    {
        public long AccountId { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; }
        public double TotalAmount { get; private set; }
        public double DiscountAmount { get; private set; }
        public double PayAmount { get; private set; }
        public bool IsPaid { get; private set; }
        public bool IsCanceled { get; private set; }
        public string IssueTrackingNo { get; private set; }
        public long RefId { get; private set; }

        public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();

        public Order(long accountId, PaymentMethod paymentMethod, double totalAmount, double discountAmount, double payAmount)
        {
            AccountId = accountId;
            PaymentMethod = paymentMethod;
            TotalAmount = totalAmount;
            DiscountAmount = discountAmount;
            PayAmount = payAmount;
            IsPaid = true;
            IsCanceled = false;
            RefId = 0;
            IssueTrackingNo = string.Empty;
        }

        public void AddItem(OrderItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            Items.Add(item);
            RecalculateAmounts();
        }

        public void ClearItems()
        {
            Items.Clear();
            RecalculateAmounts();
        }

        public void Edit(PaymentMethod paymentMethod, double totalAmount, double discountAmount, double payAmount)
        {
            PaymentMethod = paymentMethod;
            TotalAmount = totalAmount;
            DiscountAmount = discountAmount;
            PayAmount = payAmount;
        }

        private void RecalculateAmounts()
        {
            TotalAmount = Items.Sum(i => i.GetTotalPrice());
            DiscountAmount = Items.Sum(i => i.GetDiscountAmount());
            PayAmount = TotalAmount - DiscountAmount;
        }

        public void Cancel()
        {
            IsCanceled = true;
        }

        public void SetIssueTrackingNo(string number)
        {
            IssueTrackingNo = number;
        }

        public void SetRefId(long refId)
        {
            if (refId != 0) RefId = refId;
        }
    }
}
