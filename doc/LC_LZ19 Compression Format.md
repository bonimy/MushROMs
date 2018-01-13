# LC_LZ19 Format

This format is an enhancement of LZ2.

The compressed stream consists of chunks, each starting with a 1 byte header.

```
bits
76543210
CCCLLLLL

CCC:   Command bits  
LLLLL: Length
```

The header byte `$FF` marks the end of the data stream.

## List of commands

* `%000` - "Direct Copy" - followed by (L+1) bytes of data
* `%001` - "Byte Fill" - followed by 1 byte to be repeated (L+1) times
* `%010` - "Word Fill" - Followed by two bytes. Output first byte, then second, then first, then second, etc. until (L+1) bytes has been outputted
* `%011` - "Increasing Fill" - Followed by one byte to be repeated (L+1) times, but the byte is increased by 1 after each write
* `%100` - "Repeat" - Followed by two bytes (big endian) containing address (in the output buffer) to copy (L+1) bytes from
* `%101` - "Bit-Reverse Repeat" - Same concept as above, except all the copied bytes are reversed in bit order. (`$AF` = `%10101111` -> `$F5` = `%11110101`)
* `%110` - "Backwards Repeat" - Same concept as "Repeat", except the bytes are copied in reversed order. The address points to the first byte to be copied, and it decrements as each byte is copied.
* `%111` - "Long length" - This command has got a two-byte header:
```
111CCCLL LLLLLLLL
CCC:        Real command
LLLLLLLLLL: Length
```
Normally you have 5 bits for the length, so the maximum value you can represent is 31 (which outputs 32 bytes). With the long length, you get 5 more bits for the length, so the maximum value you can represent becomes 1023, outputting 1024 bytes at a time.