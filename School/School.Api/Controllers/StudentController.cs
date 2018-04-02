using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using School.Model;

namespace School.Api.Controllers
{
    public class StudentController : ApiController
    {
        // GET: api/Student
        [HttpGet]
        [Route("api/students")]
        public IEnumerable<Student> Get()
        {
            return ApiModel.GetAllStudents();
        }

        // GET api/Student/5
        [HttpGet]
        [Route("api/student/{id:int}")]
        public Student Get(int id)
        {
            return ApiModel.GetStudent(id);
        }

        // POST api/Student
        [HttpPost]
        [Route("api/student/{id:int}")]
        public void Post(string value)
        {
            var student = JsonConvert.DeserializeObject<Student>(value); // Convert JSON to Users
            ApiModel.CreateStudent(student); // Create a new User
        }

        // PUT api/Student/5
        [HttpPut]
        [Route("api/student/{id:int}")]
        public void Put(int id, string value)
        {
            var student = JsonConvert.DeserializeObject<Student>(value);
            ApiModel.UpdateStudent(id, student);
        }

        // DELETE api/Student/5
        [HttpDelete]
        [Route("api/student/{id:int}")]
        public void Delete(int id)
        {
            ApiModel.DeleteStudent(id);
        }
    }
}
