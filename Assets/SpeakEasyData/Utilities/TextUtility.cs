public static class TextUtility
{
    public static bool IsWhitespace(this char character)
    {
        switch (character)
        {
            case '\u0020':
            case '\u00A0':
            case '\u1680':
            case '\u2000':
            case '\u2001':
            case '\u2002':
            case '\u2003':
            case '\u2004':
            case '\u2005':
            case '\u2006':
            case '\u2007':
            case '\u2008':
            case '\u2009':
            case '\u200A':
            case '\u202F':
            case '\u205F':
            case '\u3000':
            case '\u2028':
            case '\u2029':
            case '\u0009':
            case '\u000A':
            case '\u000B':
            case '\u000C':
            case '\u000D':
            case '\u0085':
            {
                return true;
            }
 
            default:
            {
                return false;
            }
        }
    }
 
    // https://stackoverflow.com/a/37368176
    public static string RemoveWhitespaces(this string text)
    {
        int textLength = text.Length;
 
        char[] textCharacters = text.ToCharArray();
 
        int currentWhitespacelessTextLength = 0;
 
        for (int currentCharacterIndex = 0; currentCharacterIndex < textLength; ++currentCharacterIndex)
        {
            char currentTextCharacter = textCharacters[currentCharacterIndex];
 
            if (currentTextCharacter.IsWhitespace())
            {
                continue;
            }
 
            textCharacters[currentWhitespacelessTextLength++] = currentTextCharacter;
        }
 
        return new string(textCharacters, 0, currentWhitespacelessTextLength);
    }
 
    // https://stackoverflow.com/questions/3210393/how-do-i-remove-all-non-alphanumeric-characters-from-a-string-except-dash
    public static string RemoveSpecialCharacters(this string text)
    {
        int textLength = text.Length;
 
        char[] textCharacters = text.ToCharArray();
 
        int currentWhitespacelessTextLength = 0;
 
        for (int currentCharacterIndex = 0; currentCharacterIndex < textLength; ++currentCharacterIndex)
        {
            char currentTextCharacter = textCharacters[currentCharacterIndex];
 
            if (!char.IsLetterOrDigit(currentTextCharacter) && !currentTextCharacter.IsWhitespace() && currentTextCharacter != '_')
            {
                continue;
            }
 
            textCharacters[currentWhitespacelessTextLength++] = currentTextCharacter;
        }
 
        return new string(textCharacters, 0, currentWhitespacelessTextLength);
    }

    public static string GetSubstringBefore(this string text, string stopAt = "_")
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            int charLocation = text.IndexOf(stopAt, System.StringComparison.Ordinal);

            if (charLocation > 0)
            {
                return text.Substring(0, charLocation);
            }
        }

        return string.Empty;
    }
}