using DataAccess.Commands;
using DataAccess.Models;
using DataAccess.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RabbitMQProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        private readonly IMediator _mediatR;

        public StudentController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpGet("getstudentlist")]
        public async Task<List<Student>> GetListOfStudent()
        {
            return await _mediatR.Send(new GetStudentListQuery());
        }

        [HttpPost("addstudent")]
        public async Task<Student> AddNewStudent([FromBody] Student s)
        {
            var result = new AddStudentCommand(s.FirstName, s.LastName, s.Age);
            return await _mediatR.Send(result);
        }

        [HttpGet("getstudentbyid")]

        public async Task<Student> GetStudentbyId(int id)
        {
            var result = new GetStudentByIdQuery(id);
            return await _mediatR.Send(result);
        }
    }
}
