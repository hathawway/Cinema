using Cinema.Domain.Db;
using Cinema.Domain.Models.Film;
using Cinema.Models;
using Cinema.Models.Common;
using Cinema.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.Linq;
using Xceed.Document.NET;
using Xceed.Words.NET;

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
            return View(defaultObject);
        }
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
            _context.SaveChanges();
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
            ViewData["TableName"] = "Семпы фильма";
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
            }else
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
            if (model.Kod != 0)
            {

            }

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
            var values = _context.Films.Select(x => new
            {
                Country = x.CountryKod,
                Director = _context.FilmsEmps.
                                        Where(fe => fe.FilmKod == x.Kod && fe.TypeEmployeeKod == 2).
                                        Select(fe => fe.EmployeeKod).ToArray(),
                Film = x.Name,
                Rating = x.RatingKod,
                Tickets = _context.BoxOffices.
                                    Where(bo => bo.KodFilm == x.Kod).
                                    Select(bo => bo.TotalSum).Sum()
            }).ToArray();

            var itog = values.GroupBy(x => new { x.Country, x.Director}).Select(x => 
                new 
                {
                    CountryKod = x.Key.Country,
                    DirectorKod = x.Key.Director,
                    Films = x.Select(f => new {f.Film, f.Rating, f.Tickets }).OrderBy(f => f.Rating)
                }
            ).ToArray();
            var itog_2 = itog.Select(x => 
                    new
                    {
                      Country = _context.Countries.First(c => c.Kod == x.CountryKod),
                      Director = _context.Employees.First(e => x.DirectorKod.Contains(e.Kod)),
                      FilmName = x.Films.Select(f => f.Film).ToArray(),
                      Tickets = x.Films.First().Tickets,
                    }).ToArray();
            string pathDocument = AppDomain.CurrentDomain.BaseDirectory + "example.docx";

            // создаём документ
            DocX document = DocX.Create(pathDocument);

            // создаём таблицу с 3 строками и 2 столбцами
            Table table = document.AddTable(3, 5);
            // располагаем таблицу по центру
            table.Alignment = Alignment.center;
            // меняем стандартный дизайн таблицы
            table.Design = TableDesign.TableGrid;

            // заполнение ячейки текстом
            table.Rows[0].Cells[0].Paragraphs[0].Append("Тест");
            // заполнение ячейки ссылкой
            Hyperlink hyperlinkBlog =
                    document.AddHyperlink("progtask.ru", new Uri("https://progtask.ru"));

            table.Rows[0].Cells[1].Paragraphs[0].AppendHyperlink(hyperlinkBlog).
                                                 UnderlineStyle(UnderlineStyle.singleLine);

            // объединяем 2 ячейки
            table.Rows[1].MergeCells(0, 1);
            // заполняем полученную ячейку
            table.Rows[1].Cells[0].Paragraphs[0].Append("Тест").
                                            // устанавливаем размер текста
                                            FontSize(26).
                                            // выравниваем текст по центру ячейки
                                            Alignment = Alignment.center;

            // заполняем ячейку, меняя цвет текста и его размер
            table.Rows[2].Cells[0].Paragraphs[0].Append("Тест").
                                                 Color(Color.Green).
                                                 FontSize(20);
            // удаляем ячейку
            table.DeleteAndShiftCellsLeft(2, 1);

            // создаём параграф и вставляем таблицу
            document.InsertParagraph().InsertTableAfterSelf(table);

            // сохраняем документ
            document.Save();
            return View();
        }

    }
}
