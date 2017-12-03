using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Restaurant.Core.Entities;

namespace Restaurant.Infrastructure.Database
{
	public class ContextDb : IdentityDbContext<UserIdentity>
	{
		public ContextDb()
			: base("name=Restaurant")
		{

		}
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
