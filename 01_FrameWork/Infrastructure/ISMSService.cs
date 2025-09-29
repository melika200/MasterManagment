using _01_FrameWork.Application;

namespace _01_FrameWork.Infrastructure;

public interface ISMSService
{
    Task<OperationResult> SendOTPAsync(string phone);
    Task<OperationResult> SendSMSAsync(string phone, string smsText);
}
