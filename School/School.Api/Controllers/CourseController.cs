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
    public class CourseController : ApiController
    {
        // GET: api/courses
        [HttpGet]
        [Route("api/courses")]
        public IEnumerable<Course> Get()
        {
            return ApiModel.GetAllCourses();
        }

        // GET api/course/5
        [HttpGet]
        [Route("api/course/{id:int}")]
        public Course Get(int id)
        {
            return ApiModel.GetCourse(id);
        }

        // POST api/course
        [HttpPost]
        [Route("api/course/{id:int}")]
        public void Post(string value)
        {
            var course = JsonConvert.DeserializeObject<Course>(value); // Convert JSON to Course
            ApiModel.CreateCourse(course); // Create a new Course
        }

        // PUT api/course/5
        [HttpPut]
        [Route("api/course/{id:int}")]
        public void Put(int id, string value)
        {
            var course = JsonConvert.DeserializeObject<Course>(value);
            ApiModel.UpdateCourse(id, course);
        }

        // DELETE api/course/5
        [HttpDelete]
        [Route("api/course/{id:int}")]
        public void Delete(int id)
        {
            ApiModel.DeleteCourse(id);
        }
    }
}
