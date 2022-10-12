﻿using System.Collections.Generic;
using System.Linq;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class PropertyDeclarationSyntax : MemberDeclarationSyntax
    {
        public PropertyDeclarationSyntax(MemberDeclarationSyntax heading = null)
            : base(heading)
        {
        }

        public PropertyDeclarationSyntax(IEnumerable<AccessorDeclarationSyntax> accessors, MemberDeclarationSyntax heading = null)
            : this(heading)
        {
            Accessors = accessors.ToList();
        }

        public override SyntaxType Kind => SyntaxType.Property;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitPropertyDeclaration(this);

        public override IEnumerable<BaseSyntax> ChildNodes =>
            base.ChildNodes.Concat(GetNodes(Type, Getter, Setter));

        public TypeSyntax Type { get; set; }

        public string Identifier { get; set; }

        public List<AccessorDeclarationSyntax> Accessors { get; set; } = new List<AccessorDeclarationSyntax>();

        public AccessorDeclarationSyntax Getter => Accessors.FirstOrDefault(a => a.IsGetter);

        public AccessorDeclarationSyntax Setter => Accessors.FirstOrDefault(a => a.IsSetter);

        public override MemberDeclarationSyntax WithTypeAndName(ParameterSyntax typeAndName)
        {
            Type = typeAndName.Type;
            Identifier = typeAndName.Identifier ?? typeAndName.Type.Identifier;
            return this;
        }
    }
}
