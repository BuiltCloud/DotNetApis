﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DotNetApis.Structure
{
    /// <summary>
    /// Structured documentation for a pointer type.
    /// </summary>
    public sealed class PointerTypeReference : ITypeReference
    {
        public EntityReferenceKind Kind => EntityReferenceKind.Pointer;

        /// <summary>
        /// The type that this is a pointer to.
        /// </summary>
        [JsonProperty("t")]
        public ITypeReference ElementType { get; set; }
    }
}
