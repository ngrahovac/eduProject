using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.IO;
using Microsoft.AspNetCore.Http;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("files")]
    public class UploadController : ControllerBase
    {
        [DisableRequestSizeLimit]
        [HttpPost("images")]
        public async Task<ActionResult<string>> UploadImage()
        {
            try
            {
                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    var filename = Guid.NewGuid().ToString() + ".jpg";
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Images", filename);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return Ok(filename);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
