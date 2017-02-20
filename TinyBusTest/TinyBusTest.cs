using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBus;
using TinyBusTest.Messages;

namespace TinyBusTest
{
    [TestClass]
    public class TinyBusTest
    {
        [TestMethod]
        public void Handlers_should_get_copies_of_message()
        {
            var bus = TinyBusControl.Create();

            bus.Handle((TestMessageInt tm) =>
            {
                tm.Value += 1;
                return tm;
            });

            bus.Handle((TestMessageInt tm) =>
            {
                tm.Value += 2;
                return tm;
            });

            const int value = 7;

            var result = bus.Publish(new TestMessageInt { Value = value }).Result;

            Assert.IsTrue(result.Sum(x => x.Value - value) == 3);
        }

        [TestMethod]
        public void Handlers_should_be_notified_and_return_values()
        {
            var bus = TinyBusControl.Create();

            var resultA = new TestMessageString {Text = "TestA"};
            var resultB = new TestMessageInt {Value = 77};
            var resultC = new TestMessageString { Text = "TestC" };

            var handlerA = new TestHandler<TestMessageString>(bus, resultA);
            var handlerB = new TestHandler<TestMessageInt>(bus, resultB);
            var handlerC = new TestHandler<TestMessageString>(bus, resultC);

            var message = new TestMessageString
            {
                Text = "input"
            };

            var testresult = bus.Publish(message).Result;

            Assert.IsNotNull(testresult.Single(x => x.Compares(resultA)));
            Assert.IsNotNull(testresult.Single(x => x.Compares(resultC)));

            Assert.IsTrue(testresult.Length == 2);

            Assert.IsTrue(handlerA.ReceivedValue.Compares(message));
            Assert.IsTrue(handlerB.ReceivedValue == null);
            Assert.IsTrue(handlerC.ReceivedValue.Compares(message));
        }

        [TestMethod]
        public void Handlers_should_work_parallel()
        {
            var bus = TinyBusControl.Create();

            const int count = 10;
            const int sleepMs = 1000;

            var handlers = new List<TestHandler<TestMessageString>>();
            for (var t = 0; t < count; t++)
            {
                handlers.Add(new TestHandler<TestMessageString>(bus, new TestMessageString(), sleepMs));
            }

            var message = new TestMessageString
            {
                Text = "input"
            };

            var sw = new Stopwatch();
            sw.Start();

            var testresult = bus.Publish(message).Result;
            
            sw.Stop();

            Assert.IsTrue(testresult.Length == count);
            Assert.IsTrue(handlers.All(x => message.Compares(x.ReceivedValue)));

            Assert.IsTrue(sw.Elapsed.Milliseconds < count * sleepMs);

            Console.WriteLine("Processing took ms: " + sw.Elapsed.Milliseconds);
        }
    }
}
