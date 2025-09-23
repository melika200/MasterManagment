using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterManagment.Application.Contracts.Order
{
    public interface IOrderApplication
    {
        long PlaceOrder(CreateOrderCommand command);  // ثبت سفارش جدید
        void Edit(EditOrderCommand command);          // ویرایش سفارش موجود
        void Cancel(long id);                          // لغو سفارش
        double GetAmountBy(long id);                   // دریافت مبلغ سفارش به ازای شناسه
        List<OrderItemViewModel> GetItems(long orderId);  // دریافت آیتم‌های سفارش
        List<OrderViewModel> Search(OrderSearchCriteria searchModel); // جستجوی سفارش‌ها
    }
}
