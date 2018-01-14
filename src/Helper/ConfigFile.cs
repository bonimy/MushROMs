// <copyright file="ConfigFile.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Helper
{
    public class ConfigFile
    {
        private const string Uncategorized = "Uncategorized";

        private const char CommentChar = '#';
        private const char AltCommentChar = ';';

        private static StringComparer _defaultComparer;

        private static StringComparer DefaultComparer
        {
            get
            {
                if (_defaultComparer == null)
                {
                    _defaultComparer = StringComparer.OrdinalIgnoreCase;
                }

                return _defaultComparer;
            }

            set
            {
                _defaultComparer = value;
            }
        }

        private Dictionary<string, Entry> Entries
        {
            get;
            set;
        }

        private SectionSizeManager SectionSizes
        {
            get;
            set;
        }

        public bool AutoAdd
        {
            get;
            set;
        }

        public bool NiceAlignment
        {
            get;
            set;
        }

        public bool ShowComments
        {
            get;
            set;
        }

        public bool AlphaSort
        {
            get;
            set;
        }

        public bool TimeSort
        {
            get;
            set;
        }

        public string CurrentPath
        {
            get;
            private set;
        }

        public ConfigFile()
        {
            Entries = new Dictionary<string, Entry>(DefaultComparer);
            SectionSizes = new SectionSizeManager();

            NiceAlignment = true;
            ShowComments = true;
            AlphaSort = true;
        }

        public void Clear()
        {
            Entries.Clear();
        }

        public void Load(string path)
        {
            Console.WriteLine("Reading config file: '{0}'", path);

            string[] lines;
            try
            {
                lines = File.ReadAllLines(path);
            }
            catch (IOException ex)
            {
                Console.Error.WriteLine(
                    "Could not load config file: '{0}'{1}\t{2}",
                    path,
                    Environment.NewLine,
                    ex.Message);
                return;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(
                    "An unexpected error occurred while loading the config file: '{0}'{1}\t{2}",
                    path,
                    Environment.NewLine,
                    ex.Message);
                return;
            }

            var name = Path.GetFileName(path);
            CurrentPath = path;

            var section = String.Empty;
            var line1 = 0;
            var line2 = 0;

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                line1 = line2++;

                // Ignore empty lines
                if (String.IsNullOrEmpty(line))
                {
                    continue;
                }

                // Ignore comment lines (note this ignores the ## character key, which only works for assigned values)
                if (line[0] == CommentChar || line[0] == AltCommentChar)
                {
                    continue;
                }

                // Determine if this is a new section line
                if (line[0] == '[')
                {
                    if (line[line.Length - 1] != ']')
                    {
                        Console.Error.WriteLine("{0}:[{1}]: Ignoring invalid section header.", name, line);
                        continue;
                    }

                    section = line.Substring(1, line.Length - 2);
                    continue;
                }

                // Join lines containing a joined line separator.
                while (line[line.Length - 1] == '\\')
                {
                    line = line.Remove(line.Length - 1);
                    line2++;
                    if (++i == lines.Length)
                    {
                        Console.Error.WriteLine("Unexpected EOF reading config file '{0}'", name);
                        return;
                    }

                    line += lines[i].Trim();
                }

                var index = line.IndexOf('=');
                if (index < 0)
                {
                    Console.Error.WriteLine("{0}:[{1}]: Ignoring invalid entry", name, line);
                    continue;
                }

                var key = line.Remove(index).Trim();
                var val = line.Substring(index + 1);
                var comment = TrimCommented(ref val);
                if (val.Length >= 2 && val[0] == '"' && val[val.Length - 1] == '"')
                {
                    val = val.Substring(1, val.Length - 2);
                }

                var entry = new Entry(line1, section, key, val, comment);
                if (Entries.Remove(entry.FullKey))
                {
                    SectionSizes[section]--;
                }

                Entries[entry.FullKey] = entry;
                SectionSizes[section]++;
            }

            Console.WriteLine("Config file successfully loaded.");
        }

        public void Save()
        {
            Save(CurrentPath);
        }

        public void Save(string path)
        {
            Console.WriteLine("Saving config file: '{0}'", path);

            var sb = new StringBuilder();
            var keys = new List<string>(Entries.Keys);
            keys.Sort(SectionThenKeyComparer);

            sb.AppendLine("# Auto-generated config file");
            sb.Append("# ");
            sb.AppendLine(DateTime.Now.ToString());

            var section = String.Empty;

            int maxKeyLen = 0, maxValLen = 0, maxLeftDiv = 0, maxRightDiv = 1, size = 0;

            for (var i = 0; i < keys.Count; i++)
            {
                var entry = Entries[keys[i]];
                if (entry.Section != section)
                {
                    section = entry.Section;
                    sb.AppendLine(String.Empty);
                    sb.AppendLine("[" + section + "]");

                    if (NiceAlignment)
                    {
                        maxKeyLen = 0;
                        maxValLen = 0;
                        maxLeftDiv = 0;
                        maxRightDiv = -1;

                        for (var j = i; j < keys.Count; j++)
                        {
                            entry = Entries[keys[j]];
                            if (entry.Section != section)
                            {
                                break;
                            }

                            size = entry.Key.LastIndexOf(':');
                            maxRightDiv = Math.Max(maxRightDiv, size);

                            size = entry.Key.Length - size;
                            maxLeftDiv = Math.Max(maxLeftDiv, size);

                            maxKeyLen = Math.Max(maxKeyLen, entry.Key.Length);

                            if (ShowComments)
                            {
                                var value = entry.Value.Trim();
                                size = value.Length;
                                for (var k = size; --k >= 0;)
                                {
                                    if (value[k] == '#')
                                    {
                                        size++;
                                    }
                                }

                                if (value != entry.Value)
                                {
                                    size += 2;
                                }

                                maxValLen = Math.Max(maxValLen, size);
                            }
                        }

                        maxKeyLen += 3;
                        maxValLen = Math.Min(maxValLen, 48);
                    }

                    do
                    {
                        entry = Entries[keys[i]];
                        if (entry.Section != section)
                        {
                            break;
                        }

                        var value = entry.Value.Trim();
                        if (value != entry.Value)
                        {
                            value = "\"" + value + "\"";
                        }

                        value = value.Replace("#", "##");

                        if (NiceAlignment)
                        {
                            size = entry.Key.LastIndexOf(':');
                            var sub = 0;
                            if (size < maxRightDiv)
                            {
                                for (var j = size; j < maxRightDiv; j++)
                                {
                                    sb.Append(' ');
                                }

                                sub = maxRightDiv - size;
                                size = maxRightDiv;
                            }

                            size += maxLeftDiv - entry.Key.Length;
                            for (var j = entry.Key.Length + size + 3; j < maxKeyLen; j++)
                            {
                                sb.Append(' ');
                            }

                            sb.Append(entry.Key);
                            for (var j = 0; j < size - sub; j++)
                            {
                                sb.Append(' ');
                            }

                            sb.Append(" = ");
                            sb.Append(value);
                        }
                        else
                        {
                            sb.Append(entry.Key);
                            sb.Append(" = ");
                            sb.Append(entry.Value);
                        }

                        if (ShowComments && !String.IsNullOrEmpty(entry.Comment))
                        {
                            if (NiceAlignment)
                            {
                                for (var j = value.Length; j < maxValLen; j++)
                                {
                                    sb.Append(' ');
                                }
                            }

                            sb.Append("  # ");
                            sb.Append(entry.Comment);
                        }

                        sb.AppendLine();
                    }
                    while (++i < keys.Count);
                }
            }

            try
            {
                File.WriteAllText(CurrentPath, sb.ToString());
            }
            catch (IOException ex)
            {
                Console.Error.WriteLine("Could not save config file: '{0}'{1}\t{2}", path, Environment.NewLine, ex.Message);
                return;
            }

            CurrentPath = path;

            Console.WriteLine("Config file saved successfully.");
        }

        private static bool IsCommentChar(char x)
        {
            return x == CommentChar || x == AltCommentChar;
        }

        public string GetString(string key, string fallback)
        {
            if (!Entries.ContainsKey(key))
            {
                return fallback;
            }

            return Entries[key].Value;
        }

        public void SetString(string key, string value)
        {
            SetString(key, value, String.Empty);
        }

        public void SetString(string key, string value, string comment)
        {
            Entries[key] = new Entry(key, value, comment);
        }

        public int GetInt(string key, int fallback)
        {
            if (!Entries.ContainsKey(key))
            {
                return fallback;
            }

            if (!Int32.TryParse(Entries[key].Value, out var result))
            {
                return fallback;
            }

            return result;
        }

        public void SetInt(string key, int value)
        {
            SetInt(key, value, String.Empty);
        }

        public void SetInt(string key, int value, string comment)
        {
            Entries[key] = new Entry(key, value.ToString(), comment);
        }

        public bool GetBool(string key, bool fallback)
        {
            if (!Entries.ContainsKey(key))
            {
                return fallback;
            }

            if (!Boolean.TryParse(Entries[key].Value, out var result))
            {
                return fallback;
            }

            return result;
        }

        public void SetBool(string key, bool value)
        {
            SetBool(key, value, String.Empty);
        }

        public void SetBool(string key, bool value, string comment)
        {
            Entries[key] = new Entry(key, value.ToString(), comment);
        }

        public object GetEnum(string key, Enum fallback, Type type)
        {
            if (!Entries.ContainsKey(key))
            {
                return fallback;
            }

            if (!Enum.IsDefined(type, Entries[key].Value))
            {
                return fallback;
            }

            return Enum.Parse(type, Entries[key].Value);
        }

        public T GetEnum<T>(string key, T fallback)
        {
            if (!Entries.ContainsKey(key))
            {
                return fallback;
            }

            if (!Enum.IsDefined(typeof(T), Entries[key].Value))
            {
                return fallback;
            }

            return (T)Enum.Parse(typeof(T), Entries[key].Value);
        }

        public void SetEnum(string key, Enum value)
        {
            SetEnum(key, value, String.Empty);
        }

        public void SetEnum(string key, Enum value, string comment)
        {
            Entries[key] = new Entry(key, value.ToString(), comment);
        }

        private int SectionThenKeyComparer(string x, string y)
        {
            return SectionThenKeyComparer(Entries[x], Entries[y]);
        }

        private int SectionThenKeyComparer(Entry x, Entry y)
        {
            if (x.Section != y.Section)
            {
                var svx = SectionSizes.GetSectionSize(x.Section);
                var svy = SectionSizes.GetSectionSize(y.Section);
                if (svx != svy)
                {
                    return svx - svy;
                }

                return DefaultComparer.Compare(x.Section, y.Section);
            }

            return DefaultComparer.Compare(x.Key, y.Key);
        }

        private int KeyComparer(string x, string y)
        {
            return KeyComparer(Entries[x], Entries[y]);
        }

        private int KeyComparer(Entry x, Entry y)
        {
            if (x.Section != y.Section)
            {
                return DefaultComparer.Compare(x.Section, y.Section);
            }

            return DefaultComparer.Compare(x.Key, y.Key);
        }

        private int LineComparer(string x, string y)
        {
            return LineComparer(Entries[x], Entries[y]);
        }

        private int LineComparer(Entry x, Entry y)
        {
            if (x.Line == y.Line)
            {
                return DefaultComparer.Compare(x.Key, y.Key);
            }

            if (y.Line < 0)
            {
                return +1;
            }

            if (x.Line < 0)
            {
                return -1;
            }

            return x.Line - y.Line;
        }

        private static string TrimCommented(ref string value)
        {
            var comment = String.Empty;
            value = value.Trim();

            for (var start = 0; true;)
            {
                var i = value.IndexOf('#', start);
                if (i != -1)
                {
                    if (value.Length > i + 1 && value[i + 1] == '#')
                    {
                        value = value.Remove(i, 1);
                        start = i + 1;
                        continue;
                    }
                    else
                    {
                        comment = value.Substring(i + 1).Trim();
                        value = value.Remove(i).Trim();
                    }
                }

                break;
            }

            return comment;
        }

        private class Entry : IComparer<Entry>, IComparable<Entry>, IComparable
        {
            public int Line
            {
                get;
                private set;
            }

            public string Section
            {
                get;
                private set;
            }

            public string Key
            {
                get;
                private set;
            }

            public string FullKey
            {
                get
                {
                    return SR.GetString("{0}::{1}", Section, Key);
                }
            }

            public string Value
            {
                get;
                private set;
            }

            public string Comment
            {
                get;
                private set;
            }

            public bool Used
            {
                get;
                private set;
            }

            public Entry(int line, string section, string key, string value, string comment)
            {
                Line = line;
                Section = String.IsNullOrEmpty(section) ? String.Empty : section.Trim();
                Key = key.Trim();
                Value = String.IsNullOrEmpty(value) ? String.Empty : value;
                Comment = String.IsNullOrEmpty(value) ? String.Empty : comment;
                Used = false;
            }

            public Entry(string key)
            {
                Key = String.IsNullOrEmpty(key) ? String.Empty : key;
                ParseKey();
            }

            public Entry(string key, string value, string comment)
            {
                Line = -1;
                Key = key;
                Value = value;
                Comment = String.IsNullOrEmpty(comment) ? String.Empty : comment;
                ParseKey();
            }

            private void ParseKey()
            {
                var i = Key.IndexOf("::");
                if (i != -1)
                {
                    Section = Key.Remove(i).Trim();
                    Key = Key.Substring(i + 2).Trim();
                }
                else
                {
                    Section = Uncategorized;
                }
            }

            public int CompareTo(object obj)
            {
                if (obj is Entry)
                {
                    return Compare(this, (Entry)obj);
                }

                return +1;
            }

            public int CompareTo(Entry other)
            {
                return Compare(this, other);
            }

            public int Compare(Entry x, Entry y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }

                if (x == null)
                {
                    return -1;
                }

                if (y == null)
                {
                    return +1;
                }

                return DefaultComparer.Compare(x.FullKey, y.FullKey);
            }
        }

        private class SectionSizeManager
        {
            private Dictionary<string, int> Sections
            {
                get;
                set;
            }

            public int this[string key]
            {
                get
                {
                    if (!Sections.ContainsKey(key))
                    {
                        Sections[key] = 0;
                    }

                    return Sections[key];
                }

                set
                {
                    if (Sections.ContainsKey(key))
                    {
                        Sections[key] = value;
                    }
                }
            }

            public SectionSizeManager()
            {
                Sections = new Dictionary<string, int>(DefaultComparer);
            }

            public int GetSectionSize(string section)
            {
                var enumerator = Sections.GetEnumerator();

                var count = 0;
                var bs = GetBaseSectionName(section);

                foreach (var kvp in Sections)
                {
                    if (DefaultComparer.Compare(bs, GetBaseSectionName(kvp.Key)) == 0)
                    {
                        count += kvp.Value;
                    }
                }

                return count;
            }

            public void Clear()
            {
                Sections.Clear();
            }

            private static string GetBaseSectionName(string section)
            {
                var index = section.LastIndexOf('\\');
                if (index != -1)
                {
                    return section.Remove(index);
                }

                return section;
            }
        }
    }
}
