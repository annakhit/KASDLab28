public interface IMyIterator<T>
{
    bool HasNext();
    T Next();
    void Remove();
}
