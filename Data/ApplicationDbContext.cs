using Microsoft.EntityFrameworkCore;
using Prototipo.Models;

namespace Prototipo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<EmpresaDireccion> EmpresaDireccion { get; set; }
       

    }
}
