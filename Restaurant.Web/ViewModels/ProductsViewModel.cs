using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Restaurant.Core.Entities;

namespace Restaurant.Web.ViewModels
{
	public class ProductsViewModel
	{
		public string Name { get; set; }
		public string Desctription { get; set; }
		public string CategoryName { get; set; }
		public decimal Price { get; set; }
		public UInt16 Count { get; set; }

		public UInt16 DeliveryCount { get; set; }
		public decimal TotalPrice { get; set; }

		public IEnumerable<Product> Products { get; set; }
	}
}