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

using ExtendedXmlSerializer.ContentModel;
using ExtendedXmlSerializer.ContentModel.Content;
using ExtendedXmlSerializer.ContentModel.Format;
using ExtendedXmlSerializer.ContentModel.Members;

namespace ExtendedXmlSerializer.ExtensionModel.Xml
{
	sealed class XmlInnerContentActivator : IInnerContentActivator
	{
		readonly IReader _activator;
		readonly IXmlContentsActivator _contents;

		public XmlInnerContentActivator(IReader activator, IXmlContentsActivator contents)
		{
			_activator = activator;
			_contents = contents;
		}

		public IInnerContent Get(IFormatReader parameter)
		{
			var xml = (System.Xml.XmlReader) parameter.Get();
			var attributes = xml.HasAttributes ? new XmlAttributes(xml) : (XmlAttributes?) null;

			var depth = XmlDepth.Default.Get(xml);
			var content = depth.HasValue ? new XmlElements(xml, depth.Value) : (XmlElements?) null;

			var result = parameter.IsAssigned()
				             ? _contents.Create(parameter, _activator.Get(parameter), new XmlContent(attributes, content))
				             : null;
			return result;
		}
	}
}