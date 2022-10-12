using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class FieldDeclaratorSyntax : BaseSyntax
    {
        public override SyntaxType Kind => SyntaxType.FieldDeclarator;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitFieldDeclarator(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Expression);

        public string Identifier { get; set; }

        public ExpressionSyntax Expression { get; set; }
    }
}
