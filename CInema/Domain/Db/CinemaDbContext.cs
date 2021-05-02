using Cinema.Domain.Models;
using Cinema.Domain.Models.Common;
using Cinema.Domain.Models.Film;
using Cinema.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Domain.Db
{
    public class CinemaDbContext : DbContext
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options)
        {

        }

        public DbSet<BoxOffice> BoxOffices { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<FilmSemp> FilmSemps { get; set; }
        public DbSet<FilmStudio> FilmStudios { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<SessionFilms> SessionFilms { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
