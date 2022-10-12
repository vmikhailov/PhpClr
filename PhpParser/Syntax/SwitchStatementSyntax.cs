using System.Collections.Generic;
using System.Linq;
using PhpClr.Parsers.PhpParser.Toolbox;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class SwitchStatementSyntax : StatementSyntax
    {
        public override SyntaxType Kind => SyntaxType.SwitchStatement;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitSwitchStatement(this);

        public override IEnumerable<BaseSyntax> ChildNodes =>
            GetNodes(Expression).Concat(WhenClauses.EmptyIfNull().Where(n => n != null));

        public ExpressionSyntax Expression { get; set; }

        public List<WhenClauseSyntax> WhenClauses { get; set; } = new List<WhenClauseSyntax>();
    }
}
