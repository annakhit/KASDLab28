using System.Collections.Generic;

public interface IMyNavigableMap<K, V> : IMySortedMap<K, V>
{
    K LowerKey(K key);

    K FloorKey(K key);

    K HigherKey(K key);

    K CeilingKey(K key);

    V FirstEntry();

    V LastEntry();

    KeyValuePair<K, V> LowerEntry(K key);

    KeyValuePair<K, V> FloorEntry(K key);

    KeyValuePair<K, V> HigherEntry(K key);

    KeyValuePair<K, V> CeilingEntry(K key);

    KeyValuePair<K, V> PollFirstEntry();

    KeyValuePair<K, V> PollLastEntry();
}