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
using ExtendedXmlSerializer.ContentModel.Reflection;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.ExtensionModel.Types;
using JetBrains.Annotations;

namespace ExtendedXmlSerializer.ExtensionModel.Markup
{
	[UsedImplicitly]
	public sealed class StaticExtension : IMarkupExtension
	{
		readonly ISingletons _singletons;
		readonly string _memberPath;

		public StaticExtension(string typeName, string memberName)
			: this(string.Concat(typeName, DefaultClrDelimiters.Default.Separator, memberName)) {}

		public StaticExtension(string memberPath) : this(Singletons.Default, memberPath) {}

		StaticExtension(ISingletons singletons, string memberPath)
		{
			_singletons = singletons;
			_memberPath = memberPath;
		}

		public object ProvideValue(System.IServiceProvider serviceProvider)
			=> _singletons.Get(serviceProvider.GetValid<IReflectionParser>()
			                                  .Get(_memberPath)
			                                  .AsValid<PropertyInfo>());
	}
}