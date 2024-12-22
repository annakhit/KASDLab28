using System;
using System.Collections.Generic;
using System.Linq;
public class MyPriorityQueue<T>: IMyQueue<T>
{
    public class MyIterator<E> : IMyIterator<MyPriotiryTask<E>>
    {
        private int cursor;

        private readonly MyPriorityQueue<E> queue;

        public MyIterator(MyPriorityQueue<E> queue)
        {
            this.queue = queue;
            cursor = -1;
        }

        public bool HasNext() => cursor < queue.Size() - 1;

        public MyPriotiryTask<E> Next()
        {
            if (!HasNext()) throw new InvalidOperationException();
            cursor++;
            return queue.queue[cursor];
        }

        public void Remove()
        {
            if (cursor < 0) throw new InvalidOperationException();
            queue.Remove(queue.queue[cursor]);
            cursor--;
        }
    }

    protected MyPriotiryTask<T>[] queue;
    private int size = 0;
    private readonly IComparer<MyPriotiryTask<T>> comparator = new MyPriotiryTaskComparer<T>();

    public MyPriorityQueue()
    {
        queue = new MyPriotiryTask<T>[11];
    }

    public MyPriorityQueue(IMyCollection<T> collection)
    {
        foreach (T element in collection.ToArray()) Add(element);
    }

    public MyPriorityQueue(uint initialCapacity)
    {
        queue = new MyPriotiryTask<T>[initialCapacity];
    }

    public MyPriorityQueue(uint initialCapacity, IComparer<MyPriotiryTask<T>> comparator)
    {
        queue = new MyPriotiryTask<T>[initialCapacity];
        this.comparator = comparator;
    }

    public MyPriorityQueue(MyPriorityQueue<T> priorityQueue)
    {
        queue = (MyPriotiryTask<T>[])priorityQueue.queue.Clone();
        size = priorityQueue.size;
        comparator = priorityQueue.comparator;
    }

    public void Add(T element) => Add(element, 0);

    public void Add(T element, int priority = 0)
    {
        if (size == queue.Length) UpdateCapacity();
        queue[size] = new MyPriotiryTask<T>(element, priority);
        size++;
        Array.Sort(queue, 0, size, comparator);
    }

    public void AddAll(IMyCollection<T> collection)
    {
        foreach (T element in collection.ToArray()) Add(element);
    }

    public void Clear()
    {
        queue = new MyPriotiryTask<T>[11];
        size = 0;
    }

    public bool Contains(object obj)
    {
        foreach (MyPriotiryTask<T> element in queue)
        {
            if (element == null) return false;
            if (obj.Equals(element.value)) return true;
        }
        return false;
    }

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
        int offset = 0;

        MyPriotiryTask<T>[] newQueue = new MyPriotiryTask<T>[size];

        for (int index = 0; index < size; index++)
        {
            if (obj.Equals(queue[index].value)) offset++;
            newQueue[index] = queue[index + offset];
        }

        size -= offset;
        queue = newQueue;
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
        T[] result = new T[size];
        for (int index = 0; index < size; index++)
        {
            result[index] = queue[index].value;
        }
        return result;
    }

    public T[] ToArray(ref T[] array)
    {
        if (array == null) return ToArray();

        if (array.Length < size)
            throw new ArgumentOutOfRangeException();

        for (int index = 0; index < size; index++)
        {
            array[index] = queue[index].value;
        }
        return array;
    }

    public T Peek()
    {
        if (size == 0) return default;
        return queue[0].value;
    }

    public T Element() => Peek();

    public T Poll()
    {
        if (size == 0) return default;

        T element = queue[0].value;

        for (int index = 0; index < size - 1; index++) queue[index] = queue[index + 1];
        queue[size - 1] = default;
        size--;

        return element;
    }

    private void UpdateCapacity()
    {
        MyPriotiryTask<T>[] newQueue = new MyPriotiryTask<T>[size < 64 ? size * 2 + 1 : (int)(size * 1.5) + 1];
        for (int i = 0; i < size; i++) newQueue[i] = queue[i];
        queue = newQueue;
    }

    public MyIterator<T> Iterator() => new MyIterator<T>(this);

    public void Print()
    {
        foreach (MyPriotiryTask<T> task in queue.Where(value => value != null)) Console.WriteLine("Значение: {0}  Приоритет: {1}", task.value, task.priority);
    }
}