using Cinema.Domain.Db;
using Cinema.Domain.Models;
using Cinema.Domain.Models.Common;
using Cinema.Domain.Models.Film;
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
        public IActionResult Countries(long id)
        {
            CountryViewModel defaultViewModel = new ();

            if (id != 0)
            {
                defaultViewModel = _context.Countries.Where(x => x.Kod == id).Select(x => new CountryViewModel()
                {
                    Id = x.Kod,
                    Name = x.Name,
                }).First();
            }

            var countries = _context.Countries.Select(x => new CountryViewModel 
            {
                Id = x.Kod,
                Name = x.Name
            }).OrderBy(x => x.Name).AsEnumerable();
            
            ViewData["TableName"] = "Страны";
            ViewData["Headers"] = new string[] { "", "Название" };
            ViewData["TableData"] = countries;
            return View(defaultViewModel);
        }
        [HttpPost]
        public IActionResult AddOrUpdateCountries(CountryViewModel country)
        {
            if (country.Id != 0) 
            {
                var editingCountry = _context.Countries.First(x => x.Kod == country.Id);
                editingCountry.Name = country.Name;
            }
            else
            {
                _context.Countries.Add(new Country()
                {
                    Name = country.Name
                });
            }
            
            _context.SaveChanges();
            return RedirectToAction("Countries", "Dictionary");
        }
        [HttpPost]
        public IActionResult DeleteCounty(long id)
        {
            _context.Countries.Remove(_context.Countries.First(x => x.Kod == id));
            _context.SaveChanges();
            return RedirectToAction("Countries", "Dictionary");
        }

        [HttpGet]
        public IActionResult EmployeeTypes(long id)
        {
            TypeEmployeeViewModel defaultViewModel = new();
            if (id != 0)
            {
                defaultViewModel = _context.EmployeeTypes.Where(x => x.Kod == id).
                    Select(x => new TypeEmployeeViewModel()
                    {
                        Id = x.Kod,
                        Name = x.Name
                    }).First();
            }

            var employeeTypes = _context.EmployeeTypes.Select(x => new TypeEmployeeViewModel() 
            {
                Id = x.Kod,
                Name = x.Name
            }).OrderBy(x => x.Name).ToArray();
            ViewData["TableName"] = "Должности";
            ViewData["Headers"] = new string[] { "", "Название" };
            ViewData["TableData"] = employeeTypes;

            return View(defaultViewModel);
        }
        [HttpPost]
        public IActionResult AddOrUpdateEmployeeTypes(TypeEmployeeViewModel employeeType)
        {
            if (employeeType.Id == 0)
            {
                _context.EmployeeTypes.Add(new TypeEmployee()
                {
                    Name = employeeType.Name
                });
            } else
            {
                var employee = _context.EmployeeTypes.First(x => x.Kod == employeeType.Id);
                employee.Name = employeeType.Name;
            }
            
            _context.SaveChanges();
            return RedirectToAction("EmployeeTypes", "Dictionary");
        }
        [HttpPost]
        public IActionResult DeleteEmployeeType(long id)
        {
            _context.EmployeeTypes.Remove(_context.EmployeeTypes.First(x => x.Kod == id));
            _context.SaveChanges();
            return RedirectToAction("EmployeeTypes", "Dictionary");
        }

        [HttpGet]
        public IActionResult Genre(long id)
        {
            GenreViewModel defaultViewModel = new ();
            if (id != 0)
            {
                defaultViewModel = _context.Genres.Where(x => x.Kod == id).Select(x => new GenreViewModel()
                {
                    Id = x.Kod,
                    Name = x.Name
                }).First();
            }
            var genres = _context.Genres.Select(x => new GenreViewModel 
            {
                Id = x.Kod,
                Name = x.Name
            }).OrderBy(x => x.Name).AsEnumerable();
            ViewData["TableName"] = "Жанры";
            ViewData["Headers"] = new string[] { "", "Название" };
            ViewData["TableData"] = genres;
            return View(defaultViewModel);
        }
        [HttpPost]
        public IActionResult AddOrUpdateGenre(GenreViewModel genre)
        {
            if (genre.Id != 0)
            {
                _context.Genres.First(x => x.Kod == genre.Id).Name = genre.Name;
            }
            {
                _context.Genres.Add(new Genre()
                {
                    Name = genre.Name
                });
            }
            _context.SaveChanges();
            return RedirectToAction("Genre", "Dictionary");
        }
        [HttpPost]
        public IActionResult DeleteGenre(long id)
        {
            _context.Genres.Remove(_context.Genres.First(x => x.Kod == id));
            _context.SaveChanges();
            return RedirectToAction("Genre", "Dictionary");
        }

        [HttpGet]
        public IActionResult Rating(long id)
        {
            RatingViewModel defaultViewModel = new();

            if (id != 0)
            {
                defaultViewModel = _context.Ratings.Where(x => x.Kod == id).Select(x => new RatingViewModel()
                {
                    Id = x.Kod,
                    Name = x.Name
                }).First();
            }

            var ratings = _context.Ratings.Select(x => new RatingViewModel
            {
                Id = x.Kod,
                Name = x.Name
            }).OrderBy(x => x.Name).AsEnumerable();
            ViewData["TableName"] = "Рейтинг";
            ViewData["Headers"] = new string[] { "", "Название" };
            ViewData["TableData"] = ratings;
            return View(defaultViewModel);
        }
        [HttpPost]
        public IActionResult AddRating(RatingViewModel rating)
        {
            var genres = _context.Ratings.Add(new Rating()
            {
                Name = rating.Name
            });
            _context.SaveChanges();
            return RedirectToAction("Rating", "Dictionary");
        }
        [HttpPost]
        public IActionResult DeleteRating(long id)
        {
            _context.Ratings.Remove(_context.Ratings.First(x => x.Kod == id));
            _context.SaveChanges();
            return RedirectToAction("Rating", "Dictionary");
        }

        [HttpGet]
        public IActionResult FilmStudio(long id)
        {

            FilmStudioViewModel defaultViewModel = new();

            if (id != 0)
            {
                defaultViewModel = _context.FilmStudios.Where(x => x.Kod == id).Select(x => new FilmStudioViewModel()
                {
                    Id = x.Kod,
                    Name = x.Name
                }).First();
            }

            var filmStudios = _context.FilmStudios.Select(x => new FilmStudioViewModel 
            {
                Id = x.Kod,
                Name = x.Name
            }).OrderBy(x => x.Name).AsEnumerable();
            ViewData["TableName"] = "Студии";
            ViewData["Headers"] = new string[] { "", "Название" };
            ViewData["TableData"] = filmStudios;
            return View(defaultViewModel);
        }
        [HttpPost]
        public IActionResult AddFilmStudio(FilmStudioViewModel filmStudio)
        {
            _context.FilmStudios.Add(new FilmStudio()
            {
                Name = filmStudio.Name
            });
            _context.SaveChanges();
            return RedirectToAction("FilmStudio", "Dictionary");
        }
        [HttpPost]
        public IActionResult DeleteFilmStudio(long id)
        {
            _context.FilmStudios.Remove(_context.FilmStudios.First(x => x.Kod == id));
            _context.SaveChanges();
            return RedirectToAction("FilmStudio", "Dictionary");
        }

    }
}
