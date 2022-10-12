using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class WhileStatementSyntax : StatementSyntax
    {
        public override SyntaxType Kind => SyntaxType.WhileStatement;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitWhileStatement(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Expression, Statement);

        public ExpressionSyntax Expression { get; set; }

        public StatementSyntax Statement { get; set; }
    }
}
