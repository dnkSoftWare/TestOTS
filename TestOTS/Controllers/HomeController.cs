using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestOTS.Models;

namespace TestOTS.Controllers
{
    public class HomeController : Controller
  
    {
        IDataRepository _repository = new DataRepository(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        
        
        public ActionResult Index(string in_date)
        {
            if (string.IsNullOrEmpty(in_date))
                return View(new ViewDataModel("Рабочий график сотрудников", _repository.GetDataFromDB("20190712")));
            else
                return View(new ViewDataModel("Рабочий график сотрудников", _repository.GetDataFromDB(in_date)));
        }

    }
}