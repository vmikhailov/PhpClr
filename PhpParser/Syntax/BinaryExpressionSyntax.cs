using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class BinaryExpressionSyntax : ExpressionSyntax
    {
        public override SyntaxType Kind => SyntaxType.BinaryExpression;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitBinaryExpression(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Left, Right);

        public ExpressionSyntax Left { get; set; }

        public string Operator { get; }

        public ExpressionSyntax Right { get; set; }
    }
}
