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
using ExtendedXmlSerializer.ContentModel.Reflection;
using ExtendedXmlSerializer.ReflectionModel;

namespace ExtendedXmlSerializer.ContentModel.Collections
{
	sealed class ArrayContents : ICollectionContents
	{
		readonly IInnerContentServices _contents;
		readonly IEnumerators _enumerators;
		readonly IClassification _classification;

		public ArrayContents(IInnerContentServices contents, IEnumerators enumerators, IClassification classification)
		{
			_contents = contents;
			_enumerators = enumerators;
			_classification = classification;
		}

		public ISerializer Get(CollectionContentInput parameter)
			=> new Serializer(new ArrayReader(_contents, _classification, parameter.Classification, parameter.Item),
				new EnumerableWriter(_enumerators, parameter.Item).Adapt());
	}


	// ATTRIBUTION: https://stackoverflow.com/a/1183019/3602057
}