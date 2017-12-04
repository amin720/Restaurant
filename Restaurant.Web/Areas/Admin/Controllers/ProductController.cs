using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Restaurant.Core.Entities;
using Restaurant.Core.Interfaces;
using Restaurant.Infrastructure.Repositories;
using Restaurant.Web.Areas.Admin.ViewModels;

namespace Restaurant.Web.Areas.Admin.Controllers
{
	// /admin/product

	[RouteArea("Admin")]
	[RoutePrefix("Product")]
	[Authorize]
	public class ProductController : Controller
	{
		private readonly IProductRepository _productRepository;
		private readonly IUserRepository _usersRepository;
		private readonly IProductCategoryRepostitory _categoryRepostitory;

		public ProductController()
			: this(new ProductRepository(), new UserRepository(), new ProductCategoryRepostitory())
		{
		}

		public ProductController(IProductRepository repository, IUserRepository userRepository, IProductCategoryRepostitory productCategoryRepostitory)
		{
			_productRepository = repository;
			_usersRepository = userRepository;
			_categoryRepostitory = productCategoryRepostitory;
		}

		// GET: Admin/Product
		[Route("")]
		public async Task<ActionResult> Index()
		{
			if (!User.IsInRole("author"))
			{
				return View(await _productRepository.GetAllAsync());
			}

			var user = await GetloggedInUser();
			var products = await _productRepository.GetProductsByAuthorAsync(user.Id);

			return View(products);
		}

		// /admin/Product/create
		[HttpGet]
		[Route("Create")]
		public async Task<ActionResult> Create()
		{
			var model = new ProductViewModel()
			{
				Categories = await _categoryRepostitory.GetAllAsync(),
			};


			return View(model: model, viewName: "Create");

		}


		// /admin/Product/create
		[HttpPost]
		[Route("Create")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(ProductViewModel model)
		{
			try
			{
				var product = new Product();
				var categories = await _categoryRepostitory.GetAllAsync();
				model.Categories = categories;

				if (!ModelState.IsValid)
				{
					return View(model);
				}

				var user = await GetloggedInUser();

				if (string.IsNullOrWhiteSpace(model.Name))
				{
					ModelState.AddModelError(string.Empty, "یک نام مناسب انتخاب کنید");
				}

				if (string.IsNullOrWhiteSpace(model.Price.ToString(CultureInfo.InvariantCulture)))
				{
					ModelState.AddModelError(string.Empty, "یک قیمت مناسب انتخاب کنید");
				}

				if (string.IsNullOrWhiteSpace(model.SKU))
				{
					ModelState.AddModelError(string.Empty, "یک کد محصول مناسب انتخاب کنید");
				}

				var category = categories.SingleOrDefault(cat => cat.Id == model.CategoryId);
				//var category = _categoryRepostitory.GetAsync(model.CategoryId);

				product.Name = model.Name;
				product.Price = model.Price;
				product.ImageUrl = null;
				product.SKU = model.SKU;
				product.Count = model.Count;
				product.Discount = model.Discount;
				product.Content = model.Content;
				product.FromDateDiscount = model.FromDateDiscount;
				product.ToDateDiscount = model.ToDateDiscount;
				product.AuthorId = user.Id;
				product.Published = DateTime.Now;
				product.CategoryId = category.Id;


				// TODO: update model in data store
				await _productRepository.CreateAsync(product);

				return RedirectToAction("Index");
			}
			catch (Exception e)
			{
				ModelState.AddModelError(string.Empty, e.Message);
				return View(model: model, viewName: "Create");
			}

		}

		// /admin/Product/edit/product-to-edit
		[HttpGet]
		[Route("Edit/{productId}")]
		public async Task<ActionResult> Edit(int productId)
		{

			// TODO: to retrieve the model from the data store
			var product = await _productRepository.GetByIdAsync(productId);
			var model = new ProductViewModel();

			if (product == null)
			{
				return HttpNotFound();
			}

			var category = await _categoryRepostitory.GetAsync(product.CategoryId);

			model.Name = product.Name;
			model.Content = product.Content;
			model.Discount = product.Discount;
			model.Price = product.Price;
			model.CategoryId = product.CategoryId;
			model.CategoryName = true;
			model.AuthorId = product.AuthorId;
			model.FromDateDiscount = product.FromDateDiscount;
			model.ToDateDiscount = product.ToDateDiscount;
			model.SKU = product.SKU;
			model.Count = product.Count;


			model.Categories = await _categoryRepostitory.GetAllAsync();

			if (User.IsInRole("author"))
			{
				var user = await GetloggedInUser();

				if (product.AuthorId != user.Id)
				{
					return new HttpUnauthorizedResult();
				}
			}

			return View(model: model);
		}

		// /admin/Product/edit/product-to-edit
		[HttpPost]
		[Route("Edit/{postId}")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(int productId, ProductViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			if (User.IsInRole("author"))
			{
				var user = await GetloggedInUser();
				var product = await _productRepository.GetByIdAsync(productId);
				try
				{
					if (product.AuthorId != user.Id)
					{
						return new HttpUnauthorizedResult();
					}
				}
				catch (Exception)
				{

					throw new ArgumentException("کاربر دسترسی ندارد.");
				}

			}

			if (string.IsNullOrWhiteSpace(model.Name))
			{
				ModelState.AddModelError("", "یک نام مناسب انتخاب کنید");
			}

			if (string.IsNullOrWhiteSpace(model.Price.ToString(CultureInfo.InvariantCulture)))
			{
				ModelState.AddModelError("", "یک قیمت مناسب انتخاب کنید");
			}

			if (string.IsNullOrWhiteSpace(model.SKU))
			{
				ModelState.AddModelError("key", "یک کد محصول مناسب انتخاب کنید");
			}

			try
			{
				var product = new Product
				{
					Name = model.Name,
					Content = model.Content,
					CategoryId = model.CategoryId,
					Discount = model.Discount,
					FromDateDiscount = model.FromDateDiscount,
					ToDateDiscount = model.ToDateDiscount,
					Price = model.Price,
					Count = model.Count,
					SKU = model.SKU,

				};

				// TODO: update model in data store
				await _productRepository.EditAsync(productId, product);

				return RedirectToAction("Index");
			}
			catch (KeyNotFoundException e)
			{
				return HttpNotFound();
			}
			catch (Exception e)
			{
				ModelState.AddModelError(string.Empty, e.Message);
				return View(model);
			}
		}

		// /admin/Product/delete/product-to-delete
		[HttpGet]
		[Route("Delete/{productId}")]
		[Authorize(Roles = "admin, editor")]
		[AllowAnonymous]

		public async Task<ActionResult> Delete(int productId)
		{
			var product = await _productRepository.GetByIdAsync(productId);

			if (product == null)
			{
				return HttpNotFound();
			}

			return View(product);
		}

		// /admin/Product/delete/product-to-delete
		[HttpPost]
		[Route("Delete/{productId}")]
		[Authorize(Roles = "admin, editor")]
		[ValidateAntiForgeryToken]
		[AllowAnonymous]

		public async Task<ActionResult> Delete(int productId, string foo)
		{

			try
			{
				using (var db = new RestaurantEntities())
				{
					var cart = await db.CartItems.Where(p => p.ProductId == productId).ToListAsync();
					if (cart.Count != 0)
					{
						db.CartItems.Remove(cart.First());
					}

					await _productRepository.DeleteAsync(productId);

					return RedirectToAction("Index");
				}
			}
			catch (KeyNotFoundException e)
			{

				return HttpNotFound();
			}
		}

		#region Method

		private bool _isDisposed;
		protected override void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				_usersRepository.Dispose();
			}

			_isDisposed = true;
			base.Dispose(disposing);
		}

		private async Task<UserIdentity> GetloggedInUser()
		{
			return await _usersRepository.GetUserByNameAsync(User.Identity.Name);
		}

		private IList<Category> _categories = new List<Category>();
		private void CategoryList(Category item)
		{
			if (item.Id != null)
			{
				_categories.Add(item);

			}
		}

		#endregion
	}
}