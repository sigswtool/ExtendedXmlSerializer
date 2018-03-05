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

using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.Core.Sources;
using ExtendedXmlSerializer.ReflectionModel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ExtendedXmlSerializer.ContentModel.Members
{
	sealed class MemberSerializationBuilder : IParameterizedSource<IEnumerable<IMemberSerializer>, IMemberSerialization>
	{
		readonly static Func<IMemberSerializer, bool> Property =
			IsTypeSpecification<PropertyMemberSerializer>.Default.IsSatisfiedBy;

		public static MemberSerializationBuilder Default { get; } = new MemberSerializationBuilder();
		MemberSerializationBuilder() : this(Property) {}

		readonly Func<IMemberSerializer, bool> _property;

		public MemberSerializationBuilder(Func<IMemberSerializer, bool> property) => _property = property;

		public IMemberSerialization Get(IEnumerable<IMemberSerializer> parameter)
		{
			var members = parameter.Fixed();
			var properties = members.Where(_property)
			                        .ToArray();
			var runtime = members.OfType<IRuntimeSerializer>()
			                     .ToArray();
			var contents = members.Except(properties.Concat(runtime))
			                      .ToArray();
			var list = runtime.Any()
				           ? new RuntimeMemberList(_property, properties, runtime, contents)
				           : (IRuntimeMemberList) new FixedRuntimeMemberList(properties.Concat(contents)
				                                                                       .ToImmutableArray());
			var all = properties.Concat(runtime)
			                    .Concat(contents)
			                    .OrderBy(x => x.Profile.Order)
			                    .ToImmutableArray();
			var result = new MemberSerialization(list, all);
			return result;
		}
	}
}