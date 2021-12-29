// Copyright (c) Tunnel Vision Laboratories, LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace TunnelVisionLabs.LanguageTypes.SourceGenerator
{
    using System;

    internal static class StringExtensions
    {
        public static string ReplaceLineEndings(this string input)
        {
            return ReplaceLineEndings(input, Environment.NewLine);
        }

        public static string ReplaceLineEndings(this string input, string replacementText)
        {
            // First normalize to LF
            var lineFeedInput = input
                .Replace("\r\n", "\n")
                .Replace("\r", "\n")
                .Replace("\f", "\n")
                .Replace("\x0085", "\n")
                .Replace("\x2028", "\n")
                .Replace("\x2029", "\n");

            // Then normalize to the replacement text
            return lineFeedInput.Replace("\n", replacementText);
        }
    }
}
