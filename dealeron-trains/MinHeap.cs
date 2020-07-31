using System;
using System.Collections.Generic;

namespace dealeron_trains
{
    //Using a library would make more sense for real code, but I like to limit the dependencies for take-home code tests like this
    public class MinHeap<TKey, TValue> where TKey: IComparable
    {
        private (TKey key, TValue value)[] data = new (TKey, TValue)[33];
        private int free = 1;
        //keeps track of where a given value is
        //this way we can easily decrease the key of a given value rather than needing to search the array
        private readonly Dictionary<TValue, int> reverseIndex = new Dictionary<TValue, int>();

        public int Length => free - 1;

        public void Insert(TKey key, TValue value)
        {
            if (reverseIndex.ContainsKey(value))
                throw new ArgumentException("Value has already been inserted");
            data[free] = (key, value);
            reverseIndex[value] = free;
            free++;
            if (free >= data.Length)
            {
                var newData = new (TKey, TValue)[data.Length * 2 - 1];
                data.CopyTo(newData, 0);
                data = newData;
            }
            DecreaseKeyByIndex(free - 1, key);
        }

        public void DecreaseKey(TValue value, TKey key)
        {
            DecreaseKeyByIndex(reverseIndex[value], key);
        }

        private void DecreaseKeyByIndex(int index, TKey key)
        {
            data[index].key = key;
            var parent = Parent(index);
            while (index > 1 && parent.HasValue && data[parent.Value].key.CompareTo(data[index].key) > 0)
            {
                SwapIndices(index, parent.Value);
                index = parent.Value;
                parent = Parent(index);
            }
        }

        public (TKey key, TValue value)? Extract()
        {
            if (free <= 1)
                return null;

            var top = data[1];
            data[1] = data[free-1];
            free--;
            reverseIndex[data[1].value] = 1;
            Heapify(1);

            return top;
        }

        private void Heapify(int index)
        {
            if (index >= free)
                return;
            var left = Left(index);
            var right = Right(index);
            //pick min between left and right
            //have to be a little verbose because TKey might not be nullable
            if (left.HasValue && data[left.Value].key.CompareTo(data[index].key) < 0 && 
                (!right.HasValue || data[left.Value].key.CompareTo(data[right.Value].key) <= 0))
            {
                SwapIndices(index, left.Value);
                Heapify(left.Value);
            }
            else if (right.HasValue && data[right.Value].key.CompareTo(data[index].key) < 0 &&
                (!left.HasValue || data[right.Value].key.CompareTo(data[left.Value].key) <= 0))
            {
                SwapIndices(index, right.Value);
                Heapify(right.Value);
            }
        }

        private void SwapIndices(int i, int j)
        {
            (data[i], data[j]) = (data[j], data[i]);
            reverseIndex[data[i].value] = i;
            reverseIndex[data[j].value] = j;
        }

        private int? Left(int index)
        {
            var left = index * 2;
            if (left < free)
                return left;
            else
                return null;
        }

        private int? Right(int index)
        {
            var right = index * 2 + 1;
            if (right < free)
                return right;
            else
                return null;
        }

        private int? Parent(int index)
        {
            var parent = index / 2;
            if (parent < free && parent >= 1)
                return parent;
            else
                return null;
        }
    }
}
