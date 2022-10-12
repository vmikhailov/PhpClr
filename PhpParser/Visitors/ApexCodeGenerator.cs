using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhpClr.Parsers.PhpParser.Syntax;

namespace HackCLR.Parsers.PhpParser.Visitors
{
    public class ApexCodeGenerator : ApexCodeGeneratorBase
    {
        public static string GenerateApex(BaseSyntax ast, int tabSize = 4)
        {
            var generator = new ApexCodeGenerator { IndentSize = tabSize };
            ast.Accept(generator);
            return generator.Code.ToString();
        }
    }
}
