// HSLuv rev4 library for Unity.
// SPDX-FileCopyrightText: Copyright (c) Takayuki Matsuoka
// SPDX-License-Identifier: MIT

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Hsluv {
    internal static class MicroJson {
        internal static object Deserialize(string jsonString) => Parser.Parse(jsonString);

        private sealed class Parser {
            internal static object Parse(string jsonString)
            {
                if (string.IsNullOrEmpty(jsonString)) {
                    return null;
                }
                using (var reader = new StringReader(jsonString)) {
                    var parser = new Parser(reader);
                    return parser.ParseValue();
                }
            }

            //
            private readonly StringReader _jsonReader;

            private Parser(StringReader jsonReader) => _jsonReader = jsonReader;

            private void DitchChar() => _jsonReader.Read();

            private bool HasChar() => _jsonReader.Peek() != -1;

            private char PeekChar() => Convert.ToChar(_jsonReader.Peek());

            private char NextChar() => Convert.ToChar(_jsonReader.Read());

            //
            private object ParseValue() => ParseByToken(NextToken());

            private enum Token {
                NONE,
                CURLY_OPEN,
                CURLY_CLOSE,
                SQUARED_OPEN,
                SQUARED_CLOSE,
                COLON,
                COMMA,
                STRING,
                NUMBER,
                TRUE,
                FALSE,
                NULL
            };

            private object ParseByToken(Token token)
            {
                switch (token) {
                    case Token.CURLY_OPEN:   return ParseObject();
                    case Token.SQUARED_OPEN: return ParseArray();
                    case Token.STRING:       return ParseString();
                    case Token.NUMBER:       return double.TryParse(NextWord(), out double d) ? d : 0.0;
                    case Token.TRUE:         return true;
                    case Token.FALSE:        return false;
                    case Token.NULL:         return null;
                    default:                 return null;
                }
            }

            private Dictionary<string, object> ParseObject()
            {
                DitchChar(); // '{' : ditch opening brace
                var table = new Dictionary<string, object>();
                while (true) {
                    switch (NextToken()) {
                        case Token.NONE:        return null;
                        case Token.CURLY_CLOSE: return table;
                        case Token.COMMA:       continue;
                        default:
                            // name
                            string name = ParseString();
                            if (name == null) {
                                return null;
                            }
                            // :
                            if (NextToken() != Token.COLON) {
                                return null;
                            }
                            DitchChar(); // ':' : ditch the colon
                            // value
                            table[name] = ParseValue();
                            break;
                    }
                }
            }

            private List<object> ParseArray()
            {
                DitchChar(); // '[' : ditch opening bracket
                var array = new List<object>();
                while (true) {
                    Token nextToken = NextToken();
                    switch (nextToken) {
                        case Token.NONE:          return null;
                        case Token.SQUARED_CLOSE: return array;
                        case Token.COMMA:         continue;
                        default:
                            array.Add(ParseByToken(nextToken));
                            break;
                    }
                }
            }

            private string ParseString()
            {
                var sb = new StringBuilder();
                DitchChar(); // '"' : ditch opening quote
                while (HasChar()) {
                    char d = NextChar();
                    if (d == '"') {
                        break;
                    } else if (d == '\\') {
                        if (!HasChar()) {
                            break;
                        }
                        char c = NextChar();
                        switch (c) {
                            default:
                                sb.Append(c);
                                break;
                            case 'b':
                                sb.Append('\b');
                                break;
                            case 'f':
                                sb.Append('\f');
                                break;
                            case 'n':
                                sb.Append('\n');
                                break;
                            case 'r':
                                sb.Append('\r');
                                break;
                            case 't':
                                sb.Append('\t');
                                break;
                            case 'u':
                                var hex = new char[4];
                                for (int i = 0; i < 4; i++) {
                                    hex[i] = NextChar();
                                }
                                sb.Append((char)Convert.ToInt32(new string(hex), 16));
                                break;
                        }
                    } else {
                        sb.Append(d);
                    }
                }
                return sb.ToString();
            }

            private Token NextToken()
            {
                while (char.IsWhiteSpace(PeekChar())) {
                    DitchChar();
                    if (!HasChar()) {
                        break;
                    }
                }
                if (!HasChar()) {
                    return Token.NONE;
                }
                switch (PeekChar()) {
                    case '{': return Token.CURLY_OPEN;
                    case '}':
                        DitchChar();
                        return Token.CURLY_CLOSE;
                    case '[': return Token.SQUARED_OPEN;
                    case ']':
                        DitchChar();
                        return Token.SQUARED_CLOSE;
                    case ',':
                        DitchChar();
                        return Token.COMMA;
                    case '"': return Token.STRING;
                    case ':': return Token.COLON;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '-': return Token.NUMBER;
                }
                switch (NextWord()) {
                    case "false": return Token.FALSE;
                    case "true":  return Token.TRUE;
                    case "null":  return Token.NULL;
                }
                return Token.NONE;
            }

            private const string WordBreak = "{}[],:\"";

            private string NextWord()
            {
                var sb = new StringBuilder();
                while (true) {
                    char c = PeekChar();
                    if (char.IsWhiteSpace(c) || WordBreak.IndexOf(c) != -1) {
                        break;
                    }
                    sb.Append(NextChar());
                    if (!HasChar()) {
                        break;
                    }
                }
                return sb.ToString();
            }
        }
    }
}
