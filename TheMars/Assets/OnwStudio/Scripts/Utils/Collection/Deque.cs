using System;
using System.Collections;
using System.Collections.Generic;

namespace Onw.Collections
{
    public class Deque<T> : IEnumerable<T>
    {
        private readonly LinkedList<T> _buffer = new();

        public IEnumerator<T> GetEnumerator()
        {
            return _buffer.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _buffer.GetEnumerator();
        }

        public void AddFirst(T item)
        {
            _buffer.AddFirst(item);
        }

        public void AddLast(T item)
        {
            _buffer.AddLast(item);
        }

        public T PeekFirst()
        {
            if (_buffer.First == null) throw new InvalidOperationException("Deque empty");
            var result = _buffer.First.Value;
            return result;
        }

        public T PeekLast()
        {
            if (_buffer.Last == null) throw new InvalidOperationException("Deque empty");
            var result = _buffer.Last.Value;
            return result;

        }

        public bool TryPeekFirst(out T result)
        {
            if (_buffer.First != null)
            {
                result = _buffer.First.Value;
                return true;
            }
            result = default;
            return false;
        }

        public bool TryPeekLast(out T result)
        {
            if (_buffer.Last != null)
            {
                result = _buffer.Last.Value;
                return true;
            }
            result = default;
            return false;
        }

        public T DequeueFirst()
        {
            var result = PeekFirst();
            _buffer.RemoveFirst();
            return result;
        }

        public T DequeueLast()
        {
            var result = PeekLast();
            _buffer.RemoveLast();
            return result;
        }

        public bool TryDequeueFirst(out T result)
        {
            var canDeque = TryPeekFirst(out result);
            if (canDeque)
            {
                _buffer.RemoveFirst();
            }
            return canDeque;
        }

        public bool TryDequeueLast(out T result)
        {
            var canDeque = TryPeekLast(out result);
            if (canDeque)
            {
                _buffer.RemoveLast();
            }
            return canDeque;
        }

        public void Clear()
        {
            _buffer.Clear();
        }

        public bool Contains(T item)
        {
            return _buffer.Contains(item);
        }
    }
}
