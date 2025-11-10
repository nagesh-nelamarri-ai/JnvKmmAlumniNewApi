using JnvKmmAlumniApi.Entities;
using JnvKmmAlumniApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace JnvKmmAlumniApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventsRepository _repository;

        public EventsController(IEventsRepository repository)
        {
            _repository = repository;
        }

        // GET: api/events
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var events = await _repository.GetAllEvents();
            return Ok(events);
        }

        // POST: api/events
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] EventData request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "EventFiles");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, request.File.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }

            // You can save metadata (title, dateOfJoin) to a database here if needed
            request.FilePath = "EventFiles/" + request.File.FileName;
            request.FileName = request.File.FileName;

            var result = await _repository.CreateEvent(request);
            if (result == -1 || result > 0)
            {
                return Ok(new
                {
                    Message = "File uploaded successfully",
                    request.Title
                });
            }

            return BadRequest("Failed to add event");
        }
    }
}