using Microsoft.AspNetCore.Mvc;
using LocationMapper.WebScrapers.Interfaces;

namespace LocationMapper.WebUi.Controllers
{
    public class HomeController : Controller
    {
        private IUkcReader ukcReader;
        private ICragLocator cragLocator;

        public HomeController(IUkcReader ukcReader, ICragLocator cragLocator)
        {
            this.ukcReader = ukcReader;
            this.cragLocator = cragLocator;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Map(string ukcUserName)
        {
            return View();
        }
    }
}
