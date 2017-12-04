using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Restaurant.Web.Areas.Admin.ViewModels
{
	public class LoginViewModel
	{
		[Required]
		public string Username { get; set; }
		[Required]
		[MinLength(6, ErrorMessage = "Min length 6 char")]
		[DataType(DataType.Password)]
		[RegularExpression(@"^.*(?=.*[!@#$%^&*\(\)_\-+=]).*$", ErrorMessage = "Password not simple")]
		//data-val-regex-pattern="([a-zA-Z0-9&#32;.&amp;amp;&amp;#39;-]+)"      <-- MVC 4/Beta
		public string Password { get; set; }
	}
}