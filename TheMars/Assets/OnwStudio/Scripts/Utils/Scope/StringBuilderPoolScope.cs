using System;
using System.Text;
using Onw.Pool;

namespace Onw.Scope
{
    public sealed class StringBuilderPoolScope : IDisposable
    {
        private readonly StringBuilder _stringBuilder = StringBuilderPool.Shared.Get();
        private bool _disposed;

        public StringBuilder Get()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(StringBuilderPoolScope));
            return _stringBuilder;
        }

        public void Dispose()
        {
            if (_disposed) return;

            if (_stringBuilder is null)
            {
                StringBuilderPool.Shared.Release(_stringBuilder);
            }

            _disposed = true;
            GC.SuppressFinalize(this);
        }

        ~StringBuilderPoolScope()
        {
            Dispose();
        }
    }
}