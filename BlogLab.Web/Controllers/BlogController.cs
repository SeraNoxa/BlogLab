using BlogLab.Models.Blog;
using BlogLab.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace BlogLab.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _blogReposity;
        private readonly IPhotoRepository _photoRepository;

        public BlogController (IBlogRepository blogRepository, IPhotoRepository photoRepository)
        {
            _blogReposity = blogRepository;
            _photoRepository = photoRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BlogController>> Create (BlogCreate blogCreate)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

            if(blogCreate.PhotoId.HasValue)
            {
                var photo = await _photoRepository.GetAsync(blogCreate.PhotoId.Value);

                if(photo.ApplicationUserId != applicationUserId)
                {
                    return BadRequest("You did not upload the photo");
                }

            }
            var blog = await _blogReposity.UpsertAsync(blogCreate, applicationUserId);

            return Ok(blog);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResults<Blog>>> GetAll([FromQuery] BlogPaging blogPaging)
        {
            var blogs = await _blogReposity.GetAllAsync(blogPaging);
            return Ok(blogs);
        }

        [HttpGet("{blogId}")]
        public async Task<ActionResult<Blog>> Get(int blogId)
        {
            var blog = await _blogReposity.GetAsync(blogId);

            return Ok(blog);
        }

        [HttpGet("user/{applicationUserId}")]
        public async Task<ActionResult<List<Blog>>> GetByApplicationUserID(int applicationUserId)
        {
            var blogs = await _blogReposity.GetAllByUserIdAsync(applicationUserId);

            return Ok(blogs);
        }


        [HttpGet("famous")]
        public async Task<ActionResult<List<Blog>>> GetAllFamous(int applicationUserId)
        {
            var blogs = await _blogReposity.GetAllFamousAsync();

            return Ok(blogs);
        }

        [Authorize]
        [HttpDelete("{blogId}")]
        public async Task<ActionResult<List<int>>> Delete(int blogId)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);
            var foundBlog = await _blogReposity.GetAsync(blogId);

            if (foundBlog == null) return BadRequest("Blog does not exist");

            if(foundBlog.ApplicationUserId == applicationUserId)
            {
                var affectedRows = await _blogReposity.DeleteAsync(blogId);

                return Ok(affectedRows);
            }
            else
            {
                return BadRequest("You did not create this blog!");
            }
        }
    }
}
