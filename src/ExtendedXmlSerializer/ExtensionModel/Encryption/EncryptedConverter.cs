﻿// MIT License
// 
// Copyright (c) 2016-2018 Wojciech Nagórski
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

using ExtendedXmlSerializer.ContentModel.Conversion;
using System.Reflection;

namespace ExtendedXmlSerializer.ExtensionModel.Encryption
{
	sealed class EncryptedConverter : ConverterBase<object>, IConverter
	{
		readonly IEncryption _encryption;
		readonly IConverter _converter;

		public EncryptedConverter(IEncryption encryption, IConverter converter)
		{
			_encryption = encryption;
			_converter = converter;
		}

		public override string Format(object instance) => _encryption.Format(_converter.Format(instance));

		public override object Parse(string data) => _converter.Parse(_encryption.Parse(data));

		public TypeInfo Get() => _converter.Get();
	}
}