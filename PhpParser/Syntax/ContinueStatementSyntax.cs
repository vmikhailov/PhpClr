using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class ContinueStatementSyntax : StatementSyntax
    {
        public override SyntaxType Kind => SyntaxType.ContinueStatement;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitContinueStatement(this);

        public override IEnumerable<BaseSyntax> ChildNodes => NoChildren;
    }
}
