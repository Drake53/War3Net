library Reselect initializer Init
//ReselctV4
//Reselect saves the orderable Units a player had selected when deselecting them.
//When now pressing somewhere on the game screen, the saved deselected units will be reselected.
//UI clicks and another unit-selection won't trigger reselection.

//   function ReselectEnable takes boolean flag returns nothing
   // Disable/Enable Reselect Event Triggers

globals
  
   private group array Group         //Valid Reselect Targets
   private group array TempSave   //Possible Reselect Targets
   private unit array CurrentUnit   //Last Selected Unit of [playerId]
  
   private timer DeSelectTimer = CreateTimer()  
   private force DeselectForce = CreateForce()  
  
   private boolean array orderAsGroup           //(true) rightclick reselection order, will use formation for that player
   private boolean array Allowed                 //MouseClicks can Reselect, that will be calculated inside ActionDeSelectTimer
   private timer MouseTimer    = CreateTimer()   //This Timer allows to run Selection Events of a MouseClick which would execute Reselect, before reselecting.
   private force MouseForce    = CreateForce()   //That allows to select multiple other non controlable units before Reselection is applied. shop -> shop -> enemy -> press Some where -> reselect
   private unit array MouseUnit                 //CurrentUnit when Mouse Click
   private real array MouseX
   private real array MouseY
   private mousebuttontype array MouseButton
  
   public boolean array Enabled               //Enable Reselct for [playerId]; Reselect_Enabled = true, Reselect does only work for Users
   public trigger Select        = CreateTrigger()
   public trigger DeSelect    = CreateTrigger()
   public trigger Mouse        = CreateTrigger()
  
endglobals

private function ActionSelect takes nothing returns nothing
   local player eventPlayer   = GetTriggerPlayer()
   local integer index        = GetPlayerId(eventPlayer)
   if Enabled[index] then
       set CurrentUnit[index] = GetTriggerUnit()
       if GetPlayerAlliance(GetOwningPlayer(CurrentUnit[index]), eventPlayer, ALLIANCE_SHARED_CONTROL) then
           set Allowed[index] = false
           call GroupClear(Group[index])
       endif
   endif
   set eventPlayer = null
endfunction


private function ActionTimerMouseForce takes nothing returns nothing
   local player p        = GetEnumPlayer()
   local integer index   = GetPlayerId(p)
   local effect eff
   local unit fog
   //Did this player select another Unit with this MousePress?
   if CurrentUnit[index] == MouseUnit[index] then
       //No new unit, Reselect!
       call ClearSelectionForPlayer(p)
      
       if MouseButton[index] == MOUSE_BUTTON_TYPE_RIGHT then //Right click -> Order
           set eff = AddSpecialEffect("UI\\Feedback\\Confirmation\\Confirmation.mdl", MouseX[index], MouseY[index])
           call BlzSetSpecialEffectColor( eff, 0, 255, 0 )
           if GetLocalPlayer() != p then
               call BlzSetSpecialEffectScale( eff, 0.00 )
           endif
           call DestroyEffect(eff)
           set eff = null
          
           if orderAsGroup[index] then //Order together?
               call GroupPointOrder( Group[index], "smart", MouseX[index], MouseY[index])
               loop
                   set fog = FirstOfGroup(Group[index])
                   exitwhen fog == null
                   call GroupRemoveUnit(Group[index], fog)
                   call SelectUnitAddForPlayer(fog, p)
               endloop
              
           else //Order one for one
               loop
                   set fog = FirstOfGroup(Group[index])
                   exitwhen fog == null
                   call GroupRemoveUnit(Group[index], fog)
                   call SelectUnitAddForPlayer(fog, p)
                   call IssuePointOrder(fog, "smart", MouseX[index], MouseY[index])
               endloop
           endif
       else //Not Right Click, Only Reselect
           loop
               set fog = FirstOfGroup(Group[index])
               exitwhen fog == null
               call GroupRemoveUnit(Group[index], fog)
               call SelectUnitAddForPlayer(fog, p)
           endloop
       endif      
   endif
   set p = null
endfunction

private function ActionTimerMouse takes nothing returns nothing
   call DisableTrigger(DeSelect)
   call DisableTrigger(Select)
  
   call ForForce(MouseForce, function ActionTimerMouseForce)
   call ForceClear(MouseForce)
  
   call EnableTrigger(DeSelect)
   call EnableTrigger(Select)
endfunction

private function ActionMouse takes nothing returns nothing
   local player p        = GetTriggerPlayer()
   local integer index   = GetPlayerId(p)
   local real x          = BlzGetTriggerPlayerMouseX()
   local real y          = BlzGetTriggerPlayerMouseY()
   if Enabled[index] and Allowed[index] and x != 0 and y != 0  then
           //Save Data of this MousePress
           set MouseX[index]        = x
           set MouseY[index]       = y
           set MouseButton[index]    = BlzGetTriggerPlayerMouseButton()
           call ForceAddPlayer(MouseForce, p)
           set MouseUnit[index]    = CurrentUnit[index]
           //Start a 0s timer, Allow the possible selection event of this mouse Press to run first.
           call TimerStart(MouseTimer,0, false, function ActionTimerMouse)
       //endif
   endif
   set p = null
endfunction

private function ActionDeselectForce takes nothing returns nothing
   local player p        = GetEnumPlayer()
   local integer index = GetPlayerId(p)
   if not IsUnitSelected(CurrentUnit[index], p) or not GetPlayerAlliance(GetOwningPlayer(CurrentUnit[index]), p, ALLIANCE_SHARED_CONTROL) then
       call GroupAddGroup(TempSave[index], Group[index])
       set Allowed[index] = true
   endif
   call GroupClear(TempSave[index])
   set p = null
endfunction

private function ActionDeselectTimer takes nothing returns nothing
   call ForForce(DeselectForce, function ActionDeselectForce)
   call ForceClear(DeselectForce)
endfunction

private function ActionDeSelect takes nothing returns nothing
   local player eventPlayer   = GetTriggerPlayer()
   local unit eventUnit       = GetTriggerUnit()
   local integer index        = GetPlayerId(eventPlayer)
   if Enabled[index] and GetPlayerAlliance(GetOwningPlayer(eventUnit), eventPlayer, ALLIANCE_SHARED_CONTROL) then
       call GroupAddUnit(TempSave[index], eventUnit)
       call ForceAddPlayer(DeselectForce, eventPlayer)
       call TimerStart(DeSelectTimer,0, false, function ActionDeselectTimer)
   endif  
   set eventPlayer   = null
   set eventUnit    = null
endfunction

private function InitPlayers takes nothing returns nothing
   local player p      = GetEnumPlayer()
   local integer index = GetPlayerId(p)
   if GetPlayerController(p) == MAP_CONTROL_USER then
       set Enabled[index] = true
       set Group[index]        = CreateGroup()
       set TempSave[index]     = CreateGroup()
       set orderAsGroup[index] = true
       call TriggerRegisterPlayerEvent(Mouse, p, EVENT_PLAYER_MOUSE_DOWN)
       call TriggerRegisterPlayerUnitEvent(Select, p, EVENT_PLAYER_UNIT_SELECTED, null)
       call TriggerRegisterPlayerUnitEvent(DeSelect, p, EVENT_PLAYER_UNIT_DESELECTED, null)
   endif  
   set p = null
endfunction

function ReselectEnable takes boolean flag returns nothing
   if flag then
       call EnableTrigger(Select)
       call EnableTrigger(Mouse)
       call EnableTrigger(DeSelect)
   else
       call DisableTrigger(Select)
       call DisableTrigger(Mouse)
       call DisableTrigger(DeSelect)
   endif
endfunction

private function Init takes nothing returns nothing
   call TriggerAddAction(Select, function ActionSelect)
   call TriggerAddAction(DeSelect, function ActionDeSelect)
   call TriggerAddAction(Mouse, function ActionMouse)
   call ForForce(bj_FORCE_ALL_PLAYERS, function InitPlayers)
endfunction
endlibrary