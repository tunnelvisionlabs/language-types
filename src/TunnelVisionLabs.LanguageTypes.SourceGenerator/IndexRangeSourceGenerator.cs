﻿// Copyright (c) Tunnel Vision Laboratories, LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace TunnelVisionLabs.LanguageTypes.SourceGenerator
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;

    [Generator(LanguageNames.CSharp)]
    internal class IndexRangeSourceGenerator : IIncrementalGenerator
    {
        private const string SystemIndexSource = @"// <auto-generated/>

#nullable enable

namespace System
{
    using System.Runtime.CompilerServices;

    /// <summary>Represent a type can be used to index a collection either from the start or the end.</summary>
    /// <remarks>
    /// Index is used by the C# compiler to support the new index syntax
    /// <code>
    /// int[] someArray = new int[5] { 1, 2, 3, 4, 5 } ;
    /// int lastElement = someArray[^1]; // lastElement = 5
    /// </code>
    /// </remarks>
    internal readonly struct Index : IEquatable<Index>
    {
        private readonly int _value;

        /// <summary>Construct an Index using a value and indicating if the index is from the start or from the end.</summary>
        /// <param name=""value"">The index value. it has to be zero or positive number.</param>
        /// <param name=""fromEnd"">Indicating if the index is from the start or from the end.</param>
        /// <remarks>
        /// If the Index constructed from the end, index value 1 means pointing at the last element and index value 0 means pointing at beyond last element.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Index(int value, bool fromEnd = false)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, ""Non-negative number required."");
            }

            if (fromEnd)
                _value = ~value;
            else
                _value = value;
        }

        // The following private constructors mainly created for perf reason to avoid the checks
        private Index(int value)
        {
            _value = value;
        }

        /// <summary>Create an Index pointing at first element.</summary>
        public static Index Start => new Index(0);

        /// <summary>Create an Index pointing at beyond last element.</summary>
        public static Index End => new Index(~0);

        /// <summary>Create an Index from the start at the position indicated by the value.</summary>
        /// <param name=""value"">The index value from the start.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Index FromStart(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, ""Non-negative number required."");
            }

            return new Index(value);
        }

        /// <summary>Create an Index from the end at the position indicated by the value.</summary>
        /// <param name=""value"">The index value from the end.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Index FromEnd(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, ""Non-negative number required."");
            }

            return new Index(~value);
        }

        /// <summary>Returns the index value.</summary>
        public int Value
        {
            get
            {
                if (_value < 0)
                    return ~_value;
                else
                    return _value;
            }
        }

        /// <summary>Indicates whether the index is from the start or the end.</summary>
        public bool IsFromEnd => _value < 0;

        /// <summary>Calculate the offset from the start using the giving collection length.</summary>
        /// <param name=""length"">The length of the collection that the Index will be used with. length has to be a positive value</param>
        /// <remarks>
        /// For performance reason, we don't validate the input length parameter and the returned offset value against negative values.
        /// we don't validate either the returned offset is greater than the input length.
        /// It is expected Index will be used with collections which always have non negative length/count. If the returned offset is negative and
        /// then used to index a collection will get out of range exception which will be same affect as the validation.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetOffset(int length)
        {
            int offset = _value;
            if (IsFromEnd)
            {
                // offset = length - (~value)
                // offset = length + (~(~value) + 1)
                // offset = length + value + 1

                offset += length + 1;
            }
            return offset;
        }

        /// <summary>Indicates whether the current Index object is equal to another object of the same type.</summary>
        /// <param name=""value"">An object to compare with this object</param>
        public override bool Equals(object? value) => value is Index && _value == ((Index)value)._value;

        /// <summary>Indicates whether the current Index object is equal to another Index object.</summary>
        /// <param name=""other"">An object to compare with this object</param>
        public bool Equals(Index other) => _value == other._value;

        /// <summary>Returns the hash code for this instance.</summary>
        public override int GetHashCode() => _value;

        /// <summary>Converts integer number to an Index.</summary>
        public static implicit operator Index(int value) => FromStart(value);

        /// <summary>Converts the value of the current Index object to its equivalent string representation.</summary>
        public override string ToString()
        {
            if (IsFromEnd)
                return $""^{((uint)Value).ToString()}"";

            return ((uint)Value).ToString();
        }
    }
}
";

        private static readonly string SystemRangeSource = GetSystemRangeSource(hasValueTuple: true);

        private static readonly string SystemRangeWithoutValueTupleSource = GetSystemRangeSource(hasValueTuple: false);

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var referencedTypesData = context.CompilationProvider.Select(
                (compilation, cancellationToken) =>
                {
                    var hasIndex = IsCompilerTypeAvailable(compilation, "System.Index");
                    var hasRange = IsCompilerTypeAvailable(compilation, "System.Range");
                    var hasValueTuple = IsCompilerTypeAvailable(compilation, "System.ValueTuple`2");

                    return new ReferencedTypesData(
                        HasIndex: hasIndex,
                        HasRange: hasRange,
                        HasValueTuple: hasValueTuple);
                });

            context.RegisterSourceOutput(
                referencedTypesData,
                (context, referencedTypesData) =>
                {
                    var forwarders = new List<string>();

                    if (referencedTypesData.HasIndex == TypeDefinitionLocation.None)
                    {
                        context.AddSource("Index.g.cs", SystemIndexSource.ReplaceLineEndings("\r\n"));
                    }
                    else if (referencedTypesData.HasIndex == TypeDefinitionLocation.Referenced)
                    {
                        forwarders.Add("Index");
                    }

                    if (referencedTypesData.HasRange == TypeDefinitionLocation.None)
                    {
                        if (referencedTypesData.HasValueTuple != TypeDefinitionLocation.None)
                        {
                            context.AddSource("Range.g.cs", SystemRangeSource.ReplaceLineEndings("\r\n"));
                        }
                        else
                        {
                            context.AddSource("Range.g.cs", SystemRangeWithoutValueTupleSource.ReplaceLineEndings("\r\n"));
                        }
                    }
                    else if (referencedTypesData.HasRange == TypeDefinitionLocation.Referenced)
                    {
                        forwarders.Add("Range");
                    }

                    if (forwarders.Count > 0)
                    {
                        var compilerForwarders = $@"// <auto-generated/>

#nullable enable

using System;
using System.Runtime.CompilerServices;

{string.Join("\r\n", forwarders.Select(forwarder => $"[assembly: TypeForwardedTo(typeof({forwarder}))]"))}
";

                        context.AddSource("IndexRangeForwarders.g.cs", compilerForwarders.ReplaceLineEndings("\r\n"));
                    }
                });
        }

        private static string GetSystemRangeSource(bool hasValueTuple)
        {
            string getOffsetAndLengthMethod;
            if (hasValueTuple)
            {
                getOffsetAndLengthMethod = @"
        /// <summary>Calculate the start offset and length of range object using a collection length.</summary>
        /// <param name=""length"">The length of the collection that the range will be used with. length has to be a positive value.</param>
        /// <remarks>
        /// For performance reason, we don't validate the input length parameter against negative values.
        /// It is expected Range will be used with collections which always have non negative length/count.
        /// We validate the range is inside the length scope though.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (int Offset, int Length) GetOffsetAndLength(int length)
        {
            int start;
            Index startIndex = Start;
            if (startIndex.IsFromEnd)
                start = length - startIndex.Value;
            else
                start = startIndex.Value;

            int end;
            Index endIndex = End;
            if (endIndex.IsFromEnd)
                end = length - endIndex.Value;
            else
                end = endIndex.Value;

            if ((uint)end > (uint)length || (uint)start > (uint)end)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            return (start, end - start);
        }
";
            }
            else
            {
                getOffsetAndLengthMethod = string.Empty;
            }

            return $@"// <auto-generated/>

#nullable enable

namespace System
{{
    using System.Runtime.CompilerServices;

    /// <summary>Represent a range has start and end indexes.</summary>
    /// <remarks>
    /// Range is used by the C# compiler to support the range syntax.
    /// <code>
    /// int[] someArray = new int[5] {{ 1, 2, 3, 4, 5 }};
    /// int[] subArray1 = someArray[0..2]; // {{ 1, 2 }}
    /// int[] subArray2 = someArray[1..^0]; // {{ 2, 3, 4, 5 }}
    /// </code>
    /// </remarks>
    internal readonly struct Range : IEquatable<Range>
    {{
        /// <summary>Represent the inclusive start index of the Range.</summary>
        public Index Start {{ get; }}

        /// <summary>Represent the exclusive end index of the Range.</summary>
        public Index End {{ get; }}

        /// <summary>Construct a Range object using the start and end indexes.</summary>
        /// <param name=""start"">Represent the inclusive start index of the range.</param>
        /// <param name=""end"">Represent the exclusive end index of the range.</param>
        public Range(Index start, Index end)
        {{
            Start = start;
            End = end;
        }}

        /// <summary>Indicates whether the current Range object is equal to another object of the same type.</summary>
        /// <param name=""value"">An object to compare with this object</param>
        public override bool Equals(object? value) =>
            value is Range r &&
            r.Start.Equals(Start) &&
            r.End.Equals(End);

        /// <summary>Indicates whether the current Range object is equal to another Range object.</summary>
        /// <param name=""other"">An object to compare with this object</param>
        public bool Equals(Range other) => other.Start.Equals(Start) && other.End.Equals(End);

        /// <summary>Returns the hash code for this instance.</summary>
        public override int GetHashCode()
        {{
            var value64 = ((ulong)unchecked((uint)Start.GetHashCode()) << 32) + unchecked((uint)End.GetHashCode());
            return value64.GetHashCode();
        }}

        /// <summary>Converts the value of the current Range object to its equivalent string representation.</summary>
        public override string ToString()
        {{
            return $""{{getFromEndSpecifier(Start)}}{{toString(Start)}}..{{getFromEndSpecifier(End)}}{{toString(End)}}"";

            static string getFromEndSpecifier(Index index) => index.IsFromEnd ? ""^"" : string.Empty;
            static string toString(Index index) => ((uint)index.Value).ToString();
        }}

        /// <summary>Create a Range object starting from start index to the end of the collection.</summary>
        public static Range StartAt(Index start) => new Range(start, Index.End);

        /// <summary>Create a Range object starting from first element in the collection to the end Index.</summary>
        public static Range EndAt(Index end) => new Range(Index.Start, end);

        /// <summary>Create a Range object starting from first element to the end.</summary>
        public static Range All => new Range(Index.Start, Index.End);
{getOffsetAndLengthMethod}    }}
}}
";
        }

        private static TypeDefinitionLocation IsCompilerTypeAvailable(Compilation compilation, string fullyQualifiedMetadataName)
        {
            return compilation.GetBestTypeByMetadataName(fullyQualifiedMetadataName, requiresAccess: true) switch
            {
                { OriginalDefinition.ContainingAssembly: var containingAssembly } when SymbolEqualityComparer.Default.Equals(compilation.Assembly, containingAssembly) => TypeDefinitionLocation.Defined,
                { } => TypeDefinitionLocation.Referenced,
                _ => TypeDefinitionLocation.None,
            };
        }

        private sealed record ReferencedTypesData(
            TypeDefinitionLocation HasIndex,
            TypeDefinitionLocation HasRange,
            TypeDefinitionLocation HasValueTuple);
    }
}
