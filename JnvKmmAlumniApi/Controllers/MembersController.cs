using JnvKmmAlumniApi.Entities;
using JnvKmmAlumniApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace JnvKmmAlumniApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        // Simple in-memory thread-safe store for demonstration.
        // Replace with a DbContext or repository in production.
        private static readonly ConcurrentDictionary<string, Members> _store = new();
        private readonly MemberRepository _repository;
        public MembersController(MemberRepository repository)
        {
            _repository = repository;
        }

        // GET: api/members
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var members = await _repository.GetMembersAsync();
            return Ok(members);
        }

        // GET: api/members/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var members = await _repository.GetMembersAsync(id);
            return Ok(members);
        }

        // POST: api/members
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Members member)
        {
            if (member == null)
                return BadRequest("Member payload is required.");

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            // Ensure an Id exists
            //if (string.IsNullOrWhiteSpace(member.Id))
            //    member.Id = Guid.NewGuid().ToString();

            // Set timestamps
            var now = DateTime.UtcNow;
            member.JoinedDate ??= now;
            member.ModifiedDate = now;
            member.LastTs = now;

            if (member.ProfilePhoto == null || member.ProfilePhoto.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "ProfileImages");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, member.ProfilePhoto.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await member.ProfilePhoto.CopyToAsync(stream);
            }

            // last identity value
            var lastMemberId = await _repository.GetLastMemberId();
            var newMemberId = $"Jnvp{member.YearFrom}_{lastMemberId + 1}";

            member.Id = newMemberId;
            member.LastTs = now;
            member.FilePath = "ProfileImages/" + member.ProfilePhoto.FileName;
            member.FileName = member.ProfilePhoto.FileName;
            member.RoleId = member.YearFrom == 2000 || member.YearTo == 2007 ? 2 : 3;
            // Save to in-memory store (upsert behavior)
            //_store[member.Id] = member;

            var result = await _repository.InsertMemberAsync(member);

            if (result == -1 || result > 0)
            {
                var restult1 = _repository.InsertMemberIdentity(newMemberId);
                return Ok(new { message = "Member added successfully" });
            }

            return BadRequest("Failed to add member");

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(string id, [FromForm] Members member)
        {
            member.Id = id;
            member.ModifiedDate = DateTime.Now;
            var result = await _repository.UpdateMemberAsync(member);

            if (result > 0)
                return Ok(new { message = "Member updated successfully" });

            return BadRequest("Failed to update member");
        }
    }
}
