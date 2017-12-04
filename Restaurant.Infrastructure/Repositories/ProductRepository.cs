using Restaurant.Core.Entities;
using Restaurant.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Infrastructure.Repositories
{
	public class ProductRepository : IProductRepository
	{
		public async Task<Product> GetByIdAsync(int id)
		{
			using (var db = new RestaurantEntities())
			{
				var product = await db.Products.Include(cat => cat.Category)
											   .SingleOrDefaultAsync(p => p.Id == id);

				if (product == null)
				{
					throw new KeyNotFoundException("The Product Id \"" + id + "\" does not exist.");
				}

				return product;
			}
		}

		public async Task<IEnumerable<Product>> GetAllByCategoryAsync(string category)
		{
			using (var db = new RestaurantEntities())
			{
				var product = await db.Products.Include(cat => cat.Category)
											   .ToListAsync();

				if (product == null)
				{
					throw new KeyNotFoundException("The Products Category \"" + category + "\" does not exist.");
				}

				return product;
			}
		}

		public async Task<IEnumerable<Product>> GetAllAsync()
		{
			using (var db = new RestaurantEntities())
			{
				return await db.Products.Include(cat => cat.Category)
										.OrderBy(p => p.Name)
										.ToListAsync();
			}
		}

		public async Task CreateAsync(Product model)
		{
			using (var db = new RestaurantEntities())
			{
				var product =
					await db.Products.SingleOrDefaultAsync(pro => pro.Name == model.Name && pro.CategoryId == model.CategoryId);

				if (product != null)
				{
					throw new ArgumentException("A product with the id of " + model.Id + " already exsits.");
				}

				db.Products.Add(model);
				db.SaveChanges();
			}
		}

		public async Task EditAsync(int id, Product updateItem)
		{
			using (var db = new RestaurantEntities())
			{
				var product = await GetByIdAsync(id);

				if (product == null)
				{
					throw new KeyNotFoundException("A product withthe id of " + id + "does not exisst in the data store");
				}

				product.Name = updateItem.Name;
				product.CategoryId = updateItem.CategoryId;
				product.Price = updateItem.Price;
				product.ImageUrl = updateItem.ImageUrl;
				product.SKU = updateItem.SKU;
				product.Count = updateItem.Count;
				product.Discount = updateItem.Discount;
				product.Content = updateItem.Content;
				product.FromDateDiscount = updateItem.FromDateDiscount;
				product.ToDateDiscount = updateItem.ToDateDiscount;
				product.AuthorId = updateItem.AuthorId;

				db.SaveChanges();
			}
		}

		public async Task DeleteAsync(int id)
		{
			using (var db = new RestaurantEntities())
			{
				//var product = await GetByIdAsync(id);
				var product = await db.Products.SingleOrDefaultAsync(p => p.Id == id);

				if (product == null)
				{
					throw new KeyNotFoundException("The product with the id of " + id + "does not exist.");
				}

				db.Products.Remove(product);
				await db.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<Product>> GetPageAsync(int pageNumber, int pageSize)
		{
			using (var db = new RestaurantEntities())
			{
				var skip = (pageNumber - 1) * pageSize;

				return await db.Products.Where(p => p.Published < DateTime.Now)
										.Include("AspNetUser")
										.OrderByDescending(p => p.Published)
										.Skip(skip)
										.Take(pageSize)
										.ToArrayAsync();
			}
		}

		public async Task<IEnumerable<Product>> GetProductsByAuthorAsync(string authorId)
		{
			using (var db = new RestaurantEntities())
			{
				return await db.Products.Include("AspNetUser")
										.Where(p => p.AuthorId == authorId)
										.OrderByDescending(post => post.Published)
										.ToArrayAsync();
			}
		}

		public async Task<IEnumerable<Product>> GetPublishedProductsAsync()
		{
			using (var db = new RestaurantEntities())
			{
				return await db.Products
										.Include("AspNetUser")
										.Where(p => p.Published < DateTime.Now)
										.OrderByDescending(p => p.Published)
										.ToArrayAsync();
			}
		}
	}
}
