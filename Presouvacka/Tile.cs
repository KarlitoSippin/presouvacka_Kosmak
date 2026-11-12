using System;
using System.Collections.Generic;
using System.Text;

namespace Presouvacka
{
    ///Herní kámen. Hodnota 0 = prázdné pole.
    public sealed class Tile
    {
        public const int EmptyValue = 0;

        public int Value { get; }
        public bool IsEmpty => Value == EmptyValue;

        public Tile(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            Value = value;
        }

        public override string ToString() => IsEmpty ? string.Empty : Value.ToString();
    }
}
