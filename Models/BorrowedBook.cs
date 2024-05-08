using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asp_Labb4.Models
{
	public class BorrowedBook
	{
		public int BorrowedBookId { get; set; }

        [Display(Name = "Borrow date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BorrowDate { get; set; } = DateTime.Now;

		[Display(Name = "Return date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReturnDate { get; set; } = DateTime.Now.AddDays(30);

        [ForeignKey("Customer")]
		public int FkCustomerId { get; set; }
		public virtual Customer? Customer { get; set; }

		[ForeignKey("Book")]
		public int FkBookId { get; set; }
		public virtual Book? Book { get; set; }
	}
}
