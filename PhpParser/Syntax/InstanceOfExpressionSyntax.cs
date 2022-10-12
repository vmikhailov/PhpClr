using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class InstanceOfExpressionSyntax : ExpressionSyntax
    {
        public override SyntaxType Kind => SyntaxType.InstanceOfExpression;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitInstanceOfExpression(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Expression, Type);

        public ExpressionSyntax Expression { get; set; }

        public TypeSyntax Type { get; set; }
    }
}