using System.Web.Mvc;

namespace UtleiraTidtaker.Web.Controllers
{
    public class ClientController : Controller
    {
        // GET: FileClient
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Json()
        {
            return View();
        }
    }
}