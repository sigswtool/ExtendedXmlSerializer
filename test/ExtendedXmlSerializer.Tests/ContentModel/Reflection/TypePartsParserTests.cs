﻿// MIT License
//
// Copyright (c) 2016-2018 Wojciech Nagórski
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

using ExtendedXmlSerializer.ContentModel.Conversion;
using ExtendedXmlSerializer.ContentModel.Properties;
using ExtendedXmlSerializer.ContentModel.Reflection;
using ExtendedXmlSerializer.Core.Parsing;
using ExtendedXmlSerializer.Core.Sprache;
using FluentAssertions;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace ExtendedXmlSerializer.Tests.ContentModel.Reflection
{
	public class TypePartsParserTests
	{
		const string
			Type = "SomeType",
			QualifiedType = "ns1:SomeType",
			GenericType = "ns123:GenericType[sys:string, int, ns4:SomeOtherType]",
			CompoundGenericType =
				"ns100:CompoundGenericType[sys:string, exs:AnotherGenericType[ns5:AnotherType,ns6:OneMoreType], ns40:SomeOtherType]";

		[Fact]
		public void Local()
			=> Assert.Equal(new TypeParts("SomeType"), TypePartsParser.Default.Get(Type), TypePartsEqualityComparer.Default);

		[Fact]
		public void Qualified()
		{
			var expected = new TypeParts("SomeType", "ns1");
			var identity = TypePartsParser.Default.Get(QualifiedType);
			Assert.Equal(expected, identity, TypePartsEqualityComparer.Default);
		}

		[Fact]
		public void Dimensions()
		{
			TypePartsEqualityComparer.Default.Equals(
					TypePartsParser.Default.Get("testing:int^3,4,2"),
					new TypeParts("int", "testing", dimensions: ImmutableArray.Create(3, 4, 2)))
				.Should()
				.BeTrue();
		}

		[Fact]
		public void Generic()
		{
			var expected = new TypeParts("GenericType", "ns123", new[]
			                                                      {
				                                                      new TypeParts("string", "sys"),
				                                                      new TypeParts("int"),
				                                                      new TypeParts("SomeOtherType", "ns4")
			                                                      }.ToImmutableArray);

			var actual = TypePartsParser.Default.Get(GenericType);
			Assert.Equal(expected, actual, TypePartsEqualityComparer.Default);
			Assert.Equal(GenericType.Replace(" ", ""), TypePartsFormatter.Default.Get(actual));
		}

		[Fact]
		public void CompoundGeneric()
		{
			var expected = new TypeParts("CompoundGenericType", "ns100", new[]
			                                                              {
				                                                              new TypeParts("string", "sys"),
				                                                              new TypeParts("AnotherGenericType", "exs",
				                                                                             new[]
				                                                                             {
					                                                                             new TypeParts("AnotherType", "ns5"),
					                                                                             new TypeParts("OneMoreType", "ns6")
				                                                                             }.ToImmutableArray),
				                                                              new TypeParts("SomeOtherType", "ns40")
			                                                              }.ToImmutableArray);

			var actual = TypePartsParser.Default.Get(CompoundGenericType);
			Assert.Equal(CompoundGenericType.Replace(" ", ""), TypePartsFormatter.Default.Get(actual));
			Assert.Equal(expected, actual, TypePartsEqualityComparer.Default);
		}

		[Fact]
		public void List()
		{
			var actual = TypePartsList.Default.ToParser().Parse("sys:string, int, ns4:SomeOtherType");
			Assert.True(new[]
			            {
				            new TypeParts("string", "sys"),
				            new TypeParts("int"),
				            new TypeParts("SomeOtherType", "ns4")
			            }.ToImmutableArray().SequenceEqual(actual, TypePartsEqualityComparer.Default));
		}

		[Fact]
		public void InvalidList()
		{
			var actual = TypePartsList.Default.ToParser().TryParse("[sys:string, int, ns4:SomeOtherType]").WasSuccessful;
			Assert.False(actual);
		}

		[Fact]
		public void AssemblyPathParser()
		{
			const string input = "clr-namespace:ExtendedXmlSerializer.Tests.TestObject;assembly=ExtendedXmlSerializer.Tests";
			var path = ExtendedXmlSerializer.ContentModel.Reflection.AssemblyPathParser.Default.ToParser().Parse(input);
			Assert.Equal("ExtendedXmlSerializer.Tests.TestObject", path.Namespace);
			Assert.Equal("ExtendedXmlSerializer.Tests", path.Path);
		}

		[Fact]
		public void GenericName()
		{
			var name = typeof(Dictionary<,>).Name;
			var result = GenericNameParser.Default.ToParser().Parse(name);
			Assert.Equal("Dictionary", result);
		}
	}
}