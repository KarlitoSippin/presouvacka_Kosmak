using System;
using System.Collections.Generic;
using System.Text;

namespace Presouvacka
{
    ///Vytváří zamíchané desky, které jsou vždy řešitelné a nejsou už složené
    public sealed class BoardShuffler
    {
        private readonly Random _random;

        public BoardShuffler(Random? random = null)
        {
            _random = random ?? new Random();
        }

        public Board CreateShuffledBoard(int size = Board.DefaultSize)
        {
            Board board;

            do
            {
                int[] values = new int[size * size];
                for (int i = 0; i < values.Length; i++)
                    values[i] = i; // 0 = prázdné pole

                Shuffle(values);

                // neřešitelné rozmístění opravíme prohozením dvou kamenů (změní paritu inverzí)
                if (!SolvabilityChecker.IsSolvable(values, size))
                    SwapTwoTiles(values);

                board = Board.FromValues(values, size);
            }
            while (board.IsSolved);

            return board;
        }

        // Fisher–Yates míchání
        private void Shuffle(int[] values)
        {
            for (int i = values.Length - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                (values[i], values[j]) = (values[j], values[i]);
            }
        }

        // prohodí první dva neprázdné kameny
        private static void SwapTwoTiles(int[] values)
        {
            int first = -1;

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == Tile.EmptyValue)
                    continue;

                if (first < 0)
                {
                    first = i;
                }
                else
                {
                    (values[first], values[i]) = (values[i], values[first]);
                    return;
                }
            }
        }
    }
}
