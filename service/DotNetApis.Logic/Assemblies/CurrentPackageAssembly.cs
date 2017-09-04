﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DotNetApis.Nuget;
using DotNetApis.Structure;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace DotNetApis.Logic.Assemblies
{
    /// <summary>
    /// An assembly in the NuGet package that we are generating documentation for.
    /// </summary>
    public sealed class CurrentPackageAssembly : PackageAssemblyBase
    {
        private readonly Lazy<XDocument> _xmldoc;

        /// <summary>
        /// Initializes the assembly.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="path">The path of the assembly. This can include path segments, the file name, and the extension.</param>
        /// <param name="readerParameters">The parameters used when processing the assembly by Cecil.</param>
        /// <param name="xmldocIdToDnaId">A reference to the shared xmldoc to dnaid mapping, which is updated when the assembly is processed.</param>
        /// <param name="package">The package conaining the assembly.</param>
        public CurrentPackageAssembly(ILogger logger, string path, ReaderParameters readerParameters, IDictionary<string, string> xmldocIdToDnaId, NugetPackage package)
            : base(logger, path, readerParameters, xmldocIdToDnaId, package)
        {
            var xmlPath = System.IO.Path.ChangeExtension(path, "xml");
            _xmldoc = new Lazy<XDocument>(() =>
            {
                try
                {
                    var stream = Package.ReadFile(xmlPath);
                    return XDocument.Load(stream);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(0, ex, "Unable to load xmldoc file {path}", xmlPath);
                    return null;
                }
            });
        }

        protected override ILocation Location(string dnaid) => new CurrentPackageLocation { DnaId = dnaid };

        /// <summary>
        /// Gets the XML documentation for this assembly. May be <c>null</c>.
        /// </summary>
        public XDocument Xmldoc => _xmldoc.Value;
    }
}