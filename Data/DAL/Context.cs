using System;
using Microsoft.EntityFrameworkCore;
using Web_API_Relations.Data.Entity;

namespace Web_API_Relations.Data.DAL
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        public DbSet<Book> Books{ get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<BookGenre> BookGenres { get; set; }

        public DbSet<BookRelateds> BookRelateds { get; set; }

    }
}
