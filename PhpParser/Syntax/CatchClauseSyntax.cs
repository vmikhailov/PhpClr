using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class CatchClauseSyntax : BaseSyntax
    {
        public override SyntaxType Kind => SyntaxType.Catch;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitCatch(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Type, Block);

        public TypeSyntax Type { get; set; }

        public string Identifier { get; set; }

        public BlockSyntax Block { get; set; }
    }
}
