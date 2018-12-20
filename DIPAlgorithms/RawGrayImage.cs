using System;
using System.Collections.Generic;
using System.Text;

namespace DIPAlgorithms
{
    public class RawGrayImage<T> : RawImage<T>, ICloneable
    {
        public RawGrayImage(T[] raw, int width, int height) : base(raw, width, height)
        {
            if (raw.Length != width * height)
            {
                throw new ArgumentException("raw.Length must be equal the product of (width x height)");
            }
        }

        public object Clone()
        {
            T[] copy = new T[Raw.Length];
            Raw.CopyTo(copy, 0);
            return new RawGrayImage<T>(copy, Width, Height);
        }
    }
}
