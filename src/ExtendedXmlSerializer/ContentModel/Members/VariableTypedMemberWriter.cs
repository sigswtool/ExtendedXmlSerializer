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

using ExtendedXmlSerializer.ContentModel.Format;
using ExtendedXmlSerializer.ContentModel.Properties;
using ExtendedXmlSerializer.Core.Specifications;
using System;
using System.Reflection;

namespace ExtendedXmlSerializer.ContentModel.Members
{
	sealed class VariableTypedMemberWriter : IWriter
	{
		readonly ISpecification<Type> _type;
		readonly ISerializer _runtime;
		readonly IWriter _body;
		readonly IProperty<TypeInfo> _property;

		public VariableTypedMemberWriter(ISpecification<Type> type, ISerializer runtime, IWriter body)
			: this(type, runtime, body, ExplicitTypeProperty.Default) {}

		public VariableTypedMemberWriter(ISpecification<Type> type, ISerializer runtime, IWriter body,
		                                 IProperty<TypeInfo> property)
		{
			_type = type;
			_runtime = runtime;
			_body = body;
			_property = property;
			_property = property;
		}

		public void Write(IFormatWriter writer, object instance)
		{
			var type = instance?.GetType();
			if (type != null && _type.IsSatisfiedBy(type))
			{
				_property.Write(writer, type.GetTypeInfo());
				_runtime.Write(writer, instance);
			}
			else
			{
				_body.Write(writer, instance);
			}
		}
	}
}