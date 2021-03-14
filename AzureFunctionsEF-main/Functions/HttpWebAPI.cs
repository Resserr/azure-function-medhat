using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using DataLayer;
using System.Linq;
using Functions.Interfaces;
using DataLayer.Models;

namespace Snoopy.Function
{
    public class HttpWebAPI
    {
        private readonly HttpClient _client;

        // Repository Service to work with Database Context. Here I am using Repository pattern
        // You can also ask why we need to encapsulate it to another service. It is more to have loosley coupled code and 
        // unit testing friendly environment.
        private readonly IStudentRepository _studentRepository;

        // Wrapper for JsonConverter. Again, we are using wrapper because it is a good practice and can help us unit testing
        // friendly environment. It is easier to create mock if you have interface and implementation.
        private readonly IJsonConverterWrapper _jsonConverterWrapper;

        public HttpWebAPI(IHttpClientFactory httpClientFactory,
            IStudentRepository studentRepository, IJsonConverterWrapper jsonConverterWrapper)
        {
            _client = httpClientFactory.CreateClient();
            _studentRepository = studentRepository;
            _jsonConverterWrapper = jsonConverterWrapper;
        }
        [FunctionName("HttpWebAPI")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("GetStudents")]
        public async Task<IActionResult> GetStudents(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "students")] HttpRequest req,
        ILogger log)
        {
            log.LogInformation("C# HTTP GET/posts trigger function processed a request.");

            // I have moved DataBase Context to the StudentRepository service. That's why I have changed this code.
            // Also, I have added async await to increase performance for your app
            var studentsArray = await _studentRepository.GetStudentsAsync();

            return new OkObjectResult(studentsArray);
        }

        // This is endpoint to Get particular user and not all users at the same time. I am using Route parameters here
        // Route parameter = "studentId" in the route
        [FunctionName("GetStudent")]
        public async Task<IActionResult> GetStudent(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "students/{studentId}")] HttpRequest req,
        string studentId, ILogger log)
        {
            log.LogInformation("C# HTTP GET/posts trigger function processed a request.");

            var studentsArray = await _studentRepository.GetStudentAsync(studentId);

            return new OkObjectResult(studentsArray);
        }

        // Endpoint for delete operation
        [FunctionName("DeleteStudents")]
        public async Task<IActionResult> DeleteStudents(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "students/{studentId}")] HttpRequest req,
        string studentId, ILogger log)
        {
            log.LogInformation("C# HTTP Delete trigger function processed a request.");

            await _studentRepository.DeleteStudentAsync(studentId);

            return new NoContentResult();
        }
        //Endpoint for adding new student
        [FunctionName("CreateStudent")]
        public async Task<IActionResult> CreateStudents(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "students")] HttpRequest req,
        ILogger log)
        {
            log.LogInformation("C# HTTP Post trigger function processed a request.");
            
            // Read data from request body
            string requestBody = await req.HttpContext.Request.ReadAsStringAsync();
            // Transform string to Student object using serialization
            Student student = _jsonConverterWrapper.DeserializeObject<Student>(requestBody);

            await _studentRepository.AddStudentAsync(student);

            return new OkResult();
        }
        //Endpoint to update current student
        [FunctionName("UpdateStudents")]
        public async Task<IActionResult> UpdateStudents(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "students")] HttpRequest req,
        ILogger log)
        {
            log.LogInformation("C# HTTP Update trigger function processed a request.");
            
            // Read request body
            string requestBody = await req.HttpContext.Request.ReadAsStringAsync();

            // Transform string to Student object using serialization
            Student student = _jsonConverterWrapper.DeserializeObject<Student>(requestBody);

            await _studentRepository.UpdateStudentAsync(student);

            return new NoContentResult();
        }
    }
}
