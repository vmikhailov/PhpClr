using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class ReturnStatementSyntax : StatementSyntax
    {
        public override SyntaxType Kind => SyntaxType.ReturnStatement;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitReturnStatement(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Expression);

        public ExpressionSyntax Expression { get; set; }
    }
}
