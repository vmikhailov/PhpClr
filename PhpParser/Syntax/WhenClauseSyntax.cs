using System.Collections.Generic;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public abstract class WhenClauseSyntax : StatementSyntax
    {
        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Block);

        public BlockSyntax Block { get; set; }
    }
}
