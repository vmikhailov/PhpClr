using Sprache;

namespace PhpClr.Parsers.PhpParser.Toolbox
{
    public class ParseExceptionCustom : ParseException
    {
        public string[] Apexcode { get; set; }
        public int LineNumber { get; set; }
        public ParseExceptionCustom(string message, int lineNumber, string[] apexcode)
            : base(message)
        {
            LineNumber = lineNumber;
            Apexcode = apexcode;
        }
    }
}
