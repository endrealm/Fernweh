﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using PipelineExtensionLibrary.Chat;
using PipelineExtensionLibrary.Tokenizer;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace PipelineExtensionLibrary;

public class XmlDialogParser
{
    private readonly List<TokenDefinition> _tokenDefinitions;

    public XmlDialogParser()
    {
        _tokenDefinitions = new()
        {
            new TokenDefinition(TokenType.Text, "[^</>]+", 1),
            new TokenDefinition(TokenType.ColorStart, "<color(=\"[^\"]+\")?>", 1),
            new TokenDefinition(TokenType.ColorEnd, "</color>", 1),
        };
    }
    
    public IChatComponentData Parse(string value)
    {
        var stack = new Stack<ChatWrapper>();
        stack.Push(new ChatWrapper());
        foreach (var tokenValue in Tokenize(value))
        {
            if (tokenValue.Type == TokenType.ColorStart)
            {
                var parent = stack.Peek();
                var color = Color.White;
                var pieces = tokenValue.Value.Split('"');
                if (pieces.Length == 3)
                {
                    var rawColor = pieces[1];
                    color = rawColor.ToColor();
                }
                stack.Push(new ColorWrapper(color));
                parent.AddChild(stack.Peek());
                continue;
            }
            if (tokenValue.Type == TokenType.ColorEnd)
            {
                if(stack.Count == 1) continue;
                stack.Pop();
                continue;
            }
            if (tokenValue.Type == TokenType.Text)
            {
                stack.Peek().AddChild(new TextWrapper(tokenValue.Value));
                continue;
            }
        }

        ChatWrapper root;
        do
        {
            root = stack.Pop();
        }
        while (stack.Count > 0);

        var data = root.Flatten(new MergeResult
        {
            Color = Color.White
        }).Select(result => new ChatTextData(result.Color, result.Text)).ToList<IChatComponentData>();
        
        return new ChatCompoundData(data);
    }
    
    private IEnumerable<TokenValue> Tokenize(string message)
    {
        var tokenMatches = FindTokenMatches(message);

        var groupedByIndex = tokenMatches.GroupBy(x => x.StartIndex)
            .OrderBy(x => x.Key)
            .ToList();

        TokenMatch lastMatch = null;
        for (int i = 0; i < groupedByIndex.Count; i++)
        {
            var bestMatch = groupedByIndex[i].OrderBy(x => x.Precedence).First();
            if (lastMatch != null && bestMatch.StartIndex < lastMatch.EndIndex)
                continue;

            yield return new TokenValue(bestMatch.TokenType, bestMatch.Value);

            lastMatch = bestMatch;
        }

    }

    private List<TokenMatch> FindTokenMatches(string errorMessage)
    {
        var tokenMatches = new List<TokenMatch>();

        foreach (var tokenDefinition in _tokenDefinitions)
            tokenMatches.AddRange(tokenDefinition.FindMatches(errorMessage).ToList());

        return tokenMatches;
    }
}

internal class TokenValue
{
    public TokenType Type { get; }
    public string Value { get; }

    public TokenValue(TokenType type, string value = "")
    {
        Type = type;
        Value = value;
    }
}