namespace Helper
{
    public interface IIndexer<T>
    {
        T this[int index]
        {
            get;
            set;
        }
    }
}
