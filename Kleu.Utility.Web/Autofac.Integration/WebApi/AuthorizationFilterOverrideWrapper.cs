﻿// This software is part of the Autofac IoC container
// Copyright (c) 2013 Autofac Contributors
// http://autofac.org
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Security;
using System.Web.Http.Filters;

// ReSharper disable once CheckNamespace
namespace Autofac.Integration.WebApi
{
    /// <summary>
    /// Resolves a filter override for the specified metadata for each controller request.
    /// </summary>
    [SecurityCritical]
    internal sealed class AuthorizationFilterOverrideWrapper : AuthorizationFilterWrapper, IOverrideFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationFilterOverrideWrapper"/> class.
        /// </summary>
        /// <param name="filterMetadata">The filter metadata.</param>
        public AuthorizationFilterOverrideWrapper(FilterMetadata filterMetadata) : base(filterMetadata)
        {
        }

        /// <summary>
        /// Gets the metadata key used to retrieve the filter metadata.
        /// </summary>
        public override string MetadataKey
        {
            [SecurityCritical]
            get { return AutofacWebApiFilterProvider.AuthorizationFilterOverrideMetadataKey; }
        }

        /// <summary>
        /// Gets the filters to override.
        /// </summary>
        public Type FiltersToOverride
        {
            [SecurityCritical]
            get { return typeof(IAuthorizationFilter); }
        }
    }
}
