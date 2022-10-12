using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class WhenTypeClauseSyntax : WhenClauseSyntax
    {
        public override SyntaxType Kind => SyntaxType.WhenTypeClause;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitWhenTypeClauseSyntax(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Type, Block);

        public TypeSyntax Type { get; set; }

        public string Identifier { get; set; }
    }
}
