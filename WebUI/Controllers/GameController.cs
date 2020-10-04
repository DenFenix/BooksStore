using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class GameController : Controller
    {
        private IBookRepository repository;

        public GameController(IBookRepository repository)
        {
            this.repository = repository;
        }

    }
}