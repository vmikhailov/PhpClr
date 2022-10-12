using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class PrefixUnaryExpressionSyntax : ExpressionSyntax
    {
        public override SyntaxType Kind => SyntaxType.PrefixUnaryExpression;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitPrefixUnaryExpression(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Operand);

        public ExpressionSyntax Operand { get; set; }

        public string Operator { get; set; }
    }
}
