# presouvacka_Kosmak

Hra Přesouvačka na desce 4×4. Cílem je seřadit kameny 1–15 a nechat prázdné pole vpravo dole.

Datový model
Tile – nemodifikovatelný objekt s hodnotou Value. Hodnoty 1-15 jsou kameny, 0 (Tile.EmptyValue) je prázdné pole.
Position – record struct (Row, Col), číslováno od 0 z levého horního rohu.
Board – drží matici Tile[Size, Size] (výchozí Size = 4) a pozici prázdného pole EmptyPosition, aby se nemusela pokaždé hledat.
