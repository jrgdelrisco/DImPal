using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DigitalImageProcessingAlgorithms.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using DigitalImageProcessingAlgorithms.Services;

namespace DigitalImageProcessingAlgorithms.Controllers
{
    public class HomeController : Controller
    {
        private const string fileName = "cheetah.jpg";
        private readonly IFileProvider _fileProvider;
        private readonly IFilterService _filterService;

        public HomeController(IFilterService filterService, IHostingEnvironment env)
        {
            _filterService = filterService;
            _fileProvider = env.WebRootFileProvider;
        }

        public IActionResult GetImage()
        {
            string path = PathString.FromUriComponent("/images/" + fileName);
            IFileInfo fileInfo = _fileProvider.GetFileInfo(path);

            if (!fileInfo.Exists)
            {
                return NotFound();
            }

            var stream = fileInfo.CreateReadStream();
            return File(stream, "image/jpg");
        }

        public IActionResult Results()
        {
            ViewBag.Image = _filterService.ApplyFilter(fileName);

            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
