using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            return _context.Books.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id == null) return NotFound();
            Book dbBook = _context.Books.FirstOrDefault(b => b.Id == id);
            if(dbBook==null) return StatusCode(404);

            return StatusCode(200, dbBook);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Create(Book book)
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
            return StatusCode(201);
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public IActionResult Update(int id,)
        //{
        //}

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
