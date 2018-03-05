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

using ExtendedXmlSerializer.ContentModel.Identification;
using ExtendedXmlSerializer.ContentModel.Reflection;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.Core.Sources;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace ExtendedXmlSerializer.ExtensionModel.Xml
{
	sealed class ImplicitTypingExtension : ISerializerExtension
	{
		readonly ImmutableArray<TypeInfo> _registered;

		public ImplicitTypingExtension(ImmutableArray<TypeInfo> registered) => _registered = registered;


		public IServiceRepository Get(IServiceRepository parameter)
			=> parameter.Decorate<IIdentities>(Register)
			            .Decorate<ITypeIdentities>(Register);

		ITypeIdentities Register(IServiceProvider services, ITypeIdentities identities)
		{
			var formatter = services.Get<ITypeFormatter>();
			var entries = new Types(formatter).Get(_registered);
			var result = new ImplicitTypeIdentities(identities, entries);
			return result;
		}

		IIdentities Register(IServiceProvider services, IIdentities identities)
			=> new ImplicitIdentities(_registered, identities);

		void ICommand<IServices>.Execute(IServices parameter) {}

		sealed class Types : IParameterizedSource<ImmutableArray<TypeInfo>, IParameterizedSource<string, TypeInfo>>
		{
			readonly ITypeFormatter _formatter;

			public Types(ITypeFormatter formatter) => _formatter = formatter;

			public IParameterizedSource<string, TypeInfo> Get(ImmutableArray<TypeInfo> parameter)
			{
				var items = parameter.Select(Item).ToArray();
				var groups = items.GroupBy(x => x.Key).ToArray();
				var invalid = groups.FirstOrDefault(x => x.Count() > 1);
				if (invalid != null)
				{
					var types = string.Join(", ", invalid.Select(x => x.Value.FullName));
					throw new InvalidOperationException(
						$"An attempt was made to configure implicit types, but there is more than one type with the same name, which would result in conflicts.  Please remove one of these types or configure them to have different names from each other: {types} have the shared name {invalid.Key}.");
				}
				var store = items.ToDictionary();
				var result = new TableSource<string, TypeInfo>(store);
				return result;
			}

			KeyValuePair<string, TypeInfo> Item(TypeInfo parameter) => Pairs.Create(_formatter.Get(parameter), parameter);
		}
	}
}