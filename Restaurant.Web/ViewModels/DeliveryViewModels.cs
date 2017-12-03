using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Restaurant.Web.ViewModels
{
	public class DeliveryViewModels
	{
		[Required]
		public string Streets { get; set; }
		[Required]
		public string Aparteman { get; set; }
		[Required]
		public string City { get; set; }
		[Required]
		public string State { get; set; }
		[Required]
		public string ZipCode { get; set; }
	}
}