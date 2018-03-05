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

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using ExtendedXmlSerializer.Core.Specifications;
using ExtendedXmlSerializer.ReflectionModel;

namespace ExtendedXmlSerializer.Core
{
	sealed class ServiceProvider : ISpecification<Type>, IServiceProvider
	{
		readonly ImmutableArray<object> _services;
		readonly ImmutableArray<TypeInfo> _types;

		public ServiceProvider(params object[] services) : this(services.ToImmutableArray()) {}

		public ServiceProvider(ImmutableArray<object> services)
			: this(services, services.Select(x => x.GetType().GetTypeInfo()).ToImmutableArray()) {}

		public ServiceProvider(ImmutableArray<object> services, ImmutableArray<TypeInfo> types)
		{
			_services = services;
			_types = types;
		}

		public object GetService(Type serviceType)
		{
			var info = serviceType.GetTypeInfo();
			var length = _services.Length;

			for (var i = 0; i < length; i++)
			{
				var item = _services[i];
				if (info.IsInstanceOfType(item))
				{
					return item;
				}
			}

			return null;
		}

		public bool IsSatisfiedBy(Type parameter)
			=> _types.Any(IsAssignableSpecification.Delegates.Get(parameter.GetTypeInfo()));
	}
}