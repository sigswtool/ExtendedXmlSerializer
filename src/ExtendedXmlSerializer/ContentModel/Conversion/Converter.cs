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

using ExtendedXmlSerializer.Core.Specifications;
using ExtendedXmlSerializer.ReflectionModel;
using System;
using System.Reflection;

namespace ExtendedXmlSerializer.ContentModel.Conversion
{
	public class Converter<T> : ConverterBase<T>, IConverter
	{
		readonly Func<string, T> _deserialize;
		readonly Func<T, string> _serialize;

		public Converter(Func<string, T> deserialize, Func<T, string> serialize) :
			this(Specification, deserialize, serialize) {}

		public Converter(ISpecification<TypeInfo> specification, Func<string, T> deserialize, Func<T, string> serialize)
			: base(specification)
		{
			_deserialize = deserialize;
			_serialize = serialize;
		}

		public sealed override T Parse(string data) => _deserialize(data);
		public sealed override string Format(T instance) => _serialize(instance);

		object IConvert<object>.Parse(string data) => Parse(data);
		string IConvert<object>.Format(object instance) => instance != null ? Format((T) instance) : null;

		public TypeInfo Get() => Support<T>.Key;
	}
}