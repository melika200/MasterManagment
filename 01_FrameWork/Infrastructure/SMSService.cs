using _01_FrameWork.Application;
using _01_FrameWork.Infrastructure.Models;
using Microsoft.Extensions.Caching.Memory;

namespace _01_FrameWork.Infrastructure;

public class SMSService : ISMSService
{
    private IMemoryCache _memoryCache;

    public SMSService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<OperationResult> SendOTPAsync(string phone)
    {
        var opResult = new OperationResult();
        Random rnd = new Random();
        string OTPNumber = (rnd.Next(10000, 99999)).ToString();
        OTPNumber = "00000";
        _memoryCache.Set<OTPModel>(phone, new() { Number = OTPNumber, Date = DateTime.Now.AddMinutes(5) });
        string smsText = "پایا سیستم،\nکد تایید: " + OTPNumber;
        //opResult = await SendSMSAsync(phone, smsText);
        return opResult.Succedded();
    }

    public async Task<OperationResult> SendSMSAsync(string phone, string smsText)
    {
        var opResult = new OperationResult();

        var _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(60)
        };

        var baseUrl = "http://api.sms-webservice.com/api/V3/Send";
        var api = "114508-8950586d72424c8cab178b918cef190d";
        var SenderNumber = "50002233";
        var Recipient = phone.PadLeft(11, '0');
        var url = $"{baseUrl}?ApiKey={api}&Text={smsText}&Sender={SenderNumber}&Recipients={Recipient}";

        var response = await _httpClient.GetAsync(url);

        //var responseStream = await response.Content.ReadAsStringAsync();

        return opResult.Succedded();
    }
}
