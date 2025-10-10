using MasterManagement.Domain.PaymentMethodAgg;

namespace MasterManagement.Domain.PaymentMethodsTypeAgg;

public static class PaymentMethodsType
{
    public static readonly PaymentMethod Online = new PaymentMethod(1, "Online");
    public static readonly PaymentMethod CashOnDelivery = new PaymentMethod(2, "CashOnDelivery");
    public static readonly PaymentMethod CardToCard = new PaymentMethod(3, "CardToCard");
    public static readonly PaymentMethod Wallet = new PaymentMethod(4, "Wallet");
    public static readonly PaymentMethod Installment = new PaymentMethod(5, "Installment");

    public static readonly List<PaymentMethod> AllMethods = new()
    {
        Online,
        CashOnDelivery,
        CardToCard,
        Wallet,
        Installment
    };
}
