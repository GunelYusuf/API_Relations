using System;
namespace Web_API_Relations.Data.Entity
{
    public class BookRelateds
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public int GenreId { get; set; }

        public int AuthorId { get; set; }

    }
}
