using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class ForEachStatementSyntax : StatementSyntax
    {
        public override SyntaxType Kind => SyntaxType.ForEachStatement;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitForEachStatement(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Type, Expression, Statement);

        public TypeSyntax Type { get; set; }

        public string Identifier { get; set; }

        public ExpressionSyntax Expression { get; set; }

        public StatementSyntax Statement { get; set; }
    }
}
