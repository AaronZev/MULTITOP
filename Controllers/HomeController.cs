using Azure;
using Microsoft.AspNetCore.Mvc;
using Prototipo.Data;
using Prototipo.Models;
using System;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace Prototipo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult MiMetodo(string ruc, string razon_social, string estado, string[] direcciones)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    if (_context.Empresas.Any(e => e.ruc == ruc) || _context.Empresas.Any(e => e.razon_social == razon_social))
                    {
                        ModelState.AddModelError(string.Empty, "El RUC o la razón social ya existen en el sistema.");
                        return View("Index");
                    }

                    var empresa = new Empresa
                    {
                        ruc = ruc,
                        razon_social = razon_social,
                        activo = estado == "activo"
                    };

                    _context.Empresas.Add(empresa);
                    _context.SaveChanges();

                    

                    HashSet<string> direccionesUnicas = new HashSet<string>();

                    bool unicas = true;
                    foreach (var direccion in direcciones)
                    {
                        if (direccion == null || direcciones.Length == 0)
                        {
                            ModelState.AddModelError(string.Empty, "Debe ingresar al menos una dirección.");
                            return View("Index");
                        }

                        if (!direccionesUnicas.Add(direccion))
                        {
                            unicas = false;
                            break;
                        }
                    }

                    if (!unicas)
                    {
                        ModelState.AddModelError(string.Empty, "Las direcciones no pueden repetirse.");
                        return View("Index");
                    }

                    foreach (var direccion in direcciones)
                    {

                        var empresaDireccion = new EmpresaDireccion
                        {
                            empresa_codigo = empresa.codigo,
                            direccion = direccion
                        };

                        _context.EmpresaDireccion.Add(empresaDireccion);
                    }

                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }

                return View("Index");
            }
            catch(Exception )
            {
                ModelState.AddModelError(string.Empty, "Revise los datos ingresados para grabar el registro.");
                return View("Index");
            }

           
        }


        [HttpGet]
        public IActionResult BuscarPorCodigo(int codigo)
        {
            var empresa = _context.Empresas.FirstOrDefault(e => e.codigo == codigo);
            if (empresa != null)
            {
                var direcciones = _context.EmpresaDireccion
                    .Where(ed => ed.empresa_codigo == empresa.codigo)
                    .Select(ed => ed.direccion)
                    .ToList<string?>();

                var resultado = new
                {
                    codigo = empresa.codigo,
                    ruc = empresa.ruc,
                    razon_social = empresa.razon_social,
                    estado = empresa.activo ? "activo" : "inactivo",
                    direcciones = direcciones ?? new List<string>()
                };
                return Json(resultado);

            }
            else
            {
                var resultado = new
                {
                    codigo = "",
                    ruc = "",
                    razon_social = "",
                    estado = "",
                    direcciones = new List<string>(),
                    mostrarTablaDirecciones = false
                };


                return Json(resultado);

            }

        }

        [HttpDelete]
        public IActionResult EliminarEmpresa(int codigo)
        {
            var empresa = _context.Empresas.SingleOrDefault(e => e.codigo == codigo);
            var direcciones = _context.EmpresaDireccion
                    .Where(ed => ed.empresa_codigo == empresa.codigo);

            if (empresa != null)
            {
                foreach (var item in direcciones)
                {
                    _context.EmpresaDireccion.Remove(item);

                }
                _context.Empresas.Remove(empresa);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index"); 
        }


    }

}
