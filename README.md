# presouvacka_Kosmak

Hra Přesouvačka na desce 4×4. Cílem je seřadit kameny 1–15 a nechat prázdné pole vpravo dole.

Datový model
Tile – nemodifikovatelný objekt s hodnotou Value. Hodnoty 1-15 jsou kameny, 0 (Tile.EmptyValue) je prázdné pole.
Position – record struct (Row, Col), číslováno od 0 z levého horního rohu.
Board – drží matici Tile[Size, Size] (výchozí Size = 4) a pozici prázdného pole EmptyPosition, aby se nemusela pokaždé hledat.

Pravidla tahu
CanMoveTile(position) – tah je platný, když pozice leží na desce, kámen není prázdné pole a jeho Manhattanská vzdálenost od prázdného pole je přesně 1 (sousedí vodorovně nebo svisle).
TryMoveTile(position) – platný tah prohodí kámen s prázdným polem a vrátí true. Neplatný tah nic nezmění a vrátí false.

Algoritmus ověření řešitelnosti
Řešitelnost se určuje podle počtu inverzí.
Inverze je dvojice kamenů, kde větší číslo stojí v řádkovém zápisu desky před menším. Prázdné pole se nepočítá.
Deska je řešitelná, když je sudá: součet (počet inverzí + řádek prázdného pole) je lichý.

Generátor zamíchané desky
BoardShuffler.CreateShuffledBoard():
Vytvoří pole hodnot 0-15 a zamíchá ho algoritmem Fisher–Yates.
Pokud SolvabilityChecker.IsSolvable vrátí false, prohodí první dva neprázdné kameny. Prohození dvou kamenů změní paritu inverzí, řádek prázdného pole zůstane stejný, takže se deska stane řešitelnou.
Pokud by vyšla už složená deska, celý postup opakuje.
