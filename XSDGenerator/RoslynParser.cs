using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace XSDGenerator;

public static class RoslynParser
{
	public static AttributeSyntax ParseAttribute(string name)
	{
		return SyntaxFactory
			.Attribute(SyntaxFactory
				.IdentifierName(name), SyntaxFactory
					.AttributeArgumentList());
	}

	public static AttributeSyntax ParseAttribute(string name, params object[] parameters)
	{
		var attributeArgumentList = SyntaxFactory
				.AttributeArgumentList(SyntaxFactory
					.SeparatedList(parameters.Select(s =>
					{
						return SyntaxFactory.AttributeArgument(s switch
						{
							string stringValue => SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(stringValue)),
							int intValue => SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(intValue)),
							char charValue => SyntaxFactory.LiteralExpression(SyntaxKind.CharacterLiteralExpression, SyntaxFactory.Literal(charValue)),
						});
					})));

		return SyntaxFactory
			.Attribute(SyntaxFactory
				.IdentifierName(name), attributeArgumentList);
	}

	public static PropertyDeclarationSyntax ParseProperty(string name, string type, bool createGet, bool CreateSet, string defaultValue, params AttributeSyntax[] attributes)
	{
		var result = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(type), name)
			.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
			.AddAttributeLists(SyntaxFactory
					.AttributeList(SyntaxFactory
						.SeparatedList(attributes)));

		if (createGet)
		{
			result = result.AddAccessorListAccessors(SyntaxFactory
				.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
				.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));
		}

		if (CreateSet)
		{
			result = result.AddAccessorListAccessors(SyntaxFactory
				.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
				.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));
		}

		if (!String.IsNullOrWhiteSpace(defaultValue))
		{

		}

		return result;
	}
}
