using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Prototipo.Models
{
    //public class Empresa
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int empresa_id { get; set; }
    //    public string? codigo { get; set; }
    //    public string? ruc { get; set; }
    //    public string? razon_social { get; set; }
    //    public bool activo { get; set; }
    //    public List<EmpresaDireccion>? direcciones { get; set; }


    //}
    public class Empresa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int codigo { get; set; }

        [Required(ErrorMessage = "El campo RUC es obligatorio.")]
        [StringLength(11, ErrorMessage = "La longitud debe ser de 11 dígitos.")]
        public string? ruc { get; set; }

        [Required(ErrorMessage = "El campo razón social es obligatorio.")]
        [StringLength(255, ErrorMessage = "La longitud máxima para razón social es 255 caracteres.")]
        public string? razon_social { get; set; }

        public bool activo { get; set; }
    }


}
