using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Shared.Entities
{
    public class State
    {
        public int Id { get; set; }

        [Display(Name = "Estado/Departamento")]
        [Required(ErrorMessage = "El cmapo {0} es obligatorio")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede mas de {1} caracteres")]

        public string? Name { get; set; } = null!;

        /*Este control Id ademas de que el Entity lo creá. nos sirve como una manera
        de señalar en este caso, la ciudad que estabamos modificando al devolvernos
        en una página*/
        public int CountryId { get; set; }

        public Country? Country { get; set; }

        public ICollection<City>? Cities { get;set;}

        public int CitiesNumbre => Cities == null ? 0 : Cities.Count;
    }
}
