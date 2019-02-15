using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DigitalImageProcessingAlgorithms.Models;
using SixLabors.ImageSharp;
using DIPAlgorithms;
using DIPAlgorithms.GrayScale.Enhancement;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Runtime.InteropServices;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using DIPAlgorithms.GrayScale.Morphology;

namespace DigitalImageProcessingAlgorithms.Controllers
{
    public class HomeController : Controller
    {
        private const string fileName = "cheetah.jpg";
        private readonly IFileProvider _fileProvider;

        public HomeController(IHostingEnvironment env)
        {
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
            string path = PathString.FromUriComponent("/images/" + fileName);
            IFileInfo fileInfo = _fileProvider.GetFileInfo(path);

            if (!fileInfo.Exists)
            {
                return NotFound();
            }

            var outputStream = new MemoryStream();

            using (var inputStream = fileInfo.CreateReadStream())
            using (var image = Image.Load(inputStream))
            {                
                byte[] rgbaBytes = MemoryMarshal.AsBytes(image.GetPixelSpan()).ToArray();
                RawRgbaImage<byte> rawImage = new RawRgbaImage<byte>(rgbaBytes, image.Width, image.Height);
                RawGrayImage<byte> rawgray = Converter.RgbaToGray(rawImage);

                rawgray.Gradient();
                rawImage = Converter.GrayToRGBA(rawgray);
                Image<Rgba32> result = Image.LoadPixelData<Rgba32>(rawImage.Raw, rawImage.Width, rawImage.Height);
                ViewBag.Image = result.ToBase64String(ImageFormats.Jpeg);
                result.Dispose();
            }

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
