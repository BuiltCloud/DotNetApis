﻿using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetApis.Common;
using DotNetApis.Structure;
using DotNetApis.Structure.Locations;
using Microsoft.Extensions.Logging;

namespace DotNetApis.Logic.Assemblies
{
    /// <summary>
    /// A base type for assemblies that can be read by streams, providing lazy loading for both the assembly and its dnaid lookups.
    /// </summary>
    public abstract class AssemblyBase : IAssembly
    {
        private readonly string _path;
        private readonly Lazy<AssemblyDefinition> _assemblyDefinition;
        private readonly Lazy<Dictionary<string, FriendlyName>> _dnaIdToFriendlyName;

        /// <summary>
        /// Initializes the base type.
        /// </summary>
        /// <param name="logger">The logger. This base type reserves the event ids 1-100.</param>
        /// <param name="path">The path of the assembly. This can include path segments, the file name, and the extension.</param>
        /// <param name="readerParameters">The parameters used when processing the assembly by Cecil.</param>
        /// <param name="xmldocIdToDnaId">A reference to the shared xmldoc to dnaid mapping, which is updated when the assembly is processed.</param>
        protected AssemblyBase(ILogger<AssemblyBase> logger, string path, ReaderParameters readerParameters, IDictionary<string, string> xmldocIdToDnaId)
        {
            _path = path;
            Name = Path.GetFileNameWithoutExtension(path);
            _assemblyDefinition = new Lazy<AssemblyDefinition>(() =>
            {
                try
                {
                    if (_path.EndsWith(".xml", StringComparison.InvariantCultureIgnoreCase))
                        return null;

                    // Cecil requires a seekable stream to read the file correctly.
                    var source = Read();
                    if (!source.CanSeek)
                    {
                        var stream = new MemoryStream();
                        source.CopyTo(stream);
                        stream.Position = 0;
                        source = stream;
                    }
                    return AssemblyDefinition.ReadAssembly(source, readerParameters);
                }
                catch (Exception ex)
                {
                    logger.AssemblyLoadFailed(GetType().Name, path, ex);
                    return null;
                }
            });
            _dnaIdToFriendlyName = new Lazy<Dictionary<string, FriendlyName>>(() =>
            {
                try
                {
                    var result = new Dictionary<string, FriendlyName>();
                    if (AssemblyDefinition == null)
                        return null;
                    new AssemblyIndexer(result, xmldocIdToDnaId).AddExposed(AssemblyDefinition);
                    return result;
                }
                catch (Exception ex)
                {
                    logger.AssemblyProcessingFailed(GetType().Name, path, ex);
                    return null;
                }
            });
        }

        /// <summary>
        /// Reads the assembly binary as a stream.
        /// </summary>
        protected abstract Stream Read();

        /// <summary>
        /// Get a location structure for a given dnaid.
        /// </summary>
        /// <param name="dnaid">The dnaid to locate.</param>
        protected abstract ILocation Location(string dnaid);

        public string Name { get; }

        public AssemblyDefinition AssemblyDefinition => _assemblyDefinition.Value;

        public (ILocation Location, FriendlyName FriendlyName)? TryGetDnaIdLocationAndFriendlyName(string dnaId)
        {
            // If the assembly has not been demand-loaded yet, return null.
            if (!_assemblyDefinition.IsValueCreated)
                return null;

            // If there was a problem loading the assembly, return null.
            if (_dnaIdToFriendlyName.Value == null)
                return null;

            return _dnaIdToFriendlyName.Value.ContainsKey(dnaId) ? (Location(dnaId), _dnaIdToFriendlyName.Value[dnaId]) : ((ILocation, FriendlyName)?)null;
        }

        public override string ToString() => _path;
    }

    internal static partial class Logging
    {
        public static void AssemblyLoadFailed(this ILogger<AssemblyBase> logger, string type, string path, Exception exception) =>
            Logger.Log(logger, 1, LogLevel.Warning, "Unable to load {type} assembly from {path}", type, path, exception);

        public static void AssemblyProcessingFailed(this ILogger<AssemblyBase> logger, string type, string path, Exception exception) =>
            Logger.Log(logger, 2, LogLevel.Warning, "Unable to process assembly {type} from {path}", type, path, exception);
    }
}
