using System;
using System.Collections.Generic;
using System.Text;

namespace DIPAlgorithms
{
    public class Converter
    {
        public static RawRgbaImage<byte> GrayToRGBA(RawGrayImage<byte> image)
        {
            byte[] rawGray = image.Raw;
            byte[] rawRgba = new byte[rawGray.Length * 4];
            int length = rawGray.Length;

            unsafe
            {
                fixed (byte* grayFixed = rawGray, rgbaFixed = rawRgba)
                {
                    byte* grayPtr = grayFixed;
                    byte* rgbaPtr = rgbaFixed;

                    for (int i = 0; i < length; i++)
                    {
                        byte grayValue = *grayPtr;
                        *rgbaPtr = grayValue;
                        *(rgbaPtr + 1) = grayValue;
                        *(rgbaPtr + 2) = grayValue;
                        *(rgbaPtr + 3) = 255;

                        grayPtr++;
                        rgbaPtr += 4;
                    }
                }
            }

            return new RawRgbaImage<byte>(rawRgba, image.Width, image.Height);
        }

        public static RawGrayImage<byte> RgbaToGray(RawRgbaImage<byte> image)
        {
            byte[] rawRGBA = image.Raw;
            byte[] rawGray = new byte[rawRGBA.Length / 4];
            int length = rawRGBA.Length;

            unsafe
            {
                fixed (byte* rgbaFixed = rawRGBA, grayFixed = rawGray)
                {
                    byte* rgbaPtr = rgbaFixed;
                    byte* grayPtr = grayFixed;
                    for (int i = 0; i < length; i += 4)
                    {
                        byte r = *rgbaPtr;
                        byte g = *(rgbaPtr + 1);
                        byte b = *(rgbaPtr + 2);
                        byte grayValue = (byte)(0.2126 * r + 0.7152 * g + 0.0722 * b);
                        *grayPtr = grayValue;
                        
                        rgbaPtr += 4;
                        grayPtr++;
                    }
                }
            }

            return new RawGrayImage<byte>(rawGray, image.Width, image.Height);
        }
    }
}
