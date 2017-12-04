﻿using Microsoft.Owin.Security;
using Restaurant.Core.Interfaces;
using Restaurant.Infrastructure.Repositories;
using Restaurant.Web.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Restaurant.Web.Areas.Admin.Controllers
{
	[RouteArea("admin")]
	[RoutePrefix("")]
	[Authorize]
	public class AdminController : Controller
	{
		private readonly IUserRepository _userRepository;
		private readonly IProductCategoryRepostitory _categoryRepostitory;

		public AdminController()
			: this(new UserRepository(), new ProductCategoryRepostitory())
		{
		}

		public AdminController(IUserRepository userRepository, IProductCategoryRepostitory categoryRepostitory)
		{
			_userRepository = userRepository;
			_categoryRepostitory = categoryRepostitory;
		}

		// GET: Admin/Admin
		[Route("")]
		public async Task<ActionResult> Index()
		{
			var users = await _userRepository.GetAllUsersAsync();
			var model = new DashboardViewModel()
			{
				UserLogin = User.Identity.Name,
				Users = users.Select(u => u.Roles.Where(r => r.RoleId == "54ec7407-2468-4579-82d9-99e701f5c761")).Count(),
				Price = 0,
				Domain = 0,
				Host = 0,
				Categories = await _categoryRepostitory.GetAllAsync()

			};
			if (User.IsInRole("admin"))
			{
				model.Role = "Admin";
			}
			else if (User.IsInRole("editor"))
			{
				model.Role = "Editor";
			}
			else if (User.IsInRole("author"))
			{
				model.Role = "Author";
			}


			return View(model);
		}

		// GET: Admin/Admin/Login
		[HttpGet]
		[Route("login")]
		[AllowAnonymous]
		public async Task<ActionResult> Login()
		{
			return View();
		}

		// product: Admin/Admin/Login
		[HttpPost]
		[Route("login")]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginViewModel model)
		{
			try
			{
				var user = await _userRepository.GetLoginUserAsync(model.Username, model.Password);

				if (user == null)
				{
					//ModelState.AddModelError(string.Empty, "The user with supplied credentials does not exist.");
					ModelState.AddModelError(string.Empty, "کاربر با مدارک ارائه شده موجود نیست.");
				}

				var authManager = HttpContext.GetOwinContext().Authentication;

				var userIdentity = await _userRepository.CreateIdentityAsync(user);

				authManager.SignIn(new AuthenticationProperties(), userIdentity);


				return RedirectToAction("Index");
			}
			catch (Exception e)
			{
				ModelState.AddModelError(string.Empty, e.Message);
				return View(viewName: "Login");
			}
		}


		// GET: Admin/Admin/Logout
		[HttpGet]
		[Route("logout")]
		public async Task<ActionResult> Logout()
		{
			var authManager = HttpContext.GetOwinContext().Authentication;

			authManager.SignOut();

			return RedirectToAction("Index");
		}

		//// GET: Admin/Admin/ForgotPassword
		//[HttpGet]
		//[Route("forgotpassword")]
		//[AllowAnonymous]
		//public async Task<ActionResult> Forgotpassword()
		//{
		//	return View(viewName: "Forgotpassword");
		//}


		// product: Admin/ForgotPassword
		//[HttpPost]
		//[Route("forgotpassword")]
		//[AllowAnonymous]
		//public async Task<ActionResult> Forgotpassword(LoginViewModel model)
		//{
		//	try
		//	{
		//		if (!ModelState.IsValid)
		//		{
		//			return HttpNotFound();
		//		}

		//		var users = await _userRepository.GetAllUsersAsync();

		//		var user = users.SingleOrDefault(u => u.UserName == model.Username);

		//		if (user == null)
		//		{
		//			ModelState.AddModelError(string.Empty, "The user with supplied credentials does not exist.");
		//		}

		//		model.RecoveryPassword = user.PasswordHash;

		//		return RedirectToAction("RecoveryPassword", model);

		//		//return PartialView(model: userPassword, viewName: "Password");
		//	}
		//	catch (Exception e)
		//	{
		//		ModelState.AddModelError("keys", e);
		//		return RedirectToAction("Login");
		//	}

		//}

		//[HttpGet]
		//[Route("RecoveryPassword")]
		//[AllowAnonymous]
		//public ActionResult RecoveryPassword(LoginViewModel model)
		//{
		//	//var model = new LoginViewModel();

		//	return View(viewName: "RecoveryPassword", model: model);
		//}

		[AllowAnonymous]
		public async Task<PartialViewResult> AdminMenu()
		{
			var items = new List<AdminMenuItem>();

			if (User.Identity.IsAuthenticated)
			{
				items.Add(new AdminMenuItem
				{
					Text = "Admin Home",
					Action = "index",
					RouteInfo = new { controller = "admin", area = "admin" }
				});

				if (User.IsInRole("admin"))
				{
					items.Add(new AdminMenuItem
					{
						Text = "Users",
						Action = "index",
						RouteInfo = new { controller = "user", area = "admin" }
					});

					items.Add(new AdminMenuItem()
					{
						Text = "Products",
						Action = "index",
						RouteInfo = new { controller = "product", area = "admin" }
					});
				}
				else
				{
					items.Add(new AdminMenuItem
					{
						Text = "Profile",
						Action = "edit",
						RouteInfo = new { controller = "user", area = "admin", username = User.Identity.Name }
					});
				}

				if (!User.IsInRole("author"))
				{
					items.Add(new AdminMenuItem
					{
						Text = "Tags",
						Action = "index",
						RouteInfo = new { controller = "tag", area = "admin" }
					});
				}

				items.Add(new AdminMenuItem
				{
					Text = "Posts",
					Action = "index",
					RouteInfo = new { controller = "product", area = "admin" }
				});

				items.Add(new AdminMenuItem
				{
					Text = "Categories",
					Action = "index",
					RouteInfo = new { controller = "postcategory", area = "admin" }
				});
			}

			return PartialView(items);
		}

		[AllowAnonymous]
		public async Task<PartialViewResult> AuthenticationLink()
		{
			var item = new AdminMenuItem
			{
				RouteInfo = new { controller = "admin", area = "admin" }
			};

			if (User.Identity.IsAuthenticated)
			{
				item.Text = "Logout";
				item.Action = "logout";
			}
			else
			{
				item.Text = "Login";
				item.Action = "login";
			}

			return PartialView("_menuLink", item);
		}

		private bool _isDisposed;

		protected override void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				_userRepository.Dispose();
			}

			_isDisposed = true;
			base.Dispose(disposing);
		}
	}
}