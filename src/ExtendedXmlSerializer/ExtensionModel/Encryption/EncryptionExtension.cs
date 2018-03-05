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
using ExtendedXmlSerializer.ContentModel.Members;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.Core.Sources;
using ExtendedXmlSerializer.Core.Specifications;
using ExtendedXmlSerializer.ExtensionModel.Content.Members;
using ExtendedXmlSerializer.ReflectionModel;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Reflection;

namespace ExtendedXmlSerializer.ExtensionModel.Encryption
{
	public sealed class EncryptionExtension : ISerializerExtension, ISpecification<IAlteration<IConverter>>
	{
		readonly static EncryptionConverterAlteration Alteration = EncryptionConverterAlteration.Default;

		readonly ISpecification<MemberInfo> _specification;
		readonly IAlteration<IConverter> _alteration;

		[UsedImplicitly]
		public EncryptionExtension() : this(Alteration) {}

		public EncryptionExtension(IAlteration<IConverter> alteration) : this(alteration, new HashSet<MemberInfo>()) {}

		public EncryptionExtension(ICollection<MemberInfo> registered) : this(Alteration, registered) {}

		public EncryptionExtension(IAlteration<IConverter> alteration, ICollection<MemberInfo> registered)
			: this(new ContainsSpecification<MemberInfo>(registered), alteration, registered) {}

		public EncryptionExtension(ISpecification<MemberInfo> specification, IAlteration<IConverter> alteration,
		                           ICollection<MemberInfo> registered)
		{
			_specification = specification;
			_alteration = alteration;
			Registered = registered;
		}

		public bool IsSatisfiedBy(IAlteration<IConverter> parameter) => parameter == _alteration;

		public ICollection<MemberInfo> Registered { get; }

		public IServiceRepository Get(IServiceRepository parameter)
			=> parameter.Decorate<IMemberContents>(Register)
			            .Decorate<IMemberConverters>(Register);

		IMemberConverters Register(IServiceProvider services, IMemberConverters converters)
			=> new AlteredMemberConverters(_specification, _alteration, converters);

		IMemberContents Register(IServiceProvider services, IMemberContents contents)
			=> new AlteredMemberContents(_specification, _alteration, contents, services.Get<IConverters>(),
			                             services.Get<ISerializers>()
			                                     .Get(Support<string>.Key));
		void ICommand<IServices>.Execute(IServices parameter) {}
	}
}