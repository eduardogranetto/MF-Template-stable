using App.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Diagnostics;
using System.Linq;
using VendaERP.Core;
using VendaERP.Core.Models;
using static MongoDB.Driver.WriteConcern;

namespace App.Controllers
{
    public class DashboardController : Controller
    {
        ///<summary>
        ///Variavel padrão de acesso ao Banco de Dados
        ///</summary>
        private readonly DBAccess _db;
        private DashboardPadrao _model;
        public DashboardController(DBAccess db)
        {
            this._db = db;
            this._model = new DashboardPadrao();
        }
        

        public IActionResult Index()
        {
            return Redirect("/Etiquetas/Index");            
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
    }
}