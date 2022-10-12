using System;
using System.Collections.Generic;
using System.Linq;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax;

public class CompilationUnitSyntax : BaseSyntax
{
	private readonly List<MemberDeclarationSyntax> _members;

	public override SyntaxType Kind => SyntaxType.CompilationUnit;

	public override void Accept(ApexSyntaxVisitor visitor) => throw new InvalidOperationException();

	public override IEnumerable<BaseSyntax> ChildNodes => _members;
	
	public IEnumerable<MemberDeclarationSyntax> Members => _members;

	public CompilationUnitSyntax(IEnumerable<MemberDeclarationSyntax> children)
	{
		_members = children.ToList();
	}
}