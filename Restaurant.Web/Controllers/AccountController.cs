using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Restaurant.Web.Controllers
{
	[RoutePrefix("Login")]
    public class AccountController : Controller
    {
        // GET: Account
		[Route("")]
		[HttpGet]
        public ActionResult Index()
        {
            return View();
        }

    }
}