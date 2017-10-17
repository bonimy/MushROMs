namespace Helper
{
    public interface IReadOnlyIndexer<T>
    {
        T this[int index] { get; }
    }
}
