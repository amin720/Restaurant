using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Restaurant.Core.Entities;
using Restaurant.Core.Interfaces;
using Restaurant.Infrastructure.Repositories;
using Restaurant.Web.ViewModels;

namespace Restaurant.Web.Controllers
{
	[RoutePrefix("Login")]
    public class AccountController : Controller
	{
		private readonly IUserRepository _userRepository;

		public AccountController()
			:this(new UserRepository())
		{
			
		}

		public AccountController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

        // GET: Account
		[Route("")]
		[HttpGet]
		[AllowAnonymous]
		public ActionResult Index()
        {
            return View();
        }

	    [HttpPost]
	    [AllowAnonymous]
	    [ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(AccountViewModels model)
	    {
			var user = await _userRepository.GetLoginUserAsync(model.Username, model.Password);

		    if (user == null)
		    {
			    ModelState.AddModelError(string.Empty, "کاربر با مدارک ارائه شده موجود نیست.");
			    //ModelState.AddModelError(string.Empty, "The user with supplied credentials does not exist.");
		    }

		    var authManager = HttpContext.GetOwinContext().Authentication;

		    var userIdentity = await _userRepository.CreateIdentityAsync(user);

		    authManager.SignIn(new AuthenticationProperties(), userIdentity);


		    return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		[Route("logout")]
		public async Task<ActionResult> Logout()
		{
			var authManager = HttpContext.GetOwinContext().Authentication;

			authManager.SignOut();

			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
	    [AllowAnonymous]
	    [ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(AccountViewModels model)
	    {
			if (!ModelState.IsValid)
		    {
			    return View(viewName:"Index");
		    } 

		    var users = await _userRepository.GetAllUsersAsync();
		    var user = users.SingleOrDefault(u => u.UserName == model.Username || u.Email == model.Email);

		    if (user != null)
		    {
			    ModelState.AddModelError(string.Empty, "کاربر با مدارک ارائه شده موجود است.");

			    //ModelState.AddModelError(string.Empty, "The user with supplied credentials does exist.");
		    }

		    //var completed = await _users.CreateAsync(model);

		    //   if (completed)
		    //   {
		    //    return RedirectToAction("Login", "Profile");
		    //   }
		    var newUser = new UserIdentity()
		    {
			    Email = model.Email,
			    UserName = model.Username,
		    };

		    await _userRepository.CreateAsync(newUser, model.Password);

		    await _userRepository.AddUserToRoleAsync(newUser, "user");

		    return RedirectToAction("Index", "Account");
		}


		#region Method

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

		private async Task<UserIdentity> GetloggedInUser()
		{
			return await _userRepository.GetUserByNameAsync(User.Identity.Name);
		}

		#endregion
	}
}