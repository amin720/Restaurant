using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Restaurant.Core.Entities;
using Restaurant.Core.Interfaces;
using Restaurant.Infrastructure.Repositories;
using Restaurant.Web.Services;
using Restaurant.Web.ViewModels;

namespace Restaurant.Web.Controllers
{
	[RoutePrefix("")]
	[Authorize(Roles = "user")]
	public class ShoppingCartController : Controller
	{
		private readonly IProductRepository _productRepository;
		private readonly IUserRepository _userRepository;

		public ShoppingCartController()
			: this(new ProductRepository(), new UserRepository())
		{

		}

		public ShoppingCartController(IProductRepository productRepository, IUserRepository userRepository)
		{
			_productRepository = productRepository;
			_userRepository = userRepository;
		}

		// GET: User/ShoppingCart
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}

		[HttpGet]
		//[Route("AddToCart/{nameProduct}")]
		//public async Task<ActionResult> AddToCart(string nameProduct)
		public async Task<JsonResult> AddToCart(string nameProduct)
		{
			var products = await _productRepository.GetAllAsync();
			var product = products.SingleOrDefault(p => p.Name == nameProduct);
			var cart = new ShoppingCart(HttpContext);


			await cart.AddAsync(product.Id);



			var carts = await cart.GetCartItemsAsync();

			if (carts == null)
			{
				await cart.AddAsync(11);
			}
			else
			{
				await cart.RemoveAsync(11);
			}

			var model = new ProductsViewModel()
			{
				Price = product.Price,
				Name = product.Name,
				//CartItems = carts,
				//Discount = product.Discount,
				//Subtotal = CalcuateCartSubtotal(carts),
				//Total = CalcuateCartWithDiscount(carts),
			};

			//return Json(RedirectToAction("Dashboard","Profile"));
			return Json(model, JsonRequestBehavior.AllowGet);
			//return RedirectToAction("Dashboard", "Profile");
		}

		[HttpGet]
		//[Route("RemoveFromCart/{nameProduct}")]
		//public async Task<ActionResult> RemoveFromCart(string nameProduct)
		public async Task<JsonResult> RemoveFromCart(string nameProduct)
		{
			var products = await _productRepository.GetAllAsync();
			var product = products.SingleOrDefault(p => p.Name == nameProduct);



			var cart = new ShoppingCart(HttpContext);

			await cart.RemoveAsync(product.Id);

			var model = new ProductsViewModel()
			{
				Price = product.Price,
				Name = product.Name,
				//CartItems = await cart.GetCartItemsAsync(),
			};

			if (product.SKU == "Domains")
			{
				await _productRepository.DeleteAsync(product.Id);
			}

			//return Json(RedirectToAction("Dashboard", "Profile"),JsonRequestBehavior.DenyGet);
			return Json(model, JsonRequestBehavior.AllowGet);
			//return RedirectToAction("Dashboard", "Profile");

		}
		[HttpGet]
		[Route("Checkout")]
		public async Task<ActionResult> Checkout()

		{
			var user = await GetloggedInUser();
			var model = new CheckoutViewModel
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Address = user.Address,
				Email = user.Email,
				City = "Tehran",
				State = "Tehran",
				Country = "Iran",
				PostalCode = user.PostalCode,
				CardNumber = "4111111111111111",
				Cvv = "124",
				Month = "07",
				Year = "2020"
			};

			return View(model: model, viewName: "Checkout");
		}
		[HttpPost]
		[Route("Checkout")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Checkout(CheckoutViewModel model)
		{
			var user = await GetloggedInUser();
			model.FirstName = user.FirstName;
			model.LastName = user.LastName;
			model.Address = user.Address;
			model.Email = user.Email;
			model.City = "Tehran";
			model.State = "Tehran";
			model.Country = "Iran";
			model.PostalCode = user.PostalCode;
			model.CardNumber = "4111111111111111";
			model.Cvv = "124";
			model.Month = "07";
			model.Year = "2020";


			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var cart = new ShoppingCart(HttpContext);

			var result = await cart.CheckoutAsync(model);

			if (result.Succeeded)
			{
				TempData["transactionId"] = result.TransactionId;
				return RedirectToAction("Complete");
			}
			ModelState.AddModelError(string.Empty, result.Message);

			return View(model);
		}

		[HttpGet]
		[Route("Complete")]
		public async Task<ActionResult> Complete()
		{
			var cart = new ShoppingCart(HttpContext);

			var result = await cart.GetCartItemsAsync();

			ViewBag.TransactionId = (string)TempData["transactionId"];

			var user = await GetloggedInUser();
			var model = new CheckoutViewModel()
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Address = user.Address,
				PostalCode = user.PostalCode,
				Phone = user.PhoneNumber,
				CartItems = result,
				Subtotal = CalcuateCartSubtotal(result),
				Total = CalcuateCartWithDiscount(result)
			};

			return View(model: model, viewName: "Complete");
		}

		private static decimal CalcuateCartSubtotal(IEnumerable<CartItem> items)
		{
			var total = 0m;

			foreach (var item in items)
			{
				total += (item.Product.Price * item.Count);
			}

			return total;
		}
		private static decimal CalcuateCartWithDiscount(IEnumerable<CartItem> items)
		{
			var total = 0m;

			foreach (var item in items)
			{
				if (item.Product.Discount == null)
				{
					item.Product.Discount = 0;
				}

				total += ((item.Product.Price - (decimal)item.Product.Discount) * item.Count);
			}

			return total;
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