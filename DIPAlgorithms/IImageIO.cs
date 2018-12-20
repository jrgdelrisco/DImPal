namespace DIPAlgorithms
{
    public interface IImageIO
    {
        RawRgbaImage<byte> LoadRawRGBA(string path);
        void SaveAsRgbaJpegImage(string path, RawRgbaImage<byte> image);
    }
}
