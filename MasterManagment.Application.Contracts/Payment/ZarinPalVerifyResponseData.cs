namespace MasterManagment.Application.Contracts.Payment;

public class ZarinPalVerifyResponseData
{
    public int code { get; set; }
    public string? message { get; set; }
    public long ref_id { get; set; }
}