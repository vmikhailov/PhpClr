﻿using System.Collections.Generic;
using System.Linq;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class EnumDeclarationSyntax : MemberDeclarationSyntax
    {
        public EnumDeclarationSyntax(MemberDeclarationSyntax heading = null)
            : base(heading)
        {
        }

        public override SyntaxType Kind => SyntaxType.Enum;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitEnum(this);

        public override IEnumerable<BaseSyntax> ChildNodes =>
            base.ChildNodes.Concat(Members).Where(n => n != null);

        public string Identifier { get; set; }

        public List<EnumMemberDeclarationSyntax> Members { get; set; } = new List<EnumMemberDeclarationSyntax>();

        public List<string> InnerComments { get; set; } = new List<string>();
    }
}
