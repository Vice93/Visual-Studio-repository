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
    public class StudentCoursesController : ApiController
    {
        // GET: api/update
        [HttpGet]
        [Route("api/update")]
        public void Update()
        {
            ApiModel.UpdateStudents();
            ApiModel.UpdateCourses();
        }
    }
}
