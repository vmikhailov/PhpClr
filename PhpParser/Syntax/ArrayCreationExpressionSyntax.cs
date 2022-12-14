using System.Collections.Generic;
using System.Linq;
using PhpClr.Parsers.PhpParser.Toolbox;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class ArrayCreationExpressionSyntax : ExpressionSyntax
    {
        public override SyntaxType Kind => SyntaxType.ArrayCreationExpression;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitArrayCreationExpression(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Type).Concat(Expressions.EmptyIfNull());

        public TypeSyntax Type { get; set; }

        public List<ExpressionSyntax> Expressions { get; set; } = new List<ExpressionSyntax>();
    }
}
