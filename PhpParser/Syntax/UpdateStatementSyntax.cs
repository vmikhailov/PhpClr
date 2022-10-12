using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class UpdateStatementSyntax : StatementSyntax
    {
        public override SyntaxType Kind => SyntaxType.UpdateStatement;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitUpdateStatement(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Expression);

        public ExpressionSyntax Expression { get; set; }
    }
}
