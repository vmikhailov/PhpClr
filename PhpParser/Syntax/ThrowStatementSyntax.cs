using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class ThrowStatementSyntax : StatementSyntax
    {
        public override SyntaxType Kind => SyntaxType.ThrowStatement;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitThrowStatement(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Expression);

        public ExpressionSyntax Expression { get; set; }
    }
}
