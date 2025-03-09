using API_Commerce.ModelsDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class MunicipalityController : ControllerBase
    {
        private readonly CommerceContext _context;

        public MunicipalityController(CommerceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays the list of municipalities in the database
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Municipality>>> GetMunicipalities()
        {
            var municipalities = await _context.Municipalities.ToListAsync();
            return Ok(municipalities);
        }
    }
}
