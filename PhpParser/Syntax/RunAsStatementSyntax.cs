using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class RunAsStatementSyntax : StatementSyntax
    {
        public override SyntaxType Kind => SyntaxType.RunAsStatement;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitRunAsStatement(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Expression, Statement);

        public ExpressionSyntax Expression { get; set; }

        public StatementSyntax Statement { get; set; }
    }
}
