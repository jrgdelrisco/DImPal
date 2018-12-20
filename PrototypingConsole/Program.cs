using DIPAlgorithms;
using DIPAlgorithms.GrayScale.Enhancement;
using DIPAlgorithms.GrayScale.Morphology;
using System;

namespace PrototypingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string imagePath = "../../../images/";

            string filename = "lion.jpg";

            var loader = new ImageLoader();
            var rawImage = loader.LoadRawRGBA(imagePath + filename);

            var time = Environment.TickCount;
            var rawgray = Converter.RgbaToGray(rawImage);
            rawgray.SharpenMorphology();
            Console.WriteLine($"filter running time: {Environment.TickCount - time} miliseconds");

            rawImage = Converter.GrayToRGBA(rawgray);
            loader.SaveAsRgbaJpegImage($"{imagePath}filter_{filename}", rawImage);
            Console.WriteLine($"total time: {Environment.TickCount - time} miliseconds");

            Console.WriteLine($"Image size: {rawImage.Width} x {rawImage.Height} (width x height)");
            Console.ReadLine();
        }
    }
}
