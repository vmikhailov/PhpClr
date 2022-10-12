﻿using System.Collections.Generic;
using System.Linq;
using HackCLR.Parsers.PhpParser.Syntax;
using HackCLR.Parsers.PhpParser.Toolbox;
using Sprache;

namespace HackCLR.Parsers.PhpParser.Grammar
{
	public class PhpGrammar : ICommentParserProvider
	{
		// examples: a, Apex, code123
		protected internal virtual Parser<string> RawIdentifier =>
			from localVarPrefix in Parse.Char('$').Optional()
			from identifier in Parse.Identifier(Parse.Letter, Parse.LetterOrDigit.Or(Parse.Char('_')))
			where !PhpKeywords.ReservedWords.Contains(identifier)
			select localVarPrefix + identifier;

		protected internal virtual Parser<string> Identifier =>
			RawIdentifier.Token().Named("Identifier");

		// examples: System.debug
		protected internal virtual Parser<IEnumerable<string>> QualifiedIdentifier =>
			Identifier.DelimitedBy(Parse.Char('.').Token())
			          .Named("QualifiedIdentifier");

		// examples: /* default settings are OK */ //
		public IComment CommentParser { get; } = new CommentParser();

		// example: @isTest, returned as IsTest
		protected internal virtual Parser<AnnotationSyntax> Annotation =>
			from at in Parse.Char('@').Token()
			from name in Identifier.Select(id => id.Normalized())
			from parameters in GenericExpressionInBraces().Optional().Token(this)
			select new AnnotationSyntax
			{
				Identifier = name,
				Parameters = parameters.GetOrDefault(),
			};

		// returns the keyword normalized to its canonical representation
		// examples: void, testMethod
		protected internal virtual Parser<string> Keyword(string text) =>
			Parse.IgnoreCase(text).Then(_ => Parse.Not(Parse.LetterOrDigit.Or(Parse.Char('_')))).Return(text);

		// examples: int, void
		protected internal virtual Parser<TypeSyntax> SystemType =>
			Keyword(PhpKeywords.Blob).Or(Keyword(PhpKeywords.Boolean)).Or(Keyword(PhpKeywords.Byte))
			                         .Or(Keyword(PhpKeywords.Char)).Or(Keyword(PhpKeywords.Datetime))
			                         .Or(Keyword(PhpKeywords.Date)).Or(Keyword(PhpKeywords.Decimal))
			                         .Or(Keyword(PhpKeywords.Double)).Or(Keyword(PhpKeywords.Exception))
			                         .Or(Keyword(PhpKeywords.Float)).Or(Keyword(PhpKeywords.ID))
			                         .Or(Keyword(PhpKeywords.Integer)).Or(Keyword(PhpKeywords.Int))
			                         .Or(Keyword(PhpKeywords.Long)).Or(Keyword(PhpKeywords.Object))
			                         .Or(Keyword(PhpKeywords.SetType)).Or(Keyword(PhpKeywords.Short))
			                         .Or(Keyword(PhpKeywords.String)).Or(Keyword(PhpKeywords.List))
			                         .Or(Keyword(PhpKeywords.Map)).Or(Keyword(PhpKeywords.Void))
			                         .Token().Select(n => new TypeSyntax(n))
			                         .Named("SystemType");

		// examples: int, String, System.Collections.Hashtable
		protected internal virtual Parser<TypeSyntax> NonGenericType =>
			SystemType.Or(QualifiedIdentifier.Select(qi => new TypeSyntax(qi)));

		// examples: string, int, char
		protected internal virtual Parser<IEnumerable<TypeSyntax>> TypeParameters =>
			from open in Parse.Char('<').Token()
			from types in TypeReference.DelimitedBy(Parse.Char(',').Token())
			from close in Parse.Char('>').Token()
			select types;

		// example: string, List<string>, Map<string, List<boolean>>
		protected internal virtual Parser<TypeSyntax> TypeReference =>
			from type in NonGenericType
			from parameters in TypeParameters.Optional()
			from arraySpecifier in Parse.Char('[').Token().Then(_ => Parse.Char(']').Token()).Optional()
			select new TypeSyntax(type)
			{
				TypeParameters = parameters.GetOrElse(Enumerable.Empty<TypeSyntax>()).ToList(),
				IsArray = arraySpecifier.IsDefined,
			};

		protected internal virtual Parser<(TypeSyntax Type, ICommented<string> Name)> ParameterWithType =>
			from type in TypeReference
			from name in Identifier.Commented(this)
			select (type, name);

		protected internal virtual Parser<(TypeSyntax Type, ICommented<string> Name)> ParameterWithoutType =>
			from name in Identifier.Commented(this)
			select (new TypeSyntax("object"), name);


		// example: string name
		// from type in TypeReference.Commented(this)
		protected internal virtual Parser<ParameterSyntax> ParameterDeclaration =>
			from modifiers in Modifier.Token().Many().Commented(this)
			from param in ParameterWithType.Or(ParameterWithoutType)
			select new ParameterSyntax(param.Type, param.Name.Value)
			{
				LeadingComments = modifiers.LeadingComments.ToList(),
				Modifiers = modifiers.Value.ToList(),
				TrailingComments = param.Name.TrailingComments.ToList(),
			};

		// example: int a, Boolean flag
		protected internal virtual Parser<IEnumerable<ParameterSyntax>> ParameterDeclarations =>
			ParameterDeclaration.DelimitedBy(Parse.Char(',').Token());

		// example: (string a, char delimiter)
		protected internal virtual Parser<List<ParameterSyntax>> MethodParameters =>
			from openBrace in Parse.Char('(').Token()
			from param in ParameterDeclarations.Optional()
			from closeBrace in Parse.Char(')').Token()
			select param.GetOrElse(Enumerable.Empty<ParameterSyntax>()).ToList();

		protected internal virtual Parser<string> AccessModifier =>
			Keyword(PhpKeywords.Public)
				.Or(Keyword(PhpKeywords.Protected))
				.Or(Keyword(PhpKeywords.Private))
				.Text().Token().Named("AccessModifier");


		// examples: public, private, with sharing
		protected internal virtual Parser<string> Modifier =>
			Keyword(PhpKeywords.Static)
				.Or(Keyword(PhpKeywords.Abstract))
				.Or(Keyword(PhpKeywords.Final))
				.Or(Keyword(PhpKeywords.Override))
				.Or(Keyword(PhpKeywords.Virtual))
				.Text().Token().Named("Modifier");

		// examples:
		// @isTest void Test() {}
		// public static void Hello() {}
		protected internal virtual Parser<MethodDeclarationSyntax> MethodDeclaration =>
			from heading in MemberDeclarationHeading
			from name in Identifier
			from methodBody in MethodParametersAndBody
			select new MethodDeclarationSyntax(heading)
			{
				Identifier = name,
				ReturnType = new("object"),
				Parameters = methodBody.Parameters,
				Body = methodBody.Body,
			};

		// examples: string Name, void Test
		protected internal virtual Parser<ParameterSyntax> TypeAndName =>
			// from type in TypeReference
			from name in Identifier.Optional()
			select new ParameterSyntax("object", name.GetOrDefault());

		// examples:
		// void Test() {}
		// string Hello(string name) {}
		// int Dispose();
		protected internal virtual Parser<MethodDeclarationSyntax> MethodParametersAndBody =>
			from parameters in MethodParameters
			from methodBody in Block.Or(Parse.Char(';').Return(default(BlockSyntax))).Token()
			select new MethodDeclarationSyntax
			{
				Parameters = parameters,
				Body = methodBody,
			};

		// example: @required public String name { get; set; }
		protected internal virtual Parser<PropertyDeclarationSyntax> PropertyDeclaration =>
			from heading in MemberDeclarationHeading
			from typeAndName in TypeAndName
			from accessors in PropertyAccessors
			select new PropertyDeclarationSyntax(heading)
			{
				Type = typeAndName.Type,
				Identifier = typeAndName.Identifier,
				Accessors = accessors.Accessors,
			};

		// example: { get; set; }
		protected internal virtual Parser<PropertyDeclarationSyntax> PropertyAccessors =>
			from openBrace in Parse.Char('{').Token()
			from accessors in PropertyAccessor.Many()
			from closeBrace in Parse.Char('}').Token()
			select new PropertyDeclarationSyntax(accessors);

		// examples: get; private set; get { return 0; }
		protected internal virtual Parser<AccessorDeclarationSyntax> PropertyAccessor =>
			from heading in MemberDeclarationHeading
			from keyword in Parse.IgnoreCase(PhpKeywords.Get).Or(Parse.IgnoreCase(PhpKeywords.Set)).Token().Text()
			from body in Parse.Char(';').Return(default(BlockSyntax)).Or(Block).Commented(this)
			select new AccessorDeclarationSyntax(heading)
			{
				IsGetter = keyword == PhpKeywords.Get,
				Body = body.Value,
				TrailingComments = body.TrailingComments.ToList(),
			};

		// example: private static int x, y, z = 3;
		protected internal virtual Parser<FieldDeclarationSyntax> FieldDeclaration =>
			from heading in MemberDeclarationHeading
			from type in TypeReference
			from declarators in FieldDeclarator.DelimitedBy(Parse.Char(',').Token())
			from semicolon in Parse.Char(';').Token()
			select new FieldDeclarationSyntax(heading)
			{
				Type = type,
				Fields = declarators.ToList(),
			};

		// example: now = DateTime.Now()
		protected internal virtual Parser<FieldDeclaratorSyntax> FieldDeclarator =>
			from identifier in Identifier.Commented(this)
			from expression in Parse.Char('=').Token().Then(_ => GenericExpression).Optional()
			select new FieldDeclaratorSyntax
			{
				Identifier = identifier.Value,
				Expression = ExpressionSyntax.CreateOrDefault(expression),
				LeadingComments = identifier.LeadingComments.ToList(),
			};

		// examples: return true; if (false) return; etc.
		protected internal virtual Parser<StatementSyntax> Statement =>
			from statement in Block.Select(s => s as StatementSyntax)
			                       .Or(IfStatement)
			                       .Or(DoStatement)
			                       .Or(ForEachStatement)
			                       .Or(ForStatement)
			                       .Or(WhileStatement)
			                       .Or(BreakStatement)
			                       .Or(ContinueStatement)
			                       .Or(RunAsStatement)
			                       .Or(TryCatchFinallyStatement)
			                       .Or(ReturnStatement)
			                       .Or(ThrowStatement)
			                       .Or(InsertStatement)
			                       .Or(UpdateStatement)
			                       .Or(UpsertStatement)
			                       .Or(DeleteStatement)
			                       .Or(VariableDeclaration)
			                       .Or(SwitchStatement)
			                       .Or(UnknownGenericStatement)
			                       .Commented(this)
			select statement.Value
			                .WithLeadingComments(statement.LeadingComments)
			                .WithTrailingComments(statement.TrailingComments);

		// examples: {}, { /* inner comments */ }, { int a = 0; return; } // trailing comments
		protected internal virtual Parser<BlockSyntax> Block =>
			from comments in CommentParser.AnyComment.Token().Many()
			from openBrace in Parse.Char('{').Token()
			from statements in Statement.Many()
			from closeBrace in Parse.Char('}').Commented(this)
			select new BlockSyntax
			{
				LeadingComments = comments.ToList(),
				Statements = statements.ToList(),
				InnerComments = closeBrace.LeadingComments.ToList(),
				TrailingComments = closeBrace.TrailingComments.ToList(),
			};

		// example: int x, y, z = 3;
		protected internal virtual Parser<VariableDeclarationSyntax> VariableDeclaration =>
			from type in TypeReference
			from declarators in VariableDeclarator.DelimitedBy(Parse.Char(',').Token())
			from semicolon in Parse.Char(';')
			select new VariableDeclarationSyntax
			{
				Type = type,
				Variables = declarators.ToList(),
			};

		// example: now = DateTime.Now()
		protected internal virtual Parser<VariableDeclaratorSyntax> VariableDeclarator =>
			from identifier in Identifier
			from expression in Parse.Char('=').Token().Then(_ => GenericExpression).Optional()
			select new VariableDeclaratorSyntax
			{
				Identifier = identifier,
				Expression = ExpressionSyntax.CreateOrDefault(expression),
			};

		// examples: (MyExpr), (MyExpr ex)
		protected internal virtual Parser<CatchClauseSyntax> CatchExpressionTypeName =>
			from openBrace in Parse.Char('(').Token()
			from exceptionType in TypeReference
			from identifier in Identifier.Optional()
			from closeBrace in Parse.Char(')').Token()
			select new CatchClauseSyntax
			{
				Type = exceptionType,
				Identifier = identifier.GetOrDefault(),
			};

		// examples: catch { ... }, catch (MyEx) { ...}, catch (MyEx ex) { ... }
		protected internal virtual Parser<CatchClauseSyntax> CatchClause =>
			from @catch in Parse.IgnoreCase(PhpKeywords.Catch).Commented(this)
			from expr in CatchExpressionTypeName.Commented(this).Optional()
			from block in Block.Commented(this)
			select new CatchClauseSyntax
			{
				LeadingComments = @catch.LeadingComments.ToList(),
				Type = expr.GetOrDefault()?.Value?.Type,
				Identifier = expr.GetOrDefault()?.Value?.Identifier,
				Block = block.Value.WithLeadingComments(block.LeadingComments),
				TrailingComments = block.TrailingComments.ToList(),
			};

		// examples: finally { ... }
		protected internal virtual Parser<FinallyClauseSyntax> FinallyClause =>
			from @finally in Parse.IgnoreCase(PhpKeywords.Finally).Commented(this)
			from block in Block.Commented(this)
			select new FinallyClauseSyntax
			{
				LeadingComments = @finally.LeadingComments.ToList(),
				Block = block.Value.WithLeadingComments(block.LeadingComments),
				TrailingComments = block.TrailingComments.ToList(),
			};

		// example: try { ... } catch (Ex) { ... } finally { }
		protected internal virtual Parser<TryStatementSyntax> TryCatchFinallyStatement =>
			from @try in Parse.IgnoreCase(PhpKeywords.Try).Commented(this)
			from block in Block
			from catchClauses in CatchClause.Many()
			from @finally in FinallyClause.Optional()
			where @finally.IsDefined || catchClauses.Any()
			select new TryStatementSyntax
			{
				LeadingComments = @try.LeadingComments.ToList(),
				Block = block,
				Catches = catchClauses.ToList(),
				Finally = @finally.GetOrDefault(),
			};

		// example: System.runAs(user) { System.debug('Hi there!'); }
		protected internal virtual Parser<RunAsStatementSyntax> RunAsStatement =>
			from system in Parse.IgnoreCase(PhpKeywords.System).Token()
			from dot in Parse.Char('.').Token()
			from runAs in Parse.IgnoreCase(PhpKeywords.RunAs).Token()
			from expression in GenericExpressionInBraces()
			from statement in Statement
			select new RunAsStatementSyntax
			{
				Expression = new ExpressionSyntax(expression),
				Statement = statement,
			};

		// dummy generic parser for any unknown statement ending with a semicolon
		protected internal virtual Parser<StatementSyntax> UnknownGenericStatement =>
			from contents in GenericExpressionCore(forbidden: ";").Token()
			from semicolon in Parse.Char(';')
			select new StatementSyntax
			{
				Body = contents.Trim(),
			};

		// examples: 'hello', '\'world\'\n'
		protected internal virtual Parser<string> StringLiteral =>
			from leading in Parse.WhiteSpace.Many()
			from openQuote in Parse.Char('\'')
			from fragments in Parse.Char('\\').Then(_ => Parse.AnyChar.Select(c => $"\\{c}"))
			                       .Or(Parse.CharExcept("\\'").Many().Text()).Many()
			from closeQuote in Parse.Char('\'')
			from trailing in Parse.WhiteSpace.Many()
			select $"'{string.Join(string.Empty, fragments)}'";

		// dummy generic parser for expressions with matching braces
		protected internal virtual Parser<string> GenericExpression =>
			GenericExpressionCore(forbidden: ",;").Select(x => x.Trim());

		// creates dummy generic parser for expressions with matching braces allowing commas and semicolons by default
		protected internal virtual Parser<string> GenericExpressionCore(string forbidden = null, bool allowCurlyBraces = true)
		{
			var subExpressionParser = GenericNewExpression.Select(x => $" {x}")
			                                              .Or(
				                                              Parse.CharExcept("'/(){}[]" + forbidden)
				                                                   .Except(GenericNewExpression).Many().Text().Token())
			                                              .Or(
				                                              Parse.Char('/').Then(_ => Parse.Not(Parse.Chars('/', '*'))).Once()
				                                                   .Return("/"))
			                                              .Or(CommentParser.AnyComment.Return(string.Empty))
			                                              .Or(StringLiteral)
			                                              .Or(GenericExpressionInBraces().Select(x => $"({x})"))
			                                              .Or(GenericExpressionInBraces('[', ']').Select(x => $"[{x}]"));

			// optionally include support for curly braces
			if (allowCurlyBraces)
			{
				subExpressionParser = subExpressionParser
					.Or(GenericExpressionInBraces('{', '}').Select(x => $"{{{x}}}"));
			}

			return
				from subExpressions in subExpressionParser.Many()
				let expr = string.Join(string.Empty, subExpressions)
				where !string.IsNullOrWhiteSpace(expr)
				select expr;
		}

		// examples: new Map<string, string>
		protected internal virtual Parser<string> GenericNewExpression =>
			from prev in Toolbox.ParserExtensions.PrevChar(c => !char.IsLetterOrDigit(c), "non-alphanumeric")
			from @new in Parse.IgnoreCase(PhpKeywords.New).Then(_ => Parse.Not(Parse.LetterOrDigit)).Token()
			from type in TypeReference.Token()
			select $"new {type.AsString()}";

		// creates dummy generic parser for any expressions with matching braces
		protected internal virtual Parser<string> GenericExpressionInBraces(char open = '(', char close = ')') =>
			from openBrace in Parse.Char(open).Token()
			from expression in GenericExpressionCore().Optional()
			from closeBrace in Parse.Char(close).Token()
			select expression.GetOrElse(string.Empty).Trim();

		// example: 1.23
		protected internal virtual Parser<LiteralExpressionSyntax> DecimalLiteralExpression =>
			from token in Parse.DecimalInvariant
			select new LiteralExpressionSyntax(token, LiteralType.Numeric);

		// example: 'hello'
		protected internal virtual Parser<LiteralExpressionSyntax> StringLiteralExpression =>
			from token in StringLiteral
			select new LiteralExpressionSyntax(token, LiteralType.String);

		// example: true, false
		protected internal virtual Parser<LiteralExpressionSyntax> BooleanLiteralExpression =>
			from token in Keyword(PhpKeywords.False).Or(Keyword(PhpKeywords.True))
			select new LiteralExpressionSyntax(token, LiteralType.Boolean);

		// example: null
		protected internal virtual Parser<LiteralExpressionSyntax> NullLiteralExpression =>
			from token in Keyword(PhpKeywords.Null)
			select new LiteralExpressionSyntax(token);

		// examples: 1, true, 'hello'
		protected internal virtual Parser<LiteralExpressionSyntax> LiteralExpression =>
			from expr in DecimalLiteralExpression.XOr(StringLiteralExpression).XOr(NullLiteralExpression)
			                                     .XOr(BooleanLiteralExpression).Commented(this)
			select expr.Value
			           .WithLeadingComments(expr.LeadingComments)
			           .WithTrailingComments(expr.TrailingComments);

		// examples: (1+2*4), 'hello', (true == false ? 1 : 2), null
		protected internal virtual Parser<ExpressionSyntax> FactorExpression =>
			GenericExpressionInBraces().Select(expr => new ExpressionSyntax("(" + expr + ")"))
			                           .XOr(LiteralExpression);

		// example: break;
		protected internal virtual Parser<BreakStatementSyntax> BreakStatement =>
			from @break in Keyword(PhpKeywords.Break).Token()
			from semicolon in Parse.Char(';')
			select new BreakStatementSyntax();

		// example: continue;
		protected internal virtual Parser<ContinueStatementSyntax> ContinueStatement =>
			from @continue in Keyword(PhpKeywords.Continue).Token()
			from semicolon in Parse.Char(';')
			select new ContinueStatementSyntax();

		// simple if statement without the expressions support
		protected internal virtual Parser<IfStatementSyntax> IfStatement =>
			from ifKeyword in Keyword(PhpKeywords.If).Token()
			from expression in GenericExpressionInBraces()
			from thenBranch in Statement
			from elseBranch in Keyword(PhpKeywords.Else).Token(this).Then(_ => Statement).Optional()
			select new IfStatementSyntax
			{
				Expression = new ExpressionSyntax(expression),
				ThenStatement = thenBranch,
				ElseStatement = elseBranch.GetOrDefault(),
			};

		// simple foreach statement without the expression support
		protected internal virtual Parser<ForEachStatementSyntax> ForEachStatement =>
			from forKeyword in Parse.IgnoreCase(PhpKeywords.For).Token()
			from openBrace in Parse.Char('(').Token()
			from typeReference in TypeReference
			from identifier in Identifier
			from colon in Parse.Char(':').Token()
			from expression in GenericExpression
			from closeBrace in Parse.Char(')').Token()
			from loopBody in Statement
			select new ForEachStatementSyntax
			{
				Type = typeReference,
				Identifier = identifier,
				Expression = new ExpressionSyntax(expression),
				Statement = loopBody,
			};

		// simple for statement without the expression support
		protected internal virtual Parser<ForStatementSyntax> ForStatement =>
			from forKeyword in Parse.IgnoreCase(PhpKeywords.For).Token()
			from openBrace in Parse.Char('(').Token()
			from declaration in VariableDeclaration.Or(Parse.Char(';').Token().Return(default(VariableDeclarationSyntax)))
			from condition in GenericExpression.Optional()
			from semicolon in Parse.Char(';').Token()
			from increment in GenericExpression.DelimitedBy(Parse.Char(',').Token()).Optional()
			from closeBrace in Parse.Char(')').Token()
			from loopBody in Statement
			select new ForStatementSyntax
			{
				Declaration = declaration,
				Condition = ExpressionSyntax.CreateOrDefault(condition),
				Incrementors = increment.GetOrElse(new string[0]).Select(s => new ExpressionSyntax(s)).ToList(),
				Statement = loopBody,
			};

		// simple do-while statement without the expression support
		protected internal virtual Parser<DoStatementSyntax> DoStatement =>
			from doKeyword in Parse.IgnoreCase(PhpKeywords.Do).Token()
			from loopBody in Statement
			from whileKeyword in Parse.IgnoreCase(PhpKeywords.While).Token()
			from expression in GenericExpressionInBraces()
			from semicolon in Parse.Char(';')
			select new DoStatementSyntax
			{
				Expression = new ExpressionSyntax(expression),
				Statement = loopBody,
			};

		// simple while statement without the expression support
		protected internal virtual Parser<WhileStatementSyntax> WhileStatement =>
			from whileKeyword in Parse.IgnoreCase(PhpKeywords.While).Token()
			from expression in GenericExpressionInBraces()
			from loopBody in Statement
			select new WhileStatementSyntax
			{
				Expression = new ExpressionSyntax(expression),
				Statement = loopBody,
			};

		// example: switch on x { when 1 { return 0; } when else { return 1; } }
		protected internal virtual Parser<SwitchStatementSyntax> SwitchStatement =>
			from switchKeyword in Keyword(PhpKeywords.Switch).Token()
			from onKeyword in Keyword(PhpKeywords.On).Token()
			from expression in SwitchExpression
			from open in Parse.Char('{').Commented(this).Token()
			from whenCommented in WhenClause.Commented(this).Many()
			let whenClauses =
				from w in whenCommented
				select w.Value
				        .WithLeadingComments(w.LeadingComments)
				        .WithTrailingComments(w.TrailingComments)
			from close in Parse.Char('}').Token()
			select new SwitchStatementSyntax
			{
				Expression = new ExpressionSyntax(expression),
				WhenClauses = whenClauses.ToList(),
			};

		// examples: 1, 3+2, 'two', Identifier, etc. — can't have curly braces inside it
		protected internal virtual Parser<string> SwitchExpression =>
			GenericExpressionCore(forbidden: ",;", allowCurlyBraces: false).Select(x => x.Trim());

		// any acceptable when clause
		protected internal virtual Parser<WhenClauseSyntax> WhenClause =>
			WhenElseClause.Select(w => w as WhenClauseSyntax)
			              .Or(WhenTypeClause)
			              .Or(WhenExpressionsClause);

		// example: 1, 2, 3, 'one', 'two', SUNDAY, MONDAY
		protected internal virtual Parser<IEnumerable<ExpressionSyntax>> WhenExpressions =>
			from expr in SwitchExpression.DelimitedBy(Parse.Char(',').Token())
			select expr.Select(x => new ExpressionSyntax(x));

		// examples: when 1, 2, 3 { ... }
		protected internal virtual Parser<WhenExpressionsClauseSyntax> WhenExpressionsClause =>
			from whenKeyword in Keyword(PhpKeywords.When).Token()
			from expressions in WhenExpressions
			from block in Block
			select new WhenExpressionsClauseSyntax
			{
				Expressions = expressions.ToList(),
				Block = block,
			};

		// examples: when Contract c { ... }
		protected internal virtual Parser<WhenTypeClauseSyntax> WhenTypeClause =>
			from whenKeyword in Keyword(PhpKeywords.When).Token()
			from type in TypeReference
			from name in Identifier
			from block in Block
			select new WhenTypeClauseSyntax
			{
				Type = type,
				Identifier = name,
				Block = block,
			};

		// examples: when else { ... }
		protected internal virtual Parser<WhenElseClauseSyntax> WhenElseClause =>
			from whenKeyword in Keyword(PhpKeywords.When).Token()
			from elseKeyword in Keyword(PhpKeywords.Else).Token()
			from blockStatement in Block
			select new WhenElseClauseSyntax
			{
				Block = blockStatement,
			};

		// examples: return x; insert y; delete z;
		protected internal virtual Parser<string> KeywordExpressionStatement(string keyword) =>
			from key in Keyword(keyword).Token()
			from expr in GenericExpression.XOptional()
			from semicolon in Parse.Char(';')
			select expr.GetOrDefault();

		// example: return null;
		protected internal virtual Parser<ReturnStatementSyntax> ReturnStatement =>
			from expression in KeywordExpressionStatement(PhpKeywords.Return)
			select new ReturnStatementSyntax
			{
				Expression = expression == null ? null : new ExpressionSyntax(expression),
			};

		// examples: throw new Exception(); throw;
		protected internal virtual Parser<ThrowStatementSyntax> ThrowStatement =>
			from expression in KeywordExpressionStatement(PhpKeywords.Throw)
			select new ThrowStatementSyntax
			{
				Expression = expression == null ? null : new ExpressionSyntax(expression),
			};

		// example: insert contact;
		protected internal virtual Parser<InsertStatementSyntax> InsertStatement =>
			from expression in KeywordExpressionStatement(PhpKeywords.Insert)
			where !string.IsNullOrWhiteSpace(expression)
			select new InsertStatementSyntax
			{
				Expression = new ExpressionSyntax(expression),
			};

		// example: update items;
		protected internal virtual Parser<UpdateStatementSyntax> UpdateStatement =>
			from expression in KeywordExpressionStatement(PhpKeywords.Update)
			where !string.IsNullOrWhiteSpace(expression)
			select new UpdateStatementSyntax
			{
				Expression = new ExpressionSyntax(expression),
			};

		// example: upsert items;
		protected internal virtual Parser<UpsertStatementSyntax> UpsertStatement =>
			from expression in KeywordExpressionStatement(PhpKeywords.Upsert)
			where !string.IsNullOrWhiteSpace(expression)
			select new UpsertStatementSyntax
			{
				Expression = new ExpressionSyntax(expression),
			};

		// example: delete user;
		protected internal virtual Parser<DeleteStatementSyntax> DeleteStatement =>
			from expression in KeywordExpressionStatement(PhpKeywords.Delete)
			where !string.IsNullOrWhiteSpace(expression)
			select new DeleteStatementSyntax
			{
				Expression = new ExpressionSyntax(expression),
			};

		// examples: /* this is a member */ @isTest public
		protected internal virtual Parser<MemberDeclarationSyntax> MemberDeclarationHeading =>
			from comments in CommentParser.AnyComment.Token().Many()
			from annotations in Annotation.Many()
			from accessModifier in AccessModifier.Optional()
			from modifiers in Modifier.Many()
			select new MemberDeclarationSyntax
			{
				LeadingComments = comments.ToList(),
				Annotations = annotations.ToList(),
				Modifiers = modifiers.ToList(),
				AccessModifier = accessModifier
			};

		// example: SomeValue
		protected internal virtual Parser<EnumMemberDeclarationSyntax> EnumMember =>
			from heading in MemberDeclarationHeading
			from identifier in RawIdentifier.Commented(this)
			select new EnumMemberDeclarationSyntax(heading)
			{
				Identifier = identifier.Value,
				TrailingComments = identifier.TrailingComments.ToList(),
			};

		// example: public enum Weekday { Monday, Thursday }
		protected internal virtual Parser<EnumDeclarationSyntax> EnumDeclaration =>
			from heading in MemberDeclarationHeading
			from @enum in EnumDeclarationBody
			select new EnumDeclarationSyntax(heading)
			{
				Identifier = @enum.Identifier,
				Members = @enum.Members,
			};

		// example: enum Weekday { Monday, Thursday }
		protected internal virtual Parser<EnumDeclarationSyntax> EnumDeclarationBody =>
			from @enum in Parse.IgnoreCase(PhpKeywords.Enum).Token()
			from identifier in Identifier
			from skippedComments in CommentParser.AnyComment.Token().Many()
			from openBrace in Parse.Char('{').Token()
			from members in EnumMember.DelimitedBy(Parse.Char(',').Commented(this))
			from closeBrace in Parse.Char('}').Commented(this)
			select new EnumDeclarationSyntax
			{
				Identifier = identifier,
				Members = members.ToList(),
				InnerComments = closeBrace.LeadingComments.ToList(),
				TrailingComments = closeBrace.TrailingComments.ToList(),
			};

		// example: @TestFixture public static class Program { static void main() {} }
		public virtual Parser<ClassDeclarationSyntax> ClassDeclaration =>
			from heading in MemberDeclarationHeading
			from classBody in ClassDeclarationBody
			select ClassDeclarationSyntax.Create(heading, classBody);

		// example: class Program { void main() {} }
		protected internal virtual Parser<ClassDeclarationSyntax> ClassDeclarationBody =>
			from @class in Parse.IgnoreCase(PhpKeywords.Class).Text().Token()
			                    .Or(Parse.IgnoreCase(PhpKeywords.Interface).Text().Token())
			from className in Identifier
			from baseType in Parse.IgnoreCase(PhpKeywords.Extends).Token().Then(_ => TypeReference).Optional()
			from interfaces in Parse.IgnoreCase(PhpKeywords.Implements).Token()
			                        .Then(_ => TypeReference.DelimitedBy(Parse.Char(',').Token())).Optional()
			from skippedComments in CommentParser.AnyComment.Token().Many()
			from openBrace in Parse.Char('{').Token()
			from members in ClassMemberDeclaration.Many()
			from closeBrace in Parse.Char('}').Commented(this)
			let classBody = new ClassDeclarationSyntax()
			{
				Identifier = className,
				IsInterface = @class == PhpKeywords.Interface,
				BaseType = baseType.GetOrDefault(),
				Interfaces = interfaces.GetOrElse(Enumerable.Empty<TypeSyntax>()).ToList(),
				Members = ConvertConstructors(members, className).ToList(),
				InnerComments = closeBrace.LeadingComments.ToList(),
				TrailingComments = closeBrace.TrailingComments.ToList(),
			}
			select ClassDeclarationSyntax.Create(null, classBody);

		private IEnumerable<MemberDeclarationSyntax> ConvertConstructors(
			IEnumerable<MemberDeclarationSyntax> members,
			string className)
		{
			foreach (var member in members)
			{
				if (member is MethodDeclarationSyntax m && m.IsConstructor(className))
				{
					yield return new ConstructorDeclarationSyntax(m);
					continue;
				}

				yield return member;
			}
		}

		// examples: { instanceProperty = 0; }, static { staticProperty = 0; }
		protected internal virtual Parser<ClassInitializerSyntax> ClassInitializer =>
			from heading in MemberDeclarationHeading
			from initializer in ClassInitializerBody
			select initializer.WithProperties(heading);

		// examples: { a = 0; }
		protected internal virtual Parser<ClassInitializerSyntax> ClassInitializerBody =>
			from body in Block
			select new ClassInitializerSyntax
			{
				Body = body,
			};

		// method or property declaration starting with the type and name
		protected internal virtual Parser<MemberDeclarationSyntax> MethodOrPropertyDeclaration =>
			from name in Identifier
			from member in MethodParametersAndBody.Select(c => c as MemberDeclarationSyntax)
			select member.WithTypeAndName(new("object", name));

		// class members: methods, classes, properties
		protected internal virtual Parser<MemberDeclarationSyntax> ClassMemberDeclaration =>
			from heading in MemberDeclarationHeading
			from member in ClassInitializerBody.Select(c => c as MemberDeclarationSyntax)
			                                   .Or(EnumDeclarationBody)
			                                   .Or(ClassDeclarationBody)
			                                   .Or(MethodDeclaration)
			                                   .Or(FieldDeclaration)
			select member.WithProperties(heading);

		protected internal virtual Parser<bool> LeadingUnitSignature =>
			from sign in Parse.String("<?php").Token()
			select true;

		protected internal virtual Parser<bool> TrailingUnitSignature =>
			from sign in Parse.String("?>").Token()
			select true;

		public virtual Parser<MemberDeclarationSyntax> MemberDeclaration =>
			from member in ClassDeclaration.Select(c => c as MemberDeclarationSyntax)
			                               .Or(EnumDeclaration)
			from whiteSpace in Parse.WhiteSpace.Many()
			from trailingComments in CommentParser.AnyComment.Token().Many()
			select member.WithTrailingComments(trailingComments);

		// top-level declaration: a class or an enum followed by the end of file
		public virtual Parser<CompilationUnitSyntax> CompilationUnit =>
			from header in LeadingUnitSignature
			from members in MemberDeclaration.Many()
			from trailingComments in CommentParser.AnyComment.Token().Many()
			from footer in TrailingUnitSignature.End()
			select new CompilationUnitSyntax(members).WithTrailingComments(trailingComments);
	}
}