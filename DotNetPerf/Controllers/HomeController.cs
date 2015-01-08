using System.Web.Mvc;

namespace DotNetPerf.Controllers
{
    public class HomeController  :Controller
    {
        public ActionResult Index()
        {
            return View();
        }
         
    }
}