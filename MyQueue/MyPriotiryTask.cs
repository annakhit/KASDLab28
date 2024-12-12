public class MyPriotiryTask<T>
{
    public readonly T value;
    public readonly int priority;

    public MyPriotiryTask(T value, int priority)
    {
        this.value = value;
        this.priority = priority;
    }
}