﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApexSharp.ApexParser.Visitors;

namespace ApexSharp.ApexParser.Syntax
{
    public class ConstructorDeclarationSyntax : MethodDeclarationSyntax
    {
        public ConstructorDeclarationSyntax(MethodDeclarationSyntax method = null)
            : base(method)
        {
            if (method != null)
            {
                Body = method.Body;
                ReturnType = method.ReturnType;
                Identifier = method.Identifier;
                Parameters = method.Parameters;
            }
        }

        public override SyntaxType Kind => SyntaxType.Constructor;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitConstructorDeclaration(this);

        public override MemberDeclarationSyntax WithTypeAndName(ParameterSyntax typeAndName)
        {
            Identifier = typeAndName.Identifier ?? typeAndName.Type.Identifier;
            return this;
        }

        public ExpressionSyntax ChainedConstructorCall { get; set; }
    }
}
