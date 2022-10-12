using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class IfStatementSyntax : StatementSyntax
    {
        public override SyntaxType Kind => SyntaxType.IfStatement;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitIfStatement(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Expression, ThenStatement, ElseStatement);

        public ExpressionSyntax Expression { get; set; }

        public StatementSyntax ThenStatement { get; set; }

        public StatementSyntax ElseStatement { get; set; }
    }
}
