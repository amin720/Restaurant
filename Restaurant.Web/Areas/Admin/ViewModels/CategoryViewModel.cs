using System.Collections.Generic;
using Restaurant.Core.Entities;

namespace Restaurant.Web.Areas.Admin.ViewModels
{
	public class CategoryViewModel
	{
		public string Name { get; set; }
		public int Id { get; set; }
		public int ParentId { get; set; }
		public IEnumerable<Category> Categories { get; set; }
	}
}