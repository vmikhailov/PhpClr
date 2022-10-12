using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class FinallyClauseSyntax : BaseSyntax
    {
        public override SyntaxType Kind => SyntaxType.Finally;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitFinally(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Block);

        public BlockSyntax Block { get; set; }
    }
}
