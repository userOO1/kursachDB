using AttestationSystem.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AttestationSystem.Controllers;

 public class AccountController(ILogger<AccountController> logger, AttestationContext context)
     : Controller
 {
     // Страница для авторизации
        [HttpPost]
        public async Task<IActionResult> Login(int login, string password, string role)
        {
            if (string.IsNullOrEmpty(password))
            {
                return BadRequest("Табельный номер и пароль обязательны.");
            }

            // Проверка роли и аутентификация
            if (role == "student")
            {
                var student = await context.Студентыs
                    .FirstOrDefaultAsync(s => s.НомерСтуденческого == login && s.Пароль == password);

                if (student != null)
                {
                    // Здесь можно установить сессию или куки для пользователя
                    // Пример сессии
                    try
                    {
                        // Установка сессии
                        HttpContext.Session.SetString("UserRole", "student");
                        HttpContext.Session.SetInt32("UserTabNumber", student.НомерСтуденческого);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error while setting session for student: {StudentNumber}", student.НомерСтуденческого);
                        return View("Error", "An error occurred while setting session");
                    }
                    return RedirectToAction("Index", "Student");  // Перенаправление на страницу студента
                }
                return BadRequest("Неверный табельный номер или пароль.");
            }
            else if (role == "teacher")
            {
                var teacher = await context.Преподавателиs
                    .FirstOrDefaultAsync(t => t.НомерСотрудника == login && t.Пароль == password);

                if (teacher != null)
                {
                    // Устанавливаем сессию или куки для преподавателя
                    HttpContext.Session.SetString("UserRole", "teacher");
                    HttpContext.Session.SetInt32("UserTabNumber", teacher.НомерСотрудника);
                    return RedirectToAction("Index", "Teacher");  // Перенаправление на страницу преподавателя
                }
                return BadRequest("Неверный табельный номер или пароль.");
            }
            else
            {
                return BadRequest("Неизвестная роль.");
            }
        }

        // Страница для выхода
        public async Task<IActionResult> Logout()
        {
            // Удаление данных о сессии
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");  // Перенаправление на домашнюю страницу
        }
    }