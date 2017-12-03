using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Core.Entities;

namespace Restaurant.Core.Interfaces
{
	public interface IProductRepository
	{
		Task<Product> GetByIdAsync(int id);
		Task<IEnumerable<Product>> GetAllByCategoryAsync(string category);
		Task<IEnumerable<Product>> GetAllAsync();
		Task CreateAsync(Product model);
		Task EditAsync(int id, Product updateItem);
		Task DeleteAsync(int id);
		Task<IEnumerable<Product>> GetPageAsync(int pageNumber, int pageSize);
		Task<IEnumerable<Product>> GetProductsByAuthorAsync(string authorId);
		Task<IEnumerable<Product>> GetPublishedProductsAsync();

	}
}
