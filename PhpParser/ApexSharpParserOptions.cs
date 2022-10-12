namespace PhpClr.Parsers.PhpParser
{
    public class ApexSharpParserOptions
    {
        public string Namespace { get; set; } = "ApexSharpDemo.ApexCode";

        public int TabSize { get; set; } = 4;

        public bool UseLocalSObjectsNamespace { get; set; } = true;
    }
}
