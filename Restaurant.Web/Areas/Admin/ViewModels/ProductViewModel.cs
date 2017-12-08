using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Restaurant.Core.Entities;

namespace Restaurant.Web.Areas.Admin.ViewModels
{
	public class ProductViewModel
	{
		public string Name { get; set; }
		[AllowHtml]
		public string Content { get; set; }
		public int CategoryId { get; set; }
		public bool CategoryName { get; set; }
		public string SKU { get; set; }
		public decimal Price { get; set; }
		public decimal? Discount { get; set; }
		public DateTime? FromDateDiscount { get; set; }
		public DateTime? ToDateDiscount { get; set; }
		public int Count { get; set; }
		public string AuthorId { get; set; }
		public string ImageUrl { get; set; }
		public IEnumerable<Category> Categories { get; set; }
	}
}