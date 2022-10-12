using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class ClassOfExpressionSyntax : ExpressionSyntax
    {
        public override SyntaxType Kind => SyntaxType.ClassOfExpression;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitClassOfExpression(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Type);

        public TypeSyntax Type { get; set; }
    }
}
