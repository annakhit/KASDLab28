public interface IMySortedMap<K, V>: IMap<K, V>
{
    K FirstKey();

    K LastKey();

    IMySortedMap<K, V> HeadMap(K to, bool equal = false);

    IMySortedMap<K, V> SubMap(K fromKey, K toKey, bool fromInclude = true, bool toInclude = false);

    IMySortedMap<K, V> TailMap(K from, bool equal = false);
}