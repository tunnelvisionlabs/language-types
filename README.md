# Language Types Source Generator

A source generator providing types necessary for recent C# language features.

[![NuGet package](https://img.shields.io/nuget/v/TunnelVisionLabs.LanguageTypes.svg)](https://nuget.org/packages/TunnelVisionLabs.LanguageTypes)

## Features

* `NullableSourceGenerator`: Generates attributes for using Nullable Reference Types (C# 8 for most attributes, C# 9 for `MemberNotNullAttribute` and `MemberNotNullWhenAttribute`)
* `IndexRangeSourceGenerator`: Generates `System.Index` and `System.Range` for improved indexing, slicing, and substrings (C# 8)
* `IsExternalInitSourceGenerator`: Generates `IsExternalInit` for using records and init-only properties (C# 9)

## Installation and Use

This package can be installed by adding a package reference to **TunnelVisionLabs.LanguageTypes**. Current requirements:

* C# project
* PackageReference for NuGet dependencies
* Compiler version 4.0 or higher (included with Visual Studio 2022 and newer)
