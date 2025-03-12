using Draftboard.Api.Data;
using Draftboard.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Draftboard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly DraftboardDbContext _context;

        public StatusController(DraftboardDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public async Task<string> GetAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            return "OK";
        }
    }
}
