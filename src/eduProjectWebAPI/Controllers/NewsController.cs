using eduProjectModel.Display;
using eduProjectModel.Domain;
using eduProjectModel.Input;
using eduProjectWebAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NewsController : ControllerBase
    {
        private readonly INewsRepository newsRepository;

        public NewsController(INewsRepository newsRepository)
        {
            this.newsRepository = newsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewsInputModel model)
        {
            try
            {
                News news = new News();

                model.MapTo(news);

                await newsRepository.AddAsync(news);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + "\n" + e.StackTrace);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<ICollection<NewsDisplayModel>>> GetAll()
        {
            try
            {
                var news = await newsRepository.GetAllAsync();

                return news.Select(n => new NewsDisplayModel(n)).ToList();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + "\n" + e.StackTrace);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ICollection<NewsDisplayModel>>> Delete(int id)
        {
            try
            {
                var news = await newsRepository.GetByIdAsync(id);
                await newsRepository.DeleteAsync(news);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + "\n" + e.StackTrace);
            }
        }
    }
}
