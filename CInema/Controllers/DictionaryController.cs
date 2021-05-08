using Cinema.Domain.Db;
using Cinema.Domain.Models.Common;
using Cinema.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Cinema.Controllers
{
    public class DictionaryController : Controller
    {
        private readonly CinemaDbContext _context;
        public DictionaryController(
            CinemaDbContext context
            )
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Countries()
        {
            var countries = _context.Countries.Select(x => new CountryViewModel 
            {
                Id = x.Kod,
                Name = x.Name
            }).ToArray();
            
            ViewData["TableName"] = "Страны";
            ViewData["Headers"] = new string[] {"Название"};
            ViewData["TableData"] = countries;
            return View();
        }
        [HttpPost]
        public IActionResult AddCountries(Country country)
        {
            var countries = _context.Countries.Add(country);
            _context.SaveChanges();
            return View();
        }

        [HttpGet]
        public IActionResult EmployeeTypes()
        {
            var employeeTypes = _context.EmployeeTypes.Select(x => new TypeEmployeeViewModel 
            {
                Id = x.Kod,
                Name = x.Name
            }).ToArray();
            ViewData["TableName"] = "Должности";
            ViewData["Headers"] = new string[] { "Название" };
            ViewData["TableData"] = employeeTypes;
            return View();
        }
        [HttpPost]
        public IActionResult AddEmployeeTypes(CountryViewModel country)
        {
            var countries = _context.Countries.Add(new Country 
            { 
                Kod = country.Id,
                Name = country.Name
            });
            _context.SaveChanges();
            return View();
        }

        [HttpGet]
        public IActionResult Genre()
        {
            var genres = _context.Genres.Select(x => new GenreViewModel 
            {
                Id = x.Kod,
                Name = x.Name
            }).ToArray();
            ViewData["TableName"] = "Жанры";
            ViewData["Headers"] = new string[] {"Название" };
            ViewData["TableData"] = genres;
            return View();
        }
        
        [HttpPost]
        public IActionResult AddGenre(Genre genre )
        {
            var genres = _context.Genres.Add(genre);
            _context.SaveChanges();
            return View();
        }

        [HttpGet]
        public IActionResult Rating()
        {
            var ratings = _context.Ratings.Select(x => new RatingViewModel
            {
                Id = x.Kod,
                Name = x.Name
            }).ToArray();
            ViewData["TableName"] = "Рейтинг";
            ViewData["Headers"] = new string[] { "Название" };
            ViewData["TableData"] = ratings;
            return View();
        }
        [HttpPost]
        public IActionResult AddRating(Rating rating)
        {
            var genres = _context.Ratings.Add(rating);
            _context.SaveChanges();
            return View();
        }
        
        [HttpGet]
        public IActionResult FilmStudio()
        {
            var filmStudios = _context.FilmStudios.Select(x => new FilmStudioViewModel 
            {
                Id = x.Kod,
                Name = x.Name
            }).ToArray();
            ViewData["TableName"] = "Студии";
            ViewData["Headers"] = new string[] { "Название" };
            ViewData["TableData"] = filmStudios;
            return View();
        }
       }
}
