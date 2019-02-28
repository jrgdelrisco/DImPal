using DIPAlgorithms;
using DIPAlgorithms.GrayScale.Morphology;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DigitalImageProcessingAlgorithms.Services
{
    public class FilterService : IFilterService
    {
        private readonly IFileProvider _fileProvider;

        public FilterService(IHostingEnvironment env)
        {
            _fileProvider = env.WebRootFileProvider;
        }

        public string ApplyFilter(string fileName)
        {
            string path = PathString.FromUriComponent("/images/" + fileName);
            IFileInfo fileInfo = _fileProvider.GetFileInfo(path);

            if (!fileInfo.Exists)
            {
                throw new Exception($"The file {fileName} was not found.");
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
                return result.ToBase64String(ImageFormats.Jpeg);
            }
        }
    }
}
