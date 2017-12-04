﻿using System.Collections.Generic;
using Restaurant.Core.Entities;

namespace Restaurant.Web.Areas.Admin.ViewModels
{
	public class DashboardViewModel
	{
		public int Host { get; set; }
		public int Domain { get; set; }
		public decimal Price { get; set; }
		public int Users { get; set; }
		public string UserLogin { get; set; }
		public string Role { get; set; }
		public IEnumerable<Category> Categories { get; set; }
	}
}