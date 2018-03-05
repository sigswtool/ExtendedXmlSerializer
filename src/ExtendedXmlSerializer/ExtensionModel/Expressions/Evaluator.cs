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
using ExtendedXmlSerializer.ExtensionModel.Coercion;

namespace ExtendedXmlSerializer.ExtensionModel.Expressions
{
	sealed class Evaluator : IEvaluator
	{
		readonly ICoercers _coercers;
		readonly IExpressionEvaluator _evaluator;

		public Evaluator(ICoercers coercers, IExpressionEvaluator evaluator)
		{
			_coercers = coercers;
			_evaluator = evaluator;
		}

		public IEvaluation Get(IExpression parameter)
		{
			var expression = parameter.ToString();
			try
			{
				var instance = parameter is LiteralExpression ? expression : parameter.Get(_evaluator);
				return new Evaluation(_coercers, expression, instance);
			}
			catch (Exception e)
			{
				return new Evaluation(_coercers, expression, e);
			}
		}
	}
}