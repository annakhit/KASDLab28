using System.Collections.Generic;

public interface IMap<K, V>
{
    void Put(K key, V value);

    void Clear();

    bool ContainsKey(K key);

    bool ContainsValue(object value);

    bool Remove(K key);

    bool IsEmpty();

    int Size();

    V Get(K key);

    List<K> KeySet();

    IEnumerable<KeyValuePair<K, V>> EntrySet();
}