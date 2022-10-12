using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class BreakStatementSyntax : StatementSyntax
    {
        public override SyntaxType Kind => SyntaxType.BreakStatement;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitBreakStatement(this);

        public override IEnumerable<BaseSyntax> ChildNodes => NoChildren;
    }
}
