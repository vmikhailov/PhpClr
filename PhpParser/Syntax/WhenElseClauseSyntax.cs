using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class WhenElseClauseSyntax : WhenClauseSyntax
    {
        public override SyntaxType Kind => SyntaxType.WhenElseClause;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitWhenElseClauseSyntax(this);
    }
}
