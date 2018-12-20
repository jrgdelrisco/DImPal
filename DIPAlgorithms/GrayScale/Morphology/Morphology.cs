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
            RawGrayImage<byte> copy = image.Clone() as RawGrayImage<byte>;
            image.Open();

            unsafe
            {
                fixed (byte* copyPtr = copy.Raw, rawFixed = image.Raw)
                {
                    byte* rawPtr = rawFixed;
                    int length = image.Raw.Length;

                    for (int i = 0; i < length; i++)
                    {
                        int sub = *(copyPtr + i) - *rawPtr;
                        sub = sub < 0 ? 0 : sub;
                        *rawPtr = (byte)sub;
                        rawPtr++;
                    }
                }
            }
        }

        public static void BlackTopHat(this RawGrayImage<byte> image)
        {
            RawGrayImage<byte> copy = image.Clone() as RawGrayImage<byte>;
            image.Close();

            unsafe
            {
                fixed (byte* copyPtr = copy.Raw, rawFixed = image.Raw)
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
    }
}
