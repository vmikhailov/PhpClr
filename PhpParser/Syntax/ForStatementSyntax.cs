﻿using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class ForStatementSyntax : StatementSyntax
    {
        public override SyntaxType Kind => SyntaxType.ForStatement;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitForStatement(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Declaration, Statement);

        public VariableDeclarationSyntax Declaration { get; set; }

        public ExpressionSyntax Condition { get; set; }

        public List<ExpressionSyntax> Incrementors { get; set; }

        public StatementSyntax Statement { get; set; }
    }
}
