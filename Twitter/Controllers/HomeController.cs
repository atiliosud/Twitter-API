using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Twitter.Models;
using Microsoft.AspNetCore.Http;
using Twitter.Extensions;

namespace Twitter.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppSettings _appSettings;
        public Models.Twitter _twitter{ get; set; }
        public string _accessToken { get; set; }

        public HomeController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _twitter = new Models.Twitter(_appSettings);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _accessToken = await _twitter.GetAccessToken();
            HttpContext.Session.SetString("Token", _accessToken);
            IList<string> _twitts = await _twitter.GetTwitts(_accessToken);
            HttpContext.Session.SetComplexData("Twitts", _twitts);
            return View(_twitts);
        }

        [HttpGet]
        public async Task<IActionResult> Pesquisar(string parameter)
        {
            _accessToken = HttpContext.Session.GetString("Token");
            IList<string> _twitts = await _twitter.GetFilteredTwitts(parameter);
            return View(_twitts);
        }

        [HttpGet]
        public FileStreamResult Exportar()
        {
            var twitts = HttpContext.Session.GetComplexData<List<string>>("Twitts");
            
            var sbTwitts = new StringBuilder();
            twitts.ForEach(line =>
            {
                sbTwitts.AppendLine(string.Join(",", line));
            });

            var fsr = new FileStreamResult(new MemoryStream(Encoding.UTF32.GetBytes(sbTwitts.ToString())), "text/csv")
            {
                FileDownloadName = string.Format("ExportacaoTwitts{0}.csv", DateTime.Now.ToFileTime())
            };

            return fsr;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "TWITTER API: Desenvolvedor : Atilio Camargo Moreira.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Desenvolvedor: Atilio Camargo Moreira.";

            return View();
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
