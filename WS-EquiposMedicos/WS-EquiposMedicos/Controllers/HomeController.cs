using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WS_EquiposMedicos.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public JsonResult listadoResultados()
        {
            DLResultados dl = new DLResultados();
            List<ELResultado> lstMaestro = dl.DL_ConsultarAmbiente();
            return Json(lstMaestro, JsonRequestBehavior.AllowGet);
        }
    }
}
