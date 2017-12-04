using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Restaurant.Web.ViewModels
{
	public class AccountViewModels
	{
		[Required]
		public string Username { get; set; }
		[Required]
		[MinLength(6,ErrorMessage = "Min length 6 char")]
		[DataType(DataType.Password)]
		[RegularExpression(@"^.*(?=.*[!@#$%^&*\(\)_\-+=]).*$", ErrorMessage = "Password not simple")]
		//data-val-regex-pattern="([a-zA-Z0-9&#32;.&amp;amp;&amp;#39;-]+)"      <-- MVC 4/Beta
		public string Password { get; set; }
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Required]
		[MinLength(6, ErrorMessage = "Min length 6 char")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The new password and confirmation password do not match")]
		public string ConfrimedPassword { get; set; }
	}
}