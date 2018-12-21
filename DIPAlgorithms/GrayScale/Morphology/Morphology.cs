using System;
using System.Collections.Generic;
using System.Text;

namespace DIPAlgorithms.GrayScale.Morphology
{
    public static class Morphology
    {
        public static void Erode(this RawGrayImage<byte> image)
        {
            BasicMorphologyScan(image, (a, b) => Math.Min(a, b));
        }

        public static void Dilate(this RawGrayImage<byte> image)
        {
            BasicMorphologyScan(image, (a, b) => Math.Max(a, b));
        }

        public static void Open(this RawGrayImage<byte> image)
        {
            image.Erode();
            image.Dilate();
        }

        public static void Close(this RawGrayImage<byte> image)
        {
            image.Dilate();
            image.Erode();
        }

        public static void WhiteTopHat(this RawGrayImage<byte> image)
        {
            TopHatTransform(image, true);
        }

        public static void BlackTopHat(this RawGrayImage<byte> image)
        {
            TopHatTransform(image, false);
        }

        public static void Gradient(this RawGrayImage<byte> image)
        {
            RawGrayImage<byte> copy = image.Clone() as RawGrayImage<byte>;

            image.Dilate();
            copy.Erode();

            unsafe
            {
                fixed (byte* rawFixed = image.Raw, copyPtr = copy.Raw)
                {
                    byte* rawPtr = rawFixed;
                    int length = image.Raw.Length;

                    for (int i = 0; i < length; i++)
                    {
                        int sub = *rawPtr - *(copyPtr + i);
                        sub = sub < 0 ? 0 : sub;
                        *rawPtr = (byte)sub;
                        rawPtr++;
                    }
                }
            }

        }

        private static void BasicMorphologyScan(RawGrayImage<byte> image, Func<int, int, int> kernelFunc)
        {
            RawGrayImage<byte> copy = image.Clone() as RawGrayImage<byte>;

            unsafe
            {
                fixed (byte* copyPtr = copy.Raw, rawFixed = image.Raw)
                {
                    byte* rawPtr = rawFixed;
                    int width = image.Width;
                    int height = image.Height;

                    Func<int, int, int> position = (i, j) => i * width + j;

                    for (int i = 1; i < height - 1; i++)
                    {
                        for (int j = 1; j < width - 1; j++)
                        {
                            int grayValue = kernelFunc(*(copyPtr + position(i, j)), *(copyPtr + position(i - 1, j)));
                            grayValue = kernelFunc(grayValue, *(copyPtr + position(i + 1, j)));
                            grayValue = kernelFunc(grayValue, *(copyPtr + position(i, j - 1)));
                            grayValue = kernelFunc(grayValue, *(copyPtr + position(i, j + 1)));

                            *(rawPtr + position(i, j)) = (byte)grayValue;
                        }
                    }
                }
            }
        }

        private static void TopHatTransform(RawGrayImage<byte> image, bool white)
        {
            RawGrayImage<byte> copy = image.Clone() as RawGrayImage<byte>;

            if (white)
            {
                image.Open();
            }
            else
            {
                image.Close();
            }

            unsafe
            {
                fixed (byte* copyPtr = copy.Raw, rawFixed = image.Raw)
                {
                    byte* rawPtr = rawFixed;
                    int length = image.Raw.Length;

                    Func<int, int, int> op;
                    if (white)
                    {
                        op = (a, b) => a - b;
                    }
                    else
                    {
                        op = (a, b) => b - a;
                    }

                    for (int i = 0; i < length; i++)
                    {
                        int saturate = op(*(copyPtr + i), *rawPtr);
                        *rawPtr = (byte)(Math.Max(0, saturate));
                        rawPtr++;
                    }
                }
            }
        }
    }
}
