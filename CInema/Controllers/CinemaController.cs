using Cinema.Domain.Db;
using Cinema.Domain.Models.Employee;
using Cinema.Domain.Models.Film;
using Cinema.Models;
using Cinema.Models.Common;
using Cinema.Service.Interfaces;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;

namespace Cinema.Controllers
{
    public class CinemaController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private readonly CinemaDbContext _context;
        public CinemaController(
            CinemaDbContext context,
            IWebHostEnvironment environment
)
        {
            this.environment = environment;
            _context = context;
        }

        #region Films
        [HttpGet]
        public IActionResult Films(long id)
        {
            FilmViewModel defaultObject = new();
            if (id != 0)
            {
                defaultObject = _context.Films.Where(x => x.Kod == id).Select(x => new FilmViewModel
                {
                    Id = x.Kod,
                    Country = _context.Countries.
                                        Where(c => c.Kod == x.CountryKod).
                                        Select(c => new IdName()
                                        {
                                            Id = c.Kod,
                                            Name = c.Name
                                        }).First(),
                    Genre = _context.Genres.
                                        Where(g => g.Kod == x.GenreKod).
                                        Select(g => new IdName()
                                        {
                                            Id = g.Kod,
                                            Name = g.Name
                                        }).
                                        First(),
                    Name = x.Name,
                    Rating = _context.Ratings.
                                        Where(r => r.Kod == x.RatingKod).
                                        Select(r => new IdName()
                                        {
                                            Id = r.Kod,
                                            Name = r.Name
                                        }).
                                        First(),
                    StartDate = x.StartDate,
                    Studio = _context.FilmStudios.
                                        Where(s => s.Kod == x.StudioKod).
                                        Select(s => new IdName()
                                        {
                                            Id = s.Kod,
                                            Name = s.Name
                                        }).
                                        First(),
                    TimeDuration = x.TimeDuration
                }).First();
            }
            var cinemas = _context.Films.Select(x => new FilmViewModel
            {
                Id = x.Kod,
                Country = _context.Countries.
                                        Where(c => c.Kod == x.CountryKod).
                                        Select(c => new IdName()
                                        {
                                            Id = c.Kod,
                                            Name = c.Name
                                        }).First(),
                Genre = _context.Genres.
                                        Where(g => g.Kod == x.GenreKod).
                                        Select(g => new IdName()
                                        {
                                            Id = g.Kod,
                                            Name = g.Name
                                        }).
                                        First(),
                Name = x.Name,
                Rating = _context.Ratings.
                                        Where(r => r.Kod == x.RatingKod).
                                        Select(r => new IdName()
                                        {
                                            Id = r.Kod,
                                            Name = r.Name
                                        }).
                                        First(),
                StartDate = x.StartDate,
                Studio = _context.FilmStudios.
                                        Where(s => s.Kod == x.StudioKod).
                                        Select(s => new IdName()
                                        {
                                            Id = s.Kod,
                                            Name = s.Name
                                        }).
                                        First(),
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

            if (errorMesaage != "")
            {
                ViewData["ERROR"] = errorMesaage;
                errorMesaage = "";
            }
            return View(defaultObject);
        }
        static string errorMesaage;
        [HttpPost]
        public IActionResult AddOrReplaceFilms(FilmViewModel model)
        {
            if (model.Id != 0)
            {
                Film toEdit = _context.Films.First(x => x.Kod == model.Id);
                toEdit.CountryKod = model.Country.Id;
                toEdit.GenreKod = model.Genre.Id;
                toEdit.Name = model.Name;
                toEdit.RatingKod = model.Rating.Id;
                toEdit.StartDate = model.StartDate;
                toEdit.StudioKod = model.Studio.Id;
                toEdit.TimeDuration = model.TimeDuration;
            } else
            {
                _context.Films.Add(new Film()
                {
                    Kod = model.Id,
                    CountryKod = model.Country.Id,
                    GenreKod = model.Genre.Id,
                    Name = model.Name,
                    RatingKod = model.Rating.Id,
                    StartDate = model.StartDate,
                    StudioKod = model.Studio.Id,
                    TimeDuration = model.TimeDuration
                });
            }
            try
            {
                _context.SaveChanges();
            } catch (DbUpdateException e)
            {
                if (e.InnerException.ToString().Contains("KINO.TRIGGER_DATE"))
                {
                    errorMesaage = "Дата введена больше текущей";
                }
            }


            return RedirectToAction("Films", "Cinema");
        }
        [HttpPost]
        public IActionResult DeleteFilms(long id)
        {
            _context.Films.Remove(_context.Films.First(x => x.Kod == id));
            _context.SaveChanges();
            return RedirectToAction("Films", "Cinema");
        }
        #endregion

        [HttpGet]
        public IActionResult BoxOffice()
        {
            var boxOffices = _context.BoxOffices.Select(x => new BoxOfficeViewModel
            {
                Film = _context.Films.First(g => g.Kod == x.KodFilm).Name,
                TotalSum = x.TotalSum,
                Date = x.DateBoxOffice,
            }).ToArray();
            ViewData["TableName"] = "Кассовые сборы";
            ViewData["Headers"] = new string[] { "Фильм", "Сумма", "Дата" };
            ViewData["TableData"] = boxOffices;
            return View();
        }

        #region FilmEmps
        [HttpGet]
        public IActionResult FilmsEmp(long id)
        {
            FilmsEmpViewModel defautlModel = new();
            if (id != 0)
            {
                defautlModel = _context.FilmsEmps.Where(x => x.Kod == id).Select(x => new FilmsEmpViewModel
                {
                    Kod = x.Kod,
                    Employee = _context.Employees.
                                    Where(h => h.Kod == x.EmployeeKod).
                                    Select(h => new IdName()
                                    {
                                        Id = h.Kod,
                                        Name = h.FIO(),
                                    }).First(),
                    EmployeeType = _context.EmployeeTypes.
                                    Where(et => et.Kod == x.TypeEmployeeKod).
                                    Select(et => new IdName()
                                    {
                                        Id = et.Kod,
                                        Name = et.Name,
                                    }).First(),
                    Film = _context.Films.
                                    Where(f => f.Kod == x.FilmKod).
                                    Select(f => new IdName()
                                    {
                                        Id = f.Kod,
                                        Name = f.Name
                                    }).First(),

                }).First();
            }
            var filmsEmpsViewModel = _context.FilmsEmps.Select(x => new FilmsEmpViewModel
            {
                Kod = x.Kod,
                Employee = _context.Employees.
                                    Where(h => h.Kod == x.EmployeeKod).
                                    Select(h => new IdName()
                                    {
                                        Id = h.Kod,
                                        Name = h.FIO(),
                                    }).First(),
                EmployeeType = _context.EmployeeTypes.
                                    Where(et => et.Kod == x.TypeEmployeeKod).
                                    Select(et => new IdName()
                                    {
                                        Id = et.Kod,
                                        Name = et.Name,
                                    }).First(),
                Film = _context.Films.
                                    Where(f => f.Kod == x.FilmKod).
                                    Select(f => new IdName()
                                    {
                                        Id = f.Kod,
                                        Name = f.Name
                                    }).First(),

            }).ToArray();
            ViewData["TableName"] = "Участие в фильмах";
            ViewData["Headers"] = new string[] { "", "Фильм", "Сотрудник", "Должность" };
            ViewData["TableData"] = filmsEmpsViewModel;
            ViewData["Films"] = _context.Films.Select(x => new FilmViewModel
            {
                Id = x.Kod,
                Name = x.Name
            }).AsEnumerable();
            ViewData["Employees"] = _context.Employees.Select(x => new EmployeeViewModel
            {
                Kod = x.Kod,
                ThirdName = x.FIO()
            }).AsEnumerable();
            ViewData["EmployeesType"] = _context.EmployeeTypes.Select(x => new TypeEmployeeViewModel
            {
                Id = x.Kod,
                Name = x.Name
            }).AsEnumerable();
            return View(defautlModel);
        }
        [HttpPost]
        public IActionResult AddOrReplaceFilmsEmps(FilmsEmpViewModel model)
        {
            if (model.Kod == 0)
            {
                _context.FilmsEmps.Add(new FilmsEmp()
                {
                    EmployeeKod = model.Employee.Id,
                    TypeEmployeeKod = model.EmployeeType.Id,
                    FilmKod = model.Film.Id,
                });
            } else
            {
                var toEdit = _context.FilmsEmps.
                                        Where(x => x.Kod == model.Kod).
                                        Select(x => x).First();
                toEdit.EmployeeKod = model.Employee.Id;
                toEdit.TypeEmployeeKod = model.EmployeeType.Id;
                toEdit.FilmKod = model.Film.Id;
            }
            _context.SaveChanges();
            return RedirectToAction("FilmsEmp", "Cinema");
        }
        [HttpPost]
        public IActionResult DeleteFilmsEmp(long id)
        {
            _context.FilmsEmps.Remove(_context.FilmsEmps.First(x => x.Kod == id));
            _context.SaveChanges();
            return RedirectToAction("FilmsEmp", "Cinema");
        }
        #endregion

        #region SessionFilms
        [HttpGet]
        public IActionResult SessionFilms(long id)
        {
            SessionFilmsViewModel defaultModel = new();
            if (id != 0) {
                defaultModel = _context.SessionFilms.
                                        Where(x => x.Kod == id).
                                        Select(x => new SessionFilmsViewModel
                                        {
                                            Kod = x.Kod,
                                            Film = _context.Films.
                                                        Where(f => f.Kod == x.FilmKod).
                                                        Select(f => new IdName()
                                                        {
                                                            Id = f.Kod,
                                                            Name = f.Name,
                                                        }).First(),
                                            FreeSeatsAmount = x.FreePlaces,
                                            FilmStart = x.FilmStartDate,
                                            SeatsAmount = x.Places,
                                            TicketPrice = x.TicketPrice,
                                            ZalNumber = x.Hall,
                                        }).First();
            }
            var filmSession = _context.SessionFilms.Select(x => new SessionFilmsViewModel
            {
                Kod = x.Kod,
                Film = _context.Films.Where(f => f.Kod == x.FilmKod).
                                        Select(f => new IdName()
                                        {
                                            Id = f.Kod,
                                            Name = f.Name,
                                        }).First(),
                FilmStart = x.FilmStartDate,
                FreeSeatsAmount = x.FreePlaces,
                SeatsAmount = x.Places,
                TicketPrice = x.TicketPrice,
                ZalNumber = x.Hall
            }).ToArray();
            ViewData["TableName"] = "Расписание";
            ViewData["Headers"] = new string[] { "", "Фильм", "Начало", "Номер зала", "Количество мест", "Цена за билет", "Количество свободных мест" };
            ViewData["TableData"] = filmSession;
            ViewData["Films"] = _context.Films.Select(x => new FilmViewModel
            {
                Id = x.Kod,
                Name = x.Name
            }).AsEnumerable();
            return View(defaultModel);
        }
        public IActionResult AddOrReplaceSessionFilms(SessionFilmsViewModel model)
        {
            if (model.Kod == 0)
            {
                _context.SessionFilms.Add(new SessionFilms()
                {
                    FilmKod = model.Film.Id,
                    FilmStartDate = model.FilmStart,
                    FreePlaces = model.FreeSeatsAmount,
                    Hall = model.ZalNumber,
                    Places = model.SeatsAmount,
                    TicketPrice = model.TicketPrice,
                });
            }
            else
            {
                var toEdit = _context.SessionFilms.
                                        Where(x => x.Kod == model.Kod).
                                        Select(x => x).First();
                toEdit.FilmKod = model.Film.Id;
                toEdit.FilmStartDate = model.FilmStart;
                toEdit.FreePlaces = model.FreeSeatsAmount;
                toEdit.Hall = model.ZalNumber;
                toEdit.Places = model.SeatsAmount;
                toEdit.TicketPrice = model.TicketPrice;
            }
            _context.SaveChanges();
            return RedirectToAction("SessionFilms", "Cinema");
        }
        [HttpPost]
        public IActionResult DeleteSessionFilms(long id)
        {
            _context.SessionFilms.Remove(_context.SessionFilms.First(x => x.Kod == id));
            _context.SaveChanges();
            return RedirectToAction("SessionFilms", "Cinema");
        }
        #endregion

        [HttpGet]
        public IActionResult FormReport()
        {
            var otchet = _context.Films.Select(x => new
            {
                Country = _context.Countries.First(c => c.Kod == x.CountryKod).Name,
                Rating = _context.Ratings.First(r => r.Kod == x.RatingKod).Name,
                Director = _context.FilmsEmps.
                                              Where(fe => fe.FilmKod == x.Kod
                                                      && fe.TypeEmployeeKod == 2).
                                              Select(fe => fe.EmployeeKod).
                                              ToArray(),
                Tickets = _context.SessionFilms.Where(s => s.FilmKod == x.Kod).
                                                Select(s => s.Places - s.FreePlaces).Sum(),
                Film = x.Name,
            }).ToArray();


            var directors = _context.Employees.Select(x => new {
                Kod = x.Kod,
                Fio = x.FIO()
            }).ToArray();

            var directorsToFilms = otchet.GroupBy(x => new { x.Country, x.Director }).
                Select(x => new
                {
                    Country = x.Key.Country,
                    Directors = directors.Where(e => x.Key.Director.Contains(e.Kod)).
                                                    Select(e => e.Fio).ToArray(),
                    Film = x.Select(f => f.Film).ToArray(),
                    Tickets = x.Select(t => t.Tickets).First(),
                    Rating = x.Select(r => r.Rating).First(),
                }).OrderBy(x => x.Rating).ToArray();
            var output = directorsToFilms.Select(x =>
            "Страна: " + x.Country + "." +
            " Режиссёр(ы): " + string.Join(",", x.Directors) + "." +
            " Фильмы: " + string.Join(",", x.Film) + "." +
            " Количество билетов: " + x.Tickets).ToArray();

            MemoryStream ms = new();
            PdfWriter writer = new(ms);
            PdfDocument pdf = new(writer);
            Document docu = new(pdf);
            var FONT = "./Font/arial.ttf";
            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);

            docu.SetFont(font);
            foreach (string onLine in output)
            {
                docu.Add(new Paragraph(onLine));
            }
            docu.Close();
            return File(ms.GetBuffer(), "application/pdf", "grouppies.pdf");
        }

        #region Employees
        public IActionResult Employees(long id) 
        {
            var employess = _context.Employees.
                                        Select(x => new EmployeeViewModel() 
                                        {
                                            Kod = x.Kod,
                                            BirthDay = x.BirthDay,
                                            FirstName = x.FirstName,
                                            SecondName = x.SecondName,
                                            ThirdName = x.ThirdName,
                                        }).ToArray();
            EmployeeViewModel defaultModel = new();

            if (id != 0)
            {
                defaultModel = employess.First(x => x.Kod == id);
            }
            ViewData["TableData"] = employess;


            ViewData["TableName"] = "Съмочная группа";
            ViewData["Headers"] = new string[] { "", "Фамилия", "Имя", "Отчество", "Дата рождения" };
            return View(defaultModel);
        }
        
        public IActionResult AddOrReplaceEmployee(EmployeeViewModel model)
        {
            if (model.Kod != 0)
            {
                _context.Employees.Add(new Employee() 
                {
                    FirstName = model.FirstName,
                    SecondName = model.SecondName,
                    ThirdName = model.ThirdName,
                    BirthDay = model.BirthDay,
                });
            } else
            {
                var toEdit = _context.Employees.First(x => x.Kod == model.Kod);
                toEdit.FirstName = model.FirstName;
                toEdit.SecondName = model.SecondName;
                toEdit.ThirdName = model.ThirdName;
                toEdit.BirthDay= model.BirthDay;
            }
            _context.SaveChanges();
            return RedirectToAction("Employees", "Cinema");
        }

        public IActionResult DeleteEmployee(long id)
        {
            _context.Employees.Remove(_context.Employees.First(x => x.Kod == id));
            _context.SaveChanges();
            return RedirectToAction("Employees", "Cinema");
        }
        #endregion

    }
}
