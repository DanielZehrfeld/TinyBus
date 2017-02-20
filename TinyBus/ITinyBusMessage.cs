namespace TinyBus
{
    public interface ITinyBusMessage<out T>
    {
        T Clone();
    }
}
