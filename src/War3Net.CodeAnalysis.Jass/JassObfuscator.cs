// ------------------------------------------------------------------------------
// <copyright file="JassObfuscator.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
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

            File.Delete(outputFile);
            using (var fileStream = File.OpenWrite(outputFile))
            {
                using (var streamWriter = new StreamWriter(fileStream, new UTF8Encoding(false, true)))
                {
                    var renderer = new JassRenderer(streamWriter);
                    renderer.SetNewlineString(true, false);
                    renderer.Comments = false;
                    renderer.Indentation = 0;
                    renderer.OptionalWhitespace = false;
                    renderer.OmitEmptyLines = true;
                    renderer.InlineConstants = true;

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

                    renderer.SetIdentifierOptimizerMethod(
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

                    renderer.Render(fileSyntax);
                }
            }
        }
    }
}