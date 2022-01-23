// ------------------------------------------------------------------------------
// <copyright file="MapScriptBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

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
            UnitTriggerReferences = new(StringComparer.Ordinal);
            DestructableTriggerReferences = new(StringComparer.Ordinal);
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

        public HashSet<string> UnitTriggerReferences { get; set; }

        public HashSet<string> DestructableTriggerReferences { get; set; }

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
            MaxPlayerSlots = map.Info is null || map.Info.FormatVersion >= MapInfoFormatVersion.v26 ? 24 : 12;
            ForceGenerateGlobalUnitVariable = false;
            ForceGenerateGlobalDestructableVariable = false;
            ForceGenerateUnitWithSkin = map.Info is not null && map.Info.FormatVersion >= MapInfoFormatVersion.Reforged;
            ForceGenerateDestructableWithSkin = map.Info is not null && map.Info.FormatVersion >= MapInfoFormatVersion.Reforged;
            UseCSharpLua = false;
            UseLifeVariable = true;
            UseWeatherEffectVariable = true;
        }

        public virtual void FindTriggerUnitVariableReferencesForMap(Map map)
        {
            var mapTriggers = map.Triggers;
            if (mapTriggers is not null)
            {
                foreach (var trigger in mapTriggers.TriggerItems)
                {
                    if (trigger is TriggerDefinition triggerDefinition)
                    {
                        foreach (var function in triggerDefinition.Functions)
                        {
                            FindUnitVariableReferences(function);
                        }
                    }
                }
            }
        }

        public virtual JassCompilationUnitSyntax Build(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            JassCommentDeclarationSyntax commentLine1 = new("===========================================================================");
            JassCommentDeclarationSyntax commentLine2 = new("***************************************************************************");
            JassCommentDeclarationSyntax commentLine3 = new("*");

            List<IDeclarationSyntax> declarations = new();

            void AppendBanner(string bannerText)
            {
                declarations.Add(commentLine2);
                declarations.Add(commentLine3);
                declarations.Add(new JassCommentDeclarationSyntax($"*  {bannerText}"));
                declarations.Add(commentLine3);
                declarations.Add(commentLine2);
                declarations.Add(JassEmptyDeclarationSyntax.Value);
            }

            void AppendBannerAndFunction(string bannerText, Func<Map, JassFunctionDeclarationSyntax> function, Func<Map, bool> condition, bool includeCommentLine = false)
            {
                if (condition(map))
                {
                    AppendBanner(bannerText);
                    if (includeCommentLine)
                    {
                        declarations.Add(commentLine1);
                    }

                    declarations.Add(function(map));
                    declarations.Add(JassEmptyDeclarationSyntax.Value);
                }
            }

            void AppendBannerAndFunctions(string bannerText, Func<Map, IEnumerable<IDeclarationSyntax>> functions, Func<Map, bool> condition)
            {
                if (condition(map))
                {
                    AppendBanner(bannerText);
                    foreach (var function in functions(map))
                    {
                        declarations.Add(function);
                        declarations.Add(JassEmptyDeclarationSyntax.Value);
                    }
                }
            }

            void AppendFunction(Func<Map, JassFunctionDeclarationSyntax> function, Func<Map, bool> condition)
            {
                if (condition(map))
                {
                    declarations.Add(commentLine1);
                    declarations.Add(function(map));
                    declarations.Add(JassEmptyDeclarationSyntax.Value);
                }
            }

            void AppendFunctionForIndex(int index, Func<Map, int, JassFunctionDeclarationSyntax> function, Func<Map, int, bool> condition)
            {
                if (condition(map, index))
                {
                    declarations.Add(commentLine1);
                    declarations.Add(function(map, index));
                    declarations.Add(JassEmptyDeclarationSyntax.Value);
                }
            }

            declarations.AddRange(GetMapScriptHeader(map));
            declarations.Add(JassEmptyDeclarationSyntax.Value);

            AppendBanner("Global Variables");

            declarations.Add(Globals(map));
            declarations.Add(JassEmptyDeclarationSyntax.Value);

            if (InitGlobalsCondition(map))
            {
                declarations.Add(InitGlobals(map));
                declarations.Add(JassEmptyDeclarationSyntax.Value);
            }

            AppendBanner("Custom Script Code");
            AppendBannerAndFunction("Random Groups", InitRandomGroups, InitRandomGroupsCondition);
            AppendBannerAndFunctions("Map Item Tables", MapItemTables, MapItemTablesCondition);
            AppendBannerAndFunction("Items", CreateAllItems, CreateAllItemsCondition);
            AppendBannerAndFunctions("Unit Item Tables", UnitItemTables, UnitItemTablesCondition);
            AppendBannerAndFunctions("Destructable Item Tables", DestructableItemTables, DestructableItemTablesCondition);
            AppendBannerAndFunction("Sounds", InitSounds, InitSoundsCondition);
            AppendBannerAndFunction("Destructable Objects", CreateAllDestructables, CreateAllDestructablesCondition);

            if (CreateAllUnitsCondition(map))
            {
                AppendBanner("Unit Creation");

                foreach (var i in Enumerable.Range(0, MaxPlayerSlots))
                {
                    AppendFunctionForIndex(i, CreateBuildingsForPlayer, CreateBuildingsForPlayerCondition);
                    AppendFunctionForIndex(i, CreateUnitsForPlayer, CreateUnitsForPlayerCondition);
                }

                AppendFunction(CreateNeutralHostile, CreateNeutralHostileCondition);
                AppendFunction(CreateNeutralPassiveBuildings, CreateNeutralPassiveBuildingsCondition);
                AppendFunction(CreateNeutralPassive, CreateNeutralPassiveCondition);
                AppendFunction(CreatePlayerBuildings, CreatePlayerBuildingsCondition);
                AppendFunction(CreatePlayerUnits, CreatePlayerUnitsCondition);
                AppendFunction(CreateNeutralUnits, CreateNeutralUnitsCondition);
                AppendFunction(CreateAllUnits, (map) => true);
            }

            AppendBannerAndFunction("Regions", CreateRegions, CreateRegionsCondition);
            AppendBannerAndFunction("Cameras", CreateCameras, CreateCamerasCondition);

            AppendBanner("Triggers");
            if (map.Triggers is not null)
            {
                foreach (var trigger in map.Triggers.TriggerItems)
                {
                    if (trigger is TriggerDefinition triggerDefinition &&
                        InitTrigCondition(map, triggerDefinition))
                    {
                        declarations.Add(commentLine1);
                        declarations.Add(InitTrig(map, triggerDefinition));
                        declarations.Add(JassEmptyDeclarationSyntax.Value);
                    }
                }

                AppendFunction(InitCustomTriggers, InitCustomTriggersCondition);
                AppendFunction(RunInitializationTriggers, RunInitializationTriggersCondition);
            }

            if (InitUpgradesCondition(map))
            {
                AppendBanner("Upgrades");

                foreach (var i in Enumerable.Range(0, MaxPlayerSlots))
                {
                    if (InitUpgrades_PlayerCondition(map, i))
                    {
                        declarations.Add(InitUpgrades_Player(map, i));
                        declarations.Add(JassEmptyDeclarationSyntax.Value);
                    }
                }

                declarations.Add(InitUpgrades(map));
                declarations.Add(JassEmptyDeclarationSyntax.Value);
            }

            if (InitTechTreeCondition(map))
            {
                AppendBanner("TechTree");

                foreach (var i in Enumerable.Range(0, MaxPlayerSlots))
                {
                    if (InitTechTree_PlayerCondition(map, i))
                    {
                        declarations.Add(InitTechTree_Player(map, i));
                        declarations.Add(JassEmptyDeclarationSyntax.Value);
                    }
                }

                declarations.Add(InitTechTree(map));
                declarations.Add(JassEmptyDeclarationSyntax.Value);
            }

            AppendBanner("Players");

            if (InitCustomPlayerSlotsCondition(map))
            {
                declarations.Add(InitCustomPlayerSlots(map));
                declarations.Add(JassEmptyDeclarationSyntax.Value);
            }

            if (InitCustomTeamsCondition(map))
            {
                declarations.Add(InitCustomTeams(map));
                declarations.Add(JassEmptyDeclarationSyntax.Value);
            }

            if (InitAllyPrioritiesCondition(map))
            {
                var ids = Enumerable.Range(0, MaxPlayerSlots).ToArray();
                if (map.Info.Players.Any(p => ids.Any(id => p.AllyLowPriorityFlags[id] || p.AllyHighPriorityFlags[id])))
                {
                    declarations.Add(InitAllyPriorities(map));
                    declarations.Add(JassEmptyDeclarationSyntax.Value);
                }
            }

            AppendBannerAndFunction("Main Initialization", main, mainCondition, true);
            AppendBannerAndFunction("Map Configuration", config, configCondition);

            return SyntaxFactory.CompilationUnit(declarations);
        }

        protected internal virtual IEnumerable<JassCommentDeclarationSyntax> GetMapScriptHeader(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapInfo = map.Info;
            var mapTriggerStrings = map.TriggerStrings;

            yield return new JassCommentDeclarationSyntax($"===========================================================================");
            yield return new JassCommentDeclarationSyntax($" ");
            yield return new JassCommentDeclarationSyntax($" {mapInfo.MapName.Localize(mapTriggerStrings)}");
            yield return new JassCommentDeclarationSyntax($" ");
            yield return new JassCommentDeclarationSyntax($"   Warcraft III map script");
            yield return new JassCommentDeclarationSyntax($"   Generated by {Assembly.GetExecutingAssembly().GetName().Name}");
            yield return new JassCommentDeclarationSyntax($"   Date: {DateTime.Now:ddd MMM dd HH:mm:ss yyyy}");
            yield return new JassCommentDeclarationSyntax($"   Map Author: {mapInfo.MapAuthor.Localize(mapTriggerStrings)}");
            yield return new JassCommentDeclarationSyntax($" ");
            yield return new JassCommentDeclarationSyntax($"===========================================================================");
        }

        protected internal virtual void FindUnitVariableReferences(TriggerFunction function)
        {
            foreach (var parameter in function.Parameters)
            {
                FindUnitVariableReferences(parameter);
            }

            foreach (var childFunction in function.ChildFunctions)
            {
                FindUnitVariableReferences(childFunction);
            }
        }

        protected internal virtual void FindUnitVariableReferences(TriggerFunctionParameter parameter)
        {
            if (parameter.Type == TriggerFunctionParameterType.Variable &&
                parameter.Value.StartsWith("gg_unit_", StringComparison.Ordinal))
            {
                UnitTriggerReferences.Add(parameter.Value);
            }
            else if (parameter.Function is not null)
            {
                FindUnitVariableReferences(parameter.Function);
            }
        }
    }
}