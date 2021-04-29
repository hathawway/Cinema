using Cinema.Domain.Db;
using Cinema.Domain.Models;
using Cinema.Domain.Models.Users;
using Cinema.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CinemaDbContext _context;
        private readonly UserManager<User> _userManager;
        public UserController(
            UserManager<User> userManager,
            ILogger<HomeController> logger,
            CinemaDbContext context)
        {
            _logger = logger;
            _context = context;
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
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Очистить существующие куки для корректного логина
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
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
        public async Task<IActionResult> Login([FromServices] SignInManager<User> signInManager, LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = _userManager.FindByNameAsync(model.Login).Result;
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Проверьте имя пользователя и пароль");
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                    return RedirectToLocal(returnUrl);

                if (result.IsLockedOut)
                    return RedirectToAction(nameof(Lockout));

                ModelState.AddModelError(string.Empty, "Неверный логин или пароль");
                return View(model);
            }

            return View(model);
        }
        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="model">Данные о новом пользователе</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrationNewUserAsync(NewUserViewModel model)
        {

            if (!ModelState.IsValid)
                return View(model);

            if (_userManager.Users.Any(x => x.Email.ToLower() == model.EmailAddress.ToLower()))
                ModelState.AddModelError("Email", "Такой email уже используеся в системе");

            if (ModelState.ErrorCount > 0)
                return View(model);
            /*
            var profile = new Domain.Models.Users.Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
            };
            */
            var user = new User
            {
                Email = model.EmailAddress,
                UserName = model.EmailAddress,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }

            //await _userManager.AddToRoleAsync(user, SecurityConstants.СustomerRole);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Выход из системы
        /// </summary>
        /// <param name="signInManager">Менеджер авторизации</param>
        [HttpGet]
        public async Task<IActionResult> Logout([FromServices] SignInManager<User> signInManager)
        {
            await signInManager.SignOutAsync();


            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Возвращение страницы в случае блокировки пользователя
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        /// <summary>
        /// Подтверждение сброса пароля
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// Страница запрета доступа
        /// </summary>
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }


        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        [HttpGet]
        public IActionResult RegistrationNewUser()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion
    }
}
