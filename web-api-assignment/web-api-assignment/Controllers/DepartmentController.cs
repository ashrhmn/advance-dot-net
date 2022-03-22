using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using web_api_assignment.Models.Database;

namespace web_api_assignment.Controllers
{
    public class DepartmentController : ApiController
    {
        private readonly WebApiAssignmentEntities _db;

        public DepartmentController()
        {
            _db = new WebApiAssignmentEntities();
        }
        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            var departments = _db.Departments.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, departments);
        }

        // GET api/<controller>/5
        public HttpResponseMessage Get(int id)
        {
            var department = _db.Departments.FirstOrDefault(d => d.Id == id);
            return department == null ? Request.CreateErrorResponse(HttpStatusCode.NotFound,"Department not found") : Request.CreateResponse(HttpStatusCode.OK, department);
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] Department department)
        {
            var newDepartment = _db.Departments.Add(department);
            _db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created, newDepartment);
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(int id, [FromBody] Department department)
        {
            var oldDepartment = _db.Departments.FirstOrDefault(d => d.Id == id);
            if (oldDepartment == null) return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Department not found");
            oldDepartment.Name = department.Name;
            _db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created, oldDepartment);

        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(int id)
        {
            var department = _db.Departments.FirstOrDefault(d => d.Id == id);
            if (department == null) return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Department not found");
            _db.Departments.Remove(department);
            _db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created, department);
        }
    }
}