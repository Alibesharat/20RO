using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using web.util;

namespace WepApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly HostingEnvironment _env;
        public ApiController(HostingEnvironment environment)
        {
            _env = environment;
        }

        [HttpPost(nameof(Getdriverpic))]
        public async Task<IActionResult> Getdriverpic()
        {
            var files = HttpContext.Request.Form.Files;
            List<string> AllowedFileExtention = new List<string>()
            {
               ".png",
               ".jpg"
            };
            if (files != null)
            {
                var s = files[0];
                var (state, name) = await WorkFile.UploadFileAsync(s, "Dynamics/Drivers", FileType.image, AllowedFileExtention, _env);
                if (state)
                    return Ok(name);
                return Ok(name);
            }
            return Ok("false");
        }
    }
}