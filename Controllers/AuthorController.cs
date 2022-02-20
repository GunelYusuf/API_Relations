using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API_Relations.AuthorDto;
using Web_API_Relations.Data.DAL;
using Web_API_Relations.Data.Entity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web_API_Relations.Controllers
{
    [Route("[controller]")]
    //[ApiController]
    public class AuthorController : Controller
    {
        private readonly Context _context;

        public AuthorController(Context context)
        {
            _context = context;
        }

        // GET: api/values
        [HttpGet]
        public List<Author> Get()
        {
            return _context.Authors.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get([FromBody] int id)
        {
            if (id == null) return NotFound();
            Author dbAuthor = _context.Authors.FirstOrDefault(a => a.Id == id);
            if (dbAuthor == null) return StatusCode(404);

            return StatusCode(200, dbAuthor);
        }

        // POST api/values
        [HttpPost("create")]
        public IActionResult Create([FromBody]AuthorCreateDto authorCreateDto)
        {
            bool isExist = _context.Authors.Any(a => a.Name.ToLower().Trim() == authorCreateDto.Name.ToLower().Trim());
            if (isExist)
            {
                return StatusCode(401);
            }
            Author newAuthor = new Author
            {
                Name = authorCreateDto.Name,
                LastName = authorCreateDto.LastName,
                Age = authorCreateDto.Age,
            };
            _context.Authors.Add(newAuthor);
            _context.SaveChanges();
            return StatusCode(201);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]AuthorCreateDto author)
        {
            Author dbAuthor = _context.Authors.Where(a => a.Id == id).FirstOrDefault();
            if (dbAuthor == null) NotFound();
            dbAuthor.Name = author.Name??dbAuthor.Name;
            dbAuthor.LastName = author.LastName??dbAuthor.LastName;
            dbAuthor.Age = author.Age==0?dbAuthor.Age:author.Age;
            _context.SaveChanges();
            return StatusCode(200);

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            Author dbAuthor = _context.Authors.FirstOrDefault(a => a.Id == id);
            if (dbAuthor == null) return NotFound();

            _context.Authors.Remove(dbAuthor);
            _context.SaveChanges();
            return StatusCode(202);
        }
    }
}
