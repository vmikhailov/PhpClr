using System;
using System.Collections.Generic;
using System.Linq;
using HackCLR.Parsers.PhpParser.Visitors;

namespace HackCLR.Parsers.PhpParser.Syntax;

public class CompilationUnitSyntax : BaseSyntax
{
	private readonly List<MemberDeclarationSyntax> _children;
		
	public override SyntaxType Kind => SyntaxType.CompilationUnit;

	public override void Accept(ApexSyntaxVisitor visitor)=> throw new InvalidOperationException();

	public override IEnumerable<BaseSyntax> ChildNodes => _children;

	public CompilationUnitSyntax(IEnumerable<MemberDeclarationSyntax> children)
	{
		_children = children.ToList();
	}
}