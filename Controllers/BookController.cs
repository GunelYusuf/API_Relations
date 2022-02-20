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
    public class BookController : ControllerBase
    {

        private readonly Context _context;

        public BookController(Context context)
        {
            _context = context;
        }


        // GET: api/values
        [HttpGet]
        public List<Book> Get()
        {
            return _context.Books.Include(a => a.Author).ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id == null) return NotFound();

            var genres = _context.BookGenres
                .Where(b => b.BookId == id)
                .Select(g => g.Genre)
                .ToList();

            Book dbBook = _context.Books.FirstOrDefault(b => b.Id == id);

            if(dbBook==null) return StatusCode(404);

            return StatusCode(200, dbBook);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Create(Book book,int[] genreId)
        {
            bool isExist = _context.Books.Any(b => b.Name.ToLower().Trim() == book.Name.ToLower().Trim());
            if (isExist)
            {
                return StatusCode(401);
            }
            Book newBook = new Book
            {
                Name = book.Name,
                AuthorId = book.AuthorId,
            };
            _context.Books.Add(book);
            _context.SaveChanges();

            BookRelateds bookRelateds = new BookRelateds()
            {
                BookId = newBook.Id,
                AuthorId = newBook.AuthorId
            };
            _context.BookRelateds.Add(bookRelateds);

            if (genreId != null)
            {
                foreach (var item in genreId)
                {
                    BookGenre bookGenre = new BookGenre()
                    {
                        BookId = newBook.Id,
                        GenreId = item,
                    };
                    _context.BookGenres.AddAsync(bookGenre);
                    _context.SaveChanges();
                }
            }

           _context.SaveChanges();

            return StatusCode(201);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Update(int? id,Book book, List<int> genreId)
        {
            if (id == null) return NotFound();

            if (!ModelState.IsValid) return StatusCode(200);
            Book newBook = _context.Books.Find(id);
            var relatedBooks = _context.BookRelateds.Where(b => b.BookId == id && b.AuthorId == newBook.AuthorId).FirstOrDefault();

            newBook.Name = book.Name;
            newBook.AuthorId = book.AuthorId;

            _context.SaveChanges();

            if (relatedBooks.AuthorId != newBook.AuthorId)
            {
                _context.BookRelateds.Remove(relatedBooks);

                BookRelateds newBookRelation = new BookRelateds();
                newBookRelation.AuthorId = newBook.AuthorId;
                newBookRelation.BookId = newBook.Id;

                _context.BookRelateds.Add(newBookRelation);
               _context.SaveChanges();
            }

            List<int> checkGenre = _context.BookGenres
                .Where(b => b.BookId == newBook.Id)
                .Select(g => g.GenreId)
                .ToList();

            List<int> addedGenre = genreId.Except(checkGenre).ToList();
            List<int> removeGenre = checkGenre.Except(genreId).ToList();

            int addedGenreLength = addedGenre.Count();
            int removedGenreLength = removeGenre.Count();
            int FullLengthGenre = addedGenreLength + removedGenreLength;


            for (int i = 1; i <= FullLengthGenre; i++)
            {
                if (addedGenreLength >= i)
                {
                    BookGenre bookGenre = new BookGenre();

                    bookGenre.BookId = newBook.Id;
                    bookGenre.GenreId = addedGenre[i - 1];

                   _context.BookGenres.Add(bookGenre);
                   _context.SaveChanges();
                }

                if (removedGenreLength >= i)
                {
                    BookGenre bookGenre =  _context.BookGenres.FirstOrDefault(g => g.GenreId == removeGenre[i - 1] && g.BookId == newBook.Id);
                    _context.BookGenres.Remove(bookGenre);
                    _context.SaveChanges();
                }
            }

           _context.SaveChanges();

            return StatusCode(200);

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int? id,Book book)
        {
            var newBook =  _context.Books.Find(id);
            if (newBook == null) return NotFound();

            var bookGenre = _context.BookGenres.Where(b => b.BookId == id).ToList();
            if (bookGenre != null)
            {
                foreach (var genre in bookGenre)
                {
                    _context.BookGenres.Remove(genre);
                    _context.SaveChanges();
                }
            }

            var relatedBook = _context.BookRelateds.FirstOrDefault(b => b.BookId == id && b.AuthorId == newBook.AuthorId);
            _context.BookRelateds.Remove(relatedBook);


            _context.Books.Remove(newBook);
            _context.SaveChanges();

            return StatusCode(202);
        }
    }
}
