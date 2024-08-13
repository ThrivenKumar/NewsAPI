
using Microsoft.AspNetCore.Mvc;
using NewsAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace NewsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        public NewsController(INewsService newsService) {
            _newsService = newsService;

        }

        [HttpGet]
        [Route("/getheadlines")]
        public async Task<IActionResult> GetNews()
        {
            var news = await _newsService.GetNewsAsync();
            return Ok(news);
        }

        [HttpGet]
        [Route("/getcommonwords")]
        public async Task<IActionResult> GetCommonWords()
        {
            var commonwords = await _newsService.FindCommonWordsAsync();
            return Ok(commonwords);
        }

        [Authorize]
        [HttpGet]
        [Route("/testauthentication")]
        public IActionResult GetTest()
        {
            return Ok("This is a protected endpoint.");
        }
    }
}
