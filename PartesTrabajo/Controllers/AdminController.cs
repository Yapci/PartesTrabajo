using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PartesTrabajo.Models;
using System.Data.SqlClient;
using System.Data;

namespace PartesTrabajo.Controllers
{
    public class AdminController : Controller
    {
        PartesEntities context = new PartesEntities();
        List<parte_trabajo_admin> partes_trabajo = new List<parte_trabajo_admin>();
        List<serie> series = new List<serie>();

        public ActionResult Index()
        {
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

        public JsonResult GetEmpleados()
        {
            context.Configuration.ProxyCreationEnabled = false;
            //return Json(context.Empleadoes.OrderBy(c => c.Nombre).ToList(), JsonRequestBehavior.AllowGet);
            var aa = context.Partes.OrderBy(c => c.Empleado.Nombre).Select(c => c.Empleado).Distinct().ToList();
            return Json(aa.OrderBy(c => c.Nombre), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmpleados1(string fini, string ffin)
        {
            SqlConnection cn = new SqlConnection("Data Source=SERCAUDAL2;Initial Catalog=Partes;User id=sa;Password=Demase49;");
            cn.Open();
            if (fini == null)
                fini = DateTime.Now.AddDays(-1).ToShortDateString();
            if (ffin == null)
            {
                if (fini == null)
                    ffin = DateTime.Now.ToShortDateString();
                else
                    ffin = Convert.ToDateTime(fini).AddDays(1).ToShortDateString();
            }

            SqlDataAdapter da = new SqlDataAdapter("select Proyecto.Nombre as Proyecto,t3.Nombre,Minutos from (select IdProyecto,Nombre, SUM(Tiempo) as Minutos from (select IdProyecto,DATEDIFF(mi,FechaInicial,FechaFinal) as Tiempo,Nombre from(select IdProyecto,FechaInicial,FechaFinal,IdEmpleado,Parte_Trabajo.IdParte from Parte_Trabajo inner join Parte on Parte.IdParte = Parte_Trabajo.IdParte where FechaInicial >= '" + fini + "' and FechaFinal < '" + ffin + "') as t1 inner join Empleado on Empleado.IdEmpleado = t1.IdEmpleado where t1.IdParte is not null ) as t2 group by IdProyecto,Nombre) as t3 inner join Proyecto on Proyecto.IdProyecto = t3.IdProyecto order by t3.Nombre, Proyecto, t3.IdProyecto", cn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            var users = dt.AsEnumerable().Select(c => c.ItemArray[1]).Distinct().ToList();
            return Json(users, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetParteTrabajo()
        {
            try
            {
                var p = context.Partes.ToList();
                for (int j = 0; j < p.Count; j++)
                {
                    int pid = p[j].IdParte;
                    var pt = context.Parte_Trabajo.Where(c => c.IdParte == pid).ToList();
                    for (int i = 0; i < pt.Count; i++)
                    {
                        parte_trabajo_admin np = new parte_trabajo_admin();
                        try
                        {
                            np.Cliente = pt[i].Cliente.Nombre;
                        }
                        catch { np.Cliente = ""; }
                        try
                        {
                            np.Trabajo = pt[i].Trabajo.Nombre;
                        }
                        catch { np.Trabajo = ""; }
                        try
                        {
                            np.Proyecto = pt[i].Proyecto.Nombre;
                        }
                        catch { np.Proyecto = ""; }
                        np.Nota = pt[i].Nota;
                        try
                        {
                            DateTime date = Convert.ToDateTime(pt[i].FechaInicial.ToString());
                            np.FechaInicial = (long)(date - new DateTime(1970, 1, 1)).TotalMilliseconds;
                            date = Convert.ToDateTime(pt[i].FechaFinal.ToString());
                            np.FechaFinal = (long)(date - new DateTime(1970, 1, 1)).TotalMilliseconds;
                            np.Horas = (Convert.ToDateTime(pt[i].FechaFinal.ToString()) - Convert.ToDateTime(pt[i].FechaInicial.ToString())).TotalMinutes;
                        }
                        catch { }
                        np.Empleado = p[j].Empleado.Nombre;
                        partes_trabajo.Add(np);
                    }
                }
            }
            catch { }
            return Json(partes_trabajo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSeries(string fini, string ffin)
        {
            SqlConnection cn = new SqlConnection("Data Source=SERCAUDAL2;Initial Catalog=Partes;User id=sa;Password=Demase49;");
            cn.Open();
            if (fini == null)
                fini = DateTime.Now.AddDays(-1).ToShortDateString();
            if (ffin == null)
            {
                if (fini == null)
                    ffin = DateTime.Now.ToShortDateString();
                else
                    ffin = Convert.ToDateTime(fini).AddDays(1).ToShortDateString();
            }

            SqlDataAdapter da = new SqlDataAdapter("select Proyecto.Nombre as Proyecto,t3.Nombre,Minutos from (select IdProyecto,Nombre, SUM(Tiempo) as Minutos from (select IdProyecto,DATEDIFF(mi,FechaInicial,FechaFinal) as Tiempo,Nombre from(select IdProyecto,FechaInicial,FechaFinal,IdEmpleado,Parte_Trabajo.IdParte from Parte_Trabajo inner join Parte on Parte.IdParte = Parte_Trabajo.IdParte where FechaInicial >= '"+fini+"' and FechaFinal < '"+ffin+"') as t1 inner join Empleado on Empleado.IdEmpleado = t1.IdEmpleado where t1.IdParte is not null ) as t2 group by IdProyecto,Nombre) as t3 inner join Proyecto on Proyecto.IdProyecto = t3.IdProyecto order by t3.Nombre, Proyecto, t3.IdProyecto", cn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<int> valores = new List<int>();
            Boolean nuevo = true;

            var proyectos = dt.AsEnumerable().Select(c => c.ItemArray[0]).Distinct().ToList();
            var users = dt.AsEnumerable().Select(c => c.ItemArray[1]).Distinct().ToList();
            for (int i = 0; i < proyectos.Count; i++)
            {
                serie s = new serie();
                s.Name = proyectos[i].ToString().Trim();
                List<int> values = new List<int>();
                for (int j = 0; j < users.Count; j++)
                {
                    string us = users[j].ToString();                   
                    try
                    {
                        var mins = dt.AsEnumerable().FirstOrDefault(c => c.ItemArray[0].ToString().Trim() == s.Name && c.ItemArray[1].ToString().Trim() == us.Trim()).ItemArray[2];
                        values.Add(int.Parse(mins.ToString()));
                    }
                    catch
                    {
                        values.Add(0);
                    }                       
                }
                s.Value = values;
                series.Add(s);
            }

            cn.Close();
            return Json(series, JsonRequestBehavior.AllowGet);
        }
    }
}
