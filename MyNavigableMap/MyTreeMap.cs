using System;
using System.Collections.Generic;

public class MyTreeMap<K, V> : IMyNavigableMap<K, V> where K : IComparable<K> where V : IComparable<V>
{
    protected int Count { get; private set; }

    protected Node<K, V> Root;

    protected IComparer<K> Comparator = Comparer<K>.Create((a, b) => a.CompareTo(b));

    public MyTreeMap() {}
    public MyTreeMap(IComparer<K> Comparator)
    {
        this.Comparator = Comparator;
    }

    public MyTreeMap(IMap<K, V> m) 
    {
        PutAll(m);  
    }

    public MyTreeMap(IMySortedMap<K, V> m)
    {
        PutAll(m);
    }

    public void Clear()
    {
        Root = null;
        Count = 0;
    }

    public bool ContainsKey(K key)
    {
        Node<K, V> node = Root;

        while (node != null)
        {
            int compare = Comparator.Compare(key, node.Key);

            if (compare == 0) return true;

            node = compare < 0 ? node.Left : node.Right;
        }

        return false;
    }

    public bool ContainsValue(object value) => ContainsValueRecursive(Root, value);

    private bool ContainsValueRecursive(Node<K, V> node, object value)
    {
        if (node == null) return false;

        if (node.Value.Equals(value)) return true;

        bool left = ContainsValueRecursive(node.Left, value);
        bool right = ContainsValueRecursive(node.Right, value);

        return left ? left : right;
    }

    public IEnumerable<KeyValuePair<K, V>> EntrySet()
    {
        List<KeyValuePair<K, V>> nodes = new List<KeyValuePair<K, V>>();

        AddNodesRecursive(Root, nodes);

        foreach (KeyValuePair<K, V> node in nodes) yield return node;
    }

    private void AddNodesRecursive(Node<K, V> node, ICollection<KeyValuePair<K, V>> collection)
    {
        if (node == null) return;

        AddNodesRecursive(node.Left, collection);

        collection.Add(new KeyValuePair<K, V>(node.Key, node.Value));

        AddNodesRecursive(node.Right, collection);
    }

    public V Get(K key)
    {
        Node<K, V> node = Root;

        while (node != null)
        {
            int compare = Comparator.Compare(key, node.Key);

            if (compare == 0) return node.Value;

            node = compare < 0 ? node.Left : node.Right;
        }

        return default;
    }

    public bool IsEmpty() => Count == 0;

    public List<K> KeySet()
    {
        List<K> keys = new List<K>();
        AddKeysRecursive(Root, keys);
        return keys;
    }

    public IMyCollection<V> Values()
    {
        List<V> values = new List<V>();

        foreach (KeyValuePair<K, V> node in EntrySet()) values.Add(node.Value);

        return values as IMyCollection<V>;
    }

    private void AddKeysRecursive(Node<K, V> node, ICollection<K> collection)
    {
        if (node == null) return;

        AddKeysRecursive(node.Left, collection);

        collection.Add(node.Key);

        AddKeysRecursive(node.Right, collection);
    }

    public void Put(K key, V value)
    {
        Root = PutRecursive(key, value, Root, Color.Red);
    }

    public void PutAll(IMap<K, V> m)
    {
        foreach (KeyValuePair<K, V> item in m.EntrySet()) Put(item.Key, item.Value);
    }
    private Node<K, V> PutRecursive(K key, V value, Node<K, V> node, Color color)
    {
        if (node == null)
        {
            Count++;
            return new Node<K, V>(key, value);
        }

        int comp = Comparator.Compare(key, node.Key);

        if (comp < 0)
        {
            node.Left = PutRecursive(key, value, node.Left, color);

            if (color == Color.Red && node.Left.color == Color.Red)
            {
                int comp2 = Comparator.Compare(key, node.Left.Key);
                node = (comp2 < 0) ? RightRotation(node) : DoubleRightRotation(node);
            }
        }
        else if (comp > 0)
        {
            node.Right = PutRecursive(key, value, node.Right, color);

            if (color == Color.Red && node.Right.color == Color.Red)
            {
                int comp2 = Comparator.Compare(node.Right.Key, key);
                node = (comp2 < 0) ? LeftRotation(node) : DoubleLeftRotation(node);
            }
        }
        else
        {
            node.Value = value;
            return node;
        }

        node.color = GetColor(node.Left) & GetColor(node.Right);

        return node;
    }

    private Color GetColor(Node<K, V> node) => node == null ? Color.Black : node.color;

    private Node<K, V> RightRotation(Node<K, V> n2)
    {
        Node<K, V> n1 = n2.Left;

        n2.Left = n1.Right;
        n1.Right = n2;

        n2.color = Color.Red;

        return n1;
    }

    private Node<K, V> LeftRotation(Node<K, V> n1)
    {
        Node<K, V> n2 = n1.Right;

        n1.Right = n2.Left;
        n2.Left = n1;

        n2.color = n1.color;
        n1.color = Color.Red;

        return n2;
    }

    private Node<K, V> DoubleRightRotation(Node<K, V> n3)
    {
        n3.Left = LeftRotation(n3.Left);

        return RightRotation(n3);
    }

    private Node<K, V> DoubleLeftRotation(Node<K, V> n1)
    {
        n1.Right = RightRotation(n1.Right);

        return LeftRotation(n1);
    }

    public Node<K, V> RemoveNode(K key)
    {
        Node<K, V> removed = new Node<K, V>();

        Root = RemoveRecursive(key, removed, Root);

        return removed;
    }

    public bool Remove(K key) => RemoveNode(key).Key.Equals(key);

    private Node<K, V> RemoveRecursive(K key, Node<K, V> removed, Node<K, V> node)
    {
        if (node == null) return null;

        int compare = Comparator.Compare(key, node.Key);

        if (compare < 0) node.Left = RemoveRecursive(key, removed, node.Left);

        else if (compare > 0) node.Right = RemoveRecursive(key, removed, node.Right);

        else if (node.Left != null && node.Right != null)
        {
            Node<K, V> maxNode = GetMax(node.Left);

            removed.Key = node.Key;
            removed.Value = node.Value;

            node.Key = maxNode.Key;
            node.Value = maxNode.Value;
            node.Left = RemoveMax(node.Left);

            Count--;
        }
        else
        {
            removed.Key = node.Key;
            removed.Value = node.Value;
            node = node.Left ?? node.Right;
            Count--;
        }

        return RebalanceTree(node);
    }

    private Node<K, V> RemoveMax(Node<K, V> node)
    {
        if (node == null) return null;
        else if (node.Right != null)
        {
            node.Right = RemoveMax(node.Right);
            return node;
        }
        return node.Left;
    }

    private Node<K, V> GetMax(Node<K, V> node)
    {
        Node<K, V> parent = null;

        while (node != null)
        {
            parent = node;
            node = node.Right;
        }

        return parent;
    }

    private Node<K, V> RebalanceTree(Node<K, V> node)
    {
        if (node == null) return null;

        if (node.color == Color.Red && node.Left != null && node.Left.color == Color.Red)
        {
            node = RightRotation(node);
            node.color = Color.Black;
            node.Right.color = Color.Red;
        }

        if (node.color == Color.Red && node.Right != null && node.Right.color == Color.Red)
        {
            node = LeftRotation(node);
            node.color = Color.Black;
            node.Left.color = Color.Red;
        }

        if (node.color == Color.Red && node.Left != null && node.Left.Left != null && node.Left.Left.color == Color.Red)
        {
            node = RightRotation(node);
        }

        if (node.color == Color.Red && node.Right != null && node.Right.Right != null && node.Right.Right.color == Color.Red)
        {
            return LeftRotation(node);
        }

        return node;
    }

    public int Size() => Count;

    public K FirstKey()
    {
        Node<K, V> node = Root;

        while (node != null)
        {
            if (node.Left == null) return node.Key;
            node = node.Left;
        }

        return default;
    }

    public K LastKey()
    {
        Node<K, V> node = Root;

        while (node != null)
        {
            if (node.Right == null) return node.Key;
            node = node.Right;
        }

        return default;
    }

    public IMySortedMap<K, V> HeadMap(K to, bool equal = false)
    {
        MyTreeMap<K, V> map = new MyTreeMap<K, V>();

        HeadMapRecursive(to, Root, map, equal);

        return map;
    }

    private void HeadMapRecursive(K key, Node<K, V> node, MyTreeMap<K, V> map, bool equal)
    {
        if (node == null) return;

        HeadMapRecursive(key, node.Left, map, equal);

        int compare = Comparator.Compare(key, node.Key);

        if (compare > 0) map.Put(node.Key, node.Value);

        if (compare == 0 && equal) map.Put(node.Key, node.Value);

        HeadMapRecursive(key, node.Right, map, equal);
    }

    public IMySortedMap<K, V> SubMap(K fromKey, K toKey, bool fromInclude = true, bool toInclude = false)
    {

        MyTreeMap<K, V> map = new MyTreeMap<K, V>();

        SubMapRecursive(fromKey, toKey, Root, map, fromInclude, toInclude);

        return map;
    }

    private void SubMapRecursive(K from, K to, Node<K, V> node, MyTreeMap<K, V> map, bool fromInclude, bool toInclude)
    {
        if (node == null) return;

        SubMapRecursive(from, to, node.Left, map, fromInclude, toInclude);

        int compare1 = Comparator.Compare(from, node.Key);
        int compare2 = Comparator.Compare(to, node.Key);

        if (compare1 < 0 && compare2 > 0) map.Put(node.Key, node.Value);

        if (compare1 == 0 && fromInclude) map.Put(node.Key, node.Value);

        if (compare2 == 0 && toInclude) map.Put(node.Key, node.Value);

        SubMapRecursive(from, to, node.Right, map, fromInclude, toInclude);
    }

    public IMySortedMap<K, V> TailMap(K from, bool equal = false)
    {

        MyTreeMap<K, V> map = new MyTreeMap<K, V>();

        TailMapRecursive(from, Root, map, equal);

        return map;
    }

    private void TailMapRecursive(K key, Node<K, V> node, MyTreeMap<K, V> map, bool equal)
    {
        if (node == null) return;

        TailMapRecursive(key, node.Left, map, equal);

        int compare = Comparator.Compare(key, node.Key);

        if (compare < 0) map.Put(node.Key, node.Value);

        if (compare == 0 && equal) map.Put(node.Key, node.Value);

        TailMapRecursive(key, node.Right, map, equal);
    }

    public KeyValuePair<K, V> LowerEntry(K key)
    {
        MyTreeMap<K, V> map = (MyTreeMap<K, V>)HeadMap(key);

        Node<K, V> node = map.Root;

        while (node != null)
        {
            if (node.Right == null) return new KeyValuePair<K, V>(node.Key, node.Value);
            node = node.Right;
        }

        return default;
    }

    public KeyValuePair<K, V> FloorEntry(K key)
    {
        MyTreeMap<K, V> map = (MyTreeMap<K, V>)HeadMap(key, true);

        Node<K, V> node = map.Root;

        while (node != null)
        {
            if (node.Right == null) return new KeyValuePair<K, V>(node.Key, node.Value);
            node = node.Right;
        }

        return default;
    }

    public KeyValuePair<K, V> HigherEntry(K key)
    {
        MyTreeMap<K, V> map = (MyTreeMap<K, V>)TailMap(key);

        Node<K, V> node = map.Root;

        while (node != null)
        {
            if (node.Left == null) return new KeyValuePair<K, V>(node.Key, node.Value);
            node = node.Left;
        }

        return default;
    }

    public KeyValuePair<K, V> CeilingEntry(K key)
    {
        MyTreeMap<K, V> map = (MyTreeMap<K, V>)TailMap(key, true);

        Node<K, V> node = map.Root;

        while (node != null)
        {
            if (node.Left == null) return new KeyValuePair<K, V>(node.Key, node.Value);
            node = node.Left;
        }

        return default;
    }

    public K LowerKey(K key) => LowerEntry(key).Key;

    public K FloorKey(K key) => FloorEntry(key).Key;

    public K HigherKey(K key) => HigherEntry(key).Key;

    public K CeilingKey(K key) => CeilingEntry(key).Key;

    public KeyValuePair<K, V> PollFirstEntry(){
        Node<K, V> node = RemoveNode(FirstKey());
        return new KeyValuePair<K, V>(node.Key, node.Value);
    }

    public KeyValuePair<K, V> PollLastEntry()
    {
        Node<K, V> node = RemoveNode(LastKey());
        return new KeyValuePair<K, V>(node.Key, node.Value);
    }

    public V FirstEntry() => Get(FirstKey());

    public V LastEntry() => Get(LastKey());
}
