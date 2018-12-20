using System;
using System.Collections.Generic;
using System.Text;

namespace DIPAlgorithms
{
    public class RawRgbaImage<T> : RawImage<T>
    {
        public RawRgbaImage(T[] raw, int width, int height) : base(raw, width, height)
        {
            if (raw.Length != width * height * 4)
            {
                throw new ArgumentException("raw.Length must be equal the product of (width x height x 4).");
            }
        }
    }
}
