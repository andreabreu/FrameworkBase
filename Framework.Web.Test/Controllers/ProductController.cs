using Framework.Core.Repository.Interfaces;
using Framework.Web.Test.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Framework.Web.Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ITestRepository _testRepository;

        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger, ITestRepository testRepository, IUnitOfWork uow)
        {
            _logger = logger;
            _testRepository = testRepository;
        }


        [HttpGet("private")]
        [Authorize(Policy = "ceate:users")]
        public IActionResult Private()
        {
            return Ok(new
            {
                Message = "Hello from a private endpoint! You need to be authenticated to see this."
            });
        }


        [HttpGet]
        public async Task<ActionResult> NewProdut()
        {
            _testRepository.Add(new Product() { Name = "PS4" });

            return Ok();
        }

    }
}
