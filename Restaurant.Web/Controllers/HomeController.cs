using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Restaurant.Core.Interfaces;
using Restaurant.Infrastructure.Repositories;
using Restaurant.Web.Areas.Admin.Services;
using Restaurant.Web.Services;
using Restaurant.Web.ViewModels;

namespace Restaurant.Web.Controllers
{
	[RoutePrefix("")]
    public class HomeController : Controller
    {
	    private readonly IUserRepository _userRepository;
	    private readonly IProductRepository _productRepository;
	    private readonly IProductCategoryRepostitory _categoryRepostitory;
		private readonly UserService _users;


	    public HomeController()
		    : this(new UserRepository(), new ProductRepository(), new ProductCategoryRepostitory())
	    {

	    }

	    public HomeController(IUserRepository repository, IProductRepository productRepository, IProductCategoryRepostitory categoryRepostitory)
	    {
		    _userRepository = repository;
		    _productRepository = productRepository;
		    _categoryRepostitory = categoryRepostitory;
		}

		// GET: Home
		[Route("")]
		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult> Index()
		{
			var products = await _productRepository.GetAllAsync();
			var categories = await _categoryRepostitory.GetAllAsync();

			var cart = new ShoppingCart(HttpContext);

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
				Products = products,
				CartItems = carts,
				Categories = categories,
			};

			return View(viewName:"Index",model:model);
        }

	    
	}
}