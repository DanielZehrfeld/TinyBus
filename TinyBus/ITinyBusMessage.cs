namespace TinyBus
{
    public interface ITinyBusMessage<out T>
    {
        /// <summary>
        /// Is called to create a copy of the bradcasted message for each recipient
        /// </summary>
        /// <returns>A copy of the message</returns>
        T Clone();
    }
}
