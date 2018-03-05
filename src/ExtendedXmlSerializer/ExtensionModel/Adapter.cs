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

using System.Xml.Linq;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using XmlWriter = System.Xml.XmlWriter;

namespace ExtendedXmlSerializer.ExtensionModel
{
	sealed class Adapter<T> : IExtendedXmlCustomSerializer
	{
		readonly IExtendedXmlCustomSerializer<T> _configuration;

		public Adapter(IExtendedXmlCustomSerializer<T> configuration) => _configuration = configuration;

		public object Deserialize(XElement xElement) => _configuration.Deserialize(xElement);

		public void Serializer(XmlWriter xmlWriter, object instance)
			=> _configuration.Serializer(xmlWriter, instance.AsValid<T>());
	}

	/*sealed class CustomSerializers<T> : IParameterizedSource<Type, IExtendedXmlCustomSerializer<T>>
	{
		public static CustomSerializers<T> Default { get; } = new CustomSerializers<T>();
		CustomSerializers() : this(Activators.Default) {}

		readonly IActivators _activators;

		public CustomSerializers(IActivators activators) => _activators = activators;

		public IExtendedXmlCustomSerializer<T> Get(Type parameter) => _activators.New<IExtendedXmlCustomSerializer<T>>(parameter);
	}*/

/*
	public sealed class ActivatedXmlCustomSerializer<T, TSerializer> : IExtendedXmlCustomSerializer<T> where TSerializer : IExtendedXmlCustomSerializer<T>
	{
		public static ActivatedXmlCustomSerializer<T, TSerializer> Default { get; } = new ActivatedXmlCustomSerializer<T, TSerializer>();
		ActivatedXmlCustomSerializer() : this(CustomSerializers<T>.Default.Build(typeof(TSerializer))) {}

		readonly Func<IExtendedXmlCustomSerializer<T>> _source;

		public ActivatedXmlCustomSerializer(Func<IExtendedXmlCustomSerializer<T>> source) => _source = source;

		public T Deserialize(XElement xElement) => _source.Invoke().Deserialize(xElement);

		public void Serializer(XmlWriter xmlWriter, T instance)
		{
			_source.Invoke().Serializer(xmlWriter, instance);
		}
	}
*/
}