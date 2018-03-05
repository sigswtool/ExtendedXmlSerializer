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

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ExtendedXmlSerializer.Core.Sources;

namespace ExtendedXmlSerializer.ContentModel.Conversion
{
	sealed class Optimizations : IAlteration<IConverter>, IOptimizations
	{
		readonly ICollection<Action> _containers = new HashSet<Action>();

		public IConverter Get(IConverter parameter)
		{
			var parse = Create<string, object>(parameter.Parse);
			var format = Create<object, string>(parameter.Format);
			var result = new Converter<object>(parameter, parse, format);
			return result;
		}

		Func<TParameter, TResult> Create<TParameter, TResult>(Func<TParameter, TResult> source)
		{
			var dictionary = new ConcurrentDictionary<TParameter, TResult>();
			var cache = new Cache<TParameter, TResult>(source, dictionary);
			_containers.Add(dictionary.Clear);
			var result = cache.ToDelegate();
			return result;
		}

		public void Clear()
		{
			foreach (var container in _containers)
			{
				container.Invoke();
			}
		}
	}
}