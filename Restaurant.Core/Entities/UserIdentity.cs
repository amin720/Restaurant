using Microsoft.AspNet.Identity.EntityFramework;

namespace Restaurant.Core.Entities
{
	public class UserIdentity : IdentityUser
	{
		public string DisplayName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Mobile { get; set; }
		public string PostalCode { get; set; }
		public string Address { get; set; }
	}
}
