﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetApis.Structure
{
    /// <summary>
    /// The literal value <c>null</c>.
    /// </summary>
    public sealed class NullLiteral : ILiteral
    {
        public EntityLiteralKind Kind => EntityLiteralKind.Null;
    }
}
