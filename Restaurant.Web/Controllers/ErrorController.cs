using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Restaurant.Web.Controllers
{
    public class ErrorController : Controller
    {
		public ActionResult Index()
	    {
		    return View();
	    }

	    public ActionResult NotFound()
	    {
		    return View();
	    }

	    public ActionResult Forbidden()
	    {
		    return View();
	    }
	}
}