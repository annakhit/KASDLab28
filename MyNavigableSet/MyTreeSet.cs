using System;
using System.Collections.Generic;
using System.Linq;

public class MyTreeSet<E> : IMyNavigableSet<E> where E : IComparable<E>
{
    public class MyIterator<T> : IMyIterator<T> where T : IComparable<T>
    {
        private int cursor;

        private readonly MyTreeSet<T> treeSet;

        private readonly IEnumerator<KeyValuePair<T, int>> enumerator;

        public MyIterator(MyTreeSet<T> treeSet)
        {
            this.treeSet = treeSet;
            enumerator = treeSet.map.EntrySet().GetEnumerator();
            cursor = -1;
        }

        public bool HasNext() => cursor < treeSet.Size() - 1;

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
            treeSet.Remove(enumerator.Current.Key);
            cursor--;
        }
    }

    protected readonly MyTreeMap<E, int> map = new MyTreeMap<E, int>();

    public MyTreeSet() { }

    public MyTreeSet(MyTreeMap<E, int> map)
    {
        this.map = map;
    }

    public MyTreeSet(IComparer<E> Comparator)
    {
        map = new MyTreeMap<E, int>(Comparator);
    }

    public MyTreeSet(IMyCollection<E> collection)
    {
        foreach (var element in collection.ToArray()) map.Put(element, default);
    }

    public MyTreeSet(SortedSet<E> s)
    {
        foreach (var element in s) map.Put(element, default);
    }

    public void Add(E element) => map.Put(element, default);

    public void AddAll(IMyCollection<E> collection)
    {
        foreach (var element in collection.ToArray()) map.Put(element, default);
    }

    public void Clear() => map.Clear();

    public bool Contains(object value) => map.ContainsKey((E)value);

    public bool ContainsAll(IMyCollection<E> collection)
    {
        foreach (var element in collection.ToArray())
        {
            if (!map.ContainsKey(element)) return false;
        }

        return true;
    }

    public bool IsEmpty() => map.IsEmpty();

    public void Remove(object obj) => map.RemoveNode((E)obj);

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

    public E[] ToArray() => map.KeySet().ToArray();

    public E[] ToArray(ref E[] array)
    {
        E[] keys = ToArray();

        if (array == null) return keys;

        for (int index = 0; index < keys.Length; index++) array[index] = keys[index];

        return array;
    }

    public E First() => map.FirstKey();

    public E Last() => map.LastKey();

    public IMySet<E> SubSet(E fromElement, E toElement) => new MyTreeSet<E>((MyTreeMap<E, int>)map.SubMap(fromElement, toElement));

    public IMySet<E> HeadSet(E toElement) => new MyTreeSet<E>((MyTreeMap<E, int>)map.HeadMap(toElement));

    public IMySet<E> TailSet(E fromElement) => new MyTreeSet<E>((MyTreeMap<E, int>)map.TailMap(fromElement));

    public E Ceiling(E fromElement) => map.CeilingKey(fromElement);

    public E Floor(E fromElement) => map.FloorKey(fromElement);

    public E Higher(E fromElement) => map.HigherKey(fromElement);

    public E Lower(E fromElement) => map.LowerKey(fromElement);

    public IMySet<E> HeadSet(E upperBound, bool include) => new MyTreeSet<E>((MyTreeMap<E, int>)map.HeadMap(upperBound, include));

    public IMySet<E> SubSet(E lowerBound, bool lowInclude, E upperBound, bool hightInclude) => new MyTreeSet<E>((MyTreeMap<E, int>)map.SubMap(lowerBound, upperBound, lowInclude, hightInclude));

    public IMySet<E> TailSet(E lowerBound, bool include) => new MyTreeSet<E>((MyTreeMap<E, int>)map.TailMap(lowerBound, include));

    public E PollLast() => map.PollLastEntry().Key;

    public E PollFirst() => map.PollFirstEntry().Key;

    public IEnumerable<E> DescendingIterator()
    {
        IEnumerable<KeyValuePair<E, int>> nodes = map.EntrySet();

        foreach (KeyValuePair<E, int> node in nodes.Reverse()) yield return node.Key;
    }
    public MyTreeSet<E> DescendingSet()
    {
        MyTreeSet<E> set = new MyTreeSet<E>(Comparer<E>.Create((a, b) => b.CompareTo(a)));

        foreach (var element in map.EntrySet()) set.Add(element.Key);

        return set;
    }

    public MyIterator<E> Iterator() => new MyIterator<E>(this);
}

