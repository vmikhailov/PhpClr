using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;
using Sprache;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class ExpressionSyntax : BaseSyntax
    {
        public ExpressionSyntax()
        {
        }

        public ExpressionSyntax(string expr) => ExpressionString = expr;

        public static ExpressionSyntax CreateOrDefault(IOption<string> expression)
        {
            if (expression.IsDefined)
            {
                return new ExpressionSyntax(expression.Get());
            }

            return null;
        }

        public override SyntaxType Kind => SyntaxType.Expression;

        public override IEnumerable<BaseSyntax> ChildNodes => NoChildren;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitExpression(this);

        public string ExpressionString { get; set; }
    }
}
