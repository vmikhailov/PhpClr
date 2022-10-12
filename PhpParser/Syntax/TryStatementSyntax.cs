﻿using System.Collections.Generic;
using System.Linq;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class TryStatementSyntax : StatementSyntax
    {
        public override SyntaxType Kind => SyntaxType.TryStatement;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitTryStatement(this);

        public override IEnumerable<BaseSyntax> ChildNodes =>
            GetNodes(Block).Concat(Catches).Concat(GetNodes(Finally)).Where(n => n != null);

        public BlockSyntax Block { get; set; }

        public List<CatchClauseSyntax> Catches { get; set; } = new List<CatchClauseSyntax>();

        public FinallyClauseSyntax Finally { get; set; }
    }
}
