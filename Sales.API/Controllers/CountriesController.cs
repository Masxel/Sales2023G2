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



        [HttpPost]
        public async Task<ActionResult> PostAsync(Country country)
        {
            try
            {
                _dataContext.Countries.Add(country);
                await _dataContext.SaveChangesAsync();
                return Ok(country);
            }
            catch (Exception ex)
            {
                return NoContent();
            }
        }
        


    }
}
