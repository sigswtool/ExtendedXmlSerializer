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
using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.ContentModel.Identification;
using ExtendedXmlSerializer.ContentModel.Reflection;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.ReflectionModel;
using Xunit;
using Identity = ExtendedXmlSerializer.ContentModel.Identification.Identity;

namespace ExtendedXmlSerializer.Tests.ContentModel.Reflection
{
	public class PartitionedTypeCandidatesTests
	{
		[Fact]
		public void TestName()
		{
			var expected = typeof(Subject).GetTypeInfo();
			var @namespace = NamespaceFormatter.Default.Get(expected);
			var aliases = new Names(new TypedTable<string>(DefaultNames.Default));
			var formatter = new TypeFormatter(aliases);
			var partitions = new AssemblyTypePartitions(PartitionedTypeSpecification.Default, formatter);
			var type = new PartitionedTypeCandidates(TypeLoader.Default, partitions).Get(new Identity(formatter.Get(expected), @namespace)).Only();
			Assert.Equal(expected, type);
		}

		sealed class Subject {}
	}
}