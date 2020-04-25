// ------------------------------------------------------------------------------
// <copyright file="ActionBlockType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Replay
{
    // http://w3g.deepnode.de/files/w3g_actions.txt
    public enum ActionBlockType
    {
        Pause = 0x01,
        Resume = 0x02,
        SetGameSpeed = 0x03,
        IncreaseGameSpeed = 0x04,
        DecreaseGameSpeed = 0x05,
        SaveGame = 0x06,
        SaveGameFinished = 0x07,

        AbilityNoTarget = 0x10,
        AbilityLocationTarget = 0x11,
        AbilityWidgetTarget = 0x12,
        ItemDropOrGive = 0x13,
        AbilityTwoTarget = 0x14,

        ChangeSelection = 0x16,
        AssignGroupHotkey = 0x17,
        SelectGroupHotkey = 0x18,
        SelectSubgroup = 0x19,
        PreSubselection = 0x1A,
        UNK1B = 0x1B,
        SelectGroundItem = 0x1C,
        CancelHeroRevival = 0x1D,
        RemoveUnitFromBuildingQueue = 0x1E,

        CheatTheDudeAbides = 0x20,
        UNK21 = 0x21,
        CheatSombodySetUpUsTheBomb = 0x22,
        CheatWarpTen = 0x23,
        CheatIocainePowder = 0x24,
        CheatPointBreak = 0x25,
        CheatWhosYourDaddy = 0x26,
        CheatKeyserSoze = 0x27,
        CheatLeafitToMe = 0x28,
        CheatThereIsNoSpoon = 0x29,
        CheatStrengthAndHonor = 0x2A,
        CheatItVexesMe = 0x2B,
        CheatWhoIsJohnGalt = 0x2C,
        CheatGreedIsGood = 0x2D,
        CheatDayLightSavings = 0x2E,
        CheatISeeDeadPeople = 0x2F,
        CheatSynergy = 0x30,
        CheatSharpAndShiny = 0x31,
        CheatAllYourBaseAreBelongToUs = 0x32,

        ChangeAllyOptions = 0x50,
        TransferResources = 0x51,

        TriggeredChatCommand = 0x60,
        CinematicSkipped = 0x61,
        ScenarioTrigger = 0x62,

        EnterHeroSkillMenu = 0x66,
        EnterBuildingMenu = 0x67,
        PingMinimap = 0x68,
        ContinueGameA = 0x69,
        ContinueGameB = 0x6A,
        SyncInteger = 0x6B,
        SyncReal = 0x6C,
        SyncBoolean = 0x6D,
        SyncUnit = 0x6E,

        SyncStringNoData = 0x6F,
        SyncString = 0x70,

        UNK75 = 0x75,
        KeyboardEvent = 0x7A,
        UNK7B = 0x7B,
    }
}