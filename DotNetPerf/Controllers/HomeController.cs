using System.Web.Mvc;

namespace DotNetPerf.Controllers
{
    public class HomeController  :Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "DotNetPerf - .NET performance playground";
            return View();
        }
    }
}
