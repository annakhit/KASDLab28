using System;

public class MyLinkedList<T> : IMyList<T> where T : IComparable<T>
{
    public class MyIterator<E> : IMyIteratorList<E> where E : IComparable<E>
    {
        private int cursor;

        private readonly MyLinkedList<E> linkedList;

        public MyIterator(MyLinkedList<E> linkedList)
        {
            this.linkedList = linkedList;
            cursor = -1;
        }

        public MyIterator(MyLinkedList<E> linkedList, int cursor)
        {
            this.linkedList = linkedList;
            this.cursor = cursor;
        }

        public bool HasNext() => cursor < linkedList.Size() - 1;

        public E Next()
        {
            if (!HasNext()) throw new InvalidOperationException();
            cursor++;
            return linkedList.Get(cursor);
        }

        public bool HasPrevious() => cursor > 0;

        public E Previous()
        {
            if (cursor < 1) throw new InvalidOperationException();
            return linkedList.Get(cursor - 1);
        }

        public int NextIndex() => HasNext() ? cursor + 1 : default;

        public int PreviousIndex() => cursor > 1 ? cursor - 1 : default;

        public void Set(E element) => linkedList.Set(cursor, element);

        public void Add(E element) => linkedList.Add(cursor, element);

        public void Remove()
        {
            if (cursor < 0) throw new InvalidOperationException();
            linkedList.Remove(cursor);
            cursor--;
        }
    }

    internal MyLinkedListNode<T> head;
    private int size = 0;

    public MyLinkedList() {}

    public MyLinkedList(IMyCollection<T> collection)
    {
        foreach (T item in collection.ToArray()) AddLast(item);
    }

    public MyLinkedListNode<T> AddLast(T value)
    {
        MyLinkedListNode<T> result = new MyLinkedListNode<T>(value);
        if (head == null) InsertNodeToEmptyList(result);
        else
        {
            InsertNodeBefore(head, result);
        }
        return result;
    }

    public void Add(T value)  // AddFirst() Push()
    {
        MyLinkedListNode<T> result = new MyLinkedListNode<T>(value);
        if (head == null) InsertNodeToEmptyList(result);
        else
        {
            InsertNodeBefore(head, result);
            head = result;
        }
    }

    public void AddAll(IMyCollection<T> collection)
    {
        foreach (T item in collection.ToArray()) Add(item);
    }

    public void Clear()
    {
        head = null;
        size = 0;
    }

    public bool Contains(object obj) => Find(obj) != null;

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
        MyLinkedListNode<T> node = Find(obj);
        if (node != null) RemoveNode(node);
    }

    public void RemoveAll(IMyCollection<T> collection)
    {
        foreach (T element in collection.ToArray()) Remove(element);
    }

    public void RetainAll(IMyCollection<T> collection)
    {
        foreach (T element in collection.ToArray())
        {
            if (!Contains(element)) Remove(element);
        }
    }

    public int Size() => size;

    public T[] ToArray()
    {
        MyLinkedListNode<T> node = head;
        T[] array = new T[size];
        for (int i = 0; i < size; i++)
        {
            array[i] = node.item;
            node = node.next;
        }
        return array;
    }

    public T[] ToArray(ref T[] array)
    {
        T[] elements = ToArray();
        if (array == null) return elements;

        if (array.Length < elements.Length)
            throw new ArgumentOutOfRangeException();

        for (int index = 0; index < elements.Length; index++)
        {
            array[index] = elements[index];
        }
        return array;
    }

    public void Add(int index, T value)
    {
        MyLinkedListNode<T> node = GetNode(index);
        MyLinkedListNode<T> result = new MyLinkedListNode<T>(value);
        InsertNodeBefore(node, result);
        if (node == head) head = result;
    }

    public void AddAll(int index, IMyCollection<T> collection)
    {
        foreach (T item in collection.ToArray()) Add(index, item);
    }

    public T Get(int index)
    {
        MyLinkedListNode<T> node = GetNode(index);
        return node.item;
    }

    public int IndexOf(object obj)
    {
        MyLinkedListNode<T> node = head;

        int index = -1;

        for (int i = 0; i < size; i++)
        {
            if (node.item.Equals(obj))
            {
                index = i;
                break;
            }
            node = node.next;
        }

        return index;
    }

    public int LastIndexOf(object obj)
    {
        MyLinkedListNode<T> node = head.prev;

        int index = -1;

        for (int i = size; i > 0; i--)
        {
            if (node.item.Equals(obj))
            {
                index = i - 1;
                break;
            }
            node = node.prev;
        }

        return index;
    }

    public T Remove(int index)
    {
        MyLinkedListNode<T> node = GetNode(index);
        RemoveNode(node);
        return node.item;
    }

    public void Set(int index, T value)
    {
        MyLinkedListNode<T> node = GetNode(index);
        node.item = value;
    }

    public IMyList<T> SubList(int fromIndex, int toIndex)
    {
        if (fromIndex >= toIndex) throw new ArgumentOutOfRangeException();
        T[] array = new T[toIndex - fromIndex];
        Array.Copy(ToArray(), fromIndex, array, 0, toIndex - fromIndex);
        return new MyLinkedList<T>(array as IMyCollection<T>);
    }

    public T Peek() => head.item; // Element() GetFirst() PeekFirst()

    public T GetLast() // PeekLast()
    {
        if (head == null) return default;
        return head.prev.item;
    }

    public T Pull() // PollFirst() RemoveFirst()
    {
        MyLinkedListNode<T> node = head;
        if (node == null) return default;
        RemoveNode(node);
        return node.item;
    }

    public T Pop() // PollLast() RemoveLast()
    {
        if (head == null) return default;
        MyLinkedListNode<T> node = head.prev;
        RemoveNode(node);
        return node.item;
    }

    public bool RemoveLastOccurrence(T value)
    {
        int index = LastIndexOf(value);
        if (index == -1) return false;
        Remove(index);
        return true;
    }

    public bool RemoveFirstOccurrence(T value)
    {
        int index = IndexOf(value);
        if (index == -1) return false;
        Remove(index);
        return true;
    }

    private MyLinkedListNode<T> GetNode(int index)
    {
        if (index >= size) throw new ArgumentOutOfRangeException();

        MyLinkedListNode<T> node = head;

        for (int i = 0; i < index; i++) node = node.next;

        return node;
    }

    private void RemoveNode(MyLinkedListNode<T> node)
    {
        if (node.next == node) head = null;
        else
        {
            node.next.prev = node.prev;
            node.prev.next = node.next;
            if (head == node) head = node.next;
        }
        size--;
    }

    private void InsertNodeToEmptyList(MyLinkedListNode<T> newNode)
    {
        newNode.next = newNode;
        newNode.prev = newNode;
        head = newNode;
        size++;
    }

    private void InsertNodeBefore(MyLinkedListNode<T> node, MyLinkedListNode<T> newNode)
    {
        newNode.next = node;
        newNode.prev = node.prev;
        node.prev.next = newNode;
        node.prev = newNode;
        size++;
    }

    private MyLinkedListNode<T> Find(object obj)
    {
        MyLinkedListNode<T> node = head;
        if (node != null)
        {
            do
            {
                if (node.item.Equals(obj)) return node;
                node = node.next;
            } while (node != head);
        }
        return null;
    }

    public IMyIteratorList<T> ListIterator() => new MyIterator<T>(this);

    public IMyIteratorList<T> ListIterator(int index) => new MyIterator<T>(this, index);

    public void Print()
    {
        Console.WriteLine(string.Join(" ", ToArray()));
    }
}