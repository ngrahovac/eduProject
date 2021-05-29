using eduProjectModel.Display;
using eduProjectModel.Domain;
using eduProjectModel.Input;
using eduProjectWebAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class NewsController : ControllerBase
    {
        private readonly INewsRepository newsRepository;

        public NewsController(INewsRepository newsRepository)
        {
            this.newsRepository = newsRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create(NewsInputModel model)
        {
            try
            {
                News news = new News();

                model.MapTo(news);

                await newsRepository.AddAsync(news);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + "\n" + e.StackTrace);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
