﻿using System.Collections;
using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Toolbox;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class BlockSyntax : StatementSyntax, IEnumerable, IEnumerable<StatementSyntax>
    {
        public BlockSyntax()
        {
        }

        public BlockSyntax(IEnumerable<StatementSyntax> statements)
        {
            Statements.AddRange(statements.EmptyIfNull());
        }

        public override SyntaxType Kind => SyntaxType.Block;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitBlock(this);

        public override IEnumerable<BaseSyntax> ChildNodes => Statements;

        public List<StatementSyntax> Statements { get; set; } = new List<StatementSyntax>();

        public void Add(StatementSyntax statement) => Statements.Add(statement);

        public List<string> InnerComments { get; set; } = new List<string>();

        public IEnumerator GetEnumerator() => ((IEnumerable)Statements).GetEnumerator();

        IEnumerator<StatementSyntax> IEnumerable<StatementSyntax>.GetEnumerator() => Statements.GetEnumerator();
    }
}
