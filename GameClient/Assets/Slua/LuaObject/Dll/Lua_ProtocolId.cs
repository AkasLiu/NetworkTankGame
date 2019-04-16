using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtocolId : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"ProtocolId");
		addMember(l,1000,"None");
		addMember(l,1001,"Login");
		addMember(l,1002,"Register");
		addMember(l,1003,"ReturnUserDataProtocol");
		addMember(l,1004,"StartGame");
		addMember(l,1005,"Move");
		addMember(l,1006,"SyncPosition");
		addMember(l,1007,"Fire");
		addMember(l,1008,"Damage");
		addMember(l,1009,"Die");
		addMember(l,1010,"Revive");
		addMember(l,1011,"ExitGame");
		addMember(l,1012,"JoinGame");
		LuaDLL.lua_pop(l, 1);
	}
}
