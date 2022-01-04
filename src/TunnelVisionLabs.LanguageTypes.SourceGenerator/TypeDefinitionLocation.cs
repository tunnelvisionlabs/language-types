// Copyright (c) Tunnel Vision Laboratories, LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace TunnelVisionLabs.LanguageTypes.SourceGenerator
{
    internal enum TypeDefinitionLocation
    {
        /// <summary>
        /// The type is not defined as part of the current compilation or its references.
        /// </summary>
        None,

        /// <summary>
        /// The type is defined in a reference of the current compilation.
        /// </summary>
        Referenced,

        /// <summary>
        /// The type is defined in the current compilation.
        /// </summary>
        Defined,
    }
}
