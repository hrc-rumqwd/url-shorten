using Microsoft.AspNetCore.Mvc;
using UrlShorten.Infrastructures.Abstracts;

namespace UrlShorten.Controllers
{
    [ApiController]
    [Route("api/shorten-link")]
    public class ShortenController(
        IShortenService shortenService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> ShortenUrl([FromQuery] string url)
        {
            var result = await shortenService.ShortenUrlAsync(url);
            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }

        [HttpGet("/{encodedPath}")]
        public async Task<IActionResult> RedirectByShorten([FromRoute]string encodedPath)
        {
            var result = await shortenService.GetShortenPureValueAsync(encodedPath);
            return result.IsSuccess
                ? Redirect(result.Data)
                : RedirectToAction(nameof(HomeController.NotFoundShorten), controllerName: "Home");
        }
    }
}
