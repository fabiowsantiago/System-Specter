using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Specter_System.Controllers
{
    public class AreaAdministrativaController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }

        public ViewResult CadastroCursoPresencial()
        {
            return View();
        }

        public ViewResult CadastroCursoOnline()
        {
            return View();
        }
    }
}
