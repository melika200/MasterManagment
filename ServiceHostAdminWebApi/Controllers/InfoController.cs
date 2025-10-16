using MasterManagment.Application.Contracts.AboutUs;
using MasterManagment.Application.Contracts.FaqUs;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHostAdminWebApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("admin/api/v{version:apiVersion}/[controller]")]
public class InfoController : ControllerBase
{
    private readonly IFaqApplication _faqApplication;
    private readonly IAboutApplication _aboutApplication;

    public InfoController(IFaqApplication faqApplication, IAboutApplication aboutApplication)
    {
        _faqApplication = faqApplication;
        _aboutApplication = aboutApplication;
    }

    #region FAQ Admin

    [HttpPost("faq")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateFaq([FromBody] CreateFaqCommand command)
    {
        await _faqApplication.Create(command);
        return Ok(new { message = "FAQ با موفقیت ایجاد شد." });
    }

    [HttpGet("faq")]
    [ProducesResponseType(200, Type = typeof(List<FaqViewModel>))]
    public async Task<IActionResult> GetAllFaqs()
    {
        var faqs = await _faqApplication.Search(new SearchFaqCriteria { IsActive = true });
        return Ok(faqs);
    }

    [HttpPut("faq")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> EditFaq([FromBody] EditFaqCommand command)
    {
        await _faqApplication.Edit(command);
        return Ok(new { message = "FAQ با موفقیت ویرایش شد." });
    }

    #endregion

    #region About Admin

    [HttpPost("about")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateAbout([FromBody] CreateAboutCommand command)
    {
        await _aboutApplication.Create(command);
        return Ok(new { message = "About با موفقیت ایجاد شد." });
    }

    [HttpPut("about")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> EditAbout([FromBody] EditAboutCommand command)
    {
        await _aboutApplication.Edit(command);
        return Ok(new { message = "About با موفقیت ویرایش شد." });
    }
    [HttpGet("about")]
    [ProducesResponseType(200, Type = typeof(List<AboutViewModel>))]
    public async Task<IActionResult> GetAllAbout()
    {
        var faqs = await _aboutApplication.Search(new AboutSearchCriteria { IsActive = true });
        return Ok(faqs);
    }

    #endregion
}
