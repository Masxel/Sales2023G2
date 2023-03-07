using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.Shared.Entities;

namespace Sales.API.Controllers
{

    [ApiController]
    [Route("/api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public CitiesController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _dataContext.Cities.ToListAsync());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(int id)
        {
            var cities = await _dataContext.Cities.FirstOrDefaultAsync(x => x.Id == id);
            if (cities == null)
            {
                return NotFound();
            }
            return Ok(cities);
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(City city)
        {
            try
            {
                _dataContext.Update(city);
                await _dataContext.SaveChangesAsync();
                return Ok(city);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe una ciudad con el mismo nombre");
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
            var cities = await _dataContext.Cities.FirstOrDefaultAsync(x => x.Id == id);
            if (cities == null)
            {
                return NotFound();
            }


            _dataContext.Remove(cities);
            await _dataContext.SaveChangesAsync();
            return NoContent();
        }




        [HttpPost]
        public async Task<ActionResult> PostAsync(City cities)
        {
            try
            {
                _dataContext.Cities.Add(cities);
                await _dataContext.SaveChangesAsync();
                return Ok(cities);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe una ciudad con el mismo nombre");
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

