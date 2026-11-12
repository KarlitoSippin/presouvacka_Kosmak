using System;
using System.Collections.Generic;
using System.Text;

namespace Presouvacka
{
    ///Pozice na desce (řádek, sloupec), od 0 z levého horního rohu.
    public readonly record struct Position(int Row, int Col);
}
