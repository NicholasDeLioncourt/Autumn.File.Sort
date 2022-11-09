namespace Autumn.File
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    //=============================================================================================
    /// <summary>System.String extension methods.</summary>
    /// <created>L. Nicholas de Lioncourt</created>
    /// <remarks>
    /// I did not include extensions such as IsEmail, IsUri, ToJSON etc since these are better suited 
    /// to specific implementations; otherwise the method list for System.String would be overwhelming.
    /// </remarks>
    //=====================================================================================LNDL====
    public static class StringExtension
    {
        /// <summary>The point at which a StringBuilder must be used.</summary>
        private const int __stringbuilderlimit = 0x000010;

        #region COMPARE AND EQUALS CULTURE AWARE
        //=========================================================================================
        /// <summary>
        /// Compares two strings; if <paramref name="ignorecase"/> is <c>true</c> (the default), uses  
        /// <see cref="StringComparison.CurrentCultureIgnoreCase"/>; otherwise,
        /// <see cref="StringComparison.CurrentCulture"/>.
        /// </summary>
        //=========================================================================================
        [DebuggerStepThrough]
        public static int CompareCurrentCulture(this string first, string second, bool ignorecase = true)
        {
            if(first == second) { return(0); }
            if(null == first)   { return(-1); }
            if(null == second)  { return(1); }
            return(String.Compare(first, second, ignorecase? 
                   StringComparison.CurrentCultureIgnoreCase :  
                   StringComparison.CurrentCulture));
        }

        //=========================================================================================
        /// <summary>
        /// Compares substrings of two specified strings and returns an integer that indicates their 
        /// relative position in the sort order. This is a case-sensitive comparison using the current 
        /// culture to obtain culture-specific information such as casing rules and the alphabetic 
        /// order of individual characters. Neither the indices nor length may be less than 0.
        /// </summary>
        /// <param name="first">The first string to use in the comparison.</param>
        /// <param name="firstindex">The starting index of the substring in the first string.</param>
        /// <param name="second">The second string to use in the comparison.</param>
        /// <param name="secondindex">The starting index of the substring in the second string.</param>
        /// <param name="length">The maximum number of characters in the substrings to compare.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static int CompareCurrentCulture(this String first, Int32 firstindex, String second, Int32 secondindex, Int32 length)
        {
            if(null == first && null == second) { return(0); }
            if(null == first) { return(-1); }
            return(null == second ? 1 : String.Compare(first, firstindex, second, secondindex, length));
        }

        //=========================================================================================
        /// <summary>
        /// Compares two strings; if <paramref name="ignorecase"/> is <c>true</c> (the default), uses  
        /// <see cref="StringComparison.OrdinalIgnoreCase"/>; otherwise, <see cref="StringComparison.Ordinal"/>.
        /// </summary>
        //=========================================================================================
        [DebuggerStepThrough]
        public static int CompareOrdinal(this string first, string second, bool ignorecase = true)
        {
            if(first == second) { return(0); }
            if(null == first)   { return(-1); }
            return(null == second ? 1 : String.Compare(first, second, ignorecase? 
                   StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
        }

        //=========================================================================================
        /// <summary>
        /// Compares substrings of two specified strings by evaluating the numeric values of the 
        /// corresponding Char objects in each substring. This is a case-sensitive comparison. 
        /// Neither the indices nor length may be less than 0.
        /// </summary>
        /// <param name="first">The first string to use in the comparison.</param>
        /// <param name="firstindex">The starting index of the substring in the first string.</param>
        /// <param name="second">The second string to use in the comparison.</param>
        /// <param name="secondindex">The starting index of the substring in the second string.</param>
        /// <param name="length">The maximum number of characters in the substrings to compare.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static int CompareOrdinal(this String first, Int32 firstindex, String second, Int32 secondindex, Int32 length)
        {
            if(null == first && null == second) { return(0); }
            if(null == first) { return(-1); }
            return(null == second ? 1 : String.CompareOrdinal(first, firstindex, second, secondindex, length));
        }

        //=========================================================================================
        /// <summary>
        /// Compares two strings with <see cref="StringComparison.CurrentCultureIgnoreCase"/> or 
        /// <see cref="StringComparison.CurrentCulture"/>. The default is 
        /// <see cref="StringComparison.CurrentCultureIgnoreCase"/>.
        /// </summary>
        //=========================================================================================
        [DebuggerStepThrough]
        public static bool EqualsCurrentCulture(this string value, string other, bool ignorecase = true)
        {
            if(null == value && null == other) { return(true); }
            if(null == value) { return(false); }
            return(0 == String.Compare(value, other, ignorecase ?
                        StringComparison.CurrentCultureIgnoreCase :
                        StringComparison.CurrentCulture));
        }

        //=========================================================================================
        /// <summary>
        /// Compares two strings with <see cref="StringComparison.OrdinalIgnoreCase"/> or 
        /// <see cref="StringComparison.Ordinal"/>. The default is 
        /// <see cref="StringComparison.OrdinalIgnoreCase"/>.
        /// </summary>
        //=========================================================================================
        [DebuggerStepThrough]
        public static bool EqualsOrdinal(this string value, string other, bool ignorecase = true)
        {
            if(null == value && null == other) { return(true); }
            if(null == value) { return(false); }
            return(0 == String.Compare(value, other, ignorecase ?
                        StringComparison.OrdinalIgnoreCase :
                        StringComparison.Ordinal));
        }
        #endregion

        #region BASE64 CONVERSIONS
        //=========================================================================================
        /// <summary>
        /// Returns a <see cref="String"/> result converted to a BASE64 encoded <paramref name="value"/>. 
        /// If the value is <c>null</c> or empty, returns an empty string.
        /// </summary>
        /// <param name="value">The source.</param>
        //=========================================================================================
        public static string Base64Encode(this string value)
        {
            if(value.IsNullOrEmpty()) { return(String.Empty); }
            byte [] __data = Encoding.UTF8.GetBytes(value);

            return(System.Convert.ToBase64String(__data));
        }

        //=========================================================================================
        /// <summary>
        /// Returns a <see cref="String"/> result converted from a BASE64 encoded <paramref name="value"/>. 
        /// If the value is <c>null</c> or empty, returns an empty string.
        /// </summary>
        /// <param name="value">The source.</param>
        //=========================================================================================
        public static string Base64Decode(this string value)
        {
            if(value.IsNullOrEmpty()) { return(String.Empty); }
            byte [] __data = Convert.FromBase64String(value);

            return(Encoding.UTF8.GetString(__data, 0, __data.Length));
        }
        #endregion

        #region BYTE ARRAY CONVERSION
        //=========================================================================================
        /// <summary>
        /// Convert a <c>string</c> to a <c>byte</c> array through <see cref="System.Text.UnicodeEncoding"/>.
        /// </summary>
        /// <returns>The <paramref name="value"/> as a <c>byte</c> array.</returns>
        //=========================================================================================
        [DebuggerStepThrough]
        public static byte [] ToByteArrayUnicode(this string value)
        {
            return(ToByteArrayEncoded(value, new System.Text.UnicodeEncoding()));
        }

        //=========================================================================================
        /// <summary>
        /// Convert a <c>string</c> to a <c>byte</c> array through <see cref="System.Text.UTF8Encoding"/>.
        /// </summary>
        /// <returns>The <paramref name="value"/> as a <c>byte</c> array.</returns>
        //=========================================================================================
        [DebuggerStepThrough]
        public static byte [] ToByteArrayUtf8(this string value)
        {
            return(ToByteArrayEncoded(value, new System.Text.UTF8Encoding()));
        }

        //=========================================================================================
        /// <summary>
        /// Convert a <c>string</c> to a <c>byte</c> array through <see cref="System.Text.Encoding"/>.
        /// </summary>
        /// <returns>The <paramref name="value"/> as a <c>byte</c> array.</returns>
        //=========================================================================================
        [DebuggerStepThrough]
        private static byte [] ToByteArrayEncoded(string value, System.Text.Encoding encoder)
        {
            if(value.IsNullOrEmpty()) { return(EmptyArray<byte>.Instance); }
            return(null == encoder ? EmptyArray<byte>.Instance : encoder.GetBytes(value));
        }
        #endregion

        #region UNCLASSIFIED EXTENSION METHODS
        //=========================================================================================
        /// <summary>
        /// Returns the entire string from the <paramref name="start"/> of the string to the end of the  
        /// string, exclusive.
        /// </summary>
        /// <param name="value">The source string.</param>
        /// <param name="start">The starting character or word.</param>
        /// <param name="casesensitive">
        /// If set to <c>true</c>, the search is case-sensitive; the default is <c>false</c>.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string After(this string value, string start, bool casesensitive = false)
        {
            if(value.IsNullOrEmpty()) { return(String.Empty); }

            int __index = value.LastIndexOf(start, casesensitive ? System.StringComparison.CurrentCulture : System.StringComparison.CurrentCultureIgnoreCase);
            if(-1 == __index) { return(String.Empty); }

            int __adjusted = __index + start.Length;
            return(__adjusted >= value.Length ? String.Empty : value.Substring(__adjusted));
        }

        //=========================================================================================
        /// <summary>
        /// Returns the entire string from the beginning of the string to the <paramref name="end"/> 
        /// string, exclusive.
        /// </summary>
        /// <param name="value">The source string.</param>
        /// <param name="end">The ending character or word.</param>
        /// <param name="casesensitive">
        /// If set to <c>true</c>, the search is case-sensitive; the default is <c>false</c>.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string Before(this string value, string end, bool casesensitive = false)
        {
            if(value.IsNullOrEmpty()) { return(String.Empty); }

            int __index = value.IndexOf(end, casesensitive ? System.StringComparison.CurrentCulture : System.StringComparison.CurrentCultureIgnoreCase);
            return(-1 == __index ? String.Empty : value.Substring(0, __index));
        }

        //=========================================================================================
        /// <summary>
        /// Returns the substring between the <paramref name="start"/> string <paramref name="end"/> 
        /// string, exclusive. This is a forward-only search, meaning that if the <paramref name="end"/> 
        /// occurs before the <paramref name="start"/>, returns <c>String.Empty</c>.
        /// </summary>
        /// <param name="value">The source string.</param>
        /// <param name="start">The starting character or word.</param>
        /// <param name="end">The ending character or word, starting at the end of the <paramref name="value"/>.</param>
        /// <param name="casesensitive">
        /// If set to <c>true</c>, the search is case-sensitive; the default is <c>false</c>.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string Between(this string value, string start, string end, bool casesensitive = false)
        {
            if(value.IsNullOrEmpty()) { return(String.Empty); }
            if(start.IsNullOrEmpty() || end.IsNullOrEmpty()) { return(String.Empty); }

            int __indexstart = value.IndexOf(start, casesensitive ? System.StringComparison.CurrentCulture : System.StringComparison.CurrentCultureIgnoreCase);
            int __indexend   = value.LastIndexOf(end, casesensitive ? System.StringComparison.CurrentCulture : System.StringComparison.CurrentCultureIgnoreCase);

            if(-1 == __indexstart) { return(String.Empty); }
            if(-1 == __indexend)   { return(String.Empty); }

            int __adjusted = __indexstart + start.Length;
            return(__adjusted >= __indexend ? String.Empty : value.Substring(__adjusted, __indexend - __adjusted));
        }

        //=========================================================================================
        /// <summary>
        /// Returns the character found at the string <paramref name="value"/> index. If the string 
        /// is <c>null</c>, empty or the zero-based <paramref name="index"/> exceeds the bounds of 
        /// the string, returns the <paramref name="default"/> character.
        /// </summary>
        /// <param name="value">The source string.</param>
        /// <param name="index">The character index.</param>
        /// <param name="default">The <paramref name="default"/> character.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static char CharAtOrDefault(this string value, int index, char @default)
        {
            if(value.IsNullOrEmpty()) { return(@default); }
            return(index >= value.Length ? @default : value[index]);
        }

        //=========================================================================================
        /// <summary>Parcels the string into segments of N-<paramref name="size"/>.</summary>
        /// <param name="value">The source.</param>
        /// <param name="size">The size of the characters to split.</param>
        /// <example><code>
        /// const string teststring = "samantha";
        /// string [] array1 = input.Explode(2); 
        /// string result1 = String.Join(",", array1) // results in sa,ma,nt,ha
        /// 
        /// string [] array2 = input.Explode(4); 
        /// string result2 = String.Join(",", array2) // results in sama,ntha
        /// </code></example>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string [] Explode(this string value, int size)
        {
            if(value.IsNullOrEmpty()) { return(null); }
            if(0 > size) { return(new[]{value}); }

            int __count = value.Length / size;
            bool __end  = (size * __count) < value.Length;

            string [] __result = __end ? new string[__count + 1] : new string[__count];

            for(int __index = 0; __index < __count; __index++)
               { __result[__index] = value.Substring((__index * size), size); }

            if(__end) { __result[__result.Length - 1] = value.Substring(__count * size); }
            return(__result);
        }

        //=========================================================================================
        /// <summary>Removes all but numbers from the <see cref="String"/>.</summary>
        /// <param name="value">The source.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string ExtractLetters(this string value)
        {
            return(Extract(value, Char.IsLetter));
        }

        //=========================================================================================
        /// <summary>Removes all but numbers from the <see cref="String"/>.</summary>
        /// <param name="value">The source.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string ExtractNumbers(this string value)
        {
            return(Extract(value, Char.IsNumber));
        }

        //=========================================================================================
        /// <summary>
        /// Removes all but the characters matching the <paramref name="condition"/> from the 
        /// <see cref="String"/>.
        /// </summary>
        /// <param name="value">The source.</param>
        /// <param name="condition">The condition which determines which characters are retained.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string Extract(this string value, Func<char, bool> condition)
        {
            if(value.IsNullOrEmpty()) { return(String.Empty); }

            int __limit = value.Length;
            bool __usebuilder = __stringbuilderlimit < __limit; 

            if(__usebuilder)
            {
                StringBuilder __builder = new StringBuilder(__limit);
                for(int __index = 0; __index < __limit; __index++)
                   { if(condition(value[__index])) {  __builder.Append(value[__index]); }}
                return(__builder.ToString());
            }

            string __result = String.Empty;
            for(int __index = 0; __index < __limit; __index++)
               { if(condition(value[__index])) {  __result += value[__index]; }}
            return(__result);
        }

        //=========================================================================================
        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified string to 
        /// <paramref name="find"/> in the provided <paramref name="value"/>. The matching is performed 
        /// with the current culture.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="find">The string to seek.</param>
        /// <param name="start">The search starting position.</param>
        /// <param name="casesensitive">
        /// If set to <c>true</c> the search will be CASE-SENSITIVE; the default is <c>false</c>.
        /// </param>
        /// <returns>
        /// The zero-based index position of the <paramref name="find"/> parameter if that string 
        /// is found; or -1 if it is not. If <paramref name="find"/> is Empty, the return value is 
        /// start index.
        /// </returns>
        //=========================================================================================
        public static int IndexOfCurrentCulture(this string value, string find, int start = 0, bool casesensitive = false)
        {
            if(value.IsNullOrEmpty()) { return(-1); }
            return(find.IsNullOrEmpty() ? start : value.IndexOf(find, start, casesensitive ? StringComparison.CurrentCulture : 
                                                                                             StringComparison.CurrentCultureIgnoreCase));
        }

        //=========================================================================================
        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified string to 
        /// <paramref name="find"/> in the provided <paramref name="value"/>. The matching is performed 
        /// with <see cref="StringComparison.Ordinal"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="find">The string to seek.</param>
        /// <param name="start">The search starting position.</param>
        /// <param name="casesensitive">
        /// If set to <c>true</c> the search will be CASE-SENSITIVE; the default is <c>false</c>.
        /// </param>
        /// <returns>
        /// The zero-based index position of the <paramref name="find"/> parameter if that string 
        /// is found; or -1 if it is not. If <paramref name="find"/> is Empty, the return value is 
        /// start index.
        /// </returns>
        //=========================================================================================
        public static int IndexOfOrdinal(this string value, string find, int start = 0, bool casesensitive = false)
        {
            if(value.IsNullOrEmpty()) { return(-1); }
            return(find.IsNullOrEmpty() ? start : value.IndexOf(find, start, casesensitive ? StringComparison.Ordinal : 
                                                                                             StringComparison.OrdinalIgnoreCase));
        }

        //=========================================================================================
        /// <summary>
        /// Returns <c>true</c> if the <see cref="String"/> is comprised of only letters and numbers. 
        /// An empty or null string returns <c>false</c>.
        /// </summary>
        /// <param name="value">The source.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static bool IsAlphaNumeric(this string value)
        {
            return(!value.IsNullOrEmpty() && value.Cast<char>().All(Char.IsLetterOrDigit));
        }

        //=========================================================================================
        /// <summary>
        /// Returns <c>true</c> if the <see cref="String"/> is a word, phrase or sequence that reads 
        /// the same backward as forward.
        /// </summary>
        /// <param name="value">The source string.</param>
        /// <param name="containsnonalphanumeric">
        /// If <c>true</c>, all non-alphanumeric characters are not considered; this is for sentences 
        /// which contain punctuation and white space characters. If <c>false</c>, all characters 
        /// are tested, which is slightly faster for a single word. The default is <c>true</c>.
        /// </param>
        //=========================================================================================
        public static bool IsPalindrome(this string value, bool containsnonalphanumeric = true)
        {
            if(value.IsNullOrEmpty()) { return(false); }
            int __minimum = 0;
            int __maximum = value.Length - 1;

            const int __limit = Int32.MaxValue - 1;

            while(true)
            {
                //===> prevent infinite loop
                if(__minimum >= __limit)  { break; }
                if(__minimum > __maximum) { return true; }
                char __firstchar  = value[__minimum];
                char __secondchar = value[__maximum];

                if(containsnonalphanumeric)
                {
                    while(!char.IsLetterOrDigit(__firstchar))
                    {
                        __minimum++;
                        if(__minimum > __maximum) { return(true); }

                        __firstchar = value[__minimum];
                    }

                    while(!char.IsLetterOrDigit(__secondchar))
                    {
                        __maximum--;
                        if(__minimum > __maximum) { return(true); }
                        __secondchar = value[__maximum];

                    } 
                }

                if(char.ToLower(__firstchar) != char.ToLower(__secondchar)) { return(false); }
                __minimum++;
                __maximum--;
            }
            return(false);
        }

        //=========================================================================================
        /// <summary>
        /// Directly convert a string to an integer by reducing the number of local variables to 
        /// improve performance. Around 2x faster.
        /// </summary>
        /// <param name="value">The source string.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static int ParseInt32Fast(this string value)
        {
            int __result = 0;
            for(int __index = 0; __index < value.Length; __index++)  { __result = 10 * __result + (value[__index] - 48); }
            return(__result);
        }

        //=========================================================================================
        /// <summary>
        /// Removes any character classified as <see cref="System.Globalization.UnicodeCategory.Control"/>.
        /// </summary>
        /// <param name="value">The source string.</param>
        //=========================================================================================
        public static string RemoveControlCharacters(this string value)
        {
            if(value.IsNullOrEmpty()) { return(value); }

            StringBuilder __builder = new StringBuilder(value);
                          __builder.RemoveControlCharacters();
            return(__builder.ToString());
        }

        //=========================================================================================
        /// <summary>
        /// Removes any character classified as <see cref="System.Globalization.UnicodeCategory.Control"/>.
        /// </summary>
        /// <param name="value">The source <see cref="StringBuilder"/>.</param>
        //=========================================================================================
        public static void RemoveControlCharacters(this StringBuilder value)
        {
            if(null == value) { return; }

            for(int __index = 0; __index < value.Length; __index++)
            {
                if(!Char.IsControl(value[__index])) { continue; }
                value.Remove(__index, 1); __index--;
            }
        }

        //=========================================================================================
        /// <summary>
        /// Replaces any accented character such as the French [á] or German [ö] with a plain ASCII 
        /// counterpart.
        /// </summary>
        /// <param name="value">The source string to be normalized.</param>
        //=========================================================================================
        public static string RemoveDiacritics(this string value)
        {
            if(value.IsNullOrEmpty()) { return(value); }

            string __formd = value.Normalize(NormalizationForm.FormD);
            StringBuilder __builder = new StringBuilder(__formd.Length * 0x000002);

            // ReSharper disable once ForCanBeConvertedToForeach
            for(int __index = 0; __index < __formd.Length; __index++)
            {
                UnicodeCategory __category = CharUnicodeInfo.GetUnicodeCategory(__formd[__index]);
                if(__category == UnicodeCategory.NonSpacingMark) { continue; }
 
                 __builder.Append(__formd[__index]);
            }

            return(__builder.ToString().Normalize(NormalizationForm.FormC));
        }

        //=========================================================================================
        /// <summary>
        /// Replaces any accented character such as the French [á] or German [ö] with a plain ASCII 
        /// counterpart.
        /// </summary>
        /// <param name="value">The source <see cref="StringBuilder"/> to be normalized.</param>
        //=========================================================================================
        public static void RemoveDiacritics(this StringBuilder value)
        {

            string __result = value.ToString().RemoveDiacritics();
            if(value.ToString().Equals(__result, StringComparison.Ordinal)) { return; }

            value.Clear(); value.Append(__result);
        }

        //=========================================================================================
        /// <summary>
        /// Removes any character repeated, consecutively, two or more times to just one instance of 
        /// that character.
        /// </summary>
        /// <param name="value">The source string.</param>
        /// <param name="repeated">The character which might be repeated.</param>.
        //=========================================================================================
        public static string RemoveRepeated(this string value, char repeated)
        {
            if(value.IsNullOrEmpty()) { return(value); }

            string __repeated = String.Concat(repeated, repeated);

            while(value.Contains(__repeated))
            {
                value = value.Replace(__repeated, repeated.ToString(CultureInfo.CurrentCulture));
            }

            return(value);
        }

        //=========================================================================================
        /// <summary>
        /// Removes any repeated sequences of character whitespace such that only one whitespace 
        /// character remains.
        /// </summary>
        /// <param name="value">The source string.</param>
        //=========================================================================================
        public static string RemoveRepeatedWhitespace(this string value)
        {
            if(value.IsNullOrEmpty()) { return(value); }

            StringBuilder __builder = new StringBuilder(value);
                          __builder.RemoveRepeatedWhitespace();
            return(__builder.ToString());
        }

        //=========================================================================================
        /// <summary>
        /// Removes any repeated sequences of character whitespace such that only one whitespace 
        /// character remains.
        /// </summary>
        /// <param name="value">The source <see cref="StringBuilder"/>.</param>
        //=========================================================================================
        public static void RemoveRepeatedWhitespace(this StringBuilder value)
        {
            if(null == value) { return; }

            for(int __index = 0; __index < value.Length; __index++)
            {
                if(!Char.IsWhiteSpace(value[__index])) { continue; }
                if((0 < __index) && (value[__index - 1] == Chars.CharSpace))
                {
                    value.Remove(__index, 1); __index--;
                    continue;
                }
                
                value[__index] = Chars.CharSpace;
            }
        }

        //=========================================================================================
        /// <summary>
        /// Removes each character in the source <paramref name="value"/> for which the 
        /// <paramref name="condition"/> returns <c>true</c>.
        /// </summary>
        /// <param name="value">The source string.</param>
        /// <param name="condition">The condition which tests each character.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string RemoveWhere(this string value, Func<char, bool> condition)
        {
            if(value.IsNullOrEmpty()) { return(value); }
            return(null == condition ? value : new string(value.ToCharArray().Where(character => !condition(character)).ToArray()));
        }

        //=========================================================================================
        /// <summary>
        /// Removes all non-printable characters from the provided <paramref name="value"/> and replaces 
        /// each with a SPACE character. If <paramref name="usespace"/> is <c>false</c>, the 
        /// non-printable character is simply removed and not replaced with a SPACE character.
        /// </summary>
        /// <param name="value">The source string.</param>
        /// <param name="usespace">If <c>false</c>, each non-printable character is simply removed 
        /// and not replaced with a SPACE character. The default is <c>true</c></param>.
        //=========================================================================================
        public static string RemoveNonPrintable(this string value, bool usespace = true)
        {
            if(value.IsNullOrEmpty()) { return(String.Empty); }

            char [] __value = value.ToCharArray();
            int __limit = __value.Length;

            if(!usespace)
            {
                System.Text.StringBuilder __result = new System.Text.StringBuilder(__limit);

                for(int __current = 0; __current < __limit; __current++)
                {
                    char __char = __value[__current];

                    switch(CharUnicodeInfo.GetUnicodeCategory(__char))
                    {
                        case UnicodeCategory.Control:
                        case UnicodeCategory.OtherSymbol:
                        case UnicodeCategory.Format: continue;
                        default: __result.Append(__char); break;
                    }
                }

                return(__result.ToString());
            }

            //===> this would be less memory intensive
            for(int __current = 0; __current < __limit; __current++)
            {
                char __char = __value[__current];

                switch(CharUnicodeInfo.GetUnicodeCategory(__char))
                {
                    case UnicodeCategory.Control:
                    case UnicodeCategory.OtherSymbol:
                    case UnicodeCategory.Format: __value[__current] = ' '; break;
                    default: continue;
                }
            }

            return(new string(__value));
        }

        //=========================================================================================
        /// <summary>
        /// Repeats the provided string <paramref name="value"/> for N <paramref name="times"/>.
        /// </summary>
        /// <param name="value">The source string.</param>
        /// <param name="times">The number of repetitions.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string Repeat(this string value, int times)
    {
        if(null == value)     { return(null); }
        if(0 == times)        { return(null); }
        if(1 == times)        { return(value); }
        if(1 == value.Length) { return(new string(value[0], times)); }

        //===> insufficient repetitions to warrant object creation
        if(0x000010 > times)
        {
            string __result = String.Empty;
            while (times-- > 0) { __result += value; }
            return(__result);
        }

        StringBuilder __builder = new StringBuilder(times * value.Length);
        while(times-- > 0) { __builder.Append(value); }

        return(__builder.ToString());
    }

        //=========================================================================================
        /// <summary>
        /// Replaces the substring(s) <paramref name="value"/> with each item in the 
        /// <paramref name="replacelist"/>. If either the source <paramref name="value"/> or the 
        /// <paramref name="replacelist"/> is <c>null</c> or empty, returns the source value 
        /// unchanged.
        /// </summary>
        /// <param name="value">The source string.</param>
        /// <param name="with">The replacement value.</param>
        /// <param name="replacelist">The substrings to be replaced <paramref name="with"/>.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string ReplaceWith(this string value, string with, params string [] replacelist)
        {
            if(value.IsNullOrEmpty()) { return(value); }
            if(null == replacelist)   { return(value); }

            int __limit       = replacelist.Length;
            int __condition   = value.Length;
            bool __usebuilder = __stringbuilderlimit < __condition; 

            if(__usebuilder)
            {
                StringBuilder __builder = new StringBuilder(value);

                for(int __index = 0; __index < __limit; __index++) { __builder.Replace(replacelist[__index], with); }
                return(__builder.ToString());
            }

            string __result = value;

            for(int __index = 0; __index < __limit; __index++) { __result = __result.Replace(replacelist[__index], with); }
            return(__result);
        }

        //=========================================================================================
        /// <summary>
        /// Extracts the characters from the <paramref name="start"/> to the <paramref name="end"/> 
        /// index, inclusive for start index and exclusive for end index. Any index less than 0 will 
        /// start at the end and count backwards.
        /// </summary>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string Slice(this string value, int start, int end = 0)
        {
            if(value.IsNullOrEmpty()) { return(value); }

            //===> if < 0 start at the end and move backwards
            if (0 > end) { end = value.Length + end; }
            int __length = end - start; 
            return(value.Substring(start, __length));
        }

        //=========================================================================================
        /// <summary>
        /// Returns a substring from the index of the array resultant a String.Split.
        /// </summary>
        /// <param name="value">The value to be split.</param>
        /// <param name="index">The index of the string array.</param>
        /// <param name="delimiters">The delimiters.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string Split(this string value, int index, char [] delimiters)
        {
            if(value.IsNullOrEmpty()) { return(String.Empty); }
            if(0 > index) { return(String.Empty); }

            string [] __parts = value.Split(delimiters, StringSplitOptions.None);
            return(index >= __parts.Length ? String.Empty : __parts[index]);
        }

        //=========================================================================================
        /// <summary>
        /// Returns <c>true</c> if the substring determined with the start <paramref name="index"/> 
        /// to the <paramref name="length"/> in found in the provided <paramref name="list"/> of 
        /// strings. The comparison is case-sensitive.
        /// </summary>
        /// <param name="value">The value from which the substring will be extracted.</param>
        /// <param name="index">The zero-based starting character position of the substring.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <param name="list">The list of strings to search.</param>
        //=========================================================================================
        public static bool SubstringIn(this string value, int index, int length, params string [] list)
        {
            return(SubstringIn(value, index, length, false, list));
        }

        //=========================================================================================
        /// <summary>
        /// Returns <c>true</c> if the substring determined with the start <paramref name="index"/> 
        /// to the <paramref name="length"/> in found in the provided <paramref name="list"/> of 
        /// strings.
        /// </summary>
        /// <param name="value">The value from which the substring will be extracted.</param>
        /// <param name="index">The zero-based starting character position of the substring.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <param name="ignorecase">If <c>true</c>, the comparison will be case-insensitive.</param>
        /// <param name="list">The list of strings to search.</param>
        //=========================================================================================
        public static bool SubstringIn(this string value, int index, int length, bool ignorecase, params string [] list)
        {
            if(value.IsNullOrEmpty()) { return(false); }
            if(null == list)          { return(false); }
            if(0 > index)             { return(false); }
            if(index >= value.Length) { return(false); }

            string __substring = (index + length > value.Length) ? value.Substring(index) : value.Substring(index, length);
            return(list.Any(current => 0 == __substring.CompareCurrentCulture(current, ignorecase)));
        }

        //=========================================================================================
        /// <summary>
        /// If the last character of the <paramref name="value"/> is neither A-Z nor 0-9, that character 
        /// is removed and replaced with the <paramref name="replacement"/> parameter.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <param name="replacement">The replacement character.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string TrimLastIndexNonAlphaNumeric(this string value, string replacement)
        {
            if(value.IsNullOrEmpty()) { return(value); }
            return(Char.IsLetterOrDigit(value[value.Length - 1]) ? value : value.Substring(0, value.Length - 1) + replacement);
        }

        //=========================================================================================
        /// <summary>
        /// Ends the string <paramref name="value"/> at the character <paramref name="length"/> 
        /// provided and appends the <paramref name="ellipsis"/>; if provided. If the <paramref name="ellipsis"/> 
        /// is <c>null</c>, the default ellipsis is used (a row of three periods or full stops).
        /// </summary>
        /// <param name="value">The value to be truncated.</param>
        /// <param name="length">The maximum permitted length.</param>
        /// <param name="ellipsis">The ellipsis (suffix) indicating the string was shortened.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string Truncate(this string value, int length, string ellipsis = null)
        {
            if(value.IsNullOrEmpty())  { return(value); }
            if(value.Length <= length) { return(value); }
            if(null == ellipsis)       { ellipsis = PunctuationStrings.Ellipsis; }

            int __length = length - ellipsis.Length;
            return(value.Substring(0, __length) + ellipsis);
        }

        //=========================================================================================
        /// <summary>
        /// Returns <c>true</c> when a valid surrogate pair starts at the given <paramref name="index"/> 
        /// in this string. Put-of-range indices return <c>false</c>.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <param name="index">The position of the character.</param>
        //=========================================================================================
        public static bool ValidSurrogatePair(this string value, int index)
        {
            if(value.IsNullOrEmpty()) { return(false); }
            return(0 <= index && index <= (value.Length - 2) &&
                  Char.IsHighSurrogate(value[index]) &&
                  Char.IsLowSurrogate(value[index + 1]));

        }
        #endregion

        #region CONVERT TO ENUM
        //=========================================================================================
        /// <summary>
        /// Converts the provided <paramref name="value"/> into an <see cref="Enum"/>; if the
        /// conversion fails, returns the <paramref name="default"/>.
        /// </summary>
        /// <param name="value">The source value.</param>
        /// <param name="default">The default value if the conversion fails.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static TTypeEnum ParseEnum<TTypeEnum>(this string value, TTypeEnum @default) where TTypeEnum : struct
        {
            TTypeEnum __result;
            return(Enum.TryParse(value, true, out __result) ? __result : @default);
        }

        //=========================================================================================
        /// <summary>
        /// Converts the provided <paramref name="value"/> into an <see cref="Enum"/>; if the
        /// conversion fails, returns the result of <paramref name="onfailed"/>.
        /// </summary>
        /// <param name="value">The source value.</param>
        /// <param name="onfailed">Determines the result if the conversion fails.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static TTypeEnum ParseEnum<TTypeEnum>(this string value, Func<string, TTypeEnum> onfailed) where TTypeEnum : struct
        {
            if(null == onfailed) { return(default(TTypeEnum)); }
            TTypeEnum __result;
            return(Enum.TryParse(value, true, out __result) ? __result : onfailed(value));
        }
        #endregion

        #region CONDITIONS FOR EMPTY STRINGS
        //=========================================================================================
        /// <summary>
        /// Returns the product of <paramref name="alternative"/> if the provided <paramref name="value"/> 
        /// is <c>null</c> or empty; otherwise, returns <paramref name="value"/>.
        /// </summary>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string IfEmpty(this string value, Func<string> alternative)
        {
            if(null == alternative) { return(value); }
            return(value.IsNullOrEmpty() ? alternative() : value);
        }

        //=========================================================================================
        /// <summary>
        /// Indicates whether the specified <see cref="System.String"/> is null or an empty string.
        /// </summary>
        /// <param name="value">A <see cref="System.String"/> reference.</param>
        /// <param name="iswhitespace">
        /// If <c>true</c>, white-space characters are considered. The default is <c>true</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if at least one string is <c>null</c> or an empty string (""); otherwise, 
        /// <c>false</c>.
        /// </returns>
        //=========================================================================================
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty(this string value, bool iswhitespace = true)
        {
            return(iswhitespace ? String.IsNullOrWhiteSpace(value) : String.IsNullOrEmpty(value));
        }

        //=========================================================================================
        /// <summary>
        /// If <c>true</c>, at least one string in the series is <c>null</c> or empty; otherwise, no 
        /// string instance was <c>null</c> or empty.
        /// </summary>
        /// <param name="list">An <see cref="IEnumerable{T}"/> of strings to test.</param>
        /// <param name="iswhitespace">
        /// If <c>true</c>, white-space characters are considered. The default is <c>true</c>.
        /// </param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static bool IsNullOrEmptyAny(this IEnumerable<string> list, bool iswhitespace = true)
        {
            return(null == list || list.Any(value => IsNullOrEmpty(value, iswhitespace)));
        }
        #endregion

        #region FORMAT
        //=========================================================================================
        /// <summary>
        /// Replaces the format item in the specified <paramref name="value"/> with the string 
        /// representation of a corresponding object in the <paramref name="args"/> list 
        /// according to the provider.
        /// </summary>
        /// <param name="value">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        private static string Format(this string value, IFormatProvider provider, params object [] args)
        {
            if(value.IsNullOrEmpty()) { return(value); }
            return(null == args ? value : String.Format(provider, value, args));
        }

        //=========================================================================================
        /// <summary>
        /// Replaces the format item in the specified <paramref name="value"/> with the string 
        /// representation of a corresponding object in the <paramref name="args"/> list 
        /// according to the <see cref="System.Globalization.CultureInfo.CurrentCulture"/>
        /// </summary>
        /// <param name="value">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string FormatCulture(this string value, params object [] args)
        {
            return(Format(value, System.Globalization.CultureInfo.CurrentCulture, args));
        }

        //=========================================================================================
        /// <summary>
        /// Replaces the format item in the specified <paramref name="value"/> with the string 
        /// representation of a corresponding object in the <paramref name="args"/> list 
        /// according to the <see cref="System.Globalization.CultureInfo.InvariantCulture"/>
        /// </summary>
        /// <param name="value">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string FormatInvariant(this string value, params object [] args)
        {
            return(Format(value, System.Globalization.CultureInfo.InvariantCulture, args));
        }
        #endregion

        #region TODICTIONARY
        //=========================================================================================
        /// <summary>
        /// Converts a string comprised of a series of key/value pairs into a string 
        /// <see cref="IDictionary{TKey,TValue}"/>; as an example: key1:value1,key2:value2...
        /// </summary>
        /// <param name="value">The source string.</param>
        /// <param name="rowdelimiter">
        /// The character which indicates the end of one key/value pair and the start of another 
        /// key/value pair. The default is the comma (,) character.
        /// </param>
        /// <param name="itemdelimiter">
        /// The character which separates the keys from respective values, such as the colon (:) 
        /// character in [key1:value1] The default is the colon (:) character.
        /// </param>
        //=========================================================================================
        public static IDictionary<string, string> ToDictionary(this string value, char rowdelimiter = Chars.CharComma, char itemdelimiter = Chars.CharColon)
        {
            if(value.IsNullOrEmpty()) { return(new Dictionary<string, string>()); }

            string [] __rows = value.Split(rowdelimiter);
            if(0 == __rows.Length) { return(new Dictionary<string, string>()); }

            Dictionary<string, string> __result = new Dictionary<string, string>(__rows.Length);

            // ReSharper disable ForCanBeConvertedToForeach
            for(int __index = 0; __index < __rows.Length; __index++)
            {
                string [] __pair = __rows[__index].Split(itemdelimiter);

                __result.Add(__pair[0], 1 < __pair.Length ? __pair[1] : String.Empty);

            }
            // ReSharper restore ForCanBeConvertedToForeach
            return(__result);
        }
        #endregion

        #region MEMORY STREAM CONVERSION
        //=========================================================================================
        /// <summary>
        /// Converts the string <paramref name="value"/> to a <see cref="MemoryStream"/>. Note it is 
        /// the responsibility of the caller to properly DISPOSE the <see cref="Stream"/>.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <param name="encoder">
        /// The <see cref="System.Text.Encoding"/>; if <c>null</c>, the default is 
        /// <see cref="System.Text.UTF8Encoding"/>.
        /// </param>
        //=========================================================================================
        public static Stream ToMemoryStream(this string value, System.Text.Encoding encoder = null)
        {
            System.Text.Encoding __encoder = encoder ?? new System.Text.UTF8Encoding();
            return(new MemoryStream(__encoder.GetBytes(value)));
        }
        #endregion

        #region TOSTRING() CONVENIENCE METHODS
        //=========================================================================================
        /// <summary>
        /// Returns a copy of this string converted to lowercase, using the casing rules of the 
        /// specified culture.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to lowercase.</param>
        /// <param name="culture">An object that supplies culture-specific casing rules.</param>
        //=========================================================================================
        public static string ToLowerCulture(this string value, CultureInfo culture)
        {
            if(value.IsNullOrEmpty()) { return (String.Empty); }
            return(null == culture ? CultureInfo.CurrentCulture.TextInfo.ToLower(value) : culture.TextInfo.ToLower(value));
        }

        //=========================================================================================
        /// <summary>
        /// Returns a <c>System.String</c> representing the value of the <c>byte</c> according the 
        /// <see cref="System.Globalization.CultureInfo.CurrentCulture"/>.
        /// </summary>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string ToStringCulture(this byte value)
        {
            return(value.ToString(System.Globalization.CultureInfo.CurrentCulture));
        }

        //=========================================================================================
        /// <summary>
        /// Returns a <c>System.String</c> representing the value of the <c>int</c> according the 
        /// <see cref="System.Globalization.CultureInfo.CurrentCulture"/>.
        /// </summary>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string ToStringCulture(this int value)
        {
            return(value.ToString(System.Globalization.CultureInfo.CurrentCulture));
        }

        //=========================================================================================
        /// <summary>
        /// Returns a <c>System.String</c> representing the value of the <c>decimal</c> according 
        /// the <see cref="System.Globalization.CultureInfo.CurrentCulture"/>.
        /// </summary>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string ToStringCulture(this decimal value)
        {
            return(value.ToString(System.Globalization.CultureInfo.CurrentCulture));
        }

        //=========================================================================================
        /// <summary>
        /// Returns a <c>System.String</c> representing the value of the <c>double</c> according the 
        /// <see cref="System.Globalization.CultureInfo.CurrentCulture"/>.
        /// </summary>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string ToStringCulture(this double value)
        {
            return(value.ToString(System.Globalization.CultureInfo.CurrentCulture));
        }

        //=========================================================================================
        /// <summary>
        /// Returns a <c>System.String</c> representing the value of the <c>float</c> according the 
        /// <see cref="System.Globalization.CultureInfo.CurrentCulture"/>.
        /// </summary>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string ToStringCulture(this float value)
        {
            return(value.ToString(System.Globalization.CultureInfo.CurrentCulture));
        }

        //=========================================================================================
        /// <summary>
        /// Returns a <c>System.String</c> representing the value of the <c>long</c> according the 
        /// <see cref="System.Globalization.CultureInfo.CurrentCulture"/>.
        /// </summary>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string ToStringCulture(this long value)
        {
            return(value.ToString(System.Globalization.CultureInfo.CurrentCulture));
        }

        //=========================================================================================
        /// <summary>
        /// Returns a <c>System.String</c> representing the value of the <c>DateTime</c> according 
        /// the <see cref="System.Globalization.CultureInfo.CurrentCulture"/>.
        /// </summary>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string ToStringCulture(this DateTime source)
        {
            return(source.ToString(System.Globalization.CultureInfo.CurrentCulture));
        }

        //=========================================================================================
        /// <summary>
        /// Returns a <c>System.String</c> representing the value of the <c>byte</c> according the 
        /// <see cref="System.Globalization.CultureInfo.InvariantCulture"/>.
        /// </summary>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string ToStringInvariant(this byte value)
        {
            return(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        //=========================================================================================
        /// <summary>
        /// Returns a <c>System.String</c> representing the value of the <c>int</c> according the 
        /// <see cref="System.Globalization.CultureInfo.InvariantCulture"/>.
        /// </summary>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string ToStringInvariant(this int value)
        {
            return(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        //=========================================================================================
        /// <summary>
        /// Returns a <c>System.String</c> representing the value of the <c>decimal</c> according 
        /// the <see cref="System.Globalization.CultureInfo.InvariantCulture"/>.
        /// </summary>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string ToStringInvariant(this decimal value)
        {
            return(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        //=========================================================================================
        /// <summary>
        /// Returns a <c>System.String</c> representing the value of the <c>double</c> according the 
        /// <see cref="System.Globalization.CultureInfo.InvariantCulture"/>.
        /// </summary>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string ToStringInvariant(this double value)
        {
            return(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        //=========================================================================================
        /// <summary>
        /// Returns a <c>System.String</c> representing the value of the <c>float</c> according the 
        /// <see cref="System.Globalization.CultureInfo.InvariantCulture"/>.
        /// </summary>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string ToStringInvariant(this float value)
        {
            return(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        //=========================================================================================
        /// <summary>
        /// Returns a <c>System.String</c> representing the value of the <c>long</c> according the 
        /// <see cref="System.Globalization.CultureInfo.InvariantCulture"/>.
        /// </summary>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string ToStringInvariant(this long value)
        {
            return(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        //=========================================================================================
        /// <summary>
        /// Returns ToString from the current reference <paramref name="value"/>; or 
        /// <paramref name="default"/>, if the value is <c>null</c>.
        /// </summary>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string ToStringOrDefault<TType>(this TType value, string @default) 
        {
            return(ReferenceEquals(value, null) ? @default : value.ToString());
        }

        //=========================================================================================
        /// <summary>
        /// Returns a copy of this string converted to uppercase, using the casing rules of the 
        /// specified culture.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to uppercase.</param>
        /// <param name="culture">An object that supplies culture-specific casing rules.</param>
        //=========================================================================================
        [DebuggerStepThrough]
        public static string ToUpperCulture(this string value, CultureInfo culture)
        {
            if(value.IsNullOrEmpty()) { return(String.Empty); }
            return(null == culture ? CultureInfo.CurrentCulture.TextInfo.ToUpper(value) : culture.TextInfo.ToUpper(value));
        }

        #endregion
    }
}
