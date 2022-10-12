using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class ParenthesizedExpressionSyntax : ExpressionSyntax
    {
        public override SyntaxType Kind => SyntaxType.ParenthesizedExpression;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitParenthesizedExpression(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Expression);

        public ExpressionSyntax Expression { get; set; }
    }
}
