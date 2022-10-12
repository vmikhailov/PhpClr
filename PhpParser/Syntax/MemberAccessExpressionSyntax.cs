using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class MemberAccessExpressionSyntax : ExpressionSyntax
    {
        public override SyntaxType Kind => SyntaxType.MemberAccessExpression;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitMemberAccessExpression(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Expression);

        public ExpressionSyntax Expression { get; set; }

        public string Name { get; set; }
    }
}
