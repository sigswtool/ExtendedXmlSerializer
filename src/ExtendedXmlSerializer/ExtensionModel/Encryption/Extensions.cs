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

using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.ContentModel.Conversion;
using ExtendedXmlSerializer.Core.Sources;

namespace ExtendedXmlSerializer.ExtensionModel.Encryption
{
	using System.Reflection;

	public static class Extensions
	{
		public static IMemberConfiguration<T, TMember> Encrypt<T, TMember>(this IMemberConfiguration<T, TMember> @this)
		{
			@this.Root.With<EncryptionExtension>().Registered.Add(((ISource<MemberInfo>)@this).Get());
			return @this;
		}

		public static IConfigurationContainer UseEncryptionAlgorithm(this IConfigurationContainer @this)
			=> UseEncryptionAlgorithm(@this, EncryptionConverterAlteration.Default);

		public static IConfigurationContainer UseEncryptionAlgorithm(this IConfigurationContainer @this, IEncryption encryption)
			=> UseEncryptionAlgorithm(@this, new EncryptionConverterAlteration(encryption));

		public static IConfigurationContainer UseEncryptionAlgorithm(this IConfigurationContainer @this, IAlteration<IConverter> parameter)
			=> @this.Root
			        .With<EncryptionExtension>()
			        .IsSatisfiedBy(parameter)
				   ? @this
				   : @this.Extend(new EncryptionExtension(parameter));
	}
}