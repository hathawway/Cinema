using Cinema.Domain.Db;
using Cinema.Models;
using Cinema.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Cinema.Controllers
{
    public class CinemaController : Controller
    {
        private readonly CinemaDbContext _context;
        public CinemaController(
            CinemaDbContext context,
            ISignIn signInManager)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var defaultObject = new FilmViewModel();
            var cinemas = _context.Films.Select(x => new FilmViewModel
            {
                Kod = x.Kod,
                Country = _context.Countries.First(c => c.Kod == x.CountryKod).Name,
                Genre = _context.Genres.First(g => g.Kod == x.GenreKod).Name,
                Name = x.Name,
                Rating = _context.Ratings.First(r => r.Kod == x.RatingKod).Name,
                StartDate = x.StartDate,
                Studio = _context.FilmStudios.First(s => s.Kod == x.StudioKod).Name,
                TimeDuration = x.TimeDuration
            });
            ViewData["TableData"] = cinemas.ToArray();
            ViewData["Headers"] = new string[]{ 
                "",
                "Название фильма",
                "Киностудия",
                "Дата премьеры",
                "Страна",
                "Длительность",
                "Жанр",
                "Рейтинг"};
            ViewData["TableName"] = "Фильмы";

            ViewData["Genres"] = _context.Genres.Select(x => new GenreViewModel
            { 
                Id = x.Kod,
                Name = x.Name
            }
            ).AsEnumerable();
            ViewData["Ratings"] = _context.Ratings.Select(x => new RatingViewModel
            {
                Id = x.Kod,
                Name = x.Name
            }
           ).AsEnumerable();
            ViewData["Countries"] = _context.Countries.Select(x => new CountryViewModel
            {
                Id = x.Kod,
                Name = x.Name
            }
           ).AsEnumerable();
            ViewData["Studio"] = _context.FilmStudios.Select(x => new FilmStudioViewModel
            {
                Id = x.Kod,
                Name = x.Name
            }
           ).AsEnumerable();
            return View(defaultObject);
        }

        [HttpGet]
        public IActionResult BoxOffice()
        {
            var boxOffices = _context.BoxOffices.Select(x => new BoxOfficeViewModel
            {
                Film = _context.Films.First(g => g.Kod == x.KodFilm).Name,
                TotalSum = x.TotalSum,
                Date = x.DateBoxOffice,
            }).ToArray();
            ViewData["TableName"] = "Бокс Оффисы";
            ViewData["Headers"] = new string[] { "Фильм", "Сумма", "Дата" };
            ViewData["TableData"] = boxOffices;
            return View();
        }

        [HttpGet]
        public IActionResult FilmsEmp()
        {
            var filmsEmpsViewModel = _context.FilmsEmps.Select(x => new FilmsEmpViewModel
            {
                EmployeeName = _context.Employees.First(h => h.Kod == x.EmployeeKod).FIO(),
                EmployeeType = _context.EmployeeTypes.First(et => et.Kod == x.TypeEmployeeKod).Name,
                FilmName = _context.Films.First(f => f.Kod == x.FilmKod).Name
            }).ToArray();
            ViewData["TableName"] = "Семпы фильма";
            ViewData["Headers"] = new string[] { "", "Фильм", "Сотрудник", "Должность" };
            ViewData["TableData"] = filmsEmpsViewModel;
            return View();
        }

        [HttpGet]
        public IActionResult SessionFilms()
        {

            var filmSession = _context.SessionFilms.Select(x => new SessionFilmsViewModel
            {
                FilmName = _context.Films.First(f => f.Kod == x.FilmKod).Name,
                FilmStart = x.FilmStartDate,
                FreeSeatsAmount = x.FreePlaces,
                SeatsAmount = x.Places,
                TicketPrice = x.TicketPrice,
                ZalNumber = x.Hall
            }).ToArray();
            ViewData["TableName"] = "Расписание";
            ViewData["Headers"] = new string[] { "", "Фильм", "Начало", "Номер зала", "Количество мест", "Цена за билет", "Количество свободных мест" };
            ViewData["TableData"] = filmSession;
            return View();
        }

    }
}
