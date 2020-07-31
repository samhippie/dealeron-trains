using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dealeron_trains.tests
{
    [TestFixture]
    public class MinHeapTests
    {
        [Test]
        public void MinHeap_OrdersNumbersShort()
        {
            MinHeap_OrdersNumbers(new List<int> { 6, 1, 9, 45 });
        }

        [Test]
        public void MinHeap_OrdersNumbersMedium()
        {
            MinHeap_OrdersNumbers(new List<int> { 20, 4, 2, 4, -1, 1, 7, 2 });
        }

        [Test]
        public void MinHeap_OrdersNumbersLong()
        {
            var numbers = new List<int> { 4, 2, 6, 21, 4, 1, 7, 9, 2, 45, 84 };
            //add enough numbers to force a resize
            for (var i = 0; i < 100; i++)
            {
                numbers.Add(i);
            }
            MinHeap_OrdersNumbers(numbers);
        }

        private void MinHeap_OrdersNumbers(List<int> numbers)
        {
            var heap = new MinHeap<int, string>();
            var unique = 0; // the values have to all be unique
            foreach (var number in numbers)
            {
                heap.Insert(number, $"number {number} ({unique++})");
            }

            var output = new List<string>();
            while (heap.Length > 0)
            {
                output.Add(heap.Extract()?.value);
            }

            Assert.AreEqual(numbers.Count, output.Count);

            numbers.Sort();

            for (var i = 0; i< numbers.Count; i++)
            {
                Assert.IsTrue(output[i].StartsWith($"number {numbers[i]}"));
            }
        }

        [Test]
        public void MinHeap_DecreaseKeys1()
        {
            var heap = new MinHeap<int, string>();
            heap.Insert(2, "a");
            heap.Insert(3, "b");
            heap.Insert(4, "c");
            heap.Insert(1, "d");

            heap.DecreaseKey("c", 0);

            var min = heap.Extract();
            Assert.AreEqual(0, min.Value.key);
            Assert.AreEqual("c", min.Value.value);
        }

        [Test]
        public void MinHeap_DecreaseKeys2()
        {
            var heap = new MinHeap<int, string>();
            heap.Insert(2, "a");
            heap.Insert(4, "b");
            heap.Insert(5, "c");
            heap.Insert(10, "d");
            heap.Insert(8, "e");
            heap.Insert(2, "f");

            heap.DecreaseKey("b", 3);

            var a = heap.Extract();
            var f = heap.Extract();
            //a and f might be swapped because they have the same key
            if (a.Value.value == "f")
            {
                (a, f) = (f, a);
            }
            var b = heap.Extract();

            Assert.AreEqual(2, a.Value.key);
            Assert.AreEqual("a", a.Value.value);
            Assert.AreEqual(2, f.Value.key);
            Assert.AreEqual("f", f.Value.value);
            Assert.AreEqual(3, b.Value.key);
            Assert.AreEqual("b", b.Value.value);
        }
    }
}
