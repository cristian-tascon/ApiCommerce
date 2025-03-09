using API_Commerce.ModelsDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Commerce.Dto;
using System.Text;

namespace API_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] 
    public class BusinessmanController : ControllerBase
    {
        private readonly CommerceContext _context;
        public BusinessmanController(CommerceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the list of merchants with pagination.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Number of records per page.</param>
        /// <returns>List of paginated merchants.</returns>
        /// <response code="200">Returns the list of merchants.</response>
        /// <response code="400">If the parameters are invalid.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Businessman>>> GetBusinessmen(int page = 1, int pageSize = 5)
        {
            var businessmen = await _context.Businessmen
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return Ok(businessmen);
        }

        /// <summary>
        /// Gets a merchant by id.
        /// </summary>
        /// <response code="200">Returns a merchants.</response>
        /// <response code="400">If the parameters are invalid.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Businessman>> GetBusinessman(int id)
        {
            var businessman = await _context.Businessmen.FindAsync(id);
            if (businessman == null)
                return NotFound();
            return Ok(businessman);
        }

        /// <summary>
        /// Create a merchant
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Businessman>> CreateBusinessman([FromBody] BusinessmanDto dto)
        {
            var businessman = new Businessman
            {
                BusName = dto.Bus_Name,
                BusPhoneNumber = dto.Bus_Phone_Number,
                BusEmail = dto.Bus_Email,
                BusDateRegistration = DateOnly.FromDateTime(DateTime.UtcNow),
                BusStatus = dto.Bus_Status,
                BusMunicipality = dto.Bus_Municipality
            };
            _context.Businessmen.Add(businessman);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBusinessman), new { id = businessman.BusId }, businessman);
        }

        /// <summary>
        /// Update a merchant
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBusinessman(int id, [FromBody] BusinessmanDto dto)
        {
            var businessman = await _context.Businessmen.FindAsync(id);
            if (businessman == null)
                return NotFound();
            businessman.BusName = dto.Bus_Name;
            businessman.BusPhoneNumber = dto.Bus_Phone_Number;
            businessman.BusEmail = dto.Bus_Email;
            businessman.BusMunicipality = dto.Bus_Municipality;
            await _context.SaveChangesAsync();
            return NoContent();
        }


        /// <summary>
        /// Delete  a merchant
        /// </summary>
        //[Authorize(Roles = "Administrador")] 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusinessman(int id)
        {
            var businessman = await _context.Businessmen.FindAsync(id);
            if (businessman == null)
                return NotFound();
            _context.Businessmen.Remove(businessman);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Update the status of a merchant
        /// </summary>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateBusinessmanStatus(int id, [FromBody] BusinessmanDto dto)
        {
            var businessman = await _context.Businessmen.FindAsync(id);
            if (businessman == null)
                return NotFound();
            businessman.BusStatus = dto.Bus_Status;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Generates the active merchants report
        /// </summary>
        [HttpGet("report")]
        public async Task<IActionResult> GetActiveBusinessmenReport()
        {
            var reportData = await _context.Database.SqlQueryRaw<BusinessmanReportDto>("EXEC sp_GetActiveBusinessmenReport").ToListAsync();
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Nombre_Comerciante|Municipio|Telefono|Correo_Electronico|Fecha_Registro|Estado|Cantidad_Establecimientos|Total_Ingresos|Cantidad_Empleados");
            foreach (var item in reportData)
            {
                stringBuilder.AppendLine(
                    $"{item.Nombre_Comerciante}|" +
                    $"{item.Municipio}|" +
                    $"{(string.IsNullOrEmpty(item.Telefono) ? "SIN TELEFONO" : item.Telefono)}|" +
                    $"{(string.IsNullOrEmpty(item.Correo_Electronico) ? "SIN EMAIL" : item.Correo_Electronico)}|" +
                    $"{item.Fecha_Registro:yyyy-MM-dd}|" +
                    $"{item.Estado}|" +
                    $"{item.Cantidad_Establecimientos}|" +
                    $"{item.Total_Ingresos}|" +
                    $"{item.Cantidad_Empleados}"
                );
            }
            var fileBytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
            return File(fileBytes, "text/csv", "Businessmen_Report.csv");
        }
    }
}