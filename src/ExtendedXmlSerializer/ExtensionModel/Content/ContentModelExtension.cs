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

using ExtendedXmlSerializer.ContentModel.Collections;
using ExtendedXmlSerializer.ContentModel.Content;
using ExtendedXmlSerializer.ContentModel.Identification;
using ExtendedXmlSerializer.ContentModel.Members;
using ExtendedXmlSerializer.ContentModel.Reflection;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.Core.Sources;
using ExtendedXmlSerializer.ReflectionModel;
using VariableTypeSpecification = ExtendedXmlSerializer.ReflectionModel.VariableTypeSpecification;

namespace ExtendedXmlSerializer.ExtensionModel.Content
{
	public sealed class ContentModelExtension : ISerializerExtension
	{
		public static ContentModelExtension Default { get; } = new ContentModelExtension();

		ContentModelExtension() : this(ContentReaders.Default, ContentWriters.Default) {}

		readonly IContentReaders _readers;
		readonly IContentWriters _writers;

		public ContentModelExtension(IContentReaders readers, IContentWriters writers)
		{
			_readers = readers;
			_writers = writers;
		}

		public IServiceRepository Get(IServiceRepository parameter)
			=> parameter.Register<RuntimeElement>()
			            .Register<Element>()
			            .Register<IElement, Element>()
			            .Decorate<VariableTypeElement>(VariableTypeSpecification.Default)
			            .Decorate<GenericElement>(IsGenericTypeSpecification.Default)
			            .Decorate<ArrayElement>(IsArraySpecification.Default)
			            .Register<IClassification, Classification>()
			            .Register<IIdentityStore, IdentityStore>()
			            .Register<IInnerContentServices, InnerContentServices>()
			            .Register<IMemberHandler, MemberHandler>()
			            .Register<ICollectionContentsHandler, CollectionContentsHandler>()
			            .RegisterInstance(_writers)
			            .RegisterInstance(_readers)
			            .RegisterInstance<IAlteration<IInnerContentHandler>>(Self<IInnerContentHandler>.Default)
			            .RegisterInstance<IInnerContentResult>(InnerContentResult.Default)
			            .RegisterInstance<IMemberAssignment>(MemberAssignment.Default)
			            .RegisterInstance<ICollectionAssignment>(CollectionAssignment.Default)
			            .RegisterInstance<IListContentsSpecification>(ListContentsSpecification.Default);

		void ICommand<IServices>.Execute(IServices parameter) {}
	}
}