using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace PipelineExtensionLibrary.Tokenizer;

public class TokenDefinition
{
    private Regex _regex;
    private readonly TokenType _returnsToken;
    private readonly int _precedence;

    public TokenDefinition(TokenType returnsToken, [RegexPattern] string regexPattern, int precedence)
    {
        _regex = new Regex(regexPattern, RegexOptions.IgnoreCase|RegexOptions.Compiled);
        _returnsToken = returnsToken;
        _precedence = precedence;
    }

    public IEnumerable<TokenMatch> FindMatches(string inputString)
    {
        var matches = _regex.Matches(inputString);
        for(int i=0; i<matches.Count; i++)
        {
            yield return new TokenMatch()
            {
                StartIndex = matches[i].Index,
                EndIndex = matches[i].Index + matches[i].Length,
                TokenType = _returnsToken,
                Value = matches[i].Value,
                Precedence = _precedence
            };
        }
    }
}

public class TokenMatch
{
    public TokenType TokenType { get; set; }
    public string Value { get; set; }
    public int StartIndex { get; set; }
    public int EndIndex { get; set; }
    public int Precedence { get; set; }
}