using System.Collections.Generic;

public class Parser
{
    private List<Token> tokens;
    private int position = 0;

    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    public bool ParseProgram()
    {
        while (position < tokens.Count)
        {
            if (!ParseAssignment())
            {
                return false;
            }
        }
        return true;
    }

    private bool ParseAssignment()
    {
        if (!Match(TokenType.Identifier))
            return false;
        if (!Match(TokenType.Operator, "="))
            return false;
        if (!ParseExpression())
            return false;
        return true;
    }

    private bool ParseExpression()
    {
        if (!Match(TokenType.Number) && !Match(TokenType.Identifier))
            return false;

        while (CurrentIsOperator("+", "-", "*", "/"))
        {
            position++; // Consume operador
            if (!Match(TokenType.Number) && !Match(TokenType.Identifier))
                return false;
        }
        return true;
    }

    private bool Match(TokenType expectedType, string expectedValue = null)
    {
        if (position < tokens.Count)
        {
            Token token = tokens[position];
            if (token.Type == expectedType && (expectedValue == null || token.Value == expectedValue))
            {
                position++;
                return true;
            }
        }
        return false;
    }

    private bool CurrentIsOperator(params string[] ops)
    {
        if (position < tokens.Count && tokens[position].Type == TokenType.Operator)
        {
            foreach (var op in ops)
            {
                if (tokens[position].Value == op)
                    return true;
            }
        }
        return false;
    }
}