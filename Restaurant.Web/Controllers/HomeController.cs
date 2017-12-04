using System.Web.Mvc;

namespace Restaurant.Web.Controllers
{
	[RoutePrefix("")]
    public class HomeController : Controller
    {

		// GET: Home
		[Route("")]
		[HttpGet]
		[AllowAnonymous]
		public ActionResult Index()
        {
            return View();
        }

	    
	}
}