public enum Color { Red = 0, Black = 1 }

public class Node<K, V>
{
    public K Key { get; set; }
    public V Value { get; set; }

    public Node<K, V> Left { get; set; }
    public Node<K, V> Right { get; set; }

    public Color color = Color.Black;

    public Node() { }

    public Node(K Key, V Value) : this(Key, Value, null, null) { }

    public Node(K Key, V Value, Node<K, V> Left, Node<K, V> Right)
    {
        this.Key = Key;
        this.Value = Value;
        this.Left = Left;
        this.Right = Right;
    }
}