using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class ThisExpressionSyntax : ExpressionSyntax
    {
        public override SyntaxType Kind => SyntaxType.ThisExpression;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitThisExpression(this);

        public override IEnumerable<BaseSyntax> ChildNodes => NoChildren;
    }
}