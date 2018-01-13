# Helper class library
The helper library contains classes, structures, and enums that have been known to be helpful outside the scope of the specific MushROMs project. For this reason, they all exist in this very generic project.

### SR
A static String Resource class that provides methods for getting culture-specific strings for common scenarios. These scenarios are usually exception strings.

### StringModifierComparer
Represents a string comparison operation on strings that are first modified by a call to the abstract method `StringModifier` before being compated.

### ExtensionComparer
Represents a string comparison operation that performs a comparison of the extensions of paths. So it would compate `"file.txt"` and `@"C:\path\to\..\document.TXT" as equal because they have the same extensions. Note that the comparisons are case and culture invariant. This property can be overridden though.

### PathComparer
Like extension comparer, but compares the full paths of two strings and considers them equal if they point to the same location. The base string comparison is also case and culture invariant but can be overridden.

### AssertDictionary
A dictionary class that runs an assert method on keys before using them. This class overrides the `Add`, `ContainsKey`, `Remove`, and `TryGetValue` methods so that they each first call the abstract `AssertKey` method, which tests that the key is well-formed by some to-be-defined standard before being used.

### ExtensionDictionary
Inherits from `AssertDictionary`. The `Assert` methods demands the string be any value that `Path.GetExtension` can get an extension from. This class uses the `ExtensionComparer` equality comparer.

### PathDictionary
Inherits from `AssertDictionary`. The `Assert` methods demands the string be any value that `Path.GetFullPath` can get an extension from. This class uses the `PathComparer` equality comparer.

### Int24 and UInt24
These are three-byte data structures that act exactly like integers but only to the specified range. Their utility is just for working with primitive data arrays that are formatted as 3-byte values.

### Pointer
This is a [generic](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/) class that lets you access any element of a given array and define zero as any offset. It's meant to act as a weak C-style memory pointer, but in a C# context.

### IIndexer and IReadOnlyIndexer
A generic interface that implements the indexer method for a class.

### [U]Int[16/24]ByteIndexer
These classes let you read and write 16 and 24 bit values from and to byte arrays using an indexer method.

### Poisition2D and Range2D
Recreations of the .Net Framework Point and Size structures, but with extra functionality.

### SuffixTree and SubstringPointer
The SuffixTree class implements a [generalized suffix tree](https://en.wikipedia.org/wiki/Generalized_suffix_tree) in linear time using [Ukkonen's algorithm](https://en.wikipedia.org/wiki/Ukkonen%27s_algorithm) for byte data. It's currently only designed to find the longest matching substring before a specified index for compression purposes, but it fully implements the suffix tree, so other uses can be added to it if necessary.

### MathHelper
A sttaic class that adds some extra math-related functions that have been helpful.

### ConfigFile
Implements a config file for applying customizable settings to applications. This is a port of the snes9x config file class.

### ColorF and BlendMode
The ColorF struct provides a 4-channel ARGB color with many common color-editing methods like blending and converting to different color spaces. Each channel is represent by a floating point number ranging from 0 to 1.

### PixelFormat
Provides structures for certain pixel formats. Each structure is the size, in bytes, of the format it implements, so it can be directly cast to an unsafe pointer of that data.
