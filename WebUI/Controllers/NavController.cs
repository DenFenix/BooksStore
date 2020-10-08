using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class NavController : Controller
    {
        private IBookRepository reository;

        public NavController(IBookRepository reository)
        {
            this.reository = reository;
        }

        // GET: Nav
        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;
            IEnumerable <string> result = reository
                 .Books
                 .Select(x => x.Category)
                 .Distinct()
                 .OrderBy(x => x);
            return PartialView(result);
        }
    }
}