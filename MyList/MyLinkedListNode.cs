using System;

public class MyLinkedListNode<T> where T : IComparable<T>
{
    internal MyLinkedListNode<T> next;
    internal MyLinkedListNode<T> prev;
    internal T item;

    public MyLinkedListNode(T item)
    {
        this.item = item;
    }
}