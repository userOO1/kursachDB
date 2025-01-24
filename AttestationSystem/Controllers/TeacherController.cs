using Microsoft.AspNetCore.Mvc;
using AttestationSystem.DB;
using Microsoft.EntityFrameworkCore;

namespace AttestationSystem.Controllers
{
    public class TeacherController : Controller
    {
        private readonly AttestationContext _context;

        public TeacherController(AttestationContext context)
        {
            _context = context;
        }

        // Главная страница преподавателя 
        public IActionResult Index()
        {
            var tabNumber = HttpContext.Session.GetInt32("UserTabNumber");
            var teacher = _context.Преподавателиs
                .FirstOrDefault(s => s.НомерСотрудника == tabNumber.Value);
            // Получаем список групп
            var departments = _context.Кафедрыs.ToList();

            // Формируем путь к HTML-файлу
            var htmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "department_list.html");

            // Динамическое наполнение
            string departmentData = $@"<!DOCTYPE html>
<html lang='ru'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Кафедры</title>
    <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css' rel='stylesheet'>
</head>
<body>
<div class='container mt-4'>
<h3>Добро пожаловать, <span class='text-primary'>{teacher.Фамилия} {teacher.Имя} {teacher.Отчество}</span></h3>
        <p><strong>Номер сотрудника:</strong> {teacher.НомерСотрудника}</p>
        <p><strong>Кафедра:</strong> {teacher.КафедрыНазваниеКафедры}</p>
    <h3>Кафедры</h3>
    <table class='table table-bordered'>
        <thead>
        <tr>
            <th>Номер группы</th>
            <th>Действия</th>
        </tr>
        </thead>
        <tbody>";
            foreach (var department in departments)
            {
                departmentData += $@"
                    <tr>
                        <td>{department.НазваниеКафедры}</td>
                        <td>
                            <a href='/Teacher/GroupList?department={department.НазваниеКафедры}' class='btn btn-primary'>Просмотреть группы</a>
                        </td>
                    </tr>";
            }
            departmentData += "</tbody>\n    </table>\n</div>\n</body>\n</html>";

            
            System.IO.File.WriteAllText(htmlFilePath, departmentData);

            return File("/department_list.html", "text/html");
        }

        public IActionResult GroupList(string department)
        {
            var groups = _context.Группыs
                .Where(s => s.КафедрыНазваниеКафедры == department)
                .ToList();
            // Формируем путь к HTML-файлу
           var htmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "group_list.html");

           // Динамическое наполнение
           string groupData = $@"<!DOCTYPE html>
<html lang='ru'>
<head>
   <meta charset='UTF-8'>
   <meta name='viewport' content='width=device-width, initial-scale=1.0'>
   <title>Список групп</title>
   <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css' rel='stylesheet'>
</head>
<body>
<div class='container mt-4'>
   <h3>Список групп</h3>
   <table class='table table-bordered'>
       <thead>
       <tr>
           <th>Номер группы</th>
           <th>Действия</th>
       </tr>
       </thead>
       <tbody>";
           foreach (var group in groups)
           {
               groupData += $@"
                   <tr>
                       <td>{group.НомерГруппы}</td>
                       <td>
                           <a href='/Teacher/StudentList?groupNumber={group.НомерГруппы}' class='btn btn-primary'>Просмотреть студентов</a>
                       </td>
                   </tr>";
           }
           groupData += "</tbody>\n    </table>\n</div>\n</body>\n</html>";

           
           System.IO.File.WriteAllText(htmlFilePath, groupData);

           return File("/group_list.html", "text/html");
       }
        
        // Список студентов в группе
        public IActionResult StudentList(string groupNumber)
        {
            // Получаем список студентов по номеру группы
            var students = _context.Студентыs
                .Where(s => s.ГруппыНомерГруппы == groupNumber)
                .ToList();

            // Формируем путь к HTML-файлу
            var htmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "student_list.html");

            // Динамическое наполнение
            string studentData = $@"<!DOCTYPE html>
                                 <html lang='ru'>
                <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Список студентов</title>
                <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css' rel='stylesheet'>
                </head>
                <body>
                <div class='container mt-4'>
                <h3>Список студентов в группе {groupNumber.ToString()}</h3>
                <table class='table table-bordered'>
                <thead>
                <tr>
                <th>Номер студенческого</th>
                <th>Фамилия</th>
                <th>Имя</th>
                <th>Отчество</th>
                <th>Действия</th>
                </tr>
                </thead>";
            foreach (var student in students)
            {
                studentData += $@"
                    <tr>
                        <td>{student.НомерСтуденческого}</td>
                        <td>{student.Фамилия}</td>
                        <td>{student.Имя}</td>
                        <td>{student.Отчество}</td>
                        <td>
                            <a href='/Teacher/FillGrades?groupNumber={groupNumber}' class='btn btn-primary'>Проставить оценки</a>
                        </td>
                    </tr>";
            }
            studentData += "</tbody>\n    </table>\n</div>\n</body>\n</html>";


            System.IO.File.WriteAllText(htmlFilePath, studentData);

            return File("/student_list.html", "text/html");
        }

        // Страница выставления оценок
        public IActionResult FillGrades(string groupNumber)
        {
            // Получаем студентов, предметы, виды работ и семестры
            var students = _context.Студентыs
                .Where(s => s.ГруппыНомерГруппы == groupNumber)
                .ToList();

            var subjects = _context.Предметыs.ToList();
            var workTypes = _context.ВидыРаботs.ToList();
            var semesters = _context.Семестрыs.ToList();

            // Формируем путь к HTML-файлу
            var htmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fill_grades.html");

            // Динамическое наполнение
            string studentData = $@"<!DOCTYPE html>
<html lang='ru'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Проставление оценок</title>
    <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css' rel='stylesheet'>
</head>
<body>
<div class='container mt-4'>
    <h3>Проставление оценок для группы {groupNumber.ToString()}</h3>
    <form method='post' action='/Teacher/SubmitGrades'>
        <input type='hidden' name='GroupNumber' value={groupNumber.ToString()}>
        <table class='table table-bordered'>
            <thead>
            <tr>
                <th>Номер студенческого</th>
                <th>Фамилия</th>
                <th>Имя</th>
                <th>Отчество</th>
                <th>Предмет</th>
                <th>Семестр</th>
                <th>Вид работы</th>
                <th>Оценка</th>
                <th>Дата</th>
            </tr>
            </thead>
            <tbody>";
            int index = 0;
            foreach (var student in students)
            {
                studentData += $@"
                    <tr>
                        <td>
                            <input type='hidden' name='grades[0].TabStudent' value={student.НомерСтуденческого}>
                            {student.НомерСтуденческого}
                        </td>

                        <td>{student.Фамилия}</td>
                        <td>{student.Имя}</td>
                        <td>{student.Отчество}</td>
                        <td>
                            <select name='grades[{index}].Subject' class='form-select' >";
                foreach (var subject in subjects)
                {
                    studentData += $"<option value='{subject.НазваниеДисциплины}'>{subject.НазваниеДисциплины}</option>";
                }
                studentData += "</select></td><td>";

                studentData += $@"<select name='grades[{index}].Semester' class='form-select' >";
                foreach (var semester in semesters)
                {
                    studentData += $"<option value='{semester.Семестр}'>{semester.Семестр}</option>";
                }
                studentData += "</select></td><td>";

                studentData += $@"<select id='workTypeSelect' onchange='updateGradeOptions()' name='grades[{index}].WorkType' class='form-select'>";
                foreach (var workType in workTypes)
                {
                    studentData += $"<option value='{workType.ВидРаботы}'>{workType.ВидРаботы}</option>";
                }
                studentData += "</select></td><td>";

                studentData += $@"
                        <select  name='grades[0].Grade' class='form-select' >
                            <option value='0'>Незачет</option>
                            <option value='1'>Зачет</option>
                            <option value='2'>2</option>
                            <option value='3'>3</option>
                            <option value='4'>4</option>
                            <option value='5'>5</option>
                        </select>
                        </td>
                        <td>
                            <input type='date' name='grades[{index}].Date' class='form-control'>
                        </td>
                    </tr>";
                index++;
            }
            studentData += $@"</tbody>
        </table>
        <button type='submit' class='btn btn-success'>Сохранить оценки</button>
    </form>
</div>

</body>
</html>";

            // Заполняем HTML и возвращаем файл
            var template = System.IO.File.ReadAllText(htmlFilePath);

            System.IO.File.WriteAllText(htmlFilePath, studentData);

            return File("/fill_grades.html", "text/html");
        }

        // Метод для обработки POST-запроса и сохранения оценок
        [HttpPost]
        public IActionResult SubmitGrades(IFormCollection form)
        {
            // Получаем табельный номер студента из сессии
            var tabNumber = HttpContext.Session.GetInt32("UserTabNumber");
            var grades = form
                .Where(k => k.Key.StartsWith("grades"))
                .GroupBy(k => k.Key.Split('[')[1].Split(']')[0]) // Группируем по индексу
                .Select(g => new
                {   
                    TabStudent = int.Parse(g.FirstOrDefault(k => k.Key.EndsWith(".TabStudent")).Value),
                    Subject = g.FirstOrDefault(k => k.Key.EndsWith(".Subject")).Value,
                    Semester = int.Parse(g.FirstOrDefault(k => k.Key.EndsWith(".Semester")).Value),
                    WorkType = g.FirstOrDefault(k => k.Key.EndsWith(".WorkType")).Value,
                    Grade = int.Parse(g.FirstOrDefault(k => k.Key.EndsWith(".Grade")).Value),
                    Date = DateOnly.Parse(g.FirstOrDefault(k => k.Key.EndsWith(".Date")).Value)
                }).ToList();

            // Сохраняем оценки
            foreach (var grade in grades)
            {
                
                var newDate = new Даты
                {
                    Дата = grade.Date
                };
                _context.Датыs.Add(newDate);
                var newGrade = new Аттестации
                {
                    ПредметыНазваниеДисциплины = grade.Subject,
                    СеместрыСеместр = grade.Semester,
                    ВидыРаботВидРаботы = grade.WorkType,
                    Оценка = grade.Grade,
                    ДатыДата = grade.Date,
                    ПреподавателиНомерСотрудника = tabNumber,
                    СтудентыНомерСтуденческого = grade.TabStudent
                };
                
                _context.Аттестацииs.Add(newGrade);
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
