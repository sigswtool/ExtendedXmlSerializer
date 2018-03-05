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

using ExtendedXmlSerializer.ContentModel.Content;
using ExtendedXmlSerializer.ContentModel.Members;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.ExtensionModel.Types;
using ExtendedXmlSerializer.ReflectionModel;

namespace ExtendedXmlSerializer.ExtensionModel.Content.Members
{
	sealed class ParameterizedMembersExtension : ISerializerExtension
	{
		public static ParameterizedMembersExtension Default { get; } = new ParameterizedMembersExtension();
		ParameterizedMembersExtension() {}

		public IServiceRepository Get(IServiceRepository parameter)
			=> parameter.Register<TypeMembers>()
			            .Register<IConstructorMembers, ConstructorMembers>()
			            .Register<IQueriedConstructors, QueriedConstructors>()
			            .Register<IParameterizedMembers, ParameterizedMembers>()
			            .Decorate<IMemberAccessors, ParameterizedMemberAccessors>()
			            .Decorate<IValidConstructorSpecification, ParameterizedConstructorSpecification>()
			            .Decorate<ITypeMembers, ParameterizedTypeMembers>()
			            .Decorate<IActivators, ParameterizedActivators>()
			            .Decorate<IInnerContentResult, ParameterizedResultHandler>();

		void ICommand<IServices>.Execute(IServices parameter) {}
	}
}