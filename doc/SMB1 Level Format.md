# Super Mario All-Stars: Super Mario Bros. 1 Level Data
Here we document the format and important numbers for the SMAS SMB1 level data. All addresses are given SNES mode.
## Map number by world and level
- `$04:C026`: Max world number `08`. When the player beats Bowser, the world number is incremented. The world number is set to zero (W1) when it is equal to or greater than the max world number.
- `$04:C11C`: Level offsets per world (called at `$04:C034`).
  - `00 05 0A 0E 13 17 1B 20`: These values are indexed by world. They represent how many bytes to offset the map pointer array to load the current map by world. For example, world 1 offsets zero bytes and world 2 offsets five bytes: four bytes for each standard level, and one more byte for the pipe autowalk entrace "level" at the start of W1-2. Entering those pipes increases the base level number, and thus takes you to the real W1-2 (the underground level). Hence why some worlds have five bytes offsets instead of four. World 3's offset is 10 bytes instead of 5 because it accounts for the offset of 5 levels in World 1 and the offset of 5 levels in world 2. So this number is always increasing per world.
- `$04:C124`: Map number to load per level (called at `$04:C03C`). The table below organizes them per world according to the index offsets at `$04:C11C`.
  
  World | Offset | Map Number
  ---- | ---- | ----
  1 | `00` | `25 29 C0 26 60`
  2 | `05` | `28 29 01 27 62`
  3 | `0A` | `24 35 20 63`
  4 | `0E` | `22 29 41 2C 61`
  5 | `13` | `2A 31 26 62`
  6 | `17` | `2E 23 2D 60`
  7 | `1B` | `33 29 01 27 64`
  8 | `20` | `30 32 21 65`

  So for example, for W4-3 would have world offset `0E`. We go four values down the list because we add an extra map number for the autowalk sequence of W4-2. Thus, the map number of W4-3 is `2C`.
## Map number basics
The map number byte is more than just an index value. Some bits serve different purposes.

`0 t t i i i i i`
Bit | Description
--- | ---
0 | The highest bit is insignificant. This is because pipe pointer sprite codes use the first byte to specify the map number. The first byte of sprite codes also use the most significant bit as a page flag. So map numbers ignore the MSB since it would otherwise give different values if it had a set page flag.
`t` | The `t` bits specify the map _type_ of the map (see [below](#map-types).
`i` | The `i` bits specify the map base index value.).

### Map types
The `t` bits of the map number specify four possible map types. SMB1 interprets the values as follows:

Bits | Value | Description
---- | ----- | ---
00 | 0 | Underwater level
01 | 1 | Normal ground level
10 | 2 | Underground level
11 | 3 | Castle level

## Level address by map number
We now know how to get the map number by level, and how it is formatted. To find the pointer to the level data, we need to find the bank byte, high byte, and low byte.
- `$04:C148`: An offset index  table by map type (called at `$04:C05A`). The values in this table specify the zero index to add to the base index value described [earlier](#map-number-basics).
  
  Map type | Name | Index
  --- | --- | ---
  0 | Underwater | `1F`
  1 | Normal | `06`
  2 | Underground | `1C`
  3 | Castle | `00`
  
- The bank byte is hardcoded as `04` by the program bank byte register.
- The low byte is indexed by map number at `$04:C194` (called at `$04:C092`).

  Map Base | Number of levels | Values
  ---- | ---- | ----
  `00-1F` | 3 | `08 71 0D`
