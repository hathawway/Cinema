using Cinema.Domain.Db;
using Cinema.Models;
using Cinema.Models.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Controllers
{
    public class QueryController : Controller
    {
        private readonly CinemaDbContext _context;
        private readonly string ConnectionString;
        public QueryController(CinemaDbContext context, IConfiguration Configuration)
        {
            _context = context;
            ConnectionString = Configuration.GetConnectionString("OracleDBConnection");
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
            ViewData["TableName"] = "Кассовые сборы";
            ViewData["Headers"] = new string[] { "Фильм", "Сумма", "Дата" };
            ViewData["TableData"] = boxOffices;
            return View();
        }

        [HttpGet]
        public IActionResult ListFilms()
        {
            var russiaCode = _context.Countries.First(x => x.Name == "Россия").Kod;
            var films = _context.Films.Where(x => x.StartDate.Year == 2021 && x.CountryKod == russiaCode).Select(x => new FilmViewModel
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
            }).AsEnumerable();
            ViewData["TableName"] = "Список фильмов, вышедших в текущем году в России";
            ViewData["Headers"] = new string[] { "Фильм", "Дата", "Страна" };
            ViewData["TableData"] = films;
            return View();
        }

        [HttpGet]
        public IActionResult ListRole()
        {
            OracleConnection con = new();
            con.ConnectionString = ConnectionString;
            con.Open();

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select kino.PACKAGE__P.spisokemp from dual";
            List<List<string>> response = new();
            var lenth = 1;
            if (lenth > 0)
            {
                OracleDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    response.Add(new List<string>());
                    for (int i = 0; i < lenth; i++)
                    {
                        response[^1].Add(reader.GetString(i));
                    }
                }
            }
            else
            {
                cmd.ExecuteNonQuery();
            }

            con.Close();

            var result = response[0][0].Split("haha").ToArray();
            result = result.Take(result.Length - 1).ToArray();
            ViewData["TableName"] = "Фильмы, в которых режиссер является одновременно " +
                "исполнителем главной роли";
            ViewData["Headers"] = new string[] { "Фильм - Режиссер"};
            ViewData["TableData"] = result;
            return View();
        }

        [HttpGet]
        public IActionResult StatisticsGenre()
        {
            var russiaCode = _context.Countries.First(x => x.Name == "Россия").Kod;
            var films = _context.Films.Select(x => x).ToArray();

            var genres = films.GroupBy(x => x.GenreKod).Select(x => new 
            {
                Genre = x.Key,
                RuFilm = films.Where(f => f.GenreKod == x.Key && f.CountryKod == russiaCode).Count(),
                Count = x.Count()
            }).Select(x => new StatOnGenre
            {
                Genre = _context.Genres.First(g => g.Kod == x.Genre).Name,
                RusAmount = x.RuFilm,
                ForreignAmount = x.Count - x.RuFilm
            }).AsEnumerable();
            ViewData["TableName"] = "Статистика по жанрам";
            ViewData["Headers"] = new string[] { "Жанр", "Кол-во фильмов снятых в России", "Кол-во фильмов снятых в др. странах" };
            ViewData["TableData"] = genres;
        
            return View();
        }

        [HttpGet]
        public IActionResult ApproximateAge(IdName model)
        {
            ViewData["Employes"] = _context.Employees.Select(x => new IdName()
            {
                Id = x.Kod,
                Name = x.FIO(),
                }).AsEnumerable();

            if (model.Id == 0)
            {
                model.Id = (ViewData["Employes"] as IEnumerable<IdName>).First().Id;
            }
            OracleConnection con = new();
            con.ConnectionString = ConnectionString;
            con.Open();

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT PACKAGE__P.AGE(" + model.Id.ToString() +") FROM DUAL";

            OracleDataReader reader = cmd.ExecuteReader();
            string response = "";
            while (reader.Read())
            {
                response += reader.GetString(0);
            }

            ViewData["Table"] = response;

            ViewData["TableName"] = "Примерный возраст";
            ViewData["Headers"] = new string[] { "Возраст" };
            con.Close();

            return View();
        }

        [HttpGet]
        public IActionResult FilmTime(IdName model)
        {
            ViewData["Films"] = _context.Films.Select(x => new IdName()
            {
                Id = x.Kod,
                Name = x.Name,
            }).AsEnumerable();
            if (model.Id == 0)
            {
                model.Id = (ViewData["Films"] as IEnumerable<IdName>).First().Id;
            }
            OracleConnection con = new();
            con.ConnectionString = ConnectionString;
            con.Open();

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT PACKAGE__P.TIME_FILM(" + model.Id.ToString() + ") FROM DUAL";

            OracleDataReader reader = cmd.ExecuteReader();
            string response = "";
            while (reader.Read())
            {
                response += reader.GetString(0);
            }

            ViewData["Table"] = response;

            ViewData["TableName"] = "Строка продолжительности";
            ViewData["Headers"] = new string[] { "Продолжительность" };
            con.Close();

            return View();
        }
        [HttpGet]
        public IActionResult Fio(IdName model)
        {
            ViewData["Employes"] = _context.Employees.Select(x => new IdName()
            {
                Id = x.Kod,
                Name = x.FIO(),
            }).AsEnumerable();

            if (model.Id == 0)
            {
                model.Id = (ViewData["Employes"] as IEnumerable<IdName>).First().Id;
            }

            OracleConnection con = new();
            con.ConnectionString = ConnectionString;
            con.Open();

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT PACKAGE__P.FULLNAME(" + model.Id.ToString() + ") FROM DUAL";

            OracleDataReader reader = cmd.ExecuteReader();
            string response = "";
            while (reader.Read())
            {
                response += reader.GetString(0);
            }

            ViewData["Table"] = response;

            ViewData["TableName"] = "Строка ФИО";
            ViewData["Headers"] = new string[] { "ФИО" };

            con.Close();

            return View();
        }

        public IActionResult PotentiaGet(DateFromTo dates)
        {
            if (dates.From != DateTime.MinValue)
            {

            OracleConnection con = new();
            con.ConnectionString = ConnectionString;
            con.Open();

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT PACKAGE__P.INCOME(TO_DATE('" + dates.From.ToShortDateString() + "', 'dd.MM.yyyy')," +
                " TO_DATE('" + dates.To.ToShortDateString() + "', 'dd.MM.yyyy')) FROM DUAL";

            OracleDataReader reader = cmd.ExecuteReader();
            string response = "";
            while (reader.Read())
            {
                response += reader.GetString(0);
            }
            var dd = response.Split("\\n");
            ViewData["fact"] = dd[0];
            ViewData["potent"] = dd[1];
            con.Close();
            }


            ViewData["TableName"] = "Фактический и потенциальный доход";
            ViewData["Headers"] = new string[] { "Фактический", "Потенциальный"};
            return View();
        }

        

    }
}
