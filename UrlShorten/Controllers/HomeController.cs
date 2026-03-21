using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UrlShorten.Infrastructures.Abstracts;

namespace UrlShorten.Controllers
{
    [Controller]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("not-found")]
        public async Task<IActionResult> NotFoundShorten(string encodedPath)
        {
            return View();
        }
    }
}
