using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.Build.Script;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapScriptBuilder"/> class.
        /// </summary>
        public MapScriptBuilder()
            : this(TriggerData.Default)
        {
        }

        public MapScriptBuilder(TriggerData triggerData)
        {
            TriggerData = triggerData;
            LobbyMusic = null;
            MaxPlayerSlots = 24;
            ForceGenerateGlobalUnitVariable = false;
            ForceGenerateGlobalDestructableVariable = false;
            ForceGenerateUnitWithSkin = false;
            ForceGenerateDestructableWithSkin = false;
            UseCSharpLua = false;
            UseLifeVariable = true;
            UseWeatherEffectVariable = true;
            TriggerVariableReferences = new(StringComparer.Ordinal);
        }

        public TriggerData TriggerData { get; set; }

        public string? LobbyMusic { get; set; }

        public int MaxPlayerSlots { get; set; }

        public bool ForceGenerateGlobalUnitVariable { get; set; }

        public bool ForceGenerateGlobalDestructableVariable { get; set; }

        public bool ForceGenerateUnitWithSkin { get; set; }

        public bool ForceGenerateDestructableWithSkin { get; set; }

        public bool UseCSharpLua { get; set; }

        public bool UseLifeVariable { get; set; }

        public bool UseWeatherEffectVariable { get; set; }

        public Dictionary<string, bool> TriggerVariableReferences { get; }

        public virtual void SetDefaultOptionsForCSharpLua(string? lobbyMusic = null)
        {
            LobbyMusic = lobbyMusic;
            MaxPlayerSlots = 24;
            ForceGenerateGlobalUnitVariable = true;
            ForceGenerateGlobalDestructableVariable = true;
            ForceGenerateUnitWithSkin = false;
            ForceGenerateDestructableWithSkin = false;
            UseCSharpLua = true;
            UseLifeVariable = false;
            UseWeatherEffectVariable = false;
        }

        public virtual void SetDefaultOptionsForMap(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            LobbyMusic = null;
            MaxPlayerSlots = map.Info is null || map.Info.EditorVersion >= EditorVersion.v6060 ? 24 : 12;
            ForceGenerateGlobalUnitVariable = false;
            ForceGenerateGlobalDestructableVariable = false;
            ForceGenerateUnitWithSkin = map.Info is not null && map.Info.FormatVersion >= MapInfoFormatVersion.v31;
            ForceGenerateDestructableWithSkin = map.Info is not null && map.Info.FormatVersion >= MapInfoFormatVersion.v31;
            UseCSharpLua = false;
            UseLifeVariable = true;
            UseWeatherEffectVariable = true;
        }

        public string Build(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            using var stringWriter = new StringWriter();
            stringWriter.NewLine = JassSymbol.CarriageReturnLineFeed;
            using var writer = new IndentedTextWriter(stringWriter);

            Build(map, writer);

            return stringWriter.ToString();
        }

        public virtual void Build(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var commentLine1 = "//===========================================================================";
            var commentLine2 = "//***************************************************************************";
            var commentLine3 = "//*";

            void WriteBanner(string bannerText)
            {
                writer.WriteLine(commentLine2);
                writer.WriteLine(commentLine3);
                writer.WriteLine($"{commentLine3}  {bannerText}");
                writer.WriteLine(commentLine3);
                writer.WriteLine(commentLine2);
                writer.WriteLine();
            }

            void WriteBannerAndFunction(string bannerText, Action<Map, IndentedTextWriter> function, Func<Map, bool> condition, bool includeCommentLine = false)
            {
                if (condition(map))
                {
                    WriteBanner(bannerText);
                    if (includeCommentLine)
                    {
                        writer.WriteLine(commentLine1);
                    }

                    function.Invoke(map, writer);
                    writer.WriteLine();
                }
            }

            void WriteBannerAndFunctions(string bannerText, Action<Map, IndentedTextWriter> functions, Func<Map, bool> condition)
            {
                if (condition(map))
                {
                    WriteBanner(bannerText);
                    functions.Invoke(map, writer);
                }
            }

            void WriteFunction(Action<Map, IndentedTextWriter> function, Func<Map, bool> condition)
            {
                if (condition(map))
                {
                    writer.WriteLine(commentLine1);
                    function.Invoke(map, writer);
                    writer.WriteLine();
                }
            }

            void WriteFunctionForIndex(int index, Action<Map, int, IndentedTextWriter> function, Func<Map, int, bool> condition)
            {
                if (condition(map, index))
                {
                    writer.WriteLine(commentLine1);
                    function.Invoke(map, index, writer);
                    writer.WriteLine();
                }
            }

            WriteMapScriptHeader(map, writer);
            writer.WriteLine();

            WriteBanner("Global Variables");

            GenerateGlobals(map, writer);
            writer.WriteLine();

            if (ShouldGenerateInitGlobals(map))
            {
                GenerateInitGlobals(map, writer);
                writer.WriteLine();
            }

            WriteBanner("Custom Script Code");
            WriteBannerAndFunction("Random Groups", GenerateInitRandomGroups, ShouldGenerateInitRandomGroups);
            WriteBannerAndFunctions("Map Item Tables", GenerateMapItemTables, ShouldGenerateMapItemTables);
            WriteBannerAndFunction("Items", GenerateCreateAllItems, ShouldGenerateCreateAllItems);
            WriteBannerAndFunctions("Unit Item Tables", GenerateUnitItemTables, ShouldGenerateUnitItemTables);
            WriteBannerAndFunctions("Destructable Item Tables", GenerateDestructableItemTables, ShouldGenerateDestructableItemTables);
            WriteBannerAndFunction("Sounds", GenerateInitSounds, ShouldGenerateInitSounds);
            WriteBannerAndFunction("Destructable Objects", GenerateCreateAllDestructables, ShouldGenerateCreateAllDestructables);

            if (ShouldGenerateCreateAllUnits(map))
            {
                WriteBanner("Unit Creation");

                foreach (var i in Enumerable.Range(0, MaxPlayerSlots))
                {
                    WriteFunctionForIndex(i, GenerateCreateBuildingsForPlayer, ShouldGenerateCreateBuildingsForPlayer);
                    WriteFunctionForIndex(i, GenerateCreateUnitsForPlayer, ShouldGenerateCreateUnitsForPlayer);
                }

                WriteFunction(GenerateCreateNeutralHostile, ShouldGenerateCreateNeutralHostile);
                WriteFunction(GenerateCreateNeutralPassiveBuildings, ShouldGenerateCreateNeutralPassiveBuildings);
                WriteFunction(GenerateCreateNeutralPassive, ShouldGenerateCreateNeutralPassive);
                WriteFunction(GenerateCreatePlayerBuildings, ShouldGenerateCreatePlayerBuildings);
                WriteFunction(GenerateCreatePlayerUnits, ShouldGenerateCreatePlayerUnits);
                WriteFunction(GenerateCreateNeutralUnits, ShouldGenerateCreateNeutralUnits);
                WriteFunction(GenerateCreateAllUnits, (map) => true);
            }

            WriteBannerAndFunction("Regions", GenerateCreateRegions, ShouldGenerateCreateRegions);
            WriteBannerAndFunction("Cameras", GenerateCreateCameras, ShouldGenerateCreateCameras);

            WriteBanner("Triggers");
            if (map.Triggers is not null)
            {
                foreach (var trigger in map.Triggers.TriggerItems)
                {
                    if (trigger is TriggerDefinition triggerDefinition &&
                        ShouldRenderTrigger(map, triggerDefinition))
                    {
                        var triggerRenderer = new TriggerRenderer(writer, TriggerData, map.Triggers.Variables, isLuaTrigger: false);
                        triggerRenderer.RenderTrigger(triggerDefinition);
                        writer.WriteLine();
                    }
                }

                WriteFunction(GenerateInitCustomTriggers, ShouldGenerateInitCustomTriggers);
                WriteFunction(GenerateRunInitializationTriggers, ShouldGenerateRunInitializationTriggers);
            }

            if (ShouldGenerateInitUpgrades(map))
            {
                WriteBanner("Upgrades");

                foreach (var i in Enumerable.Range(0, MaxPlayerSlots))
                {
                    if (ShouldGenerateInitUpgradesForPlayer(map, i))
                    {
                        GenerateInitUpgradesForPlayer(map, i, writer);
                        writer.WriteLine();
                    }
                }

                GenerateInitUpgrades(map, writer);
                writer.WriteLine();
            }

            if (ShouldGenerateInitTechTree(map))
            {
                WriteBanner("TechTree");

                foreach (var i in Enumerable.Range(0, MaxPlayerSlots))
                {
                    if (ShouldGenerateInitTechTreeForPlayer(map, i))
                    {
                        GenerateInitTechTreeForPlayer(map, i, writer);
                        writer.WriteLine();
                    }
                }

                GenerateInitTechTree(map, writer);
                writer.WriteLine();
            }

            WriteBanner("Players");

            if (ShouldGenerateInitCustomPlayerSlots(map))
            {
                GenerateInitCustomPlayerSlots(map, writer);
                writer.WriteLine();
            }

            if (ShouldGenerateInitCustomTeams(map))
            {
                GenerateInitCustomTeams(map, writer);
                writer.WriteLine();
            }

            if (ShouldGenerateInitAllyPriorities(map))
            {
                var ids = Enumerable.Range(0, MaxPlayerSlots).ToArray();
                if (map.Info.Players.Any(p => ids.Any(id => p.AllyLowPriorityFlags[id] || p.AllyHighPriorityFlags[id])))
                {
                    GenerateInitAllyPriorities(map, writer);
                    writer.WriteLine();
                }
            }

            WriteBannerAndFunction("Main Initialization", GenerateMain, ShouldGenerateMain, true);
            WriteBannerAndFunction("Map Configuration", GenerateConfig, ShouldGenerateConfig);
        }

        protected internal virtual void WriteMapScriptHeader(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var mapInfo = map.Info;
            var mapTriggerStrings = map.TriggerStrings;

            writer.WriteLine("//===========================================================================");
            writer.WriteLine("// ");
            writer.WriteLine($"// {mapInfo.MapName.Localize(mapTriggerStrings)}");
            writer.WriteLine("// ");
            writer.WriteLine("//   Warcraft III map script");
            writer.WriteLine($"//   Generated by {Assembly.GetExecutingAssembly().GetName().Name}");
            writer.WriteLine($"//   Date: {DateTime.Now:ddd MMM dd HH:mm:ss yyyy}");
            writer.WriteLine($"//   Map Author: {mapInfo.MapAuthor.Localize(mapTriggerStrings)}");
            writer.WriteLine("// ");
            writer.WriteLine("//===========================================================================");
        }
    }
}