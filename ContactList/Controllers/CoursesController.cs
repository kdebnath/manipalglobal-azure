using ContactList.Models;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ContactList.Controllers
{
    public class CoursesController : ApiController
    {
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK,
            Type = typeof(IEnumerable<Course>))]
        [Route("~/courses")]
        public async Task<IEnumerable<Course>> Get()
        {
            return new List<Course>
            {
                new Course
                {
                    Id = 1,
                    Name = "Microsoft Azure",
                    Description = "Azure basics",
                    Author = "Jhon Doe",
                    Url = "http://azure.microsoft.com"
                },
                new Course
                {
                    Id = 2,
                    Name = "Xamarin",
                    Description = "Xamarin Fundamentals",
                    Author = "Hardik Mistry",
                    Url = "http://developer.xamarin.com"
                }
            };
        }
    }
}
