using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.Shared.Entities;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("/api/countries")]
    public class CountriesController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public CountriesController(DataContext context)         
        {
            _dataContext = context;
        }


        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _dataContext.Countries.ToListAsync());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(int id)
        {
           var country = await _dataContext.Countries.FirstOrDefaultAsync(x => x.Id == id);
            if(country == null)
            {
                return NotFound();
            }
            return Ok(country);
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(Country country)
        {
            try
            {
                _dataContext.Update(country);
                await _dataContext.SaveChangesAsync();
                return Ok(country);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un país con el mismo nombre");
                }
                return BadRequest(dbUpdateException.Message);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
          var country = await _dataContext.Countries.FirstOrDefaultAsync(x => x.Id == id);
            if(country == null)
            {
                return NotFound();
            }


            _dataContext.Remove(country);
            await _dataContext.SaveChangesAsync();
            return NoContent();
        }




        [HttpPost]
        public async Task<ActionResult> PostAsync(Country country)
        {
            try
            {
                _dataContext.Countries.Add(country);
                await _dataContext.SaveChangesAsync();
                return Ok(country);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un país con el mismo nombre");
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
