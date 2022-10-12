using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class SuperExpressionSyntax : ExpressionSyntax
    {
        public override SyntaxType Kind => SyntaxType.SuperExpression;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitSuperExpression(this);

        public override IEnumerable<BaseSyntax> ChildNodes => NoChildren;
    }
}