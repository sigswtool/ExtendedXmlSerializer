// MIT License
// 
// Copyright (c) 2016-2018 Wojciech Nag�rski
//                    Michael DeMond
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Reflection;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.Core.Parsing;
using ExtendedXmlSerializer.Core.Sources;
using ExtendedXmlSerializer.Core.Sprache;

namespace ExtendedXmlSerializer.ContentModel.Reflection
{
	sealed class ReflectionParser : CacheBase<string, MemberInfo>, IParser<MemberInfo>
	{
		readonly IParser<TypeInfo> _types;
		readonly ITypePartReflector _parts;
		readonly Parser<MemberParts> _parser;

		public ReflectionParser(IParser<TypeInfo> types, ITypePartReflector parts)
			: this(types, parts, MemberPartsParser.Default) {}

		public ReflectionParser(IParser<TypeInfo> types, ITypePartReflector parts, Parser<MemberParts> parser)
		{
			_types = types;
			_parts = parts;
			_parser = parser;
		}

		protected override MemberInfo Create(string parameter)
		{
			var parse = _parser.TryParse(parameter);
			if (parse.WasSuccessful)
			{
				var parts = parse.Value;
				var type = _parts.Get(parts.Type);
				var name = parts.MemberName;
				var result = type.GetMember(name).Only() ??
				             type.GetProperty(name) ??
				             type.GetField(name) ??
				             type.GetMethod(name) ??
				             (MemberInfo) type.GetEvent(name);
				return result;
			}
			return _types.Get(parameter);
		}
	}
}