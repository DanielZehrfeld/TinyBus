using TinyBus;

namespace TinyBusTest.Messages
{
    internal class TestMessageInt: ITinyBusMessage<TestMessageInt>
    {
        public int Value{ get; set; }

        public TestMessageInt Clone()
        {
            return new TestMessageInt
            {
                Value = Value
            };
        }

        public bool Compares(TestMessageInt otherMessage)
        {
            return otherMessage != null && otherMessage.Value == Value;
        }
    }
}
