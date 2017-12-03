using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Core.Entities;

namespace Restaurant.Core.Interfaces
{
	public interface IProductCategoryRepostitory
	{
		Task<Category> GetAsync(int id);
		Task<IEnumerable<Category>> GetAllAsync();
		Task CreateAsync(Category model);
		Task EditAsync(int id, Category updateItem);
		Task DeleteAsync(int id);
	}
}
