using System;
using System.Collections.Generic;

namespace dealeron_trains
{
    //Using a library would make more sense for real code, but I like to limit the dependencies for take-home code tests like this
    public class MinHeap<TKey, TValue> where TKey: IComparable
    {
        private (TKey key, TValue value)[] _data = new (TKey, TValue)[33];
        private int _free = 1;
        //keeps track of where a given value is
        //this way we can easily decrease the key of a given value rather than needing to search the array
        private readonly Dictionary<TValue, int> _reverseIndex = new Dictionary<TValue, int>();

        public int Length => _free - 1;

        public void Insert(TKey key, TValue value)
        {
            if (_reverseIndex.ContainsKey(value))
                throw new ArgumentException("Value has already been inserted");
            _data[_free] = (key, value);
            _reverseIndex[value] = _free;
            _free++;
            if (_free >= _data.Length)
            {
                var newData = new (TKey, TValue)[_data.Length * 2 - 1];
                _data.CopyTo(newData, 0);
                _data = newData;
            }
            DecreaseKeyByIndex(_free - 1, key);
        }

        public void DecreaseKey(TValue value, TKey key)
        {
            DecreaseKeyByIndex(_reverseIndex[value], key);
        }

        private void DecreaseKeyByIndex(int index, TKey key)
        {
            _data[index].key = key;
            var parent = Parent(index);
            while (index > 1 && parent.HasValue && _data[parent.Value].key.CompareTo(_data[index].key) > 0)
            {
                SwapIndices(index, parent.Value);
                index = parent.Value;
                parent = Parent(index);
            }
        }

        public (TKey key, TValue value)? Extract()
        {
            if (_free <= 1)
                return null;

            var top = _data[1];
            _reverseIndex.Remove(top.value);
            _data[1] = _data[_free-1];
            _free--;
            _reverseIndex[_data[1].value] = 1;
            Heapify(1);

            return top;
        }

        private void Heapify(int index)
        {
            if (index >= _free)
                return;
            var left = Left(index);
            var right = Right(index);
            //pick min between left and right
            //have to be a little verbose because TKey might not be nullable
            if (left.HasValue && _data[left.Value].key.CompareTo(_data[index].key) < 0 && 
                (!right.HasValue || _data[left.Value].key.CompareTo(_data[right.Value].key) <= 0))
            {
                SwapIndices(index, left.Value);
                Heapify(left.Value);
            }
            else if (right.HasValue && _data[right.Value].key.CompareTo(_data[index].key) < 0 &&
                (!left.HasValue || _data[right.Value].key.CompareTo(_data[left.Value].key) <= 0))
            {
                SwapIndices(index, right.Value);
                Heapify(right.Value);
            }
        }

        private void SwapIndices(int i, int j)
        {
            (_data[i], _data[j]) = (_data[j], _data[i]);
            _reverseIndex[_data[i].value] = i;
            _reverseIndex[_data[j].value] = j;
        }

        public TKey GetKey(TValue value)
        {
            return _data[_reverseIndex[value]].key;
        }

        private int? Left(int index)
        {
            var left = index * 2;
            if (left < _free)
                return left;
            else
                return null;
        }

        private int? Right(int index)
        {
            var right = index * 2 + 1;
            if (right < _free)
                return right;
            else
                return null;
        }

        private int? Parent(int index)
        {
            var parent = index / 2;
            if (parent < _free && parent >= 1)
                return parent;
            else
                return null;
        }
    }
}
