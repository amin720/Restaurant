using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Restaurant.Web.ViewModels;

namespace Restaurant.Web.Controllers
{
	[RoutePrefix("")]
    public class HomeController : Controller
    {

		// GET: Home
		[Route("")]
		[HttpGet]
		public ActionResult Index()
        {
            return View();
        }

	    [HttpPost]
	    public async Task<ActionResult> Login(AccountViewModels model)
	    {
		    return View();
	    }

	    [HttpPost]
	    public async Task<ActionResult> Register(AccountViewModels model)
	    {
		    return View();
		}
	}
}