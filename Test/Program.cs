// See https://aka.ms/new-console-template for more information

using System.Reflection;
using PhpClr.Parsers.PhpParser.Grammar;
using PhpClr.Parsers.PhpParser.Syntax;
using Sprache;
using Test;

class Program
{
	static string MemberStat(MemberDeclarationSyntax member)
	{
		switch (member)
		{
		}
	

		switch (member)
		{
			case AccessorDeclarationSyntax accessorDeclarationSyntax:
				break;

			case InterfaceDeclarationSyntax interfaceDeclarationSyntax:
				break;

			case ClassDeclarationSyntax classSyntax:
				return $"Class with " +
				       $"{classSyntax.Fields.Count()} field(s) and " +
				       $"{classSyntax.Methods.Count()} method(s)";

			case ClassInitializerSyntax classInitializerSyntax:
				break;

			case ConstructorDeclarationSyntax constructorDeclarationSyntax:
				break;

			case EnumDeclarationSyntax enumDeclarationSyntax:
				break;

			case EnumMemberDeclarationSyntax enumMemberDeclarationSyntax:
				break;

			case FieldDeclarationSyntax fieldDeclarationSyntax:
				break;

			case MethodDeclarationSyntax methodDeclarationSyntax:
				break;

			case PropertyDeclarationSyntax propertyDeclarationSyntax:
				break;

			default:
				break;
		}
		
		return member.GetType().ToString();
	}

	static string UnitStat(CompilationUnitSyntax unit)
	{
		var members = string.Join(", ", unit.Members.Select(MemberStat));
		return $"Members {unit.Members.Count()}: {members}";
	}
	
	public static void Main(string[] args)
	{
		var apex = new PhpGrammar();
		try
		{
			foreach (var (name, src) in ResourceHelper.ReadSeqResources("test{0:D3}.php", 1, 100))
			{
				Console.Write($"Testing {name}: ");
				var x = apex.CompilationUnit.Parse(src);
				Console.WriteLine($"ok. {UnitStat(x)}");
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}
}

//Assert.NotNull(x);