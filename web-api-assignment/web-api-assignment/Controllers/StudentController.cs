using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using web_api_assignment.Models.Database;
using web_api_assignment.Models.DTO;

namespace web_api_assignment.Controllers
{
    public class StudentController : ApiController
    {
        private readonly WebApiAssignmentEntities _db;

        public StudentController()
        {
            _db = new WebApiAssignmentEntities();
        }
        public HttpResponseMessage Get()
        {
            var students = _db.Students.ToList();
            return Request.CreateResponse(HttpStatusCode.OK,students);
        }
        
        public HttpResponseMessage Get(int? id)
        {
            if (id == null) return Request.CreateResponse(HttpStatusCode.BadRequest);
            var student = _db.Students.FirstOrDefault(s => s.Id == id);
            return student == null ? Request.CreateErrorResponse(HttpStatusCode.NotFound,"Student not found") : Request.CreateResponse(HttpStatusCode.OK,student);
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] StudentDto studentDto)
        {
            var department = _db.Departments.FirstOrDefault(d => d.Id == studentDto.DepartmentId);
            if (department == null) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid department ID");
            var newStudent = _db.Students.Add(new Student(){Name = studentDto.Name,DepartmentId = studentDto.DepartmentId});
            _db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created, newStudent);
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(int id, [FromBody] StudentDto studentDto)
        {
            var oldStudent = _db.Students.FirstOrDefault(s => s.Id == id);
            if (oldStudent == null) return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Student not found");
            var department = _db.Departments.FirstOrDefault(d => d.Id == studentDto.DepartmentId);
            if (department == null) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Department ID");
            oldStudent.Name = studentDto.Name;
            oldStudent.DepartmentId = studentDto.DepartmentId;
            _db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created, oldStudent);
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(int id)
        {
            var student = _db.Students.FirstOrDefault(s => s.Id == id);
            if (student == null) return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Student not found");
            _db.Students.Remove(student);
            _db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created, student);
        }
    }
}