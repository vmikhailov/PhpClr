using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class UpsertStatementSyntax : StatementSyntax
    {
        public override SyntaxType Kind => SyntaxType.UpsertStatement;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitUpsertStatement(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Expression);

        public ExpressionSyntax Expression { get; set; }
    }
}
