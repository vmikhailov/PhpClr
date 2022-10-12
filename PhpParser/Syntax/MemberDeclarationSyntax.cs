﻿using System;
using System.Collections.Generic;
using System.Linq;
using PhpClr.Parsers.PhpParser.Visitors;
using Sprache;

namespace PhpClr.Parsers.PhpParser.Syntax
{
	public class MemberDeclarationSyntax : BaseSyntax, IAnnotatedSyntax
	{
		public MemberDeclarationSyntax(MemberDeclarationSyntax other = null)
		{
			this.WithProperties(other);
		}

		public override SyntaxType Kind => SyntaxType.ClassMember;

		public override void Accept(ApexSyntaxVisitor visitor) => throw new InvalidOperationException();

		public override IEnumerable<BaseSyntax> ChildNodes =>
			Annotations.Where(n => n != null);

		public List<AnnotationSyntax> Annotations { get; set; } = new List<AnnotationSyntax>();

		public List<string> Modifiers { get; set; } = new List<string>();

		public IOption<string> AccessModifier { get; set; }

		public virtual MemberDeclarationSyntax WithTypeAndName(ParameterSyntax typeAndName)
		{
			return this;
		}

		public static MemberDeclarationSyntax Empty => new MemberDeclarationSyntax();
	}
}