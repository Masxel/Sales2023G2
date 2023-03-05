﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _dataContext.Countries
                .Include(x => x.States)
                .ToListAsync());
        }

        [HttpGet("full")]
        public async Task<IActionResult> GetFullAsync()
        {
            /*Aquí vamos a pedir que al traer los países, me incluya la colleccion 
             donde vienen todos los estados de cada país. Y a cada estado solicitarle
            que me retorne la lista de cada país que les pertenece.
            La primer 'x' especifica que los países se vincularan a los estados
            y la segunda 'x'(Con ThenInclude) me incluye las ciudades de cada país*/
            return Ok(await _dataContext.Countries
                .Include(x => x.States!)
                .ThenInclude(x => x.Cities)
                .ToListAsync());
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
