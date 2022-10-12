﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApexSharp.ApexParser.Visitors;

namespace ApexSharp.ApexParser.Syntax
{
    public class IfStatementSyntax : StatementSyntax
    {
        public override SyntaxType Kind => SyntaxType.IfStatement;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitIfStatement(this);

        public override IEnumerable<BaseSyntax> ChildNodes => GetNodes(Expression, ThenStatement, ElseStatement);

        public ExpressionSyntax Expression { get; set; }

        public StatementSyntax ThenStatement { get; set; }

        public StatementSyntax ElseStatement { get; set; }
    }
}
