# Super Mario All-Stars: Super Mario Bros. 1 Level Data
This is a comprehensive guide for understanding the SMAS SMB1 level data.
## Introduction and Terminology
Most people are familiar with how Super Mario Bros. 1 (SMB1) works. There are eight worlds, and each world has four levels. When we imagine a full level in SMB1, we imagine this:

![W1-1 Example](images/screen1.png)

Some of us might also imagine the bonus area being part of W1-1:

![W1-1 Bonus Room](images/screen2.png)

These are part of the same level, but are considered different "areas".

More interestingly, we have W1-3 and W5-3.

![W1-3 Example](images/screen3.png)
![W5-3 Example](images/screen4.png)

These, conversely, are different levels but the same "areas".

We will define **area** as how it was described above Areas can also be called **maps** or **rooms**. For our purpose, we will stick to _area_. Areas are distinct from **levels** in that levels can have multiple areas (the main area, a bonus area, or a miscellaneous area). A **world** is defined as every level until a Bowser is encountered. Internally, the game increments you to the next world after beating a Bowser (more precisely after stepping on his bridge-destroying axe).

- **Note**: For simplicity, when we refer to the main area of the level, we will simply refer to the level. So instead of saying "the main area of W1-1", we'll simply say "W1-1".

Every area is defined by a single byte, used as a pointer index for getting the area data (blocks, pipes, bricks, etc.) and sprite data (enemies, commands, etc.). For the images shown above, the area numbers are 0x25 for W1-1, 0xC2 for its bonus room, and 0x26 for both W1-3 and W5-3.

There are two ways the game engine determines which area number to use next:

**Current world and level number**: By starting the game or beating the level, the internal world and level number are appropriately set and these determine which area number to use.

**Exit the level**: By entering a pipe or climbing a vine, you exit the level. Using a sprite command (discussed later), the game knows which area to load if you exit the level.

We will first discuss how we get the area number. Then we will discuss how get the area data from the area number. Finally, we will document the area data format; we annotate how it is interpreted as a full level.

## Area number determined by current world and level
There are several constants, addresses, and byte tables that determine the area number when given the world and level numbers.

**`$04:C00B`**: Full routine for getting area number from world and level numbers.

**`$7E:0750`**: Stores the current area number. The range goes from `0x00-0x7F`. If it exceeds this, the highest bit is ignored. The highest bit is only set if it has to be set by a sprite command (more on this later).

**`$7E:075C`**: "Hard mode" flag. This flag changes sprite properties starting at W5-3 (e.g. bullet bills in W5-3 but not W1-3, fire bars in W6-4 but not W1-4, shorter moving platforms, etc.)

**`$7E:075F`**: Stores the current world number as a zero-based value (W1 is `00`, W2 is `01`, etc.

**`$7E:0760`**: Stores the current level number as a zero-based value (W1-1 is `00`, W8-2 is `01`, etc.

**`$7E:005C`**: Stores the area "type". This is a 2-bit value determined by bits 5 and 6 of the area number.

Bits | Value | Area type
---- | ----- | ---
00 | 0 | Underwater
01 | 1 | Normal ground
10 | 2 | Underground
11 | 3 | Castle
  
**`$04:C026`**: Max world number, `08`. When the player exceeds this, the game state is set back to W1-1.

**`$04:C11C`**: An index table that determines the start index for the "area number table per level". This table is indexed by world number (called at `$04:C034`).

World | Offset | Levels per world
---- | ---- | ----
1 | `00` | 5
2 | `05` | 5
3 | `0A` | 4
4 | `0E` | 5
5 | `13` | 4
6 | `17` | 4
7 | `1B` | 5
8 | `20` | 4 (Until Bowser is reached)

- **Note**: Some worlds show 5 levels per world. This is caused by **prelevels**. This is the autowalk preview area that occurs at the start of W1-2, W2-2, W4-2, and W7-2 (area `0x29` for all of them).
      
  ![W1-2 autowalk](images/screen5.png)
      
  This, while being shown as W1-2 along with the underground area that follows, is actually a separate level. So when the player enters the pipe, the game internally increments the level number (but doesn't display it on the screen). So technically, the underground area is level 3, the green trees area is level 4, and the castle is level 5. But the game still displays the level number we're all used to seeing. The reasoning behind this being that if you die in, say, the undergorund area of W1-2, you respawn in the underground area, and not the autowalk pipe entrance every time.
    
**`$04:C124`**: Table of area numbers (called at `$04:C03C`). The world start index determined by table `$04:C11C` is added to the current level number, and this indexes the table to get the current area number.
  
World | Table Offset | Area Number per level
---- | ---- | ----
1 | `00` | `25 29 C0 26 60`
2 | `05` | `28 29 01 27 62`
3 | `0A` | `24 35 20 63`
4 | `0E` | `22 29 41 2C 61`
5 | `13` | `2A 31 26 62`
6 | `17` | `2E 23 2D 60`
7 | `1B` | `33 29 01 27 64`
8 | `20` | `30 32 21 65`

For example, W4-3 would have world offset `0x0E`. We go four values down the list because we add an extra level number for the autowalk sequence of W4-2. Thus, the area number of W4-3 is `0x2C`.
  
- **Note**: There are interesting cases that can occur when the player does or doesn't complete a world. For example, if we put an Axe in W1-1 and the player used that to beat the level, the game would start the player at world 2, so our next level loaded would be W2-1, not W1-2.

  Even more interestingly, if we put a flag pole in W1-4 and the player used that to beat the level, the level number would increment to level 5, but the world number would still be W1. So the next level we'd go to is W1-5. To determine the area number for this, we'd move 5 bytes off of the area number table starting at index `00` for world 1. This would be area `0x28`. So we'd still be in W2-1, but the game will show us W1-5.
  
  Further, if the player made it to W1-8 (W2-4 normally) and beat the level by the axe, we wouldn't be taken to W3-1. The game will increment the world number to W2 and would reset us to level 1. Hence, we'd be taken back to area `0x28` under the standard W2-1. We would have to do this whole world again to get back on track. Acmlm's Strange Mario Bros. uses this oddity a lot.
    
## Area data from area number
Now that we know how to get the area number, we need to get the area data. The **area object data** (or **object data**) is a string of bytes that determines how to place objects in the level. We loosely define **objects** as things such as question blocks, bricks, spring boards, pipes, etc. This definition is weak because it doesn't comprehensively cover everything. There are oddities like page skips, scenery changes, pipe pointers, loop commands, and warp zone specifies, which can either be classified as object data or sprite data. A comprehensive dissection of area data vs sprite data will be given later in this document. But on this same token, we will define **area sprite data** (or **sprite data**) as the string of bytes determining how to place sprites in the level, and we equally give a loose definition of **sprites** as things such Goombas, Koopas, Lakitus, etc. We define **area data** itself as the object data, sprite data, and other area-specific information we will discuss in this section.

**`$04:C041`**: The complete routine for getting the area data.

**`$7E:00BA`**: Stores the NES-style palette and music data of the area.

Value | Description
--- | ---
0 | Light blue background with underwater music
1 | Light blue background with above ground music
2 | Black background with underground music
3 | Black background with castle music

- **Note**: This variable will always have the same value as `$7E:005C`, the area type.

**`$7E:074F`**: Area _index_. This is determined by the lower five bits of `$7E:0750`, the area _number_. The difference can be confusing at first, but will be analyzed shortly.

**`$04:C148`**: Table of relative indices to area sprite data pointer tables (called at `$04:C05A`). This table is indexed by area type. Much like how we used the world number to determine the start offset for getting the area number per level, we're using the area type to get the pointer to the sprite data per area index (an example will be provided afterward to try to clear any confusion).

Index | Area Type | Table Value
--- | --- | ---
0 | Underwater | `1F`
1 | Above ground | `06`
2 | Underground | `1C`
3 | Castle | `00`

**`$7E:00FD`**: A three byte pointer to the current area's sprite data.

**`$04:C14C`**: Table of low bytes of area sprite data pointers (called at `$04:C062`).

Area Type | Table Offset | Table Values
--- | --- | ---
Castle | `00` | `D8 FF 18 47 72 87`
Above Ground | `06` | `C1 E6 03 11 38 69 87 A4 B9 E3 E4 08 11 36 59 62 63 9D C8 F6 12 1B`
Underground | `1C` | `40 6D 9B`
Underwater | `1F` | `C8 D9 03`

**`$04:C16E`**: Table of high bytes of area sprite data pointers (called at `$04:C06C`).

Area Type | Table Offset | Table Values
--- | --- | ---
Castle | `00` | `C1 C1 C2 C2 C2 C2`
Above Ground | `06` | `C2 C2 C3 C3 C3 C3 C3 C3 C3 C3 C3 C4 C4 C4 C4 C4 C4 C4 C4 C4 C5 C5`
Underground | `1C` | `C5 C5 C5`
Underwater | `1F` | `C5 C5 C6`

**`$04:C06C`**: Hardcodes the bank byte of the area sprite data pointer to `0x04`.

As an example, we will get the sprite data pointer for the first area of W1-1. It's area number is `0x25`. Its area type is _Above ground_ and its area index is `0x05`. The table offset for _above ground_ is `0x06`, so we move 6 bytes down the low and high byte pointers (2nd row of tables). Then from the second row, we move 5 bytes down the list. So the low byte of the sprite pointer is `0x38` and the high byte is `0xC3`. The bank byte is always `0x04`, so the sprite data pointer for area 0x25 is `$04:C369`.

**`$04:C190`**: Table of relative indices to area object data pointer tables (called at `$04:C072`). This table is like table `$04:C148`.

Area Type | Table Offset | Table Values
--- | --- | ---
0 | Underwater | `00`
1 | Above ground | `03`
2 | Underground | `19`
3 | Castle | `1C`

**`$7E:00FA`**: A three byte pointer to the current area's object data.

**`$04:C194`**: Table of low bytes of area object data pointers (called at `$04:C092`).

Area Type | Table Offset | Table Values
--- | --- | ---
Underwater | `00` | `08 71 0D`
Above Ground | `03` | `0B 74 C3 1B B0 2F 9A F1 7A E7 F1 35 4A BB 28 A3 D5 6D EB 6B CA F5`
Underground | `19` | `2D D2 76`
Castle | `1C` | `17 D2 FA D8 D4 01`

**`$04:C1B6`**: Table of high bytes of area object data pointers (called at `$04:C097`).

Area Type | Table Offset | Table Values
--- | --- | ---
Underwater | `00` | `D6 D6 D7`
Above Ground | `03` | `CC CC CC CD CD CE CE CE CF CF CF D0 D0 D0 D1 D1 D1 D2 D2 D3 D3 D3`
Underground | `19` | `D4 D4 D5`
Castle | `1C` | `C6 C6 C7 C8 C9 CB`

**`$04:C09C`**: Hardcodes the bank byte of the area object data pointer to `0x04`.

So the area object data pointer for area `0x25` is `$04:CE2F`. Another point of interest is that an area index can exceed its bounds. For example, if we had area number `0x08` (not an area in the game), it would have area type _underwater_ and area index `0x08`. So we would go 8 bytes down the pointer tables starting at the underwater offset. For the area object data, this pointer would `$04:CE2F`. This is the same as area `0x25` (main area of W1-1). So what we would get is an underwater version of this area.

The sprites data however, would be undefined, as it exceeds the table size when starting at the underwater offset.

**`$7E:00DB`**: Stores the area's layer 2 background. This value is determined by table `$04:C190` and the area index. More precisely, the value that the aforementioned table returns when given the area type, is added to the area index, and the result is stored as the layer 2 background.

Value | Area number | Layer 2 background
--- | --- | ---
`00` | `00` | Underwater (short)
`01` | `01` | Underwater (full)
`02` | `02` | Underwater (castle)
`03` | `20` | Night sky (w/o mountains)
`04` | `21` | Outside castle (W8-3)
`05` | `22` | Mountains and trees
`06` | `23` | Night sky (w/ mountains)
`07` | `24` | Night sky (w/ mountains and snow)
`08` | `25` | Mountains
`09` | `26` | Waterfall
`0A` | `27` | Goomba statues/pillars
`0B` | `28` | Narrow green hills (W2-1)
`0C` | `29` | One big mountain (autowalk intro level)
`0D` | `2A` | Narrow hills w/ snow (W5-1)
`0E` | `2B` | Mario/Luigi bonus room
`0F` | `2C` | Mushrooms (W4-3)
`10` | `2D` | Night sky (w/o mountains)
`11` | `2E` | 
`12` | `2F` | 
`13` | `30` | 
`14` | `31` | 
`15` | `32` | 
`16` | `23` | 
`17` | `24` | 
`18` | `25` | 
`19` | `40` | 
`1A` | `41` | 
`1B` | `42` | 
`1C` | `60` | 
`1D` | `61` | 
`1E` | `62` | 
`1F` | `63` | 
`20` | `64` | 
`21` | `65` | 
