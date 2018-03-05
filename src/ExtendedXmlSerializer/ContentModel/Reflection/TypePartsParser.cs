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

using System.Collections.Immutable;
using ExtendedXmlSerializer.ContentModel.Conversion;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.Core.Parsing;
using ExtendedXmlSerializer.Core.Sprache;

namespace ExtendedXmlSerializer.ContentModel.Reflection
{
	sealed class TypePartsParser : Parsing<TypeParts>
	{
		readonly static Parser<char> Start = Parse.Char('[')
				.Token(),
			Finish = Parse.Char(']')
				.Token();

		public static TypePartsParser Default { get; } = new TypePartsParser();

		TypePartsParser() : this(Identity.Default, TypePartsList.Default, DimensionsParser.Default)
		{
		}

		public TypePartsParser(Parser<Key> identity, Parser<ImmutableArray<TypeParts>> arguments,
			Parser<ImmutableArray<int>> dimensions)
			: base(
				identity.SelectMany(arguments.Contained(Start, Finish)
						.Optional())
					.SelectMany(Parse.Char('^')
							.Token()
							.Then(dimensions.Accept)
							.Optional()
							.Accept,
						(key, argument) => new TypeParts(key.Item1.Name, key.Item1.Identifier, key.Item2.Build(), argument.GetAssigned()))
					.ToDelegate()
					.Cache()
					.Get()
			)
		{
		}
	}
}