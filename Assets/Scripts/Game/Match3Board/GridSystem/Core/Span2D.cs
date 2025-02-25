using System;

namespace O2.Grid{
    public readonly ref struct Span2D<T>{
        private readonly Span<T> _span;
        private readonly int _rows;
        private readonly int _cols;

        public Span2D(Span<T> span, int rows, int cols){
            if (span.Length != rows * cols)
                throw new ArgumentException("Span length does not match the given dimensions.");

            _span = span;
            _rows = rows;
            _cols = cols;
        }

        public ref T this[int row, int col] => ref _span[row * _cols + col];
    }
}