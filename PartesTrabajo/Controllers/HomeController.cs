using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PartesTrabajo.Models;

namespace PartesTrabajo.Controllers
{
    public class HomeController : Controller
    {
        PartesEntities context = new PartesEntities();
        List<parte_trabajo> partes_trabajo = new List<parte_trabajo>();

        public class DatosRecibidos
        {
            public string Fecha { get; set; }
            public string Empleado { get; set; }
            public string Tiempo { get; set; }
            public string Nota { get; set; }
            public List<parte_trabajo> datos { get; set; }
        }

        public ActionResult Index(string name, string fecha)
        {
            ViewBag.user = name;
            ViewBag.fecha = fecha;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult GetTrabajos()
        {
            context.Configuration.ProxyCreationEnabled = false;
            return Json(context.Trabajoes.OrderBy(c => c.Nombre).ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClientes()
        {
            context.Configuration.ProxyCreationEnabled = false;
            return Json(context.Clientes.OrderBy(c => c.Nombre).ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProyectos()
        {
            context.Configuration.ProxyCreationEnabled = false;
            return Json(context.Proyectoes.OrderBy(c => c.Nombre).ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetParte_Trabajo(string empleado, string fecha)
        {
            //context.Configuration.ProxyCreationEnabled = false;
            try
            {
                DateTime dfecha = Convert.ToDateTime(fecha.Replace('-','/'));
                int ide = context.Empleadoes.FirstOrDefault(c => c.Nombre == empleado).IdEmpleado;
                var p = context.Partes.FirstOrDefault(c => c.Fecha == dfecha && c.IdEmpleado == ide);
                var pt = context.Parte_Trabajo.OrderBy(c => c.FechaInicial).Where(c => c.IdParte == p.IdParte).ToList();
                for (int i = 0; i < pt.Count; i++)
                {
                    parte_trabajo np = new parte_trabajo();
                    try
                    {
                        np.Cliente = "";
                        np.Cliente = pt[i].Cliente.Nombre;
                    }
                    catch { }
                    try
                    {
                        np.Trabajo = "";
                        np.Trabajo = pt[i].Trabajo.Nombre;
                    }
                    catch { }
                    try
                    {
                        np.Proyecto = "";
                        np.Proyecto = pt[i].Proyecto.Nombre;
                    }
                    catch { }
                    np.Nota = pt[i].Nota;
                    try
                    {
                        np.HoraInicial = Convert.ToDateTime(pt[i].FechaInicial.ToString()).ToShortTimeString();
                        np.HoraFinal = Convert.ToDateTime(pt[i].FechaFinal.ToString()).ToShortTimeString();
                        np.Horas = (Convert.ToDateTime(pt[i].FechaFinal.ToString()) - Convert.ToDateTime(pt[i].FechaInicial.ToString())).TotalMinutes;
                    }
                    catch { }
                    partes_trabajo.Add(np);
                }
            }
            catch { }
            return Json(partes_trabajo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void Confirmar(DatosRecibidos dts)
        {
            DateTime fp = Convert.ToDateTime(dts.Fecha);
            string[] time = dts.Tiempo.Split(' ');
            var p = context.Partes.FirstOrDefault(c => c.Fecha == fp && c.Empleado.Nombre == dts.Empleado);
            if (p == null)
            {
                Parte np = new Parte();
                np.Fecha = fp;
                var em = context.Empleadoes.FirstOrDefault(c => c.Nombre == dts.Empleado);
                np.Empleado = em;
                np.Nota = dts.Nota;            
                context.Partes.Add(np);
                context.SaveChanges();
            }

            p = context.Partes.FirstOrDefault(c => c.Fecha == fp && c.Empleado.Nombre == dts.Empleado);
            //p.Nota = dts.Nota;
            string[] str = time[2].Split(':');
            double horas = (double.Parse(str[0]) * 60) + double.Parse(str[1]);
            p.Tiempo = horas / 60;
            p.Parte_Trabajo.Clear();
            for (int i = 0; i < dts.datos.Count; i++)
            {
                Parte_Trabajo pt = new Parte_Trabajo();
                pt.IdParte = p.IdParte;
                string cname = dts.datos[i].Cliente;
                if (cname != "")
                {
                    var nc = context.Clientes.FirstOrDefault(c => c.Nombre == cname);
                    pt.Cliente = nc;
                }
                string tname = dts.datos[i].Trabajo;
                if (tname != "")
                {
                    var nt = context.Trabajoes.FirstOrDefault(c => c.Nombre == tname);
                    pt.Trabajo = nt;
                }
                string pname = dts.datos[i].Proyecto;
                if (pname != "")
                {
                    var np = context.Proyectoes.FirstOrDefault(c => c.Nombre == pname);
                    pt.Proyecto = np;
                }
                pt.FechaInicial = Convert.ToDateTime(dts.Fecha + " " + dts.datos[i].HoraInicial);
                pt.FechaFinal = Convert.ToDateTime(dts.Fecha + " " + dts.datos[i].HoraFinal);
                pt.Nota = dts.datos[i].Nota;
                if (pt.Nota == null)
                    pt.Nota = "";
                context.Parte_Trabajo.Add(pt);
            }
            context.SaveChanges();
        }
    }
}
