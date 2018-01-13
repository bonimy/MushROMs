# LC_RLE2 Format

This is a run-length encoding format. The compressed stream consists of 2 equally-sized tables. Each table consists of chunks, each starting with a 1 byte header.

```
bits
76543210
CLLLLLLL

C:       Command bit
LLLLLLL: Length
```

Unlike most other compression formats, there is no special header value that marks the end of the data. You need to know the size of the uncompressed file in advance to use the format.

## List of commands

* %0 - "Direct Copy" - Followed by (L+1) bytes of data
* %1 - "RLE" - Followed by one byte to be repeated (L+1) times

The length has 7 bits, so the maximum value you can represent is 127 (which outputs 128 bytes). If you want more than that, you will have to use multiple commands with the same data. For example, to write 256 null bytes, you would do `$FF $00 $FF $00`.

## Two table shenanigans

The decompressed data of the first table gets written to every other byte in the buffer. The second table gets written to the buffer with an offset of 1 byte (so it fills out the gaps left by the first pass). This makes the algorithm very effective for cases where every 2nd byte is repeating often.