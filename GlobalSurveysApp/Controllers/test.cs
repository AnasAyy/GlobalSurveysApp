using GlobalSurveysApp.Data.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlobalSurveysApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class test : ControllerBase
    {
        private readonly IEncryptRepo encryptRepo;

        public test(IEncryptRepo encryptRepo)
        {
            this.encryptRepo = encryptRepo;
        }
        [HttpPost("tedst")]
        public ActionResult tedst(string tesst)
        {
            var x=encryptRepo.DecryptPassword(tesst);
            return Ok(x);
        }
    }
}
