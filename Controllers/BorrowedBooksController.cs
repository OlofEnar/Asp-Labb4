using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Asp_Labb4.Data;
using Asp_Labb4.Models;

namespace Asp_Labb4.Controllers
{
    public class BorrowedBooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BorrowedBooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BorrowedBooks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BorrowedBooks.Include(b => b.Book).Include(b => b.Customer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BorrowedBooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowedBook = await _context.BorrowedBooks
                .Include(b => b.Book)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(m => m.BorrowedBookId == id);
            if (borrowedBook == null)
            {
                return NotFound();
            }

            return View(borrowedBook);
        }

        // GET: BorrowedBooks/Borrow
        public async Task<IActionResult> Borrow(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookToBorrow = await _context.Books.FindAsync(id);
            if (bookToBorrow == null)
            {
                return NotFound();
            }

            ViewData["FkBookId"] = new SelectList(_context.Books, "BookId", "Title", bookToBorrow.BookId);
            ViewData["FkCustomerId"] = new SelectList(_context.Customers, "CustomerId", "FirstName");
            return View();
        }

        // POST: BorrowedBooks/Borrow
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Borrow([Bind("BorrowedBookId,FkCustomerId,FkBookId")] BorrowedBook borrowedBook)
        {
            if (ModelState.IsValid)
            {
                var book = await _context.Books.FindAsync(borrowedBook.FkBookId);
                if (book != null)
                {
                    book.IsAvailable = false; // Update availability when book is borrowed
                    _context.Update(book);
                }

                _context.Add(borrowedBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkBookId"] = new SelectList(_context.Books, "BookId", "Title", borrowedBook.FkBookId);
            ViewData["FkCustomerId"] = new SelectList(_context.Customers, "CustomerId", "FirstName", borrowedBook.FkCustomerId);
            return View(borrowedBook);
        }

        // GET: BorrowedBooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowedBook = await _context.BorrowedBooks.FindAsync(id);
            if (borrowedBook == null)
            {
                return NotFound();
            }
            ViewData["FkBookId"] = new SelectList(_context.Books, "BookId", "Title", borrowedBook.FkBookId);
            ViewData["FkCustomerId"] = new SelectList(_context.Customers, "CustomerId", "FirstName", borrowedBook.FkCustomerId);
            return View(borrowedBook);
        }

        // POST: BorrowedBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BorrowedBookId,BorrowDate,ReturnDate,FkCustomerId,FkBookId")] BorrowedBook borrowedBook)
        {
            if (id != borrowedBook.BorrowedBookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(borrowedBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowedBookExists(borrowedBook.BorrowedBookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkBookId"] = new SelectList(_context.Books, "BookId", "Title", borrowedBook.FkBookId);
            ViewData["FkCustomerId"] = new SelectList(_context.Customers, "CustomerId", "FirstName", borrowedBook.FkCustomerId);
            return View(borrowedBook);
        }

        // GET: BorrowedBooks/Return/5
        public async Task<IActionResult> Return(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowedBook = await _context.BorrowedBooks
                .Include(b => b.Book)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(m => m.BorrowedBookId == id);
            if (borrowedBook == null)
            {
                return NotFound();
            }

            return View(borrowedBook);
        }

        // POST: BorrowedBooks/Return/5
        [HttpPost, ActionName("Return")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnConfirmed(int id)
        {
            var borrowedBook = await _context.BorrowedBooks
                .Include(b => b.Book)
                .FirstOrDefaultAsync(m => m.BorrowedBookId == id);

            if (borrowedBook != null)
            {
                borrowedBook.Book.IsAvailable = true;
                _context.BorrowedBooks.Remove(borrowedBook);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: BorrowedBooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowedBook = await _context.BorrowedBooks
                .Include(b => b.Book)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(m => m.BorrowedBookId == id);
            if (borrowedBook == null)
            {
                return NotFound();
            }

            return View(borrowedBook);
        }

        // POST: BorrowedBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var borrowedBook = await _context.BorrowedBooks
                .Include(b => b.Book)
                .FirstOrDefaultAsync(m => m.BorrowedBookId == id);

            if (borrowedBook != null)
            {
                borrowedBook.Book.IsAvailable = true;
                _context.BorrowedBooks.Remove(borrowedBook);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowedBookExists(int id)
        {
            return _context.BorrowedBooks.Any(e => e.BorrowedBookId == id);
        }
    }
}
