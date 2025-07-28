using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebAPI.Services;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ThirdPartyAccess _thirdPartyService;

        public CommentsController(IConfiguration config, ThirdPartyAccess thirdPartyService)
        {
            _config = config;
            _thirdPartyService = thirdPartyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = new List<Comment>();
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();

            var command = new SqlCommand("SELECT Id, PostId, Name, Email, Body FROM Comments", connection);
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                comments.Add(new Comment
                {
                    Id = reader.GetInt32(0),
                    PostId = reader.GetInt32(1),
                    Name = reader.GetString(2),
                    Email = reader.GetString(3),
                    Body = reader.GetString(4)
                });
            }

            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID. ID must be a positive number.");

            try
            {
            Comment? comment = null;
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();

            var command = new SqlCommand("SELECT Id, PostId, Name, Email, Body FROM Comments WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                comment = new Comment
                {
                    Id = reader.GetInt32(0),
                    PostId = reader.GetInt32(1),
                    Name = reader.GetString(2),
                    Email = reader.GetString(3),
                    Body = reader.GetString(4)
                };
                return Ok(comment);
            }

            comment = await _thirdPartyService.GetCommentByIdAsync(id);
            if (comment == null) return NotFound();

            await using var insertConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await insertConnection.OpenAsync();

            var insert = new SqlCommand(@"INSERT INTO Comments (Id, PostId, Name, Email, Body) 
                                          VALUES (@Id, @PostId, @Name, @Email, @Body)", insertConnection);
            insert.Parameters.AddWithValue("@Id", comment.Id);
            insert.Parameters.AddWithValue("@PostId", comment.PostId);
            insert.Parameters.AddWithValue("@Name", comment.Name);
            insert.Parameters.AddWithValue("@Email", comment.Email);
            insert.Parameters.AddWithValue("@Body", comment.Body);
            await insert.ExecuteNonQueryAsync();

            return Ok(comment);
        }
            catch (SqlException ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(503, $"Third-party API unreachable: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }
    }
}
