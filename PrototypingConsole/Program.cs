using DIPAlgorithms;
using DIPAlgorithms.GrayScale.Enhancement;
using DIPAlgorithms.GrayScale.Morphology;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace PrototypingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .AddCommandLine(args);
            var configuration = configBuilder.Build();

            var imagePath = configuration["imagePath"];
            
            var fileName = "cheetah.jpg";
            var directory = Directory.GetCurrentDirectory();


            if (File.Exists(imagePath))
            {
                fileName = Path.GetFileName(imagePath);
                directory = Path.GetDirectoryName(imagePath);
            }

            var loader = new ImageLoader();
            var rawImage = loader.LoadRawRGBA(Path.Combine(directory, fileName));
            var rawgray = Converter.RgbaToGray(rawImage);

            var time = Environment.TickCount;
            rawgray.Gradient();
            Console.WriteLine($"Running time: {Environment.TickCount - time} miliseconds");

            rawImage = Converter.GrayToRGBA(rawgray);
            loader.SaveAsRgbaJpegImage(Path.Combine(directory, $"filter_{fileName}"), rawImage);

            Console.WriteLine($"Image size: {rawImage.Width} x {rawImage.Height} (width x height)");
        }
    }
}
