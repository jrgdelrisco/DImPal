using System;
using System.Collections.Generic;
using System.Text;
using DIPAlgorithms.GrayScale.Morphology;

namespace DIPAlgorithms.GrayScale.Enhancement
{
    public static class Sharpen
    {
        public static void SharpenMorphology(this RawGrayImage<byte> image, int n = 1)
        {
            if (n < 0)
            {
                throw new ArgumentException("Parameter n must be greater than 0.");
            }

            RawGrayImage<byte> copy = image.Clone() as RawGrayImage<byte>;

            int count = 0;
            while (count < n)
            {
                count++;
                copy.WhiteTopHat();
            }

            unsafe
            {
                fixed (byte* copyFixed = copy.Raw, rawPtr = image.Raw)
                {
                    byte* copyPtr = copyFixed;
                    int length = image.Raw.Length;

                    for (int i = 0; i < length; i++)
                    {
                        int add = *copyPtr + *(rawPtr + i);
                        *copyPtr = (byte)(Math.Min(add, 255));
                        copyPtr++;
                    }
                }
            }

            count = 0;
            while (count < n)
            {
                count++;
                image.BlackTopHat();
            }

            unsafe
            {
                fixed (byte* rawFixed = image.Raw, copyPtr = copy.Raw)
                {
                    byte* rawPtr = rawFixed;
                    int length = image.Raw.Length;

                    for (int i = 0; i < length; i++)
                    {
                        int sub = *(copyPtr + i) - *rawPtr;
                        *rawPtr = (byte)(Math.Max(0, sub));
                        rawPtr++;
                    }
                }
            }
        }
    }
}
