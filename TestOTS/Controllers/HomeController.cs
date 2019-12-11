using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using TestOTS.Models;

namespace TestOTS.Controllers
{
    public class HomeController : Controller
  
    {
        IDataRepository _repository = new DataRepository(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public ActionResult Index(string id = "20190712")
        {
            return View(new ViewDataModel("Рабочий график сотрудников", _repository.GetDataFromDB(id)));
        }

    }
}