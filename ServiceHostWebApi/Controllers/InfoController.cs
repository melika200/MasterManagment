using MasterManagment.Application.Contracts.AboutUs;
using MasterManagment.Application.Contracts.FaqUs;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHostWebApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class InfoController : ControllerBase
{
    private readonly IFaqApplication _faqApplication;
    private readonly IAboutApplication _aboutApplication;

    public InfoController(IFaqApplication faqApplication, IAboutApplication aboutApplication)
    {
        _faqApplication = faqApplication;
        _aboutApplication = aboutApplication;
    }

   
    [HttpGet("faq")]
    [ProducesResponseType(200, Type = typeof(List<FaqViewModel>))]
    public async Task<IActionResult> GetAllFaqs()
    {
        var faqs = await _faqApplication.Search(new SearchFaqCriteria { IsActive = true });
        return Ok(faqs);
    }

   
    [HttpGet("faq/{id}")]
    [ProducesResponseType(200, Type = typeof(FaqViewModel))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetFaq(long id)
    {
        var faq = await _faqApplication.GetById(id);
        if (faq == null)
            return NotFound(new { message = "FAQ یافت نشد." });
        return Ok(faq);
    }

    
    [HttpGet("about")]
    [ProducesResponseType(200, Type = typeof(AboutViewModel))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetAbout()
    {
        var about = await _aboutApplication.GetActiveAbout();
        if (about == null)
            return NotFound(new { message = "About یافت نشد." });
        return Ok(about);
    }
}
