namespace DIPAlgorithms
{
    public interface IRawImage<T>
    {
        T[] Raw { get; }
        int Width { get; }
        int Height { get; }
        int Channels { get; }
    }
}
