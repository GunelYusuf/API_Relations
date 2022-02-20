using System;
using System.Collections.Generic;

namespace Web_API_Relations.Data.Entity
{
    public class Genre:BaseEntity
    {
        public string Name { get; set; }

        public List<BookGenre> BookGenres { get; set; }
    }
}
