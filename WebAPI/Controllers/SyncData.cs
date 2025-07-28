using WebAPI.Models;
using WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThirdPartyController : ControllerBase
    {
        private readonly ThirdPartyAccess _thirdPartyAccess;

        public ThirdPartyController(ThirdPartyAccess thirdPartyAccess)
        {
            _thirdPartyAccess = thirdPartyAccess;
        }

         
        [HttpGet("post/{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
             var post = await _thirdPartyAccess.GetPostByIdAsync(id);
             if (post == null)
                 return NotFound();

             return Ok(post);
        }
        [HttpGet("comment/{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
              var comment = await _thirdPartyAccess.GetCommentByIdAsync(id);
              if (comment == null)
                    return NotFound();

              return Ok(comment);
        }
    }
}

