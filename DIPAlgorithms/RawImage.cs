using System;
using System.Collections.Generic;
using System.Text;

namespace DIPAlgorithms
{
    public abstract class RawImage<T> : IRawImage<T>
    {
        public T[] Raw { get; }
        public int Width { get; }
        public int Height { get; }
        public int Channels { get; }

        public RawImage(T[] raw, int width, int height)
        {
            if (raw.Length % (width * height) != 0)
            {
                throw new ArgumentException("raw.Length must be divisible by (width x height)");
            }

            Raw = raw;
            Width = width;
            Height = height;
            Channels = raw.Length / (width * height);
        }
    }
}
