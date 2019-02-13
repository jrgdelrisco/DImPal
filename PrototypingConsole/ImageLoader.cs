using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using DIPAlgorithms;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp;
using System.IO;
using SixLabors.ImageSharp.PixelFormats;

namespace PrototypingConsole
{
    class ImageLoader
    {
        public RawRgbaImage<byte> LoadRawRGBA(string path)
        {
            using (var image = Image.Load(path))
            {
                byte[] rgbaBytes = MemoryMarshal.AsBytes(image.GetPixelSpan()).ToArray();
                RawRgbaImage<byte> rawImage = new RawRgbaImage<byte>(rgbaBytes, image.Width, image.Height);

                return rawImage;
            }
        }

        public void SaveAsRgbaJpegImage(string path, RawRgbaImage<byte> image)
        {
            using (var stream = File.OpenWrite(path))
            {
                var newimage = Image.LoadPixelData<Rgba32>(image.Raw, image.Width, image.Height);
                newimage.SaveAsJpeg(stream);
            }
        }
    }
}
