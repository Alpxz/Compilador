using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CompilerManager : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Text lexerOutputText;
    public TMP_Text parserOutputText;
    public TMP_Text semanticOutputText;

    private SemanticAnalyzer semanticAnalyzer = new SemanticAnalyzer();

    public void AnalyzeCode()
    {
        string code = inputField.text;

        // Fase l�xica
        Lexer lexer = new Lexer(code);
        List<Token> tokens = lexer.Tokenize();
        DisplayLexerOutput(tokens);

        // Verificar errores l�xicos
        foreach (var token in tokens)
        {
            if (token.Type == TokenType.Unknown)
            {
                ShowParserError("Error l�xico: s�mbolo desconocido '" + token.Value + "'");
                semanticOutputText.text = "";
                return;
            }
        }

        // Fase sint�ctica
        Parser parser = new Parser(tokens);
        bool syntaxValid = parser.ParseProgram();
        if (!syntaxValid)
        {
            ShowParserError("Error de sintaxis.");
            semanticOutputText.text = "";
            return;
        }
        else
        {
            ShowParserSuccess("Sintaxis v�lida.");
        }

        // Fase sem�ntica
        AnalyzeSemantics(tokens);
    }

    private void DisplayLexerOutput(List<Token> tokens)
    {
        string output = "Tokens reconocidos:\n";
        foreach (var token in tokens)
        {
            output += $"- {token.Type}: '{token.Value}'\n";
        }
        lexerOutputText.text = output;
        lexerOutputText.color = Color.white;
    }

    private void ShowParserError(string message)
    {
        parserOutputText.text = message;
        parserOutputText.color = Color.red;
    }

    private void ShowParserSuccess(string message)
    {
        parserOutputText.text = message;
        parserOutputText.color = Color.green;
    }

    private void AnalyzeSemantics(List<Token> tokens)
    {
        semanticAnalyzer = new SemanticAnalyzer(); // Resetear variables

        List<string> declaredVariables = new List<string>();
        int position = 0;
        while (position < tokens.Count)
        {
            if (tokens[position].Type == TokenType.Identifier)
            {
                string varName = tokens[position].Value;
                semanticAnalyzer.DeclareVariable(varName, "int");
                declaredVariables.Add(varName);
                position += 3; // saltar IDENTIFICADOR = EXPRESI�N
            }
            else
            {
                position++;
            }
        }

        if (declaredVariables.Count > 0)
        {
            string output = "Variables declaradas:\n";
            foreach (var varName in declaredVariables)
            {
                output += $"- {varName}\n";
            }
            semanticOutputText.text = output;
            semanticOutputText.color = Color.green;
        }
        else
        {
            semanticOutputText.text = "No se declararon variables.";
            semanticOutputText.color = Color.yellow;
        }
    }
}