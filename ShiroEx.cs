using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public static class ShiroEx
{
    #region RemoveTags //Remove all tags
    /*Removes all tags from the string that is passed into it*/
    public static string ToShiroNone(this string input)
    {
        return Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace
        (input, @"\<color\=.*?\>|<\/color\>", "\0"), @"\[\w{6}\]|\[\-\]", "\0"), @"\<b\>|\<\/b\>", "\0"), @"\<i\>|\<\/i\>", "\0");
    }
    #endregion RemoveTags //End

    #region RemoveNonAlphaNumericCharacters //Remove unknown symbols from names
    /*Removes all nonalphanumeric characters, (symbols not on the keyboard)*/
    public static string ToShiroNonAlphanumeric(this string input)
    {
        return Regex.Replace(input, @"[^0-9a-zA-Z -:\/\\\\@\#\~\=\+\\)(\""\£\$\%\^\&\*\[\]\{\}\<\>]", "\0");
    }
    #endregion RemoveNonAlphaNumericCharacters //End

    #region RepairCodes //Add Tags to fix <color> tags
    /*Fixes broken color codes to avoid spam*/
    public static string ToShiroFixColors(this string inputString)
    {
        return inputString += Enumerable.Repeat("</color>", 
        Regex.Match(inputString, @"\<color=.*?\>").Length - Regex.Match(inputString, @"\<\/color\>").Length);
    }
    #endregion RepairCodes //End

    #region ConvertHTMLtoCSS //Convert [000000] -> <color>
    /*Converts [000000] tags to <color>*/
    public static string ToShiroConvertHTML(this string input)
    {
        return Regex.Replace(Regex.Replace(Regex.Replace(input, @"\<color\=\#+", "["), @"\>+", "]"), @"\<\/color\>", "[-]"); 
    }
    #endregion ConvertHTMLtoCSS

    #region ToPhotonPlayer //Convert int to photonplayer
    public static PhotonPlayer ToShiroPlayer(this int input)
    {
        return PhotonPlayer.Find(input);
    }
    #endregion ToPhotonPlayer //End

    #region ToIntegerArray //Regex via
    /*Finds numbers in the string then converts them. Returns as int array*/
    public static int[] ToShiroIntArray(this string input)
    {
        List<int> Ints = new List<int>();
        GroupCollection ints = Regex.Match(input, @"\d+").Groups;
        foreach (Group intse in ints)
        {
            Ints.Add(Convert.ToInt32(intse.Value));
        }
        return Ints.ToArray();
    }
    #endregion //End
}
