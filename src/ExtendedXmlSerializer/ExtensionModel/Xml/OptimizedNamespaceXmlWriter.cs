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
using ExtendedXmlSerializer.ContentModel.Format;
using ExtendedXmlSerializer.ContentModel.Identification;
using ExtendedXmlSerializer.Core;

namespace ExtendedXmlSerializer.ExtensionModel.Xml
{
	sealed class OptimizedNamespaceXmlWriter : IFormatWriter
	{
		readonly IFormatWriter _writer;
		readonly ICommand<IIdentityStore> _command;

		public OptimizedNamespaceXmlWriter(IFormatWriter writer, ICommand<IIdentityStore> command)
		{
			_writer = writer;
			_command = command;
		}

		public object Get() => _writer.Get();

		public void Start(IIdentity identity)
		{
			_writer.Start(identity);

			_command.Execute(_writer);
		}

		public void EndCurrent() => _writer.EndCurrent();

		public void Content(IIdentity property, string content) => _writer.Content(property, content);
		public void Content(string content) => _writer.Content(content);

		public void Verbatim(string content)
		{
			_writer.Verbatim(content);
		}

		public string Get(MemberInfo parameter) => _writer.Get(parameter);
		public void Dispose() => _writer.Dispose();
		public IIdentity Get(string name, string identifier) => _writer.Get(name, identifier);
	}
}