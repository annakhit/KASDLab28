using System;

internal class MyArrayList<T>: IMyList<T>
{
    public class MyIterator<E> : IMyIteratorList<E>
    {
        private int cursor;

        private readonly MyArrayList<E> arrayList;

        public MyIterator(MyArrayList<E> arrayList)
        {
            this.arrayList = arrayList;
            cursor = -1;
        }

        public MyIterator(MyArrayList<E> arrayList, int cursor)
        {
            this.arrayList = arrayList;
            this.cursor = cursor;
        }

        public bool HasNext() => cursor < arrayList.Size() - 1;

        public E Next()
        {
            if (!HasNext()) throw new InvalidOperationException();
            cursor++;
            return arrayList.elementData[cursor];
        }

        public bool HasPrevious() => cursor > 0;

        public E Previous()
        {
            if (cursor < 1) throw new InvalidOperationException();
            return arrayList.elementData[cursor - 1];
        }

        public int NextIndex() => HasNext() ? cursor + 1 : default;

        public int PreviousIndex() => cursor > 1 ? cursor - 1 : default;

        public void Set(E element) => arrayList.Set(cursor, element);

        public void Add(E element) => arrayList.Add(cursor, element);

        public void Remove()
        {
            if (cursor < 0) throw new InvalidOperationException();
            arrayList.Remove(cursor);
            cursor--;
        }
    }

    protected T[] elementData;
    private int size;

    public MyArrayList()
    {
        elementData = new T[0];
        size = 0;
    }
    public MyArrayList(IMyCollection<T> collection)
    {
        T[] array = collection.ToArray();
        elementData = array;
        size = array.Length;
    }
    public MyArrayList(int capacity)
    {
        size = 0;
        elementData = new T[capacity];
    }

    public void Add(T element)
    {
        if (size == elementData.Length)
        {
            T[] newArray = new T[(int)(size * 1.5) + 1];
            for (int i = 0; i < size; i++) newArray[i] = elementData[i];
            elementData = newArray;
        }
        elementData[size] = element;
        size++;
    }
    public void AddAll(IMyCollection<T> collection)
    {
        foreach (T element in collection.ToArray()) Add(element);
    }

    public void Clear()
    {
        elementData = new T[0];
        size = 0;
    }
    public bool Contains(object obj)
    {
        foreach (T element in elementData)
        {
            if (element.Equals(obj)) return true;
        }
        return false;
    }

    public bool ContainsAll(IMyCollection<T> collection)
    {
        foreach (T element in collection.ToArray())
        {
            if (!Contains(element)) return false;
        }
        return true;
    }

    public bool IsEmpty() => size == 0;

    public void Remove(object obj)
    {
        for (int index = 0; index < size; index++)
        {
            if (Contains(obj))
            {
                for (int specIndex = index; specIndex < size - 1; specIndex++)
                {
                    elementData[specIndex] = elementData[specIndex + 1];
                }
                size--;
            }
        }
    }

    public void RemoveAll(IMyCollection<T> collection)
    {
        foreach (T element in collection.ToArray()) Remove(element);
    }

    public void RetainAll(IMyCollection<T> collection)
    {
        foreach (T element in collection.ToArray())
        {
            if (!Contains(element))
                Remove(element);
        }
    }

    public int Size() => size;
    public T[] ToArray()
    {
        T[] result = new T[size];
        for (int index = 0; index < size; index++)
        {
            result[index] = elementData[index];
        }
        return result;
    }


    public T[] ToArray(ref T[] array)
    {
        if (array == null) return ToArray();

        if (array.Length < size) throw new ArgumentOutOfRangeException();

        for (int index = 0; index < size; index++)
        {
            array[index] = elementData[index];
        }
        return array;
    }

    public void Add(int index, T element)
    {
        if (index >= size) throw new ArgumentOutOfRangeException();

        if (size == elementData.Length)
        {
            T[] array = new T[(int)(size * 1.5) + 1];
            for (int i = 0; i < size; i++) array[i] = elementData[i];
            elementData = array;
        }

        for (int i = size; i > index; i--)
        {
            elementData[i] = elementData[i - 1];
        }
        elementData[index] = element;
        size++;
    }

    public void AddAll(int index, IMyCollection<T> collection)
    {
        T[] array = collection.ToArray();

        if (index >= size) throw new ArgumentOutOfRangeException();

        if (size + array.Length > elementData.Length)
        {
            T[] tempArray = new T[(int)(size * 1.5) + array.Length];
            for (int i = 0; i < size; i++) array[i] = elementData[i];
            elementData = tempArray;
        }

        for (int i = 0; i < array.Length; i++)
        {
            elementData[index + i + array.Length] = elementData[index + i];
            elementData[index + i] = array[i];
            size++;
        }
    }

    public T Get(int index)
    {
        if (index >= size) throw new ArgumentOutOfRangeException();

        return elementData[index];
    }

    public int IndexOf(object obj)
    {
        for (int i = 0; i < size; i++)
            if (elementData[i].Equals(obj)) return i;
        return -1;
    }

    public int LastIndexOf(object obj)
    {
        for (int i = elementData.GetUpperBound(0); i >= 0; i--)
            if (elementData[i].Equals(obj)) return i;
        return -1;
    }

    public T Remove(int index)
    {
        if (index >= size) throw new ArgumentOutOfRangeException();
        T element = elementData[index];
        for (int ind = index; ind < size - 1; ind++) elementData[ind] = elementData[ind + 1];
        elementData[index] = default;
        size--;
        return element;
    }

    public void Set(int index, T element)
    {
        if (index >= size) throw new ArgumentOutOfRangeException();
        if (element == null) throw new ArgumentNullException(element.ToString());
        elementData[index] = element;
    }

    public IMyList<T> SubList(int fromIndex, int toIndex)
    {
        if (fromIndex >= size) throw new ArgumentOutOfRangeException();
        if (toIndex > size) throw new ArgumentOutOfRangeException();
        MyArrayList<T> array = new MyArrayList<T>(toIndex - fromIndex);
        for (int i = 0; i < array.size; i++)
            array.Add(elementData[fromIndex + i]);
        return array;
    }

    public IMyIteratorList<T> ListIterator() => new MyIterator<T>(this);

    public IMyIteratorList<T> ListIterator(int index) => new MyIterator<T>(this, index);

    public void Print()
    {
        foreach (T element in elementData)
            Console.WriteLine(element.ToString());
        Console.WriteLine();
    }
}