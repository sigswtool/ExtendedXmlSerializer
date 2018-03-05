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

using ExtendedXmlSerializer.ContentModel.Content;
using ExtendedXmlSerializer.ContentModel.Format;
using ExtendedXmlSerializer.ContentModel.Reflection;
using JetBrains.Annotations;
using System;
using System.Reflection;

namespace ExtendedXmlSerializer.ContentModel
{
	[UsedImplicitly]
	sealed class RuntimeSerializer : ISerializer
	{
		readonly IContents _contents;
		readonly IClassification _classification;

		public RuntimeSerializer(IContents contents, IClassification classification)
		{
			_contents = contents;
			_classification = classification;
		}

		public void Write(IFormatWriter writer, object instance)
		{
			var typeInfo = instance.GetType()
			                       .GetTypeInfo();
			var serializer = Serializer(typeInfo);
			serializer.Write(writer, instance);
		}

		ISerializer Serializer(TypeInfo typeInfo)
		{
			var serializer = _contents.Get(typeInfo);
			if (serializer is RuntimeSerializer)
			{
				throw new InvalidOperationException(
				                                    $"The serializer for type '{typeInfo}' could not be found.  Please ensure that the type is a valid type can be activated.  The default behavior requires an empty public constructor on the (non-abstract) class to activate.");
			}

			return serializer;
		}

		public object Get(IFormatReader reader) => Serializer(_classification.Get(reader))
			.Get(reader);
	}
}