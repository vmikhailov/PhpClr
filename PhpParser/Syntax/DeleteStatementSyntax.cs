using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class DeleteStatementSyntax : StatementSyntax
    {
        public override SyntaxType Kind => SyntaxType.DeleteStatement;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitDeleteStatement(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Expression);

        public ExpressionSyntax Expression { get; set; }
    }
}