using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebAPI.Services;
using WebAPI.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ThirdPartyAccess _thirdPartyService;

        public PostsController(IConfiguration config, ThirdPartyAccess thirdPartyService)
        {
            _config = config;
            _thirdPartyService = thirdPartyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var posts = new List<Post>();
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();

            var command = new SqlCommand("SELECT Id, UserId, Title, Body FROM Posts", connection);
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                posts.Add(new Post
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    Title = reader.GetString(2),
                    Body = reader.GetString(3)
                });
            }
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            Post? post = null;
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();

            var command = new SqlCommand("SELECT Id, UserId, Title, Body FROM Posts WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                post = new Post
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    Title = reader.GetString(2),
                    Body = reader.GetString(3)
                };
                return Ok(post);
            }

            post = await _thirdPartyService.GetPostByIdAsync(id);
            if (post == null) return NotFound();

            await using var insertConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await insertConnection.OpenAsync();

            var insert = new SqlCommand(@"INSERT INTO Posts (Id, UserId, Title, Body) 
                                          VALUES (@Id, @UserId, @Title, @Body)", insertConnection);
            insert.Parameters.AddWithValue("@Id", post.Id);
            insert.Parameters.AddWithValue("@UserId", post.UserId);
            insert.Parameters.AddWithValue("@Title", post.Title);
            insert.Parameters.AddWithValue("@Body", post.Body);
            await insert.ExecuteNonQueryAsync();

            return Ok(post);
        }
    }
}
