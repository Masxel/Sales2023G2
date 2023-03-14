using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.API.Helpers;
using Sales.Shared.DTOs;
using Sales.Shared.Entities;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("/api/states")]
    public class SatesController : ControllerBase
    {

        private readonly DataContext _dataContext;

        public SatesController(DataContext context)
        {
            _dataContext = context;
        }


        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable = _dataContext.States
                .Include(x => x.Cities)
                .Where(x => x.Country!.Id == pagination.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }


            return Ok(await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync());
        }

        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDTO pagination)
        {
            var queryable = _dataContext.States
                .Where(x => x.Country!.Id == pagination.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }



        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetAsync(int id)
        {
            var state = await _dataContext.States.Include(x => x.Cities)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (state == null)
            {
                return NotFound();
            }
            return Ok(state);
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(State state)
        {
            try
            {
                _dataContext.Update(state);
                await _dataContext.SaveChangesAsync();
                return Ok(state);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un departamento/estado con el mismo nombre");
                }
                return BadRequest(dbUpdateException.Message);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var state = await _dataContext.States.FirstOrDefaultAsync(x => x.Id == id);
                if (state == null)
                {
                    return NotFound();
                }
                _dataContext.Remove(state);
                await _dataContext.SaveChangesAsync();
                return NoContent();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }




        [HttpPost]
        public async Task<ActionResult> PostAsync(State state)
        {
            try
            {
                _dataContext.States.Add(state);
                await _dataContext.SaveChangesAsync();
                return Ok(state);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un departamento/estado con el mismo nombre");
                }
                return BadRequest(dbUpdateException.Message);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }



    }
}
