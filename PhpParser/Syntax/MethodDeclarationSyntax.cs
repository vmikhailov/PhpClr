﻿using System;
using System.Collections.Generic;
using System.Linq;
using ApexSharp.ApexParser.Visitors;
using Sprache;

namespace ApexSharp.ApexParser.Syntax
{
    public class MethodDeclarationSyntax : MemberDeclarationSyntax
    {
        public MethodDeclarationSyntax(MemberDeclarationSyntax heading = null)
            : base(heading)
        {
        }

        public override SyntaxType Kind => SyntaxType.Method;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitMethodDeclaration(this);

        public override IEnumerable<BaseSyntax> ChildNodes =>
            base.ChildNodes.Concat(GetNodes(ReturnType)).Concat(Parameters).Concat(GetNodes(Body)).Where(n => n != null);

        public TypeSyntax ReturnType { get; set; }

        public string Identifier { get; set; }

        public List<ParameterSyntax> Parameters { get; set; } = new List<ParameterSyntax>();

        public BlockSyntax Body { get; set; }

        public bool IsAbstract => Body == null;

        public override MemberDeclarationSyntax WithTypeAndName(ParameterSyntax typeAndName)
        {
            ReturnType = typeAndName.Type;
            Identifier = typeAndName.Identifier ?? typeAndName.Type.Identifier;
            return this;
        }
    }
}