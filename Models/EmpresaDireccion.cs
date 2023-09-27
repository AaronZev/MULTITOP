using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Prototipo.Models
{
    public class EmpresaDireccion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int empresa_direccion_id { get; set; }
        public int empresa_codigo { get; set; }

        [Required(ErrorMessage = "El campo dirección es obligatorio.")]
        [StringLength(255, ErrorMessage = "La longitud máxima para dirección es 255 caracteres.")]
        public string? direccion { get; set; }

    }
}
