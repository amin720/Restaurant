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
		public string FullName { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email is is not valid.")]
		public string Email { get; set; }
		[Required]
		[MinLength(6, ErrorMessage = "Min length 6 char")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The new password and confirmation password do not match")]
		public string ConfrimedPassword { get; set; }
		[Required]
		[StringLength(11,ErrorMessage = "PLz Correct Phone Number"),MinLength(11, ErrorMessage = "PLz Correct Phone Number")]
		[DataType(DataType.PhoneNumber)]
		public string Mobile { get; set; }	
	}
}