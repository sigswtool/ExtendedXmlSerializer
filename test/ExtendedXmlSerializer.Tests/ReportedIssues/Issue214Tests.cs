﻿using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.ExtensionModel;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using ExtendedXmlSerializer.Tests.Support;
using FluentAssertions;
using System;
using System.Xml;
using Xunit;

namespace ExtendedXmlSerializer.Tests.ReportedIssues
{
	public sealed class Issue214Tests
	{
		[Fact]
		void Verify()
		{
			var serializer = new ConfigurationContainer().EnableMemberExceptionHandling()
			                                             .Create()
			                                             .ForTesting();
			serializer.Invoking(x => x.Serialize(new Subject()))
			          .ShouldThrowExactly<InvalidOperationException>()
			          .WithMessage("An exception was encountered while serializing member 'ExtendedXmlSerializer.Tests.ReportedIssues.Issue214Tests+Subject.Message'.  Provided instance is 'ExtendedXmlSerializer.Tests.ReportedIssues.Issue214Tests+Subject'.")
			          .WithInnerException<InvalidOperationException>();

			serializer.Invoking(x => x.Deserialize<Subject>(@"<?xml version=""1.0"" encoding=""utf-8""?><Issue214Tests-Subject xmlns=""clr-namespace:ExtendedXmlSerializer.Tests.ReportedIssues;assembly=ExtendedXmlSerializer.Tests""><Message>Hello World!</Message></Issue214Tests-Subject>"))
			          .ShouldThrowExactly<InvalidOperationException>()
			          .WithMessage("An exception was encountered while deserializing member 'ExtendedXmlSerializer.Tests.ReportedIssues.Issue214Tests+Subject.Message'.")
			          .WithInnerException<XmlException>();
		}

		sealed class Subject
		{
			public string Message
			{
				get => throw new InvalidOperationException("Failed retrieving data.");
				set => throw new InvalidOperationException("Failed setting data.");
			}
		}
	}
}