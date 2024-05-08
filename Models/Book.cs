using System.ComponentModel.DataAnnotations;

namespace Asp_Labb4.Models
{
	public class Book
	{
		[Key]
		public int BookId { get; set; }
		
		[Required]
		[StringLength(120)]
        public string Title { get; set; }
		
		[Required]
		[StringLength(80, MinimumLength=2)]
        [Display(Name = "First name")]
        public string AuthorFName { get; set; }
		
		[Required]
		[StringLength(80, MinimumLength=2)]
        [Display(Name = "Last name")]
        public string AuthorLName { get; set; }

        [Display(Name = "Author")]
		public string AuthorFullName { get { return AuthorFName + " " + AuthorLName; } }

        [StringLength(300)]
		public string? Summary { get; set; }
        
		[Display(Name = "Available")]
        public bool IsAvailable { get; set; } = true;
	}
}
