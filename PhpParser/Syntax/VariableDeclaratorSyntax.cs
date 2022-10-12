using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class VariableDeclaratorSyntax : BaseSyntax
    {
        public override SyntaxType Kind => SyntaxType.VariableDeclarator;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitVariableDeclarator(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Expression);

        public string Identifier { get; set; }

        public ExpressionSyntax Expression { get; set; }
    }
}
