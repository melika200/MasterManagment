namespace MasterManagment.Application.Contracts.Payment;

public class ZarinPalVerifyRequest
{
    public string? merchant_id { get; set; }
    public string? authority { get; set; }
    public int amount { get; set; }
}