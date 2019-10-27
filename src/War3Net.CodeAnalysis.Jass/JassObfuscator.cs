// ------------------------------------------------------------------------------
// <copyright file="JassObfuscator.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Text;

using War3Net.CodeAnalysis.Jass.Renderer;

namespace War3Net.CodeAnalysis.Jass
{
    public static class JassObfuscator
    {
        public static void Obfuscate(string inputFile, string outputFile, params string[] referenceFiles)
        {
            var fileSyntax = JassParser.ParseFile(inputFile);

            if (!Directory.Exists(new FileInfo(outputFile).DirectoryName))
            {
                Directory.CreateDirectory(new FileInfo(outputFile).DirectoryName);
            }

            var renderOptions = new JassRendererOptions();
            renderOptions.SetNewlineString(true, false);
            renderOptions.Comments = false;
            renderOptions.Indentation = 0;
            renderOptions.OptionalWhitespace = false;
            renderOptions.OmitEmptyLines = true;
            renderOptions.InlineConstants = true;

            var renameDictionary = new Dictionary<string, string>();
            var exceptions = new HashSet<string>();
            exceptions.Add("main");
            exceptions.Add("config");

            foreach (var referenceFile in referenceFiles)
            {
                foreach (var identifier in IdentifiersProvider.GetIdentifiers(referenceFile))
                {
                    exceptions.Add(identifier);
                }
            }

            renderOptions.SetIdentifierOptimizerMethod(
            (s) =>
            {
                // Make exceptions for init stuff, since these are called using ExecuteFunc
                if (exceptions.Contains(s) || s.StartsWith("jasshelper__initstructs") || s.EndsWith("__onInit"))
                {
                    return s;
                }

                if (!renameDictionary.ContainsKey(s))
                {
                    var renamed = $"j_{renameDictionary.Count}";
                    renameDictionary.Add(s, renamed);
                    return renamed;
                }

                return renameDictionary[s];
            });

            using (var fileStream = File.Create(outputFile))
            {
                using (var streamWriter = new StreamWriter(fileStream, new UTF8Encoding(false, true)))
                {
                    var renderer = new JassRenderer(streamWriter);
                    renderer.Options = renderOptions;
                    renderer.Render(fileSyntax);
                }
            }
        }
    }
}