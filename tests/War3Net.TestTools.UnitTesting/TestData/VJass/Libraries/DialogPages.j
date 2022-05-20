//===========================================================================
//
//  Dialog Pages System v1.0.2a
//  by loktar
//  -------------------------------------------------------------------------
// * Create a dialog with automatic pages
// * Optionally show an index page
// * Automatic Next/Previous/Back/Cancel buttons
//  -------------------------------------------------------------------------
//
//    -------
//    * API *
//    -------
//  *   NOTES:
//          - dialogId refers to handle id
//          - buttonId and pageId refer to zero-indexed id
//
//  *   boolean DialogPDisplay(player plr, integer dialogId, boolean display)
//          - Display/Hide dialog
//          - If displayed when dialog is already displayed, it will be updated
//          - Returns false if the dialog wasn't found
//
//  *   integer DialogPCreate(string message, boolean showIndex)
//          - Create a new dialog
//          - Returns dialog handle id (dialogId)
//
//  *   boolean DialogPDestroy(integer dialogId)
//          - Destroy dialog
//          - Returns false if the dialog wasn't found
//
//  *   boolean DialogPSetPage(integer dialogId, integer pageId)
//          - Set dialog page
//          - Set to 0 if pageId < 0 or to maximum pageId if pageId > maximum pageId
//          - Returns false if the dialog wasn't found
//
//  *   boolean DialogPSetMessage  (integer dialogId, string message)
//      boolean DialogPSetButtonsPP(integer dialogId, integer buttonsPP)
//          - Set dialog message/buttons per page
//          - Returns false if the dialog wasn't found
//
//  *   boolean DialogPSetButtonsText  (integer dialogId, string  next, string  previous, string  back, string  cancel)
//  *   boolean DialogPSetButtonsHotkey(integer dialogId, integer next, integer previous, integer back, integer cancel)
//      boolean DialogPDisplayButtons  (integer dialogId, boolean next, boolean previous, boolean back, boolean cancel)
//          - Set browse buttons text/hotkey/visibility
//          - Returns false if the dialog wasn't found
//
//  *   dialog DialogPGetHandle(integer dialogId)
//          - Get dialog handle
//          - Returns null if the dialog wasn't found
//
//  *   trigger DialogPGetTrigger(integer dialogId)
//          - Get dialog trigger
//          - Executed/Evaluated when the dialog is clicked
//          - Execution of actions prevented when Next, Previous, Back, Cancel and index page buttons are clicked
//          - Returns null if the dialog wasn't found
//
//  *   integer DialogPGetClickedId(integer dialogId)
//          - Get dialog last clicked buttonId
//          - Returns DP_BROWSED for Next, Previous, Back and index page buttons
//          - Returns DP_CANCELED for Cancel button
//          - Returns DP_NONE if no button has been clicked yet
//          - Returns DP_NOT_FOUND if the dialog wasn't found
//
//      * BUTTONS *
//      -----------
//  *   integer DialogPAddButton    (integer dialogId, string text, integer hotkey)
//      integer DialogPAddQuitButton(integer dialogId, boolean doScoreScreen, string text, integer hotkey)
//          - Add (quit) button to dialog
//          - Returns buttonId (not a handle id)
//          - Returns DP_NOT_FOUND if the dialog wasn't found
//
//  *   boolean DialogPSetButton    (integer dialogId, integer buttonId, string text, integer hotkey)
//      boolean DialogPSetQuitButton(integer dialogId, integer buttonId, boolean doScoreScreen, string text, integer hotkey)
//          - Set (quit) button (doScoreScreen), text and hotkey
//          - Can both be used on regular and quit buttons
//          -- Using SetQuitButton on a regular button will turn it into a quit button
//          - Returns false if the dialog or button wasn't found
//
//  *   string DialogPGetButtonText(integer dialogId, integer buttonId)
//          - Get button text
//          - Returns null if the dialog or button wasn't found
//
//      * INDEX *
//      ---------
//  *   boolean DialogPEnableIndex(integer diagId, boolean enable)
//          - Enable/Disable index and set page to 0
//          - If enabled, also opens index
//          - Index is only shown when number of buttons > buttons per page
//          - Returns false if the dialog wasn't found
//
//  *   boolean DialogPSetIndexPage(integer dialogId, integer pageId)
//          - Set dialog index page
//          - Set to 0 if pageId < 0 or to maximum pageId if pageId > maximum pageId
//          - Returns false if the dialog wasn't found
//
//      * INDEX BUTTONS *
//      -----------------
//  *   boolean DialogPSetIndexButton(integer dialogId, integer pageId, string text, integer hotkey)
//          - Set index button text and hotkey
//          - Text and hotkey will be saved even if page doesn't (yet) exist
//          - Returns false if the dialog wasn't found
//
//      * CONFIG VARIABLES *
//      --------------------
//  *   integer DP_btnsPP
//          - Default number of buttons to show per page
//          - Does not include Next, Previous, Back or Cancel buttons
//          - Default: 5
//
//  *   string  DP_strNext,  DP_strPrev,  DP_strBack,  DP_strCancel
//      integer DP_hkNext,   DP_hkPrev,   DP_hkBack,   DP_hkCancel
//      boolean DP_showNext, DP_showPrev, DP_showBack, DP_showCancel
//          - Default settings for Next, Previous, Back and Cancel buttons
//          - Text: "Next >", "< Previous", "< Back", "Cancel"
//          - Hotkeys: 0 (all)
//          - Visibility: true (all)
//          -- When index is enabled, Next and Previous buttons are on index pages, Back button is on normal pages
//          -- When there are 2 pages, Previous button on first page and Next button on second page are always hidden
//          -- When there are less than 2 pages, Next and Previous buttons are always hidden
//
//===========================================================================
library DialogPages
    globals
        // Dialogs Hashtable
        private hashtable htbDialogs = InitHashtable()
            // handles
        private constant key DIAG
        private constant key BTN_HTB
        private constant key TRG
            // config
        private constant key MSG
        private constant key BTNS_PP
        private constant key INDEX_ENABLE
            // state
        private constant key BTN_CNT
        private constant key PAGE_ID
        private constant key CLICKED_ID
        private constant key INDEX_OPEN
        private constant key INDEX_PAGE_ID
  
        // Buttons Hashtables
        private constant key CURR_BTNS
        private constant key BTN_STR
        private constant key BTN_HK
        private constant key BTN_QUIT
        private constant key INDEX_BTN_STR
        private constant key INDEX_BTN_HK
        private constant integer BTN_NEXT   = -1 // make sure it doesn't collide with buttonIds
        private constant integer BTN_PREV   = -2
        private constant integer BTN_BACK   = -3
        private constant integer BTN_CANCEL = -4
        private constant key BTN_SHOW
  
        // Clicked Ids
        constant integer DP_NONE      = -1 // make sure it doesn't collide with buttonIds
        constant integer DP_BROWSED   = -2
        constant integer DP_CANCELED  = -3
        constant integer DP_NOT_FOUND = -4
  
        // Settings
        integer DP_btnsPP = 5
        boolean DP_showNext   = true
        boolean DP_showPrev   = true
        boolean DP_showBack   = true
        boolean DP_showCancel = true
        string  DP_strNext   = "Next >"
        string  DP_strPrev   = "< Previous"
        string  DP_strBack   = "< Back"
        string  DP_strCancel = "Cancel"
        integer DP_hkNext   = 0
        integer DP_hkPrev   = 0
        integer DP_hkBack   = 0
        integer DP_hkCancel = 0
    endglobals
//===============================================================================
//===============================================================================

//===============================================================================
//==== DIALOGS ==================================================================
//===============================================================================
    //==== Display Current Page ====
    function DialogPDisplay takes player plr, integer diagId, boolean display returns boolean
        local dialog diag = null
        local hashtable htbBtns = null
        local integer btnCnt
        local integer btnsPP
        local integer pageId
        local integer pageIdMax
        local integer btnId
        local integer btnIdMax
        local integer keyStr
        local integer keyHk
        local integer btnHk
        local string btnStr
        local string msg
        local boolean indexEnable
        local boolean indexOpen
 
        if HaveSavedHandle(htbDialogs, diagId, DIAG) then
            set diag = LoadDialogHandle(htbDialogs, diagId, DIAG)
            set htbBtns = LoadHashtableHandle(htbDialogs, diagId, BTN_HTB)
    
            // Clear current dialog page
            call FlushChildHashtable(htbBtns, CURR_BTNS)
            call DialogClear(diag)
    
            // Prepare the new dialog page
            if display then
                set btnCnt = LoadInteger(htbDialogs, diagId, BTN_CNT)
                set btnsPP = LoadInteger(htbDialogs, diagId, BTNS_PP)
          
                if btnCnt > 0 and btnsPP > 0 then
                    set pageIdMax = (btnCnt-1)/btnsPP
                    set indexEnable = btnCnt > btnsPP and LoadBoolean(htbDialogs, diagId, INDEX_ENABLE)
                    set indexOpen = indexEnable and LoadBoolean(htbDialogs, diagId, INDEX_OPEN)
              
                    // Get page data
                    if indexOpen then
                        set btnCnt = pageIdMax+1
                        set pageIdMax = (btnCnt-1)/btnsPP
                        set pageId = LoadInteger(htbDialogs, diagId, INDEX_PAGE_ID)
                        set keyStr = INDEX_BTN_STR
                        set keyHk = INDEX_BTN_HK
                    else
                        set pageId = LoadInteger(htbDialogs, diagId, PAGE_ID)
                        set keyStr = BTN_STR
                        set keyHk = BTN_HK
                    endif
                    if pageId > pageIdMax then
                        set pageId = pageIdMax
                    endif
              
                    // Get btn data
                    set btnId = pageId*btnsPP
                    set btnIdMax = btnId+btnsPP-1
                    if btnIdMax >= btnCnt then
                        if btnId >= btnCnt then
                            set btnId = btnCnt-1
                        endif
                        set btnIdMax = btnCnt-1
                    endif
                
                    // Add buttons
                    loop
                        exitwhen btnId > btnIdMax
                  
                        // Get button text & hotkey
                        if HaveSavedString(htbBtns, keyStr, btnId) then
                            set btnStr = LoadStr(htbBtns, keyStr, btnId)
                        else
                            set btnStr = "Page "+I2S(btnId+1) // Added Button always has saved string
                        endif
                        if HaveSavedInteger(htbBtns, keyHk, btnId) then
                            set btnHk = LoadInteger(htbBtns, keyHk, btnId)
                        else
                            set btnHk = 0
                        endif
                      
                        if HaveSavedBoolean(htbBtns, BTN_QUIT, btnId) then
                            call SaveInteger(htbBtns, CURR_BTNS, GetHandleId(DialogAddQuitButton(diag, LoadBoolean(htbBtns, BTN_QUIT, btnId), btnStr, btnHk)), btnId)
                        else
                            call SaveInteger(htbBtns, CURR_BTNS, GetHandleId(DialogAddButton(diag, btnStr, btnHk)), btnId)
                        endif
                  
                        set btnId = btnId+1
                    endloop
            
                    // Add next/previous buttons
                    if btnCnt > btnsPP and (not indexEnable or indexOpen) then
                        if LoadBoolean(htbBtns, BTN_SHOW, BTN_NEXT) and (pageId == 0 or pageIdMax > 1) then
                            set btnId = GetHandleId(DialogAddButton(diag, LoadStr(htbBtns, BTN_STR, BTN_NEXT), LoadInteger(htbBtns, BTN_HK, BTN_NEXT)))
                            call SaveBoolean(htbBtns, CURR_BTNS, btnId, true)
                        endif
                        if LoadBoolean(htbBtns, BTN_SHOW, BTN_PREV) and (pageId == 1 or pageIdMax > 1) then
                            set btnId = GetHandleId(DialogAddButton(diag, LoadStr(htbBtns, BTN_STR, BTN_PREV), LoadInteger(htbBtns, BTN_HK, BTN_PREV)))
                            call SaveBoolean(htbBtns, CURR_BTNS, btnId, false)
                        endif
                    endif
                else
                    set indexEnable = false
                    set indexOpen = false
                endif
          
                // Add Back/cancel buttons
                if indexEnable and not indexOpen and LoadBoolean(htbBtns, BTN_SHOW, BTN_BACK) then
                    set btnId = GetHandleId(DialogAddButton(diag, LoadStr(htbBtns, BTN_STR, BTN_BACK), LoadInteger(htbBtns, BTN_HK, BTN_BACK)))
                    call SaveBoolean(htbBtns, CURR_BTNS, btnId, true)
                endif
                if LoadBoolean(htbBtns, BTN_SHOW, BTN_CANCEL) then
                    call DialogAddButton(diag, LoadStr(htbBtns, BTN_STR, BTN_CANCEL), LoadInteger(htbBtns, BTN_HK, BTN_CANCEL))
                endif
        
                // Set message
                set msg = LoadStr(htbDialogs, diagId, MSG)
                if msg != "" and msg != null then
                    call DialogSetMessage(diag, msg)
                endif
            endif
    
            call DialogDisplay(plr, diag, display)
    
            set diag = null
            set htbBtns = null
            return true
        endif
 
        return false // dialog not found
    endfunction
    //========
 
    //==== Dialog Clicked ====
    private function DialogClickedCndAcn takes nothing returns boolean
        local integer diagId = GetHandleId(GetClickedDialog())
        local integer btnId = GetHandleId(GetClickedButton())
        local hashtable htbBtns = LoadHashtableHandle(htbDialogs, diagId, BTN_HTB)
        local integer pageId
        local integer btnCnt = LoadInteger(htbDialogs, diagId, BTN_CNT)
        local integer btnsPP = LoadInteger(htbDialogs, diagId, BTNS_PP)
        local integer keyPageId
        local boolean indexEnable = btnCnt > btnsPP and LoadBoolean(htbDialogs, diagId, INDEX_ENABLE)
        local boolean indexOpen = indexEnable and LoadBoolean(htbDialogs, diagId, INDEX_OPEN)
 
        if HaveSavedInteger(htbBtns, CURR_BTNS, btnId) then
            // INDEX BTN
            if indexOpen then
                // Save pageId & ID, hide index, and show dialog
                call SaveInteger(htbDialogs, diagId, PAGE_ID, LoadInteger(htbBtns, CURR_BTNS, btnId))
                call SaveInteger(htbDialogs, diagId, CLICKED_ID, DP_BROWSED)
                call SaveBoolean(htbDialogs, diagId, INDEX_OPEN, false)
                call DialogPDisplay(GetTriggerPlayer(), diagId, true)
          
            // REGULAR BTN
            else
                // Save clicked ID and hide/clear dialog
                call SaveInteger(htbDialogs, diagId, CLICKED_ID, LoadInteger(htbBtns, CURR_BTNS, btnId))
                call DialogPDisplay(GetTriggerPlayer(), diagId, false)
        
                set htbBtns = null
                return true
            endif
      
        // Back, Next or Previous Pushed
        elseif HaveSavedBoolean(htbBtns, CURR_BTNS, btnId) then
            if indexEnable then
                // INDEX NEXT/PREVIOUS BTN
                if indexOpen then
                    // Set new Page data
                    set keyPageId = INDEX_PAGE_ID
                    set btnCnt = (LoadInteger(htbDialogs, diagId, BTN_CNT)-1)/btnsPP+1
              
                // BACK BTN
                else // open index
                    call SaveBoolean(htbDialogs, diagId, INDEX_OPEN, true)
                endif
          
            // REGULAR NEXT/PREVIOUS BTN
            else
                // Set new Page data
                set keyPageId = PAGE_ID
                set btnCnt = LoadInteger(htbDialogs, diagId, BTN_CNT)
            endif
      
            if not indexEnable or indexOpen then
                set pageId = LoadInteger(htbDialogs, diagId, keyPageId)
          
                // NEXT
                if LoadBoolean(htbBtns, CURR_BTNS, btnId) then
                    if pageId >= (btnCnt-1)/btnsPP then
                        set pageId = 0
                    else
                        set pageId = pageId+1
                    endif
                // PREVIOUS
                elseif pageId == 0 then
                    set pageId = (btnCnt-1)/btnsPP
                else
                    set pageId = pageId-1
                endif
        
                // Save pageId and ID (-1)
                call SaveInteger(htbDialogs, diagId, keyPageId, pageId)
                call SaveInteger(htbDialogs, diagId, CLICKED_ID, DP_BROWSED)
            endif
      
            // Show new dialog
            call DialogPDisplay(GetTriggerPlayer(), diagId, true)
      
        // CANCEL
        else
            // Save ID (-2) and hide/clear dialog
            call SaveInteger(htbDialogs, diagId, CLICKED_ID, DP_CANCELED)
            call DialogPDisplay(GetTriggerPlayer(), diagId, false)
        endif
 
        set htbBtns = null
        return false
    endfunction
    //========

    //==== Create Dialog ====
    function DialogPCreate takes string msg, boolean showIndex returns integer
        local dialog diag = DialogCreate()
        local trigger trg = CreateTrigger()
        local hashtable htbBtns = InitHashtable()
        local integer diagId = GetHandleId(diag)
 
        // Handles
        call SaveDialogHandle(htbDialogs, diagId, DIAG, diag)
        call SaveHashtableHandle(htbDialogs, diagId, BTN_HTB, htbBtns)
        call SaveTriggerHandle(htbDialogs, diagId, TRG, trg)
        // Config
        call SaveStr(htbDialogs, diagId, MSG, msg)
        call SaveInteger(htbDialogs, diagId, BTNS_PP, DP_btnsPP)
        call SaveBoolean(htbDialogs, diagId, INDEX_ENABLE, showIndex)
        call SaveBoolean(htbBtns, BTN_SHOW, BTN_NEXT,   DP_showNext)
        call SaveBoolean(htbBtns, BTN_SHOW, BTN_PREV,   DP_showPrev)
        call SaveBoolean(htbBtns, BTN_SHOW, BTN_BACK,   DP_showBack)
        call SaveBoolean(htbBtns, BTN_SHOW, BTN_CANCEL, DP_showCancel)
        call SaveStr(htbBtns, BTN_STR, BTN_NEXT,   DP_strNext)
        call SaveStr(htbBtns, BTN_STR, BTN_PREV,   DP_strPrev)
        call SaveStr(htbBtns, BTN_STR, BTN_BACK,   DP_strBack)
        call SaveStr(htbBtns, BTN_STR, BTN_CANCEL, DP_strCancel)
        call SaveInteger(htbBtns, BTN_HK, BTN_NEXT,   DP_hkNext)
        call SaveInteger(htbBtns, BTN_HK, BTN_PREV,   DP_hkPrev)
        call SaveInteger(htbBtns, BTN_HK, BTN_BACK,   DP_hkBack)
        call SaveInteger(htbBtns, BTN_HK, BTN_CANCEL, DP_hkCancel)
        // State
        call SaveInteger(htbDialogs, diagId, BTN_CNT, 0)
        call SaveInteger(htbDialogs, diagId, PAGE_ID, 0)
        call SaveInteger(htbDialogs, diagId, CLICKED_ID, DP_NONE)
        call SaveBoolean(htbDialogs, diagId, INDEX_OPEN, showIndex)
        call SaveInteger(htbDialogs, diagId, INDEX_PAGE_ID, 0)
 
        // Register dialog event
        call TriggerRegisterDialogEvent(trg, diag)
        call TriggerAddCondition(trg, function DialogClickedCndAcn)
 
        set diag = null
        set trg = null
        set htbBtns = null
 
        return diagId
    endfunction
    //========
 
    //==== Destroy Dialog ====
    function DialogPDestroy takes integer diagId returns boolean
        local dialog diag = null
 
        if HaveSavedHandle(htbDialogs, diagId, DIAG) then
            set diag = LoadDialogHandle(htbDialogs, diagId, DIAG)
      
            // Destroy trigger & dialog
            if HaveSavedHandle(htbDialogs, diagId, TRG) then
                call DestroyTrigger(LoadTriggerHandle(htbDialogs, diagId, TRG))
            endif
            call DialogClear(diag)
            call DialogDestroy(diag)
      
            // Flush hashtables
            call FlushParentHashtable(LoadHashtableHandle(htbDialogs, diagId, BTN_HTB))
            call FlushChildHashtable(htbDialogs, diagId)
    
            set diag = null
            return true
        endif
 
        return false // Dialog not found
    endfunction
    //========
 
    //==== Set Page ====
    function DialogPSetPage takes integer diagId, integer pageId returns boolean
        local integer pageIdMax
        if HaveSavedInteger(htbDialogs, diagId, PAGE_ID) then
            if pageId < 0 then
                set pageId = 0
            elseif pageId > 0 then
                set pageIdMax = (LoadInteger(htbDialogs, diagId, BTN_CNT)-1)/LoadInteger(htbDialogs, diagId, BTNS_PP)
                if pageId > pageIdMax then
                    set pageId = pageIdMax
                endif
            endif
            call SaveInteger(htbDialogs, diagId, PAGE_ID, pageId)
            return true
        endif
        return false // dialog not found
    endfunction
    //========
 
    //==== Set Message ====
    function DialogPSetMessage takes integer diagId, string msg returns boolean
        if HaveSavedString(htbDialogs, diagId, MSG) then
            call SaveStr(htbDialogs, diagId, MSG, msg)
            return true
        endif
        return false // dialog not found
    endfunction
    //========
 
    //==== Set Buttons Per Page ====
    function DialogPSetButtonsPP takes integer diagId, integer btnsPP returns boolean
        if HaveSavedInteger(htbDialogs, diagId, BTNS_PP) then
            call SaveInteger(htbDialogs, diagId, BTNS_PP, btnsPP)
            return true
        endif
        return false // dialog not found
    endfunction
    //========
 
    //==== Set Browse Buttons Text ====
    function DialogPSetButtonsText takes integer diagId, string next, string prev, string back, string cancel returns boolean
        local hashtable htbBtns = null
  
        if HaveSavedHandle(htbDialogs, diagId, BTN_HTB) then
            set htbBtns = LoadHashtableHandle(htbDialogs, diagId, BTN_HTB)
      
            call SaveStr(htbBtns, BTN_STR, BTN_NEXT,   next)
            call SaveStr(htbBtns, BTN_STR, BTN_PREV,   prev)
            call SaveStr(htbBtns, BTN_STR, BTN_BACK,   back)
            call SaveStr(htbBtns, BTN_STR, BTN_CANCEL, cancel)
      
            set htbBtns = null
            return true
        endif
  
        return false // dialog not found
    endfunction
    //========
 
    //==== Set Browse Buttons Hotkey ====
    function DialogPSetButtonsHotkey takes integer diagId, integer next, integer prev, integer back, integer cancel returns boolean
        local hashtable htbBtns = null
  
        if HaveSavedHandle(htbDialogs, diagId, BTN_HTB) then
            set htbBtns = LoadHashtableHandle(htbDialogs, diagId, BTN_HTB)
      
            call SaveInteger(htbBtns, BTN_HK, BTN_NEXT,   next)
            call SaveInteger(htbBtns, BTN_HK, BTN_PREV,   prev)
            call SaveInteger(htbBtns, BTN_HK, BTN_BACK,   back)
            call SaveInteger(htbBtns, BTN_HK, BTN_CANCEL, cancel)
      
            set htbBtns = null
            return true
        endif
  
        return false // dialog not found
    endfunction
    //========
 
    //==== Display Browse Buttons ====
    function DialogPDisplayButtons takes integer diagId, boolean next, boolean prev, boolean back, boolean cancel returns boolean
        local hashtable htbBtns = null
  
        if HaveSavedHandle(htbDialogs, diagId, BTN_HTB) then
            set htbBtns = LoadHashtableHandle(htbDialogs, diagId, BTN_HTB)
      
            call SaveBoolean(htbBtns, BTN_SHOW, BTN_NEXT,   next)
            call SaveBoolean(htbBtns, BTN_SHOW, BTN_PREV,   prev)
            call SaveBoolean(htbBtns, BTN_SHOW, BTN_BACK,   back)
            call SaveBoolean(htbBtns, BTN_SHOW, BTN_CANCEL, cancel)
      
            set htbBtns = null
            return true
        endif
  
        return false // dialog not found
    endfunction
    //========
 
    //==== Get Dialog Handle ====
    function DialogPGetHandle takes integer diagId returns dialog
        if HaveSavedHandle(htbDialogs, diagId, DIAG) then
            return LoadDialogHandle(htbDialogs, diagId, DIAG)
        endif
        return null // dialog not found
    endfunction
    //========
 
    //==== Get Dialog Trigger ====
    function DialogPGetTrigger takes integer diagId returns trigger
        if HaveSavedHandle(htbDialogs, diagId, TRG) then
            return LoadTriggerHandle(htbDialogs, diagId, TRG)
        endif
        return null // dialog not found
    endfunction
    //========
 
    //==== Get Clicked Button Id ====
    function DialogPGetClickedId takes integer diagId returns integer
        if HaveSavedInteger(htbDialogs, diagId, CLICKED_ID) then
            return LoadInteger(htbDialogs, diagId, CLICKED_ID)
        endif
        return DP_NOT_FOUND // Dialog not found
    endfunction
    //===========================================================================
    //===========================================================================
//===============================================================================
//===============================================================================

//===============================================================================
//==== BUTTONS ==================================================================
//===============================================================================
    //==== Add Button ====
    function DialogPAddButton takes integer diagId, string text, integer hotkey returns integer
        local integer btnCnt
        local hashtable htbBtns = null
 
        if HaveSavedHandle(htbDialogs, diagId, BTN_HTB) then
            set btnCnt = LoadInteger(htbDialogs, diagId, BTN_CNT)
            set htbBtns = LoadHashtableHandle(htbDialogs, diagId, BTN_HTB)
    
            call SaveStr(htbBtns, BTN_STR, btnCnt, text)
            call SaveInteger(htbBtns, BTN_HK, btnCnt, hotkey)
            call SaveInteger(htbDialogs, diagId, BTN_CNT, btnCnt+1)
    
            set htbBtns = null
    
            return btnCnt
        endif
 
        return DP_NOT_FOUND // dialog not found
    endfunction
    //========
 
    //==== Add Quit Button ====
    function DialogPAddQuitButton takes integer diagId, boolean doSS, string text, integer hotkey returns integer
        local integer id = DialogPAddButton(diagId, text, hotkey)
        if id != DP_NOT_FOUND then
            call SaveBoolean(LoadHashtableHandle(htbDialogs, diagId, BTN_HTB), BTN_QUIT, id, doSS)
        endif
        return id
    endfunction
    //========
 
    //==== Set Button Text/Hotkey ====
    function DialogPSetButton takes integer diagId, integer btnId, string text, integer hotkey returns boolean
        local hashtable htbBtns = null
 
        if HaveSavedHandle(htbDialogs, diagId, BTN_HTB) then
            set htbBtns = LoadHashtableHandle(htbDialogs, diagId, BTN_HTB)
    
            if HaveSavedString(htbBtns, BTN_STR, btnId) then
                call SaveStr(htbBtns, BTN_STR, btnId, text)
                call SaveInteger(htbBtns, BTN_HK, btnId, hotkey)
                set htbBtns = null
                return true
            endif
    
            set htbBtns = null
        endif
 
        return false // dialog or button not found
    endfunction
    //========
 
    //==== Set Quit Button doSS/Text/Hotkey ====
    function DialogPSetQuitButton takes integer diagId, integer btnId, boolean doSS, string text, integer hotkey returns boolean
        if DialogPSetButton(diagId, btnId, text, hotkey) then
            call SaveBoolean(LoadHashtableHandle(htbDialogs, diagId, BTN_HTB), BTN_QUIT, btnId, doSS)
            return true
        endif
        return false
    endfunction
    //========
 
    //==== Get Button Text ====
    function DialogPGetButtonText takes integer diagId, integer btnId returns string
        local hashtable htbBtns = null
        local string txt
  
        if HaveSavedHandle(htbDialogs, diagId, BTN_HTB) then
            set htbBtns = LoadHashtableHandle(htbDialogs, diagId, BTN_HTB)
            if HaveSavedString(htbBtns, BTN_STR, btnId) then
                set txt = LoadStr(htbBtns, BTN_STR, btnId)
                set htbBtns = null
                return txt
            endif
            set htbBtns = null
        endif
        return null // dialog or btn not found
    endfunction
    //===========================================================================
    //===========================================================================
//===============================================================================
//===============================================================================

//===============================================================================
//==== INDEX ====================================================================
//===============================================================================
    //==== Enable/Disable Index Page ====
    function DialogPEnableIndex takes integer diagId, boolean enable returns boolean
        if HaveSavedBoolean(htbDialogs, diagId, INDEX_ENABLE) then
            call SaveBoolean(htbDialogs, diagId, INDEX_ENABLE, enable)
            call SaveBoolean(htbDialogs, diagId, INDEX_OPEN, enable)
            call SaveInteger(htbDialogs, diagId, INDEX_PAGE_ID, 0)
            return true
        endif
        return false // dialog not found
    endfunction
    //========
 
    //==== Set Index Page ====
    function DialogPSetIndexPage takes integer diagId, integer pageId returns boolean
        local integer pageIdMax
        local integer btnsPP
        if HaveSavedInteger(htbDialogs, diagId, INDEX_PAGE_ID) then
            if pageId < 0 then
                set pageId = 0
            elseif pageId > 0 then
                set btnsPP = LoadInteger(htbDialogs, diagId, BTNS_PP)
                set pageIdMax = ((LoadInteger(htbDialogs, diagId, BTN_CNT)-1)/btnsPP)/btnsPP
                if pageId > pageIdMax then
                    set pageId = pageIdMax
                endif
            endif
            call SaveInteger(htbDialogs, diagId, INDEX_PAGE_ID, pageId)
            return true
        endif
        return false // dialog not found
    endfunction
    //===========================================================================
    //===========================================================================
//===============================================================================
//===============================================================================

//===============================================================================
//==== INDEX BUTTONS ============================================================
//===============================================================================
    //==== Set Index Button Text/Hotkey ====
    function DialogPSetIndexButton takes integer diagId, integer btnId, string text, integer hotkey returns boolean
        local hashtable htbBtns = null
 
        if HaveSavedHandle(htbDialogs, diagId, BTN_HTB) then
            set htbBtns = LoadHashtableHandle(htbDialogs, diagId, BTN_HTB)
    
            call SaveStr(htbBtns, INDEX_BTN_STR, btnId, text)
            call SaveInteger(htbBtns, INDEX_BTN_HK, btnId, hotkey)
      
            set htbBtns = null
            return true
        endif
 
        return false // dialog not found
    endfunction
//===============================================================================
//===============================================================================
endlibrary