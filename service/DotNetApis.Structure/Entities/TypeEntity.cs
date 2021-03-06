﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetApis.Structure.TypeReferences;
using DotNetApis.Structure.Xmldoc;
using Newtonsoft.Json;

namespace DotNetApis.Structure.Entities
{
    /// <summary>
    /// Structured documentation for a class, struct, or interface.
    /// </summary>
    public sealed class TypeEntity : IEntity, IHaveNamespace, IHaveGenericParameters
    {
        public EntityKind Kind { get; set; }
        public string DnaId { get; set; }
        public string Name { get; set; }
        public IReadOnlyList<AttributeJson> Attributes { get; set; }
        public EntityAccessibility Accessibility { get; set; }
        public EntityModifiers Modifiers { get; set; }
        public Xmldoc.Xmldoc Xmldoc { get; set; }
        public string Namespace { get; set; }
        public IReadOnlyList<GenericParameterJson> GenericParameters { get; set; }

        /// <summary>
        /// The base type (if any) and interfaces inherited by this type.
        /// </summary>
        [JsonProperty("t")]
        public IReadOnlyList<ITypeReference> BaseTypeAndInterfaces { get; set; }

        /// <summary>
        /// The members of this entity, already grouped and sorted.
        /// </summary>
        [JsonProperty("e")]
        public TypeEntityMemberGrouping Members { get; set; }

        public override string ToString()
        {
            var result = Namespace == null ? Name : Namespace + "." + Name;
            if (GenericParameters.Count != 0)
                result += "<" + string.Join(",", GenericParameters) + ">";
            return result;
        }
    }
}
