using System.Collections.Generic;
using System.Linq;
using PhpClr.Parsers.PhpParser.Toolbox;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class WhenExpressionsClauseSyntax : WhenClauseSyntax
    {
        public override SyntaxType Kind => SyntaxType.WhenExpressionsClause;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitWhenExpressionsClauseSyntax(this);

        public override IEnumerable<BaseSyntax> ChildNodes =>
            Expressions.EmptyIfNull().Where(n => n != null).Concat(GetNodes(Block));

        // note: Apex only allows literals here
        public List<ExpressionSyntax> Expressions { get; set; } = new List<ExpressionSyntax>();
    }
}
