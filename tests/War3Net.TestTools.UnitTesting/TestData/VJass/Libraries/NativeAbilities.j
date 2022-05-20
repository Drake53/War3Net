library NativeAbilities requires /*
    ------------
    */ ListT, /*
    ------------
        - A doubly-linked list library that provides object-based list functionalities
        - Author: Bannar
        - Link: "https://www.hiveworkshop.com/threads/containers-list-t.249011/"

    ----------------------
    */ optional UnitDex /*
    ----------------------
        - A library that provides basic unit creation and removal events
          and also assigns unique ids for each unit.
        - Breaks user-based SetUnitUserData behavior
        - Optional to use, as it adds some much needed "garbage collection" behavior
          for out-of-scope passives, but may not play well with other indexers
          which may have their own index event.
        - Author: TriggerHappy
        - Link: "https://www.hiveworkshop.com/threads/system-unitdex-unit-indexer.248209/"

    -------
      API
    -------
        - RegisterPassiveAbility(int abilId)
            Registers a given ability as a passive ability.
            This has no logical checks for when an ability
            is actually passive, so it's up to the user to
            pass the right ability.
          
        - IsAbilityPassive(int abilId) -> bool
            Returns true if the ability is marked as passive.

        - RegisterMetaAbility(int abilId)
            Registers a given ability as a meta ability.
            Meta abilities are abilities which manipulate
            the list of abilities a unit has at any given
            moment.
            This has no logical checks for when an ability
            is actually a meta ability, so it's up to the
            user to pass the right ability.
            Note that the ability must be registered
            as a passive ability beforehand.

        - IsAbilityMeta(int abilId) -> bool
            Returns true if the ability is marked as meta.
      
        - UnitSilencePassives(unit whichUnit, bool disable, bool hide)
            Disables all passive abilities found in the unit.
            Will make a list of passive abilities for the
            unit if such a list did not exist beforehand.
            Will not check for new passive abilities if
            the list already existed during that time.

        - UnitClearPassives(unit whichUnit)
            Only invoke this function when the unit is
            about to be removed.

        - UnitUpdatePassives(unit whichUnit)
            This function should be invoked sparingly, and should
            be called when the unit has a new passive ability
            that was not caught and added to the unit's list.

            Ideally, this should be handled smoothly by the
            system, but there are cases where it can't catch
            everything ability related, so this is exposed
            for the user's convenience.

    ------------------------
        Additional Notes
    ------------------------
        - This hooks UnitAddAbility and UnitRemoveAbility
          so there might be some performance degradation
          when dealing with these functions, and in some
          cases, completely halt the entire function
          (oplimit reached).

        - The flag INCLUDE_ITEM_AS_ABILITIES is set to
          false by default. Setting it to true will make
          iteration of abilities much more expensive, but
          will correctly filter out item abilities added
          to units as passive abilities.
*/

//! runtextmacro DEFINE_LIST("private", "AbilityIdList", "integer")

globals
    private TableArray passMap              = 0
    private TableArray metaAbilMap          = 0
    private Table      tempTable            = 0
    private Table   array passAbilMap
    private integer array passAbilArr

    //  Flag variables
    //  Setting this to true will also include item abilities
    //  as if they are unit abilities.
    private constant boolean INCLUDE_ITEM_AS_ABILITIES  = false
endglobals

// A defined function which returns a unique index for a specified unit
// Uses GetHandleId by default, but can be altered to use values given
// by a unit indexer
private function GetUnitIndex takes unit whichUnit returns integer
static if LIBRARY_UnitDex then
    return GetUnitId(whichUnit)
else
    return GetHandleId(whichUnit)
endif
endfunction

function RegisterPassiveAbility takes integer abilId returns nothing
    if not passMap[1].has(abilId) then
    set passAbilArr[0] = passAbilArr[0] + 1
        set passAbilArr[passAbilArr[0]] = abilId
        set passMap[3][abilId] = passAbilArr[0]
    endif
    set passMap[1][abilId] = passMap[1][abilId] + 1
endfunction

function IsAbilityPassive takes integer abilId returns boolean
    return passMap[1][abilId] > 0
endfunction

function RegisterMetaAbility takes integer abilId returns nothing
    if not metaAbilMap[2].has(abilId) and IsAbilityPassive(abilId) then
        set metaAbilMap[1][0] = metaAbilMap[1][0] + 1
        set metaAbilMap[2][metaAbilMap[1][0]] = abilId
        set metaAbilMap[3][abilId] = metaAbilMap[1][0]
    endif
    set metaAbilMap[1][abilId] = metaAbilMap[1][abilId] + 1
endfunction

function IsAbilityMeta takes integer abilId returns boolean
    return metaAbilMap[1][abilId] > 0
endfunction

function UnitSilencePassives takes unit whichUnit, boolean disable, boolean hide returns nothing
    local integer i     = 1
    local integer id    = GetUnitIndex(whichUnit)
    if not passMap[2].has(id) then
        set passMap[2][id]               = AbilityIdList.create()
        set passAbilMap[passMap[2][id]]  = Table.create()
        loop
            exitwhen i > passAbilArr[0]
            if GetUnitAbilityLevel(whichUnit, passAbilArr[i]) != 0 then
                set passAbilMap[passMap[2][id]][i] = AbilityIdList(passMap[2][id]).push(i).last
            endif  
            call BlzUnitDisableAbility(whichUnit, passAbilArr[i], disable, hide)
            set i = i + 1
        endloop
    else
        set i = AbilityIdList(passMap[2][id]).first
        loop
            exitwhen i == 0
            call BlzUnitDisableAbility(whichUnit, passAbilArr[AbilityIdListItem(i).data], disable, hide)
            set i = AbilityIdListItem(i).next
        endloop
    endif
endfunction

// This should only be called when a unit is about to be removed
function UnitClearPassives takes unit whichUnit returns nothing
    local integer i     = 1
    local integer id    = GetUnitIndex(whichUnit)
    if passMap[2].has(id) then
        call passAbilMap[passMap[2][id]].destroy()
        call AbilityIdList(passMap[2][id]).clear()
        call AbilityIdList(passMap[2][id]).destroy()
    endif
    call passMap[2].remove(id)
endfunction

// This should be called sparingly, on such events as levelling up a skill
function UnitUpdatePassives takes unit whichUnit returns nothing
    local integer i    = 1
    local integer id   = GetUnitIndex(whichUnit)
    if passMap[2].has(id) then
        call tempTable.flush()
        set i = AbilityIdList(passMap[2][id]).first
        loop
            exitwhen i == 0
            if BlzGetUnitAbility(whichUnit, passAbilArr[AbilityIdListItem(i).data]) == null then
                call AbilityIdList(passMap[2][id]).erase(i)
            else
                set tempTable.boolean[AbilityIdListItem(i).data] = true
            endif
            set i = AbilityIdListItem(i).next
        endloop
        // Search for any abilities that might've been added
        set i = 1
        loop
            exitwhen i > passAbilArr[0]
            if (BlzGetUnitAbility(whichUnit, passAbilArr[i]) != null) then
                set passAbilMap[passMap[2][id]][i] = AbilityIdList(passMap[2][id]).push(i).last
            endif
            loop
                set i = i + 1
                exitwhen not tempTable.boolean[i]
            endloop
        endloop
    else
        call UnitSilencePassives(whichUnit, true, false)
        call UnitSilencePassives(whichUnit, false, false)
    endif
endfunction

private function OnUnitAddPassive takes unit whichUnit, integer abilId returns nothing
    local integer i
    local integer id
    if not IsAbilityPassive(abilId) then
        return
    endif
    set id = GetUnitIndex(whichUnit)
    // Assume passMap[2][id] has a value
    if not passMap[2].has(id) then
        call UnitSilencePassives(whichUnit, true, false)
        call UnitSilencePassives(whichUnit, false, false)
    else
        if not IsAbilityMeta(abilId) then
            set i = passMap[3][abilId]
            if not passAbilMap[passMap[2][id]].has(i) then
                set passAbilMap[passMap[2][id]][i] = AbilityIdList(passMap[2][id]).push(i).last
            endif
        else
            call passAbilMap[passMap[2][id]].destroy()
            call AbilityIdList(passMap[2][id]).clear()
            call AbilityIdList(passMap[2][id]).destroy()
            call passMap[2].remove(id)

            call UnitSilencePassives(whichUnit, true, false)
            call UnitSilencePassives(whichUnit, false, false)
        endif
    endif
endfunction

private function OnUnitRemovePassive takes unit whichUnit, integer abilId returns nothing
    local integer i
    local integer id
    if not IsAbilityPassive(abilId) then
        return
    endif
    set id = GetUnitIndex(whichUnit)
    // Assume passMap[2][id] has a value
    if not passMap[2].has(id) then
        call UnitSilencePassives(whichUnit, true, false)
        call UnitSilencePassives(whichUnit, false, false)
    else
        if not IsAbilityMeta(abilId) then
            set i = passMap[3][abilId]
            if passAbilMap[passMap[2][id]].has(i) then
                call AbilityIdList(passMap[2][id]).erase(passAbilMap[passMap[2][id]][i])
            endif
        else
            call passAbilMap[passMap[2][id]].destroy()
            call AbilityIdList(passMap[2][id]).clear()
            call AbilityIdList(passMap[2][id]).destroy()
            call passMap[2].remove(id)

            call UnitSilencePassives(whichUnit, true, false)
            call UnitSilencePassives(whichUnit, false, false)
        endif
    endif
endfunction

hook UnitAddAbility OnUnitAddPassive
hook UnitRemoveAbility OnUnitRemovePassive

//  Hardcoded variants
private function InitPassives takes nothing returns nothing
    call RegisterPassiveAbility('Aroc')
    call RegisterPassiveAbility('Sch4')
    call RegisterPassiveAbility('Srtt')
    call RegisterPassiveAbility('Aroc')
    call RegisterPassiveAbility('Afbt')
    call RegisterPassiveAbility('Afbk')
    call RegisterPassiveAbility('Aflk')
    call RegisterPassiveAbility('Agyb')
    call RegisterPassiveAbility('Afsh')
    call RegisterPassiveAbility('Aphx')
    call RegisterPassiveAbility('Asph')
    call RegisterPassiveAbility('Asth')
    call RegisterPassiveAbility('Ahsb')
    call RegisterPassiveAbility('Agyv')
    call RegisterPassiveAbility('AHbh')
    call RegisterPassiveAbility('AHab')
    call RegisterPassiveAbility('AHad')
    call RegisterPassiveAbility('Sbsk')
    call RegisterPassiveAbility('Abof')
    call RegisterPassiveAbility('Abun')
    call RegisterPassiveAbility('Sca6')
    call RegisterPassiveAbility('Sca1')
    call RegisterPassiveAbility('Sca4')
    call RegisterPassiveAbility('Sca5')
    call RegisterPassiveAbility('Sca2')
    call RegisterPassiveAbility('Sca3')
    call RegisterPassiveAbility('Achl')
    call RegisterPassiveAbility('Adt1')
    call RegisterPassiveAbility('Advc')
    call RegisterPassiveAbility('Aven')
    call RegisterPassiveAbility('Aliq')
    call RegisterPassiveAbility('Apak')
    call RegisterPassiveAbility('Asal')
    call RegisterPassiveAbility('Awar')
    call RegisterPassiveAbility('Arbr')
    call RegisterPassiveAbility('Aspi')
    call RegisterPassiveAbility('Aakb')
    call RegisterPassiveAbility('AOcr')
    call RegisterPassiveAbility('AOae')
    call RegisterPassiveAbility('AOr2')
    call RegisterPassiveAbility('AOre')
    call RegisterPassiveAbility('AOr3')
    call RegisterPassiveAbility('Aabr')
    call RegisterPassiveAbility('Abgl')
    call RegisterPassiveAbility('Abgs')
    call RegisterPassiveAbility('Abgm')
    call RegisterPassiveAbility('Sch2')
    call RegisterPassiveAbility('Agyd')
    call RegisterPassiveAbility('Aap1')
    call RegisterPassiveAbility('Apts')
    call RegisterPassiveAbility('Aap2')
    call RegisterPassiveAbility('Aexh')
    call RegisterPassiveAbility('Afrz')
    call RegisterPassiveAbility('Afr2')
    call RegisterPassiveAbility('Afra')
    call RegisterPassiveAbility('Afrb')
    call RegisterPassiveAbility('Agho')
    call RegisterPassiveAbility('Aeth')
    call RegisterPassiveAbility('Aspa')
    call RegisterPassiveAbility('Atru')
    call RegisterPassiveAbility('AUts')
    call RegisterPassiveAbility('AUau')
    call RegisterPassiveAbility('AUav')
    // Night Elf
    call RegisterPassiveAbility('Acor')
    call RegisterPassiveAbility('Aegm')
    call RegisterPassiveAbility('Aetl')
    call RegisterPassiveAbility('Alit')
    call RegisterPassiveAbility('Arsk')
    call RegisterPassiveAbility('Aspo')
    call RegisterPassiveAbility('Amim')
    call RegisterPassiveAbility('Asp1')
    call RegisterPassiveAbility('Asp2')
    call RegisterPassiveAbility('Asp3')
    call RegisterPassiveAbility('Asp4')
    call RegisterPassiveAbility('Asp5')
    call RegisterPassiveAbility('Asp6')
    call RegisterPassiveAbility('Ault')
    call RegisterPassiveAbility('Aimp')
    call RegisterPassiveAbility('AEev')
    call RegisterPassiveAbility('AEah')
    call RegisterPassiveAbility('AEar')
    // Neutral Hostile
    call RegisterPassiveAbility('ACbh')
    call RegisterPassiveAbility('ANbh')
    call RegisterPassiveAbility('ACba')
    call RegisterPassiveAbility('ACce')
    call RegisterPassiveAbility('ACac')
    call RegisterPassiveAbility('ACct')
    call RegisterPassiveAbility('ACav')
    call RegisterPassiveAbility('Scae')
    call RegisterPassiveAbility('ACvs')
    call RegisterPassiveAbility('ACes')
    call RegisterPassiveAbility('ACev')
    call RegisterPassiveAbility('Afbb')
    call RegisterPassiveAbility('ACnr')
    call RegisterPassiveAbility('ANre')
    call RegisterPassiveAbility('SCva')
    call RegisterPassiveAbility('ACpv')
    call RegisterPassiveAbility('ACrn')
    call RegisterPassiveAbility('ACrk')
    call RegisterPassiveAbility('ACsk')
    call RegisterPassiveAbility('Asla')
    call RegisterPassiveAbility('Aspy')
    call RegisterPassiveAbility('Aspt')
    call RegisterPassiveAbility('Asod')
    call RegisterPassiveAbility('Assp')
    call RegisterPassiveAbility('Aspd')
    call RegisterPassiveAbility('ACm2')
    call RegisterPassiveAbility('ACm3')
    call RegisterPassiveAbility('ACmi')
    call RegisterPassiveAbility('ANth')
    call RegisterPassiveAbility('ANt2')
    call RegisterPassiveAbility('ACah')
    call RegisterPassiveAbility('ACat')
    call RegisterPassiveAbility('ACua')
    call RegisterPassiveAbility('ACvp')
    // Heroes
    call RegisterPassiveAbility('Aamk')
    call RegisterPassiveAbility('ANde')
    call RegisterPassiveAbility('ANd1')
    call RegisterPassiveAbility('ANd2')
    call RegisterPassiveAbility('ANd3')
    call RegisterPassiveAbility('ANdb')
    call RegisterPassiveAbility('Acdb')
    call RegisterPassiveAbility('ANeg')
    // Passive
    call RegisterPassiveAbility('Atdg')
    call RegisterPassiveAbility('Ansk')
    call RegisterPassiveAbility('Aasl')
    call RegisterPassiveAbility('Atsp')
    call RegisterPassiveAbility('Atwa')
    // Other
    call RegisterPassiveAbility('Amnz')
    call RegisterPassiveAbility('Amnx')
    call RegisterPassiveAbility('Adda')
    call RegisterPassiveAbility('Abdl')
    call RegisterPassiveAbility('Abds')
    call RegisterPassiveAbility('Abdt')
    call RegisterPassiveAbility('Sch3')
    call RegisterPassiveAbility('Sch5')
    call RegisterPassiveAbility('Achd')
    call RegisterPassiveAbility('Agld')
    call RegisterPassiveAbility('AInv')
    call RegisterPassiveAbility('Avul')
    call RegisterPassiveAbility('Amin')
    call RegisterPassiveAbility('ANpi')
    call RegisterPassiveAbility('Apig')
    call RegisterPassiveAbility('Apiv')
    call RegisterPassiveAbility('Apmf')
    call RegisterPassiveAbility('Apoi')
    call RegisterPassiveAbility('Argd')
    call RegisterPassiveAbility('Argl')
    call RegisterPassiveAbility('Arlm')
    call RegisterPassiveAbility('Arng')
    call RegisterPassiveAbility('Arev')
    call RegisterPassiveAbility('Aawa')
    call RegisterPassiveAbility('Apit')
    call RegisterPassiveAbility('Aall')
    call RegisterPassiveAbility('Adtg')
    call RegisterPassiveAbility('ANtr')
    call RegisterPassiveAbility('Aihn')
    call RegisterPassiveAbility('Aien')
    call RegisterPassiveAbility('Aion')
    call RegisterPassiveAbility('Aiun')
    call RegisterPassiveAbility('Awan')
    call RegisterPassiveAbility('Awrp')
    // Items
static if INCLUDE_ITEM_AS_ABILITIES then
    call RegisterPassiveAbility('AIbx')
    call RegisterPassiveAbility('AIba')
    call RegisterPassiveAbility('AIcd')
    call RegisterPassiveAbility('AIcs')
    call RegisterPassiveAbility('AIad')
    call RegisterPassiveAbility('AIae')
    call RegisterPassiveAbility('AIev')
    call RegisterPassiveAbility('AIam')
    call RegisterPassiveAbility('AIgm')
    call RegisterPassiveAbility('AId1')
    call RegisterPassiveAbility('AId0')
    call RegisterPassiveAbility('AId2')
    call RegisterPassiveAbility('AId3')
    call RegisterPassiveAbility('AId5')
    call RegisterPassiveAbility('AId7')
    call RegisterPassiveAbility('AId8')
    call RegisterPassiveAbility('AIcb')
    call RegisterPassiveAbility('AIfb')
    call RegisterPassiveAbility('AIgd')
    call RegisterPassiveAbility('AIob')
    call RegisterPassiveAbility('AIf2')
    call RegisterPassiveAbility('AIlb')
    call RegisterPassiveAbility('AIll')
    call RegisterPassiveAbility('AIpb')
    call RegisterPassiveAbility('AIsb')
    call RegisterPassiveAbility('AIsx')
    call RegisterPassiveAbility('AIs2')
    call RegisterPassiveAbility('AItg')
    call RegisterPassiveAbility('AItn')
    call RegisterPassiveAbility('AItc')
    call RegisterPassiveAbility('AItf')
    call RegisterPassiveAbility('AIth')
    call RegisterPassiveAbility('AItx')
    call RegisterPassiveAbility('AIat')
    call RegisterPassiveAbility('AIti')
    call RegisterPassiveAbility('AItj')
    call RegisterPassiveAbility('AIt6')
    call RegisterPassiveAbility('AItk')
    call RegisterPassiveAbility('AItl')
    call RegisterPassiveAbility('AIt9')
    call RegisterPassiveAbility('AIzb')
    call RegisterPassiveAbility('AIx4')
    call RegisterPassiveAbility('AIx3')
    call RegisterPassiveAbility('AIa1')
    call RegisterPassiveAbility('AIx1')
    call RegisterPassiveAbility('AIi1')
    call RegisterPassiveAbility('AIs1')
    call RegisterPassiveAbility('AIaz')
    call RegisterPassiveAbility('AIx2')
    call RegisterPassiveAbility('AIa3')
    call RegisterPassiveAbility('AIi3')
    call RegisterPassiveAbility('AIs3')
    call RegisterPassiveAbility('AIa4')
    call RegisterPassiveAbility('AIi4')
    call RegisterPassiveAbility('AIs4')
    call RegisterPassiveAbility('AIx5')
    call RegisterPassiveAbility('AIa6')
    call RegisterPassiveAbility('AIi6')
    call RegisterPassiveAbility('AIs6')
    call RegisterPassiveAbility('AIcf')
    call RegisterPassiveAbility('AIl2')
    call RegisterPassiveAbility('AIlf')
    call RegisterPassiveAbility('AIl1')
    call RegisterPassiveAbility('AIlz')
    call RegisterPassiveAbility('Arel')
    call RegisterPassiveAbility('Arll')
    call RegisterPassiveAbility('AIva')
    call RegisterPassiveAbility('AImz')
    call RegisterPassiveAbility('AI2m')
    call RegisterPassiveAbility('AImv')
    call RegisterPassiveAbility('AIbm')
    call RegisterPassiveAbility('AImb')
    call RegisterPassiveAbility('AIrm')
    call RegisterPassiveAbility('AIrn')
    call RegisterPassiveAbility('AIms')
    call RegisterPassiveAbility('AIrc')
    call RegisterPassiveAbility('AIsi')
    call RegisterPassiveAbility('AIft')
    call RegisterPassiveAbility('AIfw')
    call RegisterPassiveAbility('AIfx')
    call RegisterPassiveAbility('Apo2')
    call RegisterPassiveAbility('AIgx')
    call RegisterPassiveAbility('AIsr')
    call RegisterPassiveAbility('AImx')
    call RegisterPassiveAbility('AIss')
    call RegisterPassiveAbility('AIse')
    call RegisterPassiveAbility('AIar')
    call RegisterPassiveAbility('AIuv')
    call RegisterPassiveAbility('AIau')
    call RegisterPassiveAbility('AIav')
    call RegisterPassiveAbility('AIwd')
endif
endfunction

private function InitMetaAbilities takes nothing returns nothing
    call RegisterMetaAbility('ANeg')
    call RegisterMetaAbility('Aspb')
endfunction

static if LIBRARY_UnitDex then
/**
**/
private function OnUnitLeave takes nothing returns nothing
    call UnitClearPassives(GetIndexedUnit())
endfunction

private function OnUnitEnter takes nothing returns nothing
    call UnitUpdatePassives(GetIndexedUnit())
endfunction

private function InitUnitEvents takes nothing returns nothing
    call OnUnitIndex(function OnUnitEnter)
    call OnUnitDeindex(function OnUnitLeave)
endfunction
/**
**/
endif

private function Init takes nothing returns nothing
    local trigger t = CreateTrigger()
    call TriggerAddCondition(t, Filter(function InitPassives))
    call TriggerAddCondition(t, Filter(function InitMetaAbilities))
    static if LIBRARY_UnitDex then
        call TriggerAddCondition(t, Filter(function InitUnitEvents))
    endif
    call TriggerEvaluate(t)
    call DestroyTrigger(t)
    set t = null
endfunction


private module M
    private static method onInit takes nothing returns nothing
        // Index 1 - ability id mapping
        // Index 2 - ability list per unit id mapping
        // Index 3 - ability index mapping
        set passMap        = TableArray[4]
        set metaAbilMap    = TableArray[4]
        set tempTable      = Table.create()
        call Init()
    endmethod
endmodule

private struct S extends array
    implement M
endstruct

endlibrary