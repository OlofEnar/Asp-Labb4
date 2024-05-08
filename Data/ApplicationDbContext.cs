using Asp_Labb4.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Asp_Labb4.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<BorrowedBook> BorrowedBooks { get; set; }


	}
}
