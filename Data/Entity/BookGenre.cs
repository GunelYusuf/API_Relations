using System;
namespace Web_API_Relations.Data.Entity
{
    public class BookGenre:BaseEntity
    {
        public Genre Genre { get; set; }

        public int GenreId{ get; set; }

        public Book Book { get; set; }

        public int BookId { get; set; }
    }
}
