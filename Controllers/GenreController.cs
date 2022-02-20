using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API_Relations.Data.DAL;
using Web_API_Relations.Data.Entity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web_API_Relations.Controllers
{
    [Route("api/[controller]")]
    public class GenreController : Controller
    {
        private readonly Context _context;

        public GenreController(Context context)
        {
            _context = context;
        }

        // GET: api/values
        [HttpGet]
        public List<Genre> Get()
        {
            return _context.Genres.Include(g => g.BookGenres).ThenInclude(g => g.Book).ToList();

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post(Genre genre, int[] book)
        {
            if (book != null)
            {
                Genre newGenre = new Genre();
                newGenre.Name = genre.Name;
                _context.Genres.Add(newGenre);
                _context.SaveChanges();

                foreach (var item in book)
                {
                    BookGenre bookGenre = new BookGenre();
                    bookGenre.GenreId = newGenre.Id;
                    bookGenre.BookId = item;
                    _context.Add(bookGenre);
                    _context.SaveChanges();
                }
            }
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int? id, Genre genre, List<int> book)
        {
            bool isExist = _context.Genres.Any(c => c.Name.ToLower() == genre.Name.ToLower().Trim());
            Genre newGenre = _context.Genres.Find(id);


            List<int> checkedBook = _context.BookGenres.Where(g => g.GenreId == newGenre.Id).Select(b => b.BookId).ToList();

            List<int> addedBook = book.Except(checkedBook).ToList();
            List<int> removedBook = checkedBook.Except(book).ToList();

            int addedBookLength = addedBook.Count();
            int removedBookLength = removedBook.Count();
            int FullLength = addedBookLength + removedBookLength;

            newGenre.Name = genre.Name;

            for (int i = 1; i <= FullLength; i++)
            {
                if (addedBookLength >= i)
                {
                    BookGenre bookGenre = new BookGenre();
                    bookGenre.GenreId = newGenre.Id;
                    bookGenre.BookId = addedBook[i - 1];
                    _context.BookGenres.Add(bookGenre);
                    _context.SaveChanges();
                }

                if (removedBookLength >= i)
                {
                    BookGenre bookGenre = _context.BookGenres.FirstOrDefault(b => b.BookId == removedBook[i - 1] && b.GenreId == newGenre.Id);
                    _context.BookGenres.Remove(bookGenre);
                    _context.SaveChanges();
                }
            }

            _context.SaveChanges();
            return StatusCode(200);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Genre genre)
        {
            Genre _genre =  _context.Genres.FirstOrDefault(g => g.Id == genre.Id);

            if (genre == null) return NotFound();

            List<BookGenre> bookGenres = _context.BookGenres.ToList();
            foreach (var item in bookGenres)
            {
                BookGenre deleteGenre = _context.BookGenres.FirstOrDefault(g => g.BookId == genre.Id);
                if (deleteGenre != null)
                {
                    _context.BookGenres.Remove(deleteGenre);
                    _context.SaveChanges();
                }

            }
            _context.Genres.Remove(_genre);
            _context.SaveChanges();

            return StatusCode(202);

        }
    }
}
