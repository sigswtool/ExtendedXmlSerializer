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
using System.Runtime.Serialization.Formatters;
using JetBrains.Annotations;

namespace ExtendedXmlSerializer.ContentModel.Identification
{
	sealed class NamespaceFormatter : INamespaceFormatter
	{
		public static NamespaceFormatter Default { get; } = new NamespaceFormatter();
		NamespaceFormatter() : this(FormatterAssemblyStyle.Simple) {}

		[UsedImplicitly]
		public static NamespaceFormatter Full { get; } = new NamespaceFormatter(FormatterAssemblyStyle.Full);

		readonly FormatterAssemblyStyle _style;

		public NamespaceFormatter(FormatterAssemblyStyle style)
		{
			_style = style;
		}

		public string Get(TypeInfo type) => $"clr-namespace:{type.Namespace};assembly={Name(type)}";

		string Name(TypeInfo type)
		{
			var name = type.Assembly.GetName();
			switch (_style)
			{
				case FormatterAssemblyStyle.Simple:
					return name.Name;
				default:
					return name.ToString();
			}
		}
	}
}