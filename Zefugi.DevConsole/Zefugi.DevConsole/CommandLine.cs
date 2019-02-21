using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Zefugi.DevConsole
{
    public class CommandLine
    {
        public static bool CaseSensitive = false;

        private static Dictionary<string, string> _typeCodesDict;
        private static Dictionary<string, string> _typeCodes
        {
            get
            {
                if (_typeCodesDict == null)
                {
                    _typeCodesDict = new Dictionary<string, string>();
                    _typeCodesDict.Add("bool", "Boolean");
                    _typeCodesDict.Add("b", "Byte");
                    _typeCodesDict.Add("byte", "Byte");
                    _typeCodesDict.Add("c", "Char");
                    _typeCodesDict.Add("char", "Char");
                    _typeCodesDict.Add("short", "Int16");
                    _typeCodesDict.Add("i", "Int32");
                    _typeCodesDict.Add("int", "Int32");
                    _typeCodesDict.Add("long", "Int64");
                    _typeCodesDict.Add("ushort", "UInt16");
                    _typeCodesDict.Add("uint", "UInt32");
                    _typeCodesDict.Add("ulong", "UInt64");
                    _typeCodesDict.Add("f", "Single");
                    _typeCodesDict.Add("float", "Single");
                    _typeCodesDict.Add("d", "Double");
                    _typeCodesDict.Add("double", "Double");
                    _typeCodesDict.Add("s", "String");
                    _typeCodesDict.Add("str", "String");
                    _typeCodesDict.Add("string", "String");
                    _typeCodesDict.Add("chars", "Char[]");
                    _typeCodesDict.Add("bytes", "Byte[]");
                }
                return _typeCodesDict;
            }
        }

        public static bool Parse(string entry, out string command, out string[] arguments)
        {
            return Parse(entry, out command, out arguments, out char[] escList);
        }

        public static bool Parse(string entry, out string command, out string[] arguments, out char[] escapeCharList)
        {
            if (entry == null || entry.Length == 0)
            {
                command = null;
                arguments = null;
                escapeCharList = null;
                return false;
            }

            int loc = entry.IndexOf(' ');
            if (loc == -1)
            {
                command = CaseSensitive ? entry : entry.ToLower();
                arguments = new string[0];
                escapeCharList = null;
                return true;
            }
            else
            {
                List<string> argList = new List<string>();
                List<char> escList = new List<char>();
                command = CaseSensitive ? entry : entry.Substring(0, loc).ToLower();

                char esc = '\0';
                StringBuilder arg = new StringBuilder();
                while (++loc < entry.Length)
                {
                    char c = entry[loc];
                    switch (esc)
                    {
                        case '\0':
                            switch (c)
                            {
                                case '"':
                                    esc = '"';
                                    if (arg.Length != 0)
                                    {
                                        argList.Add(CaseSensitive ? arg.ToString().ToLower() : arg.ToString());
                                        escList.Add(esc);
                                        arg.Clear();
                                    }
                                    break;
                                case ' ':
                                    if (arg.Length != 0)
                                    {
                                        argList.Add(CaseSensitive ? arg.ToString().ToLower() : arg.ToString());
                                        escList.Add(esc);
                                        arg.Clear();
                                    }
                                    break;
                                default:
                                    arg.Append(c);
                                    break;
                            }
                            break;
                        case '"':
                            switch (c)
                            {
                                case '"':
                                    esc = '\0';
                                    argList.Add(arg.ToString());
                                    escList.Add(esc);
                                    arg.Clear();
                                    break;
                                case '\\':
                                    if (++loc < entry.Length)
                                    {
                                        c = entry[loc];
                                        switch (c)
                                        {
                                            case '\\': arg.Append('\\'); break;
                                            case '0': arg.Append('\0'); break;
                                            case 't': arg.Append('\t'); break;
                                            case 'r': arg.Append('\r'); break;
                                            case 'n': arg.Append('\n'); break;
                                        }
                                    }
                                    break;
                                default:
                                    arg.Append(c);
                                    break;
                            }
                            break;
                    }
                }
                if (arg.Length != 0)
                {
                    argList.Add(CaseSensitive && esc == '\0' ? arg.ToString().ToLower() : arg.ToString());
                    escList.Add(esc);
                }
                arguments = argList.ToArray();
                escapeCharList = escList.ToArray();
                return true;
            }
        }

        public static bool ParseWithTypes(string entry, out string command, out object[] arguments)
        {
            if (!Parse(entry, out command, out string[] args, out char[] escList))
            {
                arguments = new object[0];
                return false;
            }
            else
            {
                string value;
                arguments = new object[args.Length];
                for(int i = 0; i < args.Length; i++)
                {
                    string typeName;
                    if (escList[i] == '\"')
                    {
                        typeName = "String";
                        value = args[i];
                    }
                    else
                    {
                        int loc = args[i].IndexOf(':');
                        if (loc > 0 && args[i][loc - 1] != '\\')
                        {
                            typeName = args[i].Substring(0, loc);
                            if (_typeCodes.ContainsKey(typeName))
                                typeName = _typeCodes[typeName];
                            value = args[i].Substring(loc + 1);
                        }
                        else
                        {
                            if (IsInt(args[i]))
                                typeName = "Int32";
                            else if (IsFloat(args[i]))
                                typeName = "Single";
                            else if (IsBoolean(args[i]))
                                typeName = "Boolean";
                            else
                                typeName = "String";
                            value = args[i];
                        }
                    }
                    switch(typeName)
                    {
                        case "String":
                            arguments[i] = args[i];
                            break;
                        case "Char[]":
                            arguments[i] = args[i].ToCharArray();
                            break;
                        case "Byte[]":
                            arguments[i] = Encoding.UTF8.GetBytes(args[i]);
                            break;
                        default:
                            var asm = Assembly.GetAssembly(true.GetType());
                            var type = asm.GetType(typeName);
                            if(type == null)
                                type = asm.GetType("System." + typeName);
                            var method = type.GetMethod("Parse", new Type[] { typeof(String) });
                            arguments[i] = method.Invoke(null, new object[] { value });
                            break;
                    }
                }
                return true;
            }
        }

        private static bool IsInt(string value)
        {
            foreach (char c in value)
                if (!char.IsDigit(c))
                    return false;
            return true;
        }

        private static bool IsFloat(string value)
        {
            bool hasPoint = false;
            char pointChar = 0.1f.ToString()[1];
            foreach (char c in value)
            {
                if (hasPoint && c == pointChar)
                    return false;
                else if (c == pointChar)
                    hasPoint = true;
                else if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        private static bool IsBoolean(string value)
        {
            return value == "true" || value == "false";
        }
    }
}
