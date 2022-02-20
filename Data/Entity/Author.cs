using System;
using System.Collections.Generic;

namespace Web_API_Relations.Data.Entity
{
    public class Author:BaseEntity
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public List<Book> Books { get; set; }


    }
}
