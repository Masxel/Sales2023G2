using Sales.Shared.Entities;

namespace Sales.API.Data
{
    public class SeedDB
    {
        private readonly DataContext _context;


        public SeedDB(DataContext context)
        {
            _context = context;
        }

        //El método EnsureCreatedAsync() verifica si la base de datos ya existe y, si no es así, crea la base de datos y las tablas correspondientes basándose en el modelo de datos definido en el contexto de Entity Framework Core.
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckCategoriesAsync();
        }

        private async Task CheckCountriesAsync()
        {
            //El metodo Any retorna un true si encuentra por los menos un registro.
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country { Name = "Colombia" });
                _context.Countries.Add(new Country { Name = "Perú" });
                _context.Countries.Add(new Country { Name = "México" });
                _context.Countries.Add(new Country { Name = "Canadá" });
                _context.Countries.Add(new Country { Name = "Brasil" });
                _context.Countries.Add(new Country { Name = "Panamá" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Ropa" });
                _context.Categories.Add(new Category { Name = "Electrónica" });
                _context.Categories.Add(new Category { Name = "Vehículos" });
                _context.Categories.Add(new Category { Name = "Alimentos y bebidas" });
                _context.Categories.Add(new Category { Name = "Herramientas" });
                _context.Categories.Add(new Category { Name = "Decoración" });
                await _context.SaveChangesAsync();
            }
        }
    }
}
