﻿// Copyright (c) Tunnel Vision Laboratories, LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace TunnelVisionLabs.LanguageTypes.SourceGenerator
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;

    [Generator(LanguageNames.CSharp)]
    internal class NullableSourceGenerator : IIncrementalGenerator
    {
        private const string AllowNullAttributeSource = @"// <auto-generated/>

#nullable enable

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>Specifies that null is allowed as an input even if the corresponding type disallows it.</summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
    internal sealed class AllowNullAttribute : Attribute
    {
    }
}
";

        private const string DisallowNullAttributeSource = @"// <auto-generated/>

#nullable enable

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>Specifies that null is disallowed as an input even if the corresponding type allows it.</summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
    internal sealed class DisallowNullAttribute : Attribute
    {
    }
}
";

        private const string MaybeNullAttributeSource = @"// <auto-generated/>

#nullable enable

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>Specifies that an output may be null even if the corresponding type disallows it.</summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false)]
    internal sealed class MaybeNullAttribute : Attribute
    {
    }
}
";

        private const string NotNullAttributeSource = @"// <auto-generated/>

#nullable enable

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>Specifies that an output will not be null even if the corresponding type allows it.</summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false)]
    internal sealed class NotNullAttribute : Attribute
    {
    }
}
";

        private const string MaybeNullWhenAttributeSource = @"// <auto-generated/>

#nullable enable

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>Specifies that when a method returns <see cref=""ReturnValue""/>, the parameter may be null even if the corresponding type disallows it.</summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class MaybeNullWhenAttribute : Attribute
    {
        /// <summary>Initializes the attribute with the specified return value condition.</summary>
        /// <param name=""returnValue"">
        /// The return value condition. If the method returns this value, the associated parameter may be null.
        /// </param>
        public MaybeNullWhenAttribute(bool returnValue) => ReturnValue = returnValue;

        /// <summary>Gets the return value condition.</summary>
        public bool ReturnValue { get; }
    }
}
";

        private const string NotNullWhenAttributeSource = @"// <auto-generated/>

#nullable enable

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>Specifies that when a method returns <see cref=""ReturnValue""/>, the parameter will not be null even if the corresponding type allows it.</summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class NotNullWhenAttribute : Attribute
    {
        /// <summary>Initializes the attribute with the specified return value condition.</summary>
        /// <param name=""returnValue"">
        /// The return value condition. If the method returns this value, the associated parameter will not be null.
        /// </param>
        public NotNullWhenAttribute(bool returnValue) => ReturnValue = returnValue;

        /// <summary>Gets the return value condition.</summary>
        public bool ReturnValue { get; }
    }
}
";

        private const string NotNullIfNotNullAttributeSource = @"// <auto-generated/>

#nullable enable

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>Specifies that the output will be non-null if the named parameter is non-null.</summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
    internal sealed class NotNullIfNotNullAttribute : Attribute
    {
        /// <summary>Initializes the attribute with the associated parameter name.</summary>
        /// <param name=""parameterName"">
        /// The associated parameter name.  The output will be non-null if the argument to the parameter specified is non-null.
        /// </param>
        public NotNullIfNotNullAttribute(string parameterName) => ParameterName = parameterName;

        /// <summary>Gets the associated parameter name.</summary>
        public string ParameterName { get; }
    }
}
";

        private const string DoesNotReturnAttributeSource = @"// <auto-generated/>

#nullable enable

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>Applied to a method that will never return under any circumstance.</summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    internal sealed class DoesNotReturnAttribute : Attribute
    {
    }
}
";

        private const string DoesNotReturnIfAttributeSource = @"// <auto-generated/>

#nullable enable

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>Specifies that the method will not return if the associated Boolean parameter is passed the specified value.</summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class DoesNotReturnIfAttribute : Attribute
    {
        /// <summary>Initializes the attribute with the specified parameter value.</summary>
        /// <param name=""parameterValue"">
        /// The condition parameter value. Code after the method will be considered unreachable by diagnostics if the argument to
        /// the associated parameter matches this value.
        /// </param>
        public DoesNotReturnIfAttribute(bool parameterValue) => ParameterValue = parameterValue;

        /// <summary>Gets the condition parameter value.</summary>
        public bool ParameterValue { get; }
    }
}
";

        private const string MemberNotNullAttributeSource = @"// <auto-generated/>

#nullable enable

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>Specifies that the method or property will ensure that the listed field and property members have not-null values.</summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    internal sealed class MemberNotNullAttribute : Attribute
    {
        /// <summary>Initializes the attribute with a field or property member.</summary>
        /// <param name=""member"">
        /// The field or property member that is promised to be not-null.
        /// </param>
        public MemberNotNullAttribute(string member) => Members = new[] { member };

        /// <summary>Initializes the attribute with the list of field and property members.</summary>
        /// <param name=""members"">
        /// The list of field and property members that are promised to be not-null.
        /// </param>
        public MemberNotNullAttribute(params string[] members) => Members = members;

        /// <summary>Gets field or property member names.</summary>
        public string[] Members { get; }
    }
}
";

        private const string MemberNotNullWhenAttributeSource = @"// <auto-generated/>

#nullable enable

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>Specifies that the method or property will ensure that the listed field and property members have not-null values when returning with the specified return value condition.</summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    internal sealed class MemberNotNullWhenAttribute : Attribute
    {
        /// <summary>Initializes the attribute with the specified return value condition and a field or property member.</summary>
        /// <param name=""returnValue"">
        /// The return value condition. If the method returns this value, the associated parameter will not be null.
        /// </param>
        /// <param name=""member"">
        /// The field or property member that is promised to be not-null.
        /// </param>
        public MemberNotNullWhenAttribute(bool returnValue, string member)
        {
            ReturnValue = returnValue;
            Members = new[] { member };
        }

        /// <summary>Initializes the attribute with the specified return value condition and list of field and property members.</summary>
        /// <param name=""returnValue"">
        /// The return value condition. If the method returns this value, the associated parameter will not be null.
        /// </param>
        /// <param name=""members"">
        /// The list of field and property members that are promised to be not-null.
        /// </param>
        public MemberNotNullWhenAttribute(bool returnValue, params string[] members)
        {
            ReturnValue = returnValue;
            Members = members;
        }

        /// <summary>Gets the return value condition.</summary>
        public bool ReturnValue { get; }

        /// <summary>Gets field or property member names.</summary>
        public string[] Members { get; }
    }
}
";

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var referencedTypesData = context.CompilationProvider.Select(
                (compilation, cancellationToken) =>
                {
                    var hasAllowNullAttribute = IsCodeAnalysisAttributeAvailable(compilation, "System.Diagnostics.CodeAnalysis.AllowNullAttribute");
                    var hasDisallowNullAttribute = IsCodeAnalysisAttributeAvailable(compilation, "System.Diagnostics.CodeAnalysis.DisallowNullAttribute");
                    var hasMaybeNullAttribute = IsCodeAnalysisAttributeAvailable(compilation, "System.Diagnostics.CodeAnalysis.MaybeNullAttribute");
                    var hasNotNullAttribute = IsCodeAnalysisAttributeAvailable(compilation, "System.Diagnostics.CodeAnalysis.NotNullAttribute");
                    var hasMaybeNullWhenAttribute = IsCodeAnalysisAttributeAvailable(compilation, "System.Diagnostics.CodeAnalysis.MaybeNullWhenAttribute");
                    var hasNotNullWhenAttribute = IsCodeAnalysisAttributeAvailable(compilation, "System.Diagnostics.CodeAnalysis.NotNullWhenAttribute");
                    var hasNotNullIfNotNullAttribute = IsCodeAnalysisAttributeAvailable(compilation, "System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute");
                    var hasDoesNotReturnAttribute = IsCodeAnalysisAttributeAvailable(compilation, "System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute");
                    var hasDoesNotReturnIfAttribute = IsCodeAnalysisAttributeAvailable(compilation, "System.Diagnostics.CodeAnalysis.DoesNotReturnIfAttribute");
                    var hasMemberNotNullAttribute = IsCodeAnalysisAttributeAvailable(compilation, "System.Diagnostics.CodeAnalysis.MemberNotNullAttribute");
                    var hasMemberNotNullWhenAttribute = IsCodeAnalysisAttributeAvailable(compilation, "System.Diagnostics.CodeAnalysis.MemberNotNullWhenAttribute");

                    return new ReferencedTypesData(
                        hasAllowNullAttribute: hasAllowNullAttribute,
                        hasDisallowNullAttribute: hasDisallowNullAttribute,
                        hasMaybeNullAttribute: hasMaybeNullAttribute,
                        hasNotNullAttribute: hasNotNullAttribute,
                        hasMaybeNullWhenAttribute: hasMaybeNullWhenAttribute,
                        hasNotNullWhenAttribute: hasNotNullWhenAttribute,
                        hasNotNullIfNotNullAttribute: hasNotNullIfNotNullAttribute,
                        hasDoesNotReturnAttribute: hasDoesNotReturnAttribute,
                        hasDoesNotReturnIfAttribute: hasDoesNotReturnIfAttribute,
                        hasMemberNotNullAttribute: hasMemberNotNullAttribute,
                        hasMemberNotNullWhenAttribute: hasMemberNotNullWhenAttribute);
                });

            context.RegisterSourceOutput(
                referencedTypesData,
                (context, referencedTypesData) =>
                {
                    var forwarders = new List<string>();

                    if (referencedTypesData.HasAllowNullAttribute == TypeDefinitionLocation.None)
                    {
                        context.AddSource("AllowNullAttribute.g.cs", AllowNullAttributeSource.ReplaceLineEndings("\r\n"));
                    }
                    else if (referencedTypesData.HasAllowNullAttribute == TypeDefinitionLocation.Referenced)
                    {
                        forwarders.Add("AllowNullAttribute");
                    }

                    if (referencedTypesData.HasDisallowNullAttribute == TypeDefinitionLocation.None)
                    {
                        context.AddSource("DisallowNullAttribute.g.cs", DisallowNullAttributeSource.ReplaceLineEndings("\r\n"));
                    }
                    else if (referencedTypesData.HasDisallowNullAttribute == TypeDefinitionLocation.Referenced)
                    {
                        forwarders.Add("DisallowNullAttribute");
                    }

                    if (referencedTypesData.HasMaybeNullAttribute == TypeDefinitionLocation.None)
                    {
                        context.AddSource("MaybeNullAttribute.g.cs", MaybeNullAttributeSource.ReplaceLineEndings("\r\n"));
                    }
                    else if (referencedTypesData.HasMaybeNullAttribute == TypeDefinitionLocation.Referenced)
                    {
                        forwarders.Add("MaybeNullAttribute");
                    }

                    if (referencedTypesData.HasNotNullAttribute == TypeDefinitionLocation.None)
                    {
                        context.AddSource("NotNullAttribute.g.cs", NotNullAttributeSource.ReplaceLineEndings("\r\n"));
                    }
                    else if (referencedTypesData.HasNotNullAttribute == TypeDefinitionLocation.Referenced)
                    {
                        forwarders.Add("NotNullAttribute");
                    }

                    if (referencedTypesData.HasMaybeNullWhenAttribute == TypeDefinitionLocation.None)
                    {
                        context.AddSource("MaybeNullWhenAttribute.g.cs", MaybeNullWhenAttributeSource.ReplaceLineEndings("\r\n"));
                    }
                    else if (referencedTypesData.HasMaybeNullWhenAttribute == TypeDefinitionLocation.Referenced)
                    {
                        forwarders.Add("MaybeNullWhenAttribute");
                    }

                    if (referencedTypesData.HasNotNullWhenAttribute == TypeDefinitionLocation.None)
                    {
                        context.AddSource("NotNullWhenAttribute.g.cs", NotNullWhenAttributeSource.ReplaceLineEndings("\r\n"));
                    }
                    else if (referencedTypesData.HasNotNullWhenAttribute == TypeDefinitionLocation.Referenced)
                    {
                        forwarders.Add("NotNullWhenAttribute");
                    }

                    if (referencedTypesData.HasNotNullIfNotNullAttribute == TypeDefinitionLocation.None)
                    {
                        context.AddSource("NotNullIfNotNullAttribute.g.cs", NotNullIfNotNullAttributeSource.ReplaceLineEndings("\r\n"));
                    }
                    else if (referencedTypesData.HasNotNullIfNotNullAttribute == TypeDefinitionLocation.Referenced)
                    {
                        forwarders.Add("NotNullIfNotNullAttribute");
                    }

                    if (referencedTypesData.HasDoesNotReturnAttribute == TypeDefinitionLocation.None)
                    {
                        context.AddSource("DoesNotReturnAttribute.g.cs", DoesNotReturnAttributeSource.ReplaceLineEndings("\r\n"));
                    }
                    else if (referencedTypesData.HasDoesNotReturnAttribute == TypeDefinitionLocation.Referenced)
                    {
                        forwarders.Add("DoesNotReturnAttribute");
                    }

                    if (referencedTypesData.HasDoesNotReturnIfAttribute == TypeDefinitionLocation.None)
                    {
                        context.AddSource("DoesNotReturnIfAttribute.g.cs", DoesNotReturnIfAttributeSource.ReplaceLineEndings("\r\n"));
                    }
                    else if (referencedTypesData.HasDoesNotReturnIfAttribute == TypeDefinitionLocation.Referenced)
                    {
                        forwarders.Add("DoesNotReturnIfAttribute");
                    }

                    if (referencedTypesData.HasMemberNotNullAttribute == TypeDefinitionLocation.None)
                    {
                        context.AddSource("MemberNotNullAttribute.g.cs", MemberNotNullAttributeSource.ReplaceLineEndings("\r\n"));
                    }
                    else if (referencedTypesData.HasMemberNotNullAttribute == TypeDefinitionLocation.Referenced)
                    {
                        forwarders.Add("MemberNotNullAttribute");
                    }

                    if (referencedTypesData.HasMemberNotNullWhenAttribute == TypeDefinitionLocation.None)
                    {
                        context.AddSource("MemberNotNullWhenAttribute.g.cs", MemberNotNullWhenAttributeSource.ReplaceLineEndings("\r\n"));
                    }
                    else if (referencedTypesData.HasMemberNotNullWhenAttribute == TypeDefinitionLocation.Referenced)
                    {
                        forwarders.Add("MemberNotNullWhenAttribute");
                    }

                    if (forwarders.Count > 0)
                    {
                        var nullableForwarders = $@"// <auto-generated/>

#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

{string.Join("\r\n", forwarders.Select(forwarder => $"[assembly: TypeForwardedTo(typeof({forwarder}))]"))}
";

                        context.AddSource("NullableForwarders.g.cs", nullableForwarders.ReplaceLineEndings("\r\n"));
                    }
                });
        }

        private static TypeDefinitionLocation IsCodeAnalysisAttributeAvailable(Compilation compilation, string fullyQualifiedMetadataName)
        {
            return compilation.GetBestTypeByMetadataName(fullyQualifiedMetadataName, requiresAccess: true) switch
            {
                { OriginalDefinition.ContainingAssembly: var containingAssembly } when SymbolEqualityComparer.Default.Equals(compilation.Assembly, containingAssembly) => TypeDefinitionLocation.Defined,
                { } => TypeDefinitionLocation.Referenced,
                _ => TypeDefinitionLocation.None,
            };
        }

        private sealed class ReferencedTypesData
        {
            public ReferencedTypesData(
                TypeDefinitionLocation hasAllowNullAttribute,
                TypeDefinitionLocation hasDisallowNullAttribute,
                TypeDefinitionLocation hasMaybeNullAttribute,
                TypeDefinitionLocation hasNotNullAttribute,
                TypeDefinitionLocation hasMaybeNullWhenAttribute,
                TypeDefinitionLocation hasNotNullWhenAttribute,
                TypeDefinitionLocation hasNotNullIfNotNullAttribute,
                TypeDefinitionLocation hasDoesNotReturnAttribute,
                TypeDefinitionLocation hasDoesNotReturnIfAttribute,
                TypeDefinitionLocation hasMemberNotNullAttribute,
                TypeDefinitionLocation hasMemberNotNullWhenAttribute)
            {
                HasAllowNullAttribute = hasAllowNullAttribute;
                HasDisallowNullAttribute = hasDisallowNullAttribute;
                HasMaybeNullAttribute = hasMaybeNullAttribute;
                HasNotNullAttribute = hasNotNullAttribute;
                HasMaybeNullWhenAttribute = hasMaybeNullWhenAttribute;
                HasNotNullWhenAttribute = hasNotNullWhenAttribute;
                HasNotNullIfNotNullAttribute = hasNotNullIfNotNullAttribute;
                HasDoesNotReturnAttribute = hasDoesNotReturnAttribute;
                HasDoesNotReturnIfAttribute = hasDoesNotReturnIfAttribute;
                HasMemberNotNullAttribute = hasMemberNotNullAttribute;
                HasMemberNotNullWhenAttribute = hasMemberNotNullWhenAttribute;
            }

            public TypeDefinitionLocation HasAllowNullAttribute { get; }

            public TypeDefinitionLocation HasDisallowNullAttribute { get; }

            public TypeDefinitionLocation HasMaybeNullAttribute { get; }

            public TypeDefinitionLocation HasNotNullAttribute { get; }

            public TypeDefinitionLocation HasMaybeNullWhenAttribute { get; }

            public TypeDefinitionLocation HasNotNullWhenAttribute { get; }

            public TypeDefinitionLocation HasNotNullIfNotNullAttribute { get; }

            public TypeDefinitionLocation HasDoesNotReturnAttribute { get; }

            public TypeDefinitionLocation HasDoesNotReturnIfAttribute { get; }

            public TypeDefinitionLocation HasMemberNotNullAttribute { get; }

            public TypeDefinitionLocation HasMemberNotNullWhenAttribute { get; }
        }
    }
}
