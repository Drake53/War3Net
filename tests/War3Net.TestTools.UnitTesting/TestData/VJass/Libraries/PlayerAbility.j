library PlayerAbility requires Table, PlayerUtils //wc3_v1.29+
//== ==== ==== ==== ==== ==== ==== ==== ==== ==== ==== ==== ==== ==== ==== ==== ====
//  __________________
//   #  PlayerAbility
//			v1.2c, by Overfrost
//	----------------------------
//
//	  - packs abilityId, abilityLevel, and abilityOwner into one package,
//		allowing better manipulation of abilityUI
//
//	  - handles both global abilityUI and each player's abilityUI,
//		prioritizing player's abilityUI over the global
//
//  ______________
//   #  Requires:
//
//	  v Warcraft3 v1.29+
//    - Table
//          hiveworkshop.com/threads/snippet-new-table.188084
//	  - PlayerUtils
//			hiveworkshop.com/threads/playerutils.278559
//
//== ==== ==== ==== ==== ==== ==== ==== ==== ==== ==== ==== ==== ==== ==== ==== ====
//! novjass

    //
    struct PlayerAbility extends array

        //------------
        // Instancer
        //
        static method get takes integer abilId, integer level, player owner returns thistype
		// - if owner = null, then returns the global ability, otherwise returns a player's ability
        // - no destructors
        //

        //---------------
        // Basic Fields
        //
        integer id     // abilId
		integer level
		player owner
		// - if any of these changes, the instance itself also changes
		// - any field other than these are not copied to the new instance
		//

		//-------------------
		// All Level Fields
		//
		integer posX
		integer posY
		// - can take any positive integers or 0
		//
		string icon
		// - all of those apply to all levels of the ability, at once
		//

		//-------------------
		// Per Level Fields
		//
		string name  // Tooltip
		string desc  // ExtendedTooltip
		//
		string learnName  // ResearchTooltip
		string learnDesc  // ResearchExtendedTooltip
		//
		string actedName  // ActivatedTooltip
		string actedDesc  // ActivatedExtendedTooltip
		//

        //----------
        // Methods
        //
		method clearPos takes nothing returns thistype(this)   // posX/posY
		method clearIcon takes nothing returns thistype(this)
		//
		method clearTip takes nothing returns thistype(this)       // name/desc
		method clearLearnTip takes nothing returns thistype(this)  // learnName/learnDesc
		method clearActedTip takes nothing returns thistype(this)  // actedName/actedDesc
		//
		method clear takes nothing returns thistype(this)  // all
		// - for all above:
		//   - if this ability is global, clears the modifications of each player's ability
		//   - otherwise clears the modifications of this player's ability only
		//   - can't clear the modifications of a global ability itself
        //

//! endnovjass
//== ==== ==== ==== ==== ==== ==== ==== ==== ==== ==== ==== ==== ==== ==== ==== ====

//
private keyword pm
struct PlayerAbility extends array
	//
	private static integer pgCount = 0
	private static integer pgH0
	private static integer pgH1

	//---------
	// fields
	private static Table pgId
	private static Table pgAbiId
	//
	private static Table pgPos
	private static Table pgIcon
	//
	private static Table pgTip
	private static Table pgLearnTip
	private static Table pgActedTip

	//-------
	// hash
	private static method pgHash takes integer aId, integer aLevel, integer aUser returns integer
		return aId*pgH1 + aLevel*pgH0 + aUser
	endmethod //inlines
	private static method pgHash2 takes integer aId, integer aUser returns integer
		return aId*pgH1 + aUser
	endmethod //inlines
	// indeed 0 to (pgH1-1) are skipped, this is intended

	//
	static method get takes integer aAbiId, integer aLevel, player aOwner returns thistype
	//! textmacro P_PLAYER_ABILITY_GET takes ABI_ID
			//
			local integer lId = pgId[$ABI_ID$]
			local integer lHash
			//
			if (lId == 0) then
				set pgCount = pgCount + 1
				set lId = pgCount
				//
				set pgId[$ABI_ID$] = lId
				set pgAbiId[lId] = $ABI_ID$
			endif
			//
			set lHash = pgHash2(lId, bj_MAX_PLAYER_SLOTS)
			//
	//! endtextmacro
	//! runtextmacro P_PLAYER_ABILITY_GET("aAbiId")
		//
	//! textmacro P_PLAYER_ABILITY_SET_GLOBAL takes B, ABI_ID, LEVEL
			//
			static if ($B$) then
				if (not pgPos.has(lHash)) then
					set pgPos[ lHash] = BlzGetAbilityPosX($ABI_ID$)
					set pgPos[-lHash] = BlzGetAbilityPosY($ABI_ID$)
					//
					set pgIcon.string[lHash] = BlzGetAbilityIcon($ABI_ID$)
				endif
				//
				set lHash = lHash + $LEVEL$*pgH0
			endif
			//
			if (not pgTip.string.has(lHash)) then
				set pgTip.string[ lHash] = BlzGetAbilityTooltip($ABI_ID$, $LEVEL$)
				set pgTip.string[-lHash] = BlzGetAbilityExtendedTooltip($ABI_ID$, $LEVEL$)
				//
				set pgLearnTip.string[ lHash] = BlzGetAbilityResearchTooltip($ABI_ID$, $LEVEL$)
				set pgLearnTip.string[-lHash] = BlzGetAbilityResearchExtendedTooltip($ABI_ID$, $LEVEL$)
				//
				set pgActedTip.string[ lHash] = BlzGetAbilityActivatedTooltip($ABI_ID$, $LEVEL$)
				set pgActedTip.string[-lHash] = BlzGetAbilityActivatedExtendedTooltip($ABI_ID$, $LEVEL$)
			endif
			//
	//! endtextmacro
	//! runtextmacro P_PLAYER_ABILITY_SET_GLOBAL("true", "aAbiId", "aLevel")
		//
		if (aOwner == null) then
			return lHash
		endif
		//
		return lHash - bj_MAX_PLAYER_SLOTS + User[aOwner]
	endmethod

	//----------
	// helpers
	private method pId takes thistype aThis returns integer
		return (this - (aThis/pgH0)*pgH0)/pgH1
	endmethod //inlines
	private method pUser takes thistype aThis returns User
		return this - (aThis/pgH1)*pgH1
	endmethod //inlines

	//---------------
	// basic fields
	method operator id takes nothing returns integer
		return pgAbiId[pId(this)]
	endmethod
	method operator level takes nothing returns integer
		return this/pgH0
	endmethod //inlines
	method operator owner takes nothing returns player
		return pUser(this).toPlayer()
	endmethod
	//
	method operator id= takes integer aAbiId returns thistype
		local integer lLevel = level
		//
	//! runtextmacro P_PLAYER_ABILITY_GET("aAbiId")
		//
	//! runtextmacro P_PLAYER_ABILITY_SET_GLOBAL("true", "aAbiId", "lLevel")
		//
		return lHash - bj_MAX_PLAYER_SLOTS + pUser(this)
	endmethod
	method operator level= takes integer aLevel returns thistype
		local integer lId = pId(this)
		local integer lAbiId = pgAbiId[lId]
		//
		local integer lHash = pgHash(lId, aLevel, bj_MAX_PLAYER_SLOTS)
		//
	//! runtextmacro P_PLAYER_ABILITY_SET_GLOBAL("false", "lAbiId", "aLevel")
		//
		return lHash - bj_MAX_PLAYER_SLOTS + pUser(this)
	endmethod
	method operator owner= takes player aOwner returns thistype
		return (this/pgH1)*pgH1 + User[aOwner]
	endmethod //inlines

	//------------------
	// unilevel fields
//! textmacro P_PLAYER_ABILITY_ULEVEL_FIELD takes ID, TAB, SIGN, TYPE, SUFFIX, C, SIZE
		//
		method operator $ID$ takes nothing returns $TYPE$
			set this = this - (this/pgH0)*pgH0
			if ($TAB$.$TYPE$.has($SIGN$this)) then
				return $TAB$.$TYPE$[$SIGN$this]
			endif
			//
			return $TAB$.$TYPE$[$SIGN$pgHash2(pId(this), bj_MAX_PLAYER_SLOTS)]
		endmethod
		method operator $ID$= takes $TYPE$ aVal returns nothing
			local integer lId = pId(this)
			local integer lAbiId = pgAbiId[lId]
			//
			local User lUser = pUser(this)
			//
		$C$ set aVal = aVal - (aVal/$SIZE$)*$SIZE$
			//
			if (lUser == bj_MAX_PLAYER_SLOTS) then
				set $TAB$.$TYPE$[$SIGN$pgHash2(lId, bj_MAX_PLAYER_SLOTS)] = aVal
				//
				set lUser = User.first
				loop
					//
					if ((not $TAB$.$TYPE$.has($SIGN$pgHash2(lId, lUser))) and User.LocalId == lUser) then
						call BlzSetAbility$SUFFIX$(lAbiId, aVal)
					endif
					//
					exitwhen lUser == User.last
					set lUser = lUser.next
				endloop
				//
				return
			endif
			//
			if (User.LocalId == lUser) then
				call BlzSetAbility$SUFFIX$(lAbiId, aVal)
			endif
			//
			set $TAB$.$TYPE$[$SIGN$pgHash2(lId, lUser)] = aVal
		endmethod
//! endtextmacro
	//
//! runtextmacro P_PLAYER_ABILITY_ULEVEL_FIELD("posX", "pgPos",  "", "integer", "PosX", "", "4")
//! runtextmacro P_PLAYER_ABILITY_ULEVEL_FIELD("posY", "pgPos", "-", "integer", "PosY", "", "3")
	//
//! runtextmacro P_PLAYER_ABILITY_ULEVEL_FIELD("icon", "pgIcon", "", "string", "Icon", "//", "")

	//--------------------
	// multilevel fields
//! textmacro P_PLAYER_ABILITY_TOOLTIP_FIELD takes ID, TAB, SIGN, INFIX
		//
		method operator $ID$ takes nothing returns string
			if ($TAB$.string.has($SIGN$this)) then
				return $TAB$.string[$SIGN$this]
			endif
			return $TAB$.string[$SIGN$pgHash(pId(this), level, bj_MAX_PLAYER_SLOTS)]
		endmethod
		method operator $ID$= takes string aTip returns nothing
			local integer lId = pId(this)
			local integer lAbiId = pgAbiId[lId]
			local integer lLevel = level
			//
			local User lUser = pUser(this)
			//
			if (lUser == bj_MAX_PLAYER_SLOTS) then
				set lUser = User.first
				loop
					//
					if ((not $TAB$.string.has($SIGN$pgHash(lId, lLevel, lUser))) and User.LocalId == lUser) then
						call BlzSetAbility$INFIX$Tooltip(lAbiId, aTip, lLevel)
					endif
					//
					exitwhen lUser == User.last
					set lUser = lUser.next
				endloop
				//
			elseif (User.LocalId == lUser) then
				call BlzSetAbility$INFIX$Tooltip(lAbiId, aTip, lLevel)
			endif
			//
			set $TAB$.string[$SIGN$this] = aTip
		endmethod
//! endtextmacro
	//
//! runtextmacro P_PLAYER_ABILITY_TOOLTIP_FIELD("name", "pgTip",  "", "")
//! runtextmacro P_PLAYER_ABILITY_TOOLTIP_FIELD("desc", "pgTip", "-", "Extended")
	//
//! runtextmacro P_PLAYER_ABILITY_TOOLTIP_FIELD("learnName", "pgLearnTip",  "", "Research")
//! runtextmacro P_PLAYER_ABILITY_TOOLTIP_FIELD("learnDesc", "pgLearnTip", "-", "ResearchExtended")
	//
//! runtextmacro P_PLAYER_ABILITY_TOOLTIP_FIELD("actedName", "pgActedTip",  "", "Activated")
//! runtextmacro P_PLAYER_ABILITY_TOOLTIP_FIELD("actedDesc", "pgActedTip", "-", "ActivatedExtended")

	//--------------------
	// unilevel clearers
//! textmacro P_PLAYER_ABILITY_ULEVEL_CLEAR takes ID, TAB, C, TYPE, OP0, OP1, SUFFIX0, SUFFIX1
		//
		method clear$ID$ takes nothing returns thistype
			local integer lId = pId(this)
			local integer lAbiId = pgAbiId[lId]
			//
			local User lUser = pUser(this)
			//
			local thistype lHash = pgHash2(lId, bj_MAX_PLAYER_SLOTS)
			local $TYPE$ lVal0 = lHash.$OP0$
		$C$ local $TYPE$ lVal1 = lHash.$OP1$
			//
			if (lUser == bj_MAX_PLAYER_SLOTS) then
				set lUser = User.first
				loop
					set lHash = pgHash2(lId, lUser)
					//
					call $TAB$.$TYPE$.remove( lHash)
				$C$ call $TAB$.$TYPE$.remove(-lHash)
					//
					if (User.LocalId == lUser) then
						call BlzSetAbility$SUFFIX0$(lAbiId, lVal0)
					$C$ call BlzSetAbility$SUFFIX1$(lAbiId, lVal1)
					endif
					//
					exitwhen lUser == User.last
					set lUser = lUser.next
				endloop
			else
				set lHash = pgHash2(lId, lUser)
				//
				call $TAB$.$TYPE$.remove( lHash)
			$C$ call $TAB$.$TYPE$.remove(-lHash)
				//
				if (User.LocalId == lUser) then
					call BlzSetAbility$SUFFIX0$(lAbiId, lVal0)
				$C$ call BlzSetAbility$SUFFIX1$(lAbiId, lVal1)
				endif
			endif
			//
			return this
		endmethod
//! endtextmacro
	//
//! runtextmacro P_PLAYER_ABILITY_ULEVEL_CLEAR("Pos",  "pgPos",   "", "integer", "posX", "posY", "PosX", "PosY")
//! runtextmacro P_PLAYER_ABILITY_ULEVEL_CLEAR("Icon", "pgIcon", "//", "string", "icon", ""    , "Icon", "")

	//-------------------
	// tooltip clearers
//! textmacro P_PLAYER_ABILITY_TOOLTIP_CLEAR takes PREFIX, B, OP_PREFIX, INFIX
		//
		method clear$PREFIX$Tip takes nothing returns thistype
			local integer lId = pId(this)
			local integer lAbiId = pgAbiId[lId]
			local integer lLevel = level
			//
			local User lUser = pUser(this)
			//
			local thistype lHash = pgHash(lId, lLevel, bj_MAX_PLAYER_SLOTS)
			static if ($B$) then
				local string lName = lHash.$OP_PREFIX$Name
				local string lDesc = lHash.$OP_PREFIX$Desc
			else
				local string lName = lHash.name
				local string lDesc = lHash.desc
			endif
			//
			if (lUser == bj_MAX_PLAYER_SLOTS) then
				set lUser = User.first
				loop
					set lHash = pgHash(lId, lLevel, lUser)
					//
					call pg$PREFIX$Tip.string.remove( lHash)
					call pg$PREFIX$Tip.string.remove(-lHash)
					//
					if (User.LocalId == lUser) then
						call BlzSetAbility$INFIX$Tooltip(lAbiId, lName, lLevel)
						call BlzSetAbility$INFIX$ExtendedTooltip(lAbiId, lDesc, lLevel)
					endif
					//
					exitwhen lUser == User.last
					set lUser = lUser.next
				endloop
				//
			else
				call pg$PREFIX$Tip.string.remove( this)
				call pg$PREFIX$Tip.string.remove(-this)
				//
				if (User.LocalId == lUser) then
					call BlzSetAbility$INFIX$Tooltip(lAbiId, lName, lLevel)
					call BlzSetAbility$INFIX$ExtendedTooltip(lAbiId, lDesc, lLevel)
				endif
			endif
			//
			return this
		endmethod
//! endtextmacro
	//
//! runtextmacro P_PLAYER_ABILITY_TOOLTIP_CLEAR("", "false", "", "")
	//
//! runtextmacro P_PLAYER_ABILITY_TOOLTIP_CLEAR("Learn", "true", "learn", "Research")
//! runtextmacro P_PLAYER_ABILITY_TOOLTIP_CLEAR("Acted", "true", "acted", "Activated")

	//--------------
	// all clearer
//! textmacro P_PLAYER_ABILITY_CLEAR takes C
		//
		call pgTip.string.remove( lHash)
		call pgTip.string.remove(-lHash)
		//
		call pgLearnTip.string.remove( lHash)
		call pgLearnTip.string.remove(-lHash)
		//
		call pgActedTip.string.remove( lHash)
		call pgActedTip.string.remove(-lHash)
		//
		set lHash = pgHash2(lId, lUser)
		call pgPos.remove( lHash)
		call pgPos.remove(-lHash)
		//
		call pgIcon.string.remove(lHash)
		//
	$C$ set lHash = lHash + lLevel*pgH0
		//
		if (User.LocalId == lUser) then
			call BlzSetAbilityPosX(lAbiId, lPosX)
			call BlzSetAbilityPosY(lAbiId, lPosY)
			//
			call BlzSetAbilityIcon(lAbiId, lIcon)
			//
			call BlzSetAbilityTooltip(lAbiId, lName, lLevel)
			call BlzSetAbilityExtendedTooltip(lAbiId, lDesc, lLevel)
			//
			call BlzSetAbilityResearchTooltip(lAbiId, lLearnName, lLevel)
			call BlzSetAbilityResearchExtendedTooltip(lAbiId, lLearnDesc, lLevel)
			//
			call BlzSetAbilityActivatedTooltip(lAbiId, lActedName, lLevel)
			call BlzSetAbilityActivatedExtendedTooltip(lAbiId, lActedDesc, lLevel)
		endif
		//
//! endtextmacro
	method clear takes nothing returns thistype
		local integer lId = pId(this)
		local integer lAbiId = pgAbiId[lId]
		local integer lLevel = level
		//
		local User lUser = pUser(this)
		//
		local string lName
		local string lDesc
		//
		local string lLearnName
		local string lLearnDesc
		//
		local string lActedName
		local string lActedDesc
		//
		local thistype lHash = pgHash2(lId, bj_MAX_PLAYER_SLOTS)
		local integer lPosX = lHash.posX
		local integer lPosY = lHash.posY
		//
		local string lIcon = lHash.icon
		//
		set lHash = lHash + lLevel*pgH0
		set lName = lHash.name
		set lDesc = lHash.desc
		//
		set lLearnName = lHash.learnName
		set lLearnDesc = lHash.learnDesc
		//
		set lActedName = lHash.actedName
		set lActedDesc = lHash.actedDesc
		//
		if (lUser == bj_MAX_PLAYER_SLOTS) then
			set lUser = User.first
			loop
				//
			//! runtextmacro P_PLAYER_ABILITY_CLEAR("")
				//
				exitwhen lUser == User.last
				set lUser = lUser.next
			endloop
		else
			//
		//! runtextmacro P_PLAYER_ABILITY_CLEAR("//")
			//
		endif
		//
		return this
	endmethod

	implement pm
endstruct
private module pm
	//--------------
	// initializer
	private static method onInit takes nothing returns nothing
		set pgH1 = bj_MAX_PLAYER_SLOTS + 1
		set pgH0 = pgH1*0x2000
		//
		set pgId    = Table.create()
		set pgAbiId = Table.create()
		//
		set pgPos  = Table.create()
		set pgIcon = Table.create()
		//
		set pgTip      = Table.create()
		set pgLearnTip = Table.create()
		set pgActedTip = Table.create()
	endmethod
endmodule

endlibrary