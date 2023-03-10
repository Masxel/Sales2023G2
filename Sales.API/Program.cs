using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.API.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(X => X.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("name=DefaultConnection"));
//AddTransient solo se ejecuta una vez para realizar la insercción de datos, y no se volvera a ejecutar por ninguna otra clase
builder.Services.AddTransient<SeedDB>();
builder.Services.AddScoped<IApiService, ApiService>();


var app = builder.Build();

//Se realizara una injección manualmente ya que el program no permite realizarlas automaticamente ni constructores. 
SeedData(app);
void SeedData(WebApplication app)
{
    IServiceScopeFactory? scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (IServiceScope? scope = scopedFactory!.CreateScope())
    {
        SeedDB? service = scope.ServiceProvider.GetService<SeedDB>();
        //Los metodos asincronos se llaman con el operador await; pero como en este metodo
        //no puedo realizar metodos asincronos recurrimos al metodo Wait(); que hace que el subproceso que realiza la llamada espere hasta que se haya completado la tarea actual. 
        service!.SeedAsync().Wait();
    }

}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
