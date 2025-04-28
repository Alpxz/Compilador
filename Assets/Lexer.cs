using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;

public class Lexer
{
    private string input;
    private int position = 0;

    private static readonly string[] keywords = { "if", "else", "while", "return" };

    public Lexer(string input)
    {
        this.input = input;
    }

    public List<Token> Tokenize()
    {
        List<Token> tokens = new List<Token>();

        while (position < input.Length)
        {
            char current = input[position];

            if (char.IsWhiteSpace(current))
            {
                position++;
            }
            else if (char.IsLetter(current))
            {
                string word = ReadWord();
                if (keywords.Contains(word))
                    tokens.Add(new Token(TokenType.Keyword, word));
                else
                    tokens.Add(new Token(TokenType.Identifier, word));
            }
            else if (char.IsDigit(current))
            {
                tokens.Add(new Token(TokenType.Number, ReadNumber()));
            }
            else if ("+-*/=<>".Contains(current))
            {
                tokens.Add(new Token(TokenType.Operator, current.ToString()));
                position++;
            }
            else
            {
                tokens.Add(new Token(TokenType.Unknown, current.ToString()));
                position++;
            }
        }

        return tokens;
    }

    private string ReadWord()
    {
        int start = position;
        while (position < input.Length && (char.IsLetterOrDigit(input[position]) || input[position] == '_'))
        {
            position++;
        }
        return input.Substring(start, position - start);
    }

    private string ReadNumber()
    {
        int start = position;
        while (position < input.Length && char.IsDigit(input[position]))
        {
            position++;
        }
        return input.Substring(start, position - start);
    }
}
