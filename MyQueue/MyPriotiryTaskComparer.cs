using System.Collections.Generic;

class MyPriotiryTaskComparer<T> : IComparer<MyPriotiryTask<T>>
{
    public int Compare(MyPriotiryTask<T> x, MyPriotiryTask<T> y) => y.priority.CompareTo(x.priority);
}