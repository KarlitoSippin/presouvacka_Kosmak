using System;
using System.Collections.Generic;
using System.Text;

namespace Presouvacka
{
    ///Kontrola řešitelnosti rozmístění kamenů pomocí počtu inverzí.
    public static class SolvabilityChecker
    {
        ///Počet dvojic, kde větší číslo stojí před menším (prázdné pole se nepočítá).
        public static int CountInversions(IReadOnlyList<int> values)
        {
            int inversions = 0;

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] == Tile.EmptyValue)
                    continue;

                for (int j = i + 1; j < values.Count; j++)
                    if (values[j] != Tile.EmptyValue && values[i] > values[j])
                        inversions++;
            }

            return inversions;
        }

        
        /// Lichá šířka: řešitelné, když je počet inverzí sudý.
        /// Sudá šířka (4x4): řešitelné, když je (inverze + řádek prázdného pole zdola, od 1) liché.
        public static bool IsSolvable(IReadOnlyList<int> values, int size)
        {
            if (values.Count != size * size)
                throw new ArgumentException("Počet hodnot neodpovídá velikosti desky.", nameof(values));

            int inversions = CountInversions(values);

            if (size % 2 == 1)
                return inversions % 2 == 0;

            int emptyIndex = -1;
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] == Tile.EmptyValue)
                {
                    emptyIndex = i;
                    break;
                }
            }

            if (emptyIndex < 0)
                throw new ArgumentException("Deska neobsahuje prázdné pole.", nameof(values));

            int emptyRowFromBottom = size - emptyIndex / size;
            return (inversions + emptyRowFromBottom) % 2 == 1;
        }

        public static bool IsSolvable(Board board) => IsSolvable(board.ToValues(), board.Size);
    }
}
