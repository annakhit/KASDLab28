using System;
using System.Collections.Generic;

public class MyHashSet<E> : IMySet<E> where E : IComparable<E>
{
    public class MyIterator<T> : IMyIterator<T> where T : IComparable<T>
    {
        private int cursor;

        private readonly MyHashSet<T> hashSet;

        private readonly IEnumerator<KeyValuePair<T, object>> enumerator;

        public MyIterator(MyHashSet<T> hashSet)
        {
            this.hashSet = hashSet;
            enumerator = hashSet.map.EntrySet().GetEnumerator();
            cursor = -1;
        }

        public bool HasNext() => cursor < hashSet.Size() - 1;

        public T Next()
        {
            if (!HasNext()) throw new InvalidOperationException();
            enumerator.MoveNext();
            cursor++;
            return enumerator.Current.Key;
        }

        public void Remove()
        {
            if (cursor < 0) throw new InvalidOperationException();
            hashSet.Remove(enumerator.Current.Key);
            cursor--;
        }
    }

    protected readonly MyHashMap<E, object> map = new MyHashMap<E, object>();

    public MyHashSet() {}

    public MyHashSet(IMyCollection<E> collection)
    {
        foreach (var element in collection.ToArray()) map.Put(element, default);
    }

    public MyHashSet(int initialCapacity)
    {
        map = new MyHashMap<E, object>(initialCapacity);
    }
    public MyHashSet(uint initialCapacity, float loadFactor)
    {
        map = new MyHashMap<E, object>(initialCapacity, loadFactor);
    }

    public void Add(E element) => map.Put(element, default);

    public void AddAll(IMyCollection<E> collection)
    {
        foreach (var element in collection.ToArray()) map.Put(element, default);
    }

    public void Clear() => map.Clear();

    public bool Contains(object obj) => map.ContainsKey((E)obj);

    public bool ContainsAll(IMyCollection<E> collection)
    {
        foreach (var element in collection.ToArray())
        {
            if (!map.ContainsKey(element)) return false;
        }

        return true;
    }

    public void Remove(object obj) => map.Remove((E)obj);

    public void RemoveAll(IMyCollection<E> collection)
    {
        foreach (var element in collection.ToArray()) map.Remove(element);
    }

    public void RetainAll(IMyCollection<E> collection)
    {
        E[] array = collection.ToArray();

        foreach (var element in map.KeySet())
        {
            if (Array.IndexOf(array, element) == -1) map.Remove(element);
        }
    }

    public int Size() => map.Size();

    public bool IsEmpty() => Size() == 0;

    public E[] ToArray()
    {
        E[] array = map.KeySet().ToArray();
        Array.Sort(array, (a, b) => a.CompareTo(b));
        return array;
    }

    public E[] ToArray(ref E[] array)
    {
        E[] keys = ToArray();

        if (array == null) return keys;

        for (int index = 0; index < keys.Length; index++) array[index] = keys[index];

        return array;
    }

    public E First()
    {
        if (Size() == 0) return default;
        E[] keys = ToArray();
        return keys[0];
    }

    public E Last()
    {
        if (Size() == 0) return default;
        E[] keys = ToArray();
        return keys[keys.Length - 1];
    }

    public IMySet<E> SubSet(E fromElement, E toElement)
    {
        MyHashSet<E> hashSet = new MyHashSet<E>();

        foreach (var element in map.KeySet())
        {
            if (element.CompareTo(fromElement) >= 0 && element.CompareTo(toElement) < 0)
            {
                hashSet.Add(element);
            }
        }

        return hashSet;
    }

    public IMySet<E> HeadSet(E toElement)
    {
        MyHashSet<E> hashSet = new MyHashSet<E>();

        foreach (var element in map.KeySet())
        {
            if (element.CompareTo(toElement) < 0)
            {
                hashSet.Add(element);
            }
        }

        return hashSet;
    }

    public IMySet<E> TailSet(E fromElement)
    {
        MyHashSet<E> hashSet = new MyHashSet<E>();

        foreach (var element in map.KeySet())
        {
            if (element.CompareTo(fromElement) >= 0)
            {
                hashSet.Add(element);
            }
        }

        return hashSet;
    }

    public MyIterator<E> Iterator() => new MyIterator<E>(this);
}
