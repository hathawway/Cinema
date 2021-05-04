﻿using Cinema.Domain.Db;
using Cinema.Domain.Models.Users;
using Cinema.Models.User;
using Cinema.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Cinema.Controllers
{
    public class UserController : Controller
    {
        private readonly CinemaDbContext _context;
        private readonly ISignIn _signInManager;
        public UserController(
            CinemaDbContext context,
            ISignIn SignInManager)
        {
            _context = context;
            _signInManager = SignInManager;
        }
        /// <summary>
        /// Страница пользовтеля
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Личный кабинет
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Account()
        {
            // TODO: а надо ли вообще ?
            var user = _context.Employees
                .Select(x => x).OrderByDescending(x => x.SecondName);

            return View(user);
        }

        /// <summary>
        /// Форма входа в систему
        /// </summary>
        /// <param name="returnUrl">Путь перехода после авторизации</param>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            return View();
        }

        /// <summary>
        /// Авторизация в системе
        /// </summary>
        /// <param name="signInManager">Менеджер авторизации</param>
        /// <param name="model">Входные данные с формы</param>
        /// <param name="returnUrl">Путь перехода после авторизации</param>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = _context.Users.Where(x => x.Login == model.Login).Select(x => x).ToArray()[0];
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Проверьте имя пользователя и пароль");
                    return View(model);
                }
                _signInManager.SignIn(user);
            }

            return RedirectToAction("Index", "Cinema");
        }
        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="model">Данные о новом пользователе</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegistrationNewUser(NewUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_context.Users.Any(x => x.Login.ToLower() == model.Login.ToLower()))
            {
                return View(model);
            }

            var profile = new Employee
            {
                FirstName = model.FirstName,
                SecondName = model.LastName,
            };

            var employee = _context.Employees.Add(profile).Entity;
            _context.SaveChanges();

            var user = new User
            {
                Login = model.Login,
                Password = model.Password,
                RoleId = 1,
                EmployeeKod = employee.Kod
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            user = _context.Users.Where(x => x.Login == model.Login).Select(x => x).ToArray()[0];
            _signInManager.SignIn(user);

            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Выход из системы
        /// </summary>
        /// <param name="signInManager">Менеджер авторизации</param>
        [HttpGet]
        public IActionResult Logout()
        {
            //TODO сделать метод для разлогина
            _signInManager.SignIn(null);

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        [HttpGet]
        public IActionResult RegistrationNewUser()
        {
            return View();
        }
    }
}
