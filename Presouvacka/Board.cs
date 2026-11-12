using System;
using System.Collections.Generic;
using System.Text;

namespace Presouvacka
{
    ///Hrací deska: matice kamenů Size x Size a pozice prázdného pole.
    public sealed class Board
    {
        public const int DefaultSize = 4;

        private readonly Tile[,] _tiles;

        public int Size { get; }
        public Position EmptyPosition { get; private set; }

        ///Vytvoří složenou desku: 1, 2, 3 ... a prázdné pole vpravo dole.
        public Board(int size = DefaultSize)
        {
            if (size < 2)
                throw new ArgumentOutOfRangeException(nameof(size));

            Size = size;
            _tiles = new Tile[size, size];

            for (int row = 0; row < size; row++)
                for (int col = 0; col < size; col++)
                    _tiles[row, col] = new Tile(row * size + col + 1);

            _tiles[size - 1, size - 1] = new Tile(Tile.EmptyValue);
            EmptyPosition = new Position(size - 1, size - 1);
        }

        private Board(int size, IReadOnlyList<int> values) : this(size)
        {
            if (values.Count != size * size)
                throw new ArgumentException("Počet hodnot neodpovídá velikosti desky.", nameof(values));

            var seen = new bool[size * size];
            bool emptyFound = false;

            for (int i = 0; i < values.Count; i++)
            {
                int value = values[i];
                if (value < 0 || value >= size * size || seen[value])
                    throw new ArgumentException("Každé číslo 0..N*N-1 musí být právě jednou.", nameof(values));

                seen[value] = true;
                var position = new Position(i / size, i % size);
                _tiles[position.Row, position.Col] = new Tile(value);

                if (value == Tile.EmptyValue)
                {
                    EmptyPosition = position;
                    emptyFound = true;
                }
            }

            if (!emptyFound)
                throw new ArgumentException("Deska musí obsahovat prázdné pole (0).", nameof(values));
        }

        ///Vytvoří desku z hodnot po řádcích (0 = prázdné pole).
        public static Board FromValues(IReadOnlyList<int> values, int size = DefaultSize) => new(size, values);

        ///Vrátí hodnoty kamenů po řádcích (0 = prázdné pole).
        public int[] ToValues()
        {
            var values = new int[Size * Size];
            for (int row = 0; row < Size; row++)
                for (int col = 0; col < Size; col++)
                    values[row * Size + col] = _tiles[row, col].Value;
            return values;
        }

        public Tile GetTile(int row, int col)
        {
            if (!IsInside(new Position(row, col)))
                throw new ArgumentOutOfRangeException(nameof(row), "Pozice leží mimo desku.");
            return _tiles[row, col];
        }

        public bool IsInside(Position position) =>
            position.Row >= 0 && position.Row < Size &&
            position.Col >= 0 && position.Col < Size;

        ///Tah je platný, když kámen není prázdný a sousedí s prázdným polem.
        public bool CanMoveTile(Position position)
        {
            if (!IsInside(position) || _tiles[position.Row, position.Col].IsEmpty)
                return false;

            int distance = Math.Abs(position.Row - EmptyPosition.Row) +
                           Math.Abs(position.Col - EmptyPosition.Col);
            return distance == 1;
        }

        ///Přesune kámen na dané pozici do prázdného pole. Neplatný tah nic nezmění a vrátí false.
        public bool TryMoveTile(Position position)
        {
            if (!CanMoveTile(position))
                return false;

            _tiles[EmptyPosition.Row, EmptyPosition.Col] = _tiles[position.Row, position.Col];
            _tiles[position.Row, position.Col] = new Tile(Tile.EmptyValue);
            EmptyPosition = position;
            return true;
        }

        ///Posune kámen v daném směru do prázdného pole (pro klávesnici).
        public bool TryMove(Direction direction)
        {
            Position source = direction switch
            {
                Direction.Up => new Position(EmptyPosition.Row + 1, EmptyPosition.Col),
                Direction.Down => new Position(EmptyPosition.Row - 1, EmptyPosition.Col),
                Direction.Left => new Position(EmptyPosition.Row, EmptyPosition.Col + 1),
                Direction.Right => new Position(EmptyPosition.Row, EmptyPosition.Col - 1),
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };

            return TryMoveTile(source);
        }

        ///Je deska složená (1, 2, 3 ... a prázdné pole na konci)?
        public bool IsSolved
        {
            get
            {
                for (int row = 0; row < Size; row++)
                {
                    for (int col = 0; col < Size; col++)
                    {
                        bool isLast = row == Size - 1 && col == Size - 1;
                        int expected = isLast ? Tile.EmptyValue : row * Size + col + 1;
                        if (_tiles[row, col].Value != expected)
                            return false;
                    }
                }
                return true;
            }
        }
    }
}
