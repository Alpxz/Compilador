using System.Collections.Generic;

public class SemanticAnalyzer
{
    private Dictionary<string, string> symbolTable = new Dictionary<string, string>();

    public void DeclareVariable(string name, string type)
    {
        symbolTable[name] = type;
    }

    public bool CheckVariableExists(string name)
    {
        return symbolTable.ContainsKey(name);
    }

    public bool CheckType(string name, string expectedType)
    {
        return symbolTable.ContainsKey(name) && symbolTable[name] == expectedType;
    }
}
