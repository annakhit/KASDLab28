using System;
using System.Collections.Generic;

public class MyHashMap<K, V>: IMap<K, V>
{
    private int size = 0;
    private readonly float loadFactor = 0.75f;

    private LinkedList<KeyValuePair<K, V>>[] table = new LinkedList<KeyValuePair<K, V>>[16];

    public MyHashMap() { }
    public MyHashMap(int initialCapacity)
    {
        table = new LinkedList<KeyValuePair<K, V>>[initialCapacity];
    }
    public MyHashMap(uint initialCapacity, float loadFactor)
    {
        table = new LinkedList<KeyValuePair<K, V>>[initialCapacity];
        this.loadFactor = loadFactor;
    }

    public void Put(K key, V value)
    {
        int hashCode = key.GetHashCode();
        int index = Math.Abs(hashCode % table.Length);

        if (table[index] == null)
        {
            table[index] = new LinkedList<KeyValuePair<K, V>>();
            table[index].AddLast(new KeyValuePair<K, V>(key, value));
            size++;
            Resize();
            return;
        }

        LinkedListNode<KeyValuePair<K, V>> current = table[index].First;

        while (current != null)
        {
            if (current.Value.Key.GetHashCode() == hashCode && current.Value.Key.Equals(key))
            {
                current.Value = new KeyValuePair<K, V>(key, value);
                return;
            }
            current = current.Next;
        }
        table[index].AddLast(new KeyValuePair<K, V>(key, value));
        size++;
        Resize();
    }

    public void PutAll(IMap<K, V> m)
    {
        foreach (KeyValuePair<K, V> item in m.EntrySet()) Put(item.Key, item.Value);
    }

    private void Resize()
    {
        if (size < table.Length * loadFactor) return;

        int newCapacity = table.Length * 2;

        LinkedList<KeyValuePair<K, V>>[] newBuckets = new LinkedList<KeyValuePair<K, V>>[newCapacity];

        foreach (var bucket in table)
        {
            if (bucket != null)
            {
                foreach (var entry in bucket)
                {
                    int newIndex = Math.Abs(entry.Key.GetHashCode() % newCapacity);
                    if (newBuckets[newIndex] == null) newBuckets[newIndex] = new LinkedList<KeyValuePair<K, V>>();
                    newBuckets[newIndex].AddLast(entry);
                }
            }
        }

        table = newBuckets;
    }

    public void Clear()
    {
        table = new LinkedList<KeyValuePair<K, V>>[table.Length];
        size = 0;
    }

    public bool ContainsKey(K key)
    {
        int hashCode = key.GetHashCode();
        int index = Math.Abs(hashCode % table.Length);

        if (table[index] == null) return false;

        LinkedListNode<KeyValuePair<K, V>> current = table[index].First;

        while (current != null)
        {
            if (current.Value.Key.GetHashCode() == hashCode && current.Value.Key.Equals(key)) return true;
            current = current.Next;
        }

        return false;
    }

    public bool ContainsValue(object value)
    {
        foreach (var bucket in table)
        {
            if (bucket != null)
            {
                LinkedListNode<KeyValuePair<K, V>> current = bucket.First;
                while (current != null)
                {
                    if (current.Value.Value.Equals(value)) return true;
                    current = current.Next;
                }
            }
        }

        return false;
    }

    public IEnumerable<KeyValuePair<K, V>> EntrySet()
    {
        foreach (var bucket in table)
        {
            if (bucket != null)
            {
                foreach (var entry in bucket) yield return entry;
            }
        }
    }

    public IMyCollection<V> Values() 
    {
        List<V> values = new List<V>();

        foreach (var bucket in table)
        {
            if (bucket != null)
            {
                foreach (var entry in bucket) values.Add(entry.Value);
            }
        }

        return values as IMyCollection<V>;
    }

    public V Get(K key)
    {
        int hashCode = key.GetHashCode();
        int index = Math.Abs(hashCode % table.Length);

        if (table[index] == null) return default;

        LinkedListNode<KeyValuePair<K, V>> current = table[index].First;

        while (current != null)
        {
            if (current.Value.Key.GetHashCode() == hashCode && current.Value.Key.Equals(key)) return current.Value.Value;
            current = current.Next;
        }

        return default;
    }

    public bool IsEmpty() => size == 0;

    public List<K> KeySet()
    {
        List<K> keys = new List<K>();

        foreach (var bucket in table)
        {
            if (bucket != null)
            {
                foreach (var entry in bucket) keys.Add(entry.Key);
            }
        }

        return keys;
    }

    public bool Remove(K key)
    {
        int hashCode = key.GetHashCode();
        int index = Math.Abs(hashCode % table.Length);

        if (table[index] == null) return false;

        LinkedListNode<KeyValuePair<K, V>> current = table[index].First;

        while (current != null)
        {
            if (current.Value.Key.GetHashCode() == hashCode && current.Value.Key.Equals(key))
            {
                table[index].Remove(current);
                size--;
                return true;
            }

            current = current.Next;
        }

        return false;
    }

    public int Size() => size;
}