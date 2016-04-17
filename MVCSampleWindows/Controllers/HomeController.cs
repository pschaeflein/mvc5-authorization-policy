using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCSampleWindows.Controllers
{
	public class HomeController : ControllerBase
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		[Schaeflein.Community.MVC5AuthZPolicy.Authorize(Policy = "Users")]
		public ActionResult Secure()
		{
			return View();
		}
	}
}