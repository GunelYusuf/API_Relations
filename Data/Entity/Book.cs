using System;
using System.Collections.Generic;

namespace Web_API_Relations.Data.Entity
{
    public class Book:BaseEntity
    {
        public string Name { get; set; }

        public Author Author { get; set; }

        public int AuthorId { get; set; }

        public List<BookGenre> BookGenres { get; set; }
    }
}
