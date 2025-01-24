using AttestationSystem.DB;
using Microsoft.AspNetCore.Mvc;

namespace AttestationSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentApiController : ControllerBase
{
    private readonly AttestationContext _context;

    public StudentApiController(AttestationContext context)
    {
        _context = context;
    }

    [HttpGet("student-data")]
    public IActionResult GetStudentData()
    {
        var tabNumber = HttpContext.Session.GetInt32("UserLogin");
        if (!tabNumber.HasValue)
        {
            return Unauthorized();
        }

        var student = _context.Студентыs
            .FirstOrDefault(s => s.НомерСтуденческого == tabNumber.Value);

        if (student == null)
        {
            return NotFound();
        }

        var grades = _context.Аттестацииs
            .Where(a => a.СтудентыНомерСтуденческого == student.НомерСтуденческого)
            .Select(a => new { a.ПредметыНазваниеДисциплины, a.Оценка, a.СеместрыСеместр })
            .ToList();

        var studentData = new
        {
            name = $"{student.Фамилия} {student.Имя} {student.Отчество}",
            tabNumber = student.НомерСтуденческого,
            group = student.ГруппыНомерГруппы,
            department = student.КафедрыНазваниеКафедры,
            grades
        };

        return Ok(studentData);
    }
}