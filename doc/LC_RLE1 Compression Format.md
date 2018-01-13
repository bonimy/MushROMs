# LC_RLE1 Format

This is a simple run-length encoding format. The compressed stream consists of chunks, each starting with a 1 byte header.

```
bits
76543210
CLLLLLLL

C:       Command bit
LLLLLLL: Length
```

The end of the stream is marked by both the header byte and the byte following it being `$FF`.

## List of commands

* %0 - "Direct Copy" - Followed by (L+1) bytes of data
* %1 - "RLE" - Followed by one byte to be repeated (L+1) times

The length has 7 bits, so the maximum value you can represent is 127 (which outputs 128 bytes). If you want more than that, you will have to use multiple commands with the same data. For example, to write 256 null bytes, you would do `$FF $00 $FF $00`. Note that you can't have 128 `$FF` bytes in a single command, since that would be `$FF $FF`, but that sequence is reserved for EOF. You need to do `$FE $FF $00 $FF` instead.