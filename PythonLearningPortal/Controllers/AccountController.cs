using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.Models;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.ViewModels;

namespace PythonLearningPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly PythonLearningPortalContext _context;

        public AccountController(PythonLearningPortalContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Account/Login.cshtml");
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View("~/Views/Account/Register.cshtml");
        }

        // GET: /Account/RegistrationSuccess
        public IActionResult RegistrationSuccess()
        {
            return View("~/Views/Home/Index.cshtml");
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Проверяем наличие пользователя в базе данных по логину и паролю
                var user = await _context.Аккаунты
                    .Include(u => u.Роли)
                    .FirstOrDefaultAsync(u => u.Логин == model.Login && u.Пароль == model.Password);

                if (user != null)
                {
                    // Проверяем, существует ли роль пользователя
                    if (user.Роли != null)
                    {
                        var roleCode = user.Роли.Код_роли;

                        // Создаем claims
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Логин),
                            new Claim(ClaimTypes.Role, roleCode.ToString())
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true // Cookie будет сохранена даже после закрытия браузера
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                        // Перенаправляем на соответствующую страницу в зависимости от роли
                        if (roleCode == 1)
                        {
                            return RedirectToAction("Index", "MenuStudent");
                        }
                        else if (roleCode == 2)
                        {
                            return RedirectToAction("Index", "MenuTeacher");
                        }
                        else if (roleCode == 3)
                        {
                            return RedirectToAction("Index", "MenuAdmin");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Роль пользователя не определена");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Неверный логин или пароль");
                }
            }
            return View(model);
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Проверка на уникальность логина
                if (await _context.Аккаунты.AnyAsync(u => u.Логин == model.Login))
                {
                    ModelState.AddModelError("Login", "Пользователь с таким логином уже существует");
                    return View(model);
                }

                // Проверка на уникальность кода роли
                if (await _context.Роли.AllAsync(r => r.Код_роли != model.RoleCode))
                {
                    ModelState.AddModelError("RoleCode", "Роль с указанным кодом не существует");
                    return View(model);
                }

                // Создание нового аккаунта
                var account = new Аккаунты
                {
                    Логин = model.Login,
                    Пароль = model.Password,
                    Код_аккаунта = model.AccountCode
                };

                // Установка роли для аккаунта
                account.Роли = await _context.Роли.FindAsync(model.RoleCode);

                // Сохранение аккаунта в базе данных
                _context.Add(account);
                await _context.SaveChangesAsync();

                // Автоматический вход после регистрации
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.Логин),
                    new Claim(ClaimTypes.Role, account.Роли.Код_роли.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                // Проверка роли и перенаправление на соответствующую страницу
                if (model.RoleCode == 1)
                {
                    return RedirectToAction("Index", "MenuStudent");
                }
                else if (model.RoleCode == 2)
                {
                    return RedirectToAction("Index", "MenuTeacher");
                }
                else if (model.RoleCode == 3)
                {
                    return RedirectToAction("Index", "MenuAdmin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        // GET: /Account/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
