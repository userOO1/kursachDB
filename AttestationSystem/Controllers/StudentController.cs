using Microsoft.AspNetCore.Mvc;
using AttestationSystem.DB; // Модель данных
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace AttestationSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly AttestationContext _context;

        public StudentController(AttestationContext context)
        {
            _context = context;
        }

        // Статическая HTML страница для студента
        public IActionResult Index()
        {
            // Получаем табельный номер студента из сессии
            var tabNumber = HttpContext.Session.GetInt32("UserTabNumber");
            
            if (!tabNumber.HasValue)
            {
                return RedirectToAction("Login", "Account");  // Перенаправляем на страницу входа, если нет данных
            }

            // Получаем информацию о студенте
            var student = _context.Студентыs
                .FirstOrDefault(s => s.НомерСтуденческого == tabNumber.Value);

            if (student == null)
            {
                return NotFound();  // Студент не найден
            }

            // Формируем путь к HTML файлу
            var htmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "student_dashboard.html");

            // Данные для динамической вставки на страницу
            string studentData = $@"
<!DOCTYPE html>
<html lang='ru'>
<head>
    <meta charset='UTF-8'>  <!-- Указание кодировки UTF-8 -->
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Student Dashboard</title>
    <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css' rel='stylesheet'>
</head>
<body>
    <div class='container mt-4'>
        <h3>Добро пожаловать, <span class='text-primary'>{student.Фамилия} {student.Имя} {student.Отчество}</span></h3>
        <p><strong>Номер студенческого:</strong> {student.НомерСтуденческого}</p>
        <p><strong>Группа:</strong> {student.ГруппыНомерГруппы}</p>
        <p><strong>Кафедра:</strong> {student.КафедрыНазваниеКафедры}</p>

        <h4>Аттестации</h4>

        <table class='table table-bordered'>
            
            <tbody>";

            

// Получаем все семестры
            var semesters = _context.Семестрыs.ToList();


// Перебираем каждый семестр
            foreach (var semester in semesters)
            {
                // Добавляем заголовок семестра
                studentData += $@"
    <tr>
        <td colspan='5' class='table-secondary'><strong>Семестр: {semester.Семестр}</strong></td>
    </tr>
                    <tr>
                    <th>Предмет</th>
                    <th>Дата</th>
                    <th>Преподаватель</th>
                    <th>Вид работы</th>
                    <th>Оценка</th>
                    </tr>";

                // Получаем все предметы
                var subjects = _context.Предметыs.ToList(); // Здесь мы получаем все предметы

                
                    // Ищем аттестации по предмету для данного студента
                    var grades = _context.Аттестацииs
                        .Where(grade => semester.Семестр==grade.СеместрыСеместр && grade.СтудентыНомерСтуденческого == tabNumber)
                        .Join(_context.Преподавателиs, 
                            grade => grade.ПреподавателиНомерСотрудника, 
                            teacher => teacher.НомерСотрудника, 
                            (grade, teacher) => new { grade, teacher })
                        .ToList();

                    // Если есть аттестации, выводим их
                    if (grades.Any())
                    {
                        foreach (var grade in grades)
                        {
                            studentData += $@"
                <tr>
                    <td>{grade.grade.ПредметыНазваниеДисциплины}</td>
                    <td>{grade.grade.ДатыДата}</td>
                    <td>{grade.teacher.Фамилия} {grade.teacher.Имя} {grade.teacher.Отчество}</td>
                    <td>{grade.grade.ВидыРаботВидРаботы}</td>
                    <td>{grade.grade.Оценка}</td>
                </tr>";
                        }
                    }
                    else
                    {
                        // Если аттестаций нет, выводим пустые ячейки с предметом
                        studentData += $@"
            <tr>
                
                <td colspan='5' class='text-center'>Нет аттестации</td>
                
            </tr>";
                    }
                
            }

            studentData += "</tbody></table></div><script src=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js\"></script></ body > </ html > ";
            // Записываем данные в HTML файл
            System.IO.File.WriteAllText(htmlFilePath, studentData);

            // Перенаправляем на статический HTML файл
            return File("/student_dashboard.html", "text/html");
        }
    }
}