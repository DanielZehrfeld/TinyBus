using TinyBus;

namespace TinyBusTest.Messages
{
    internal class TestMessageString: ITinyBusMessage<TestMessageString>
    {
        public string Text { get; set; }

        public TestMessageString Clone()
        {
            return new TestMessageString
            {
                Text = Text
            };
        }

        public bool Compares(TestMessageString otherMessage)
        {
            return otherMessage != null && Text == otherMessage.Text;
        }
    }
}
