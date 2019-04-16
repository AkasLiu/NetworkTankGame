using System;
using System.Collections.Generic;
namespace SLua {
	[LuaBinder(2)]
	public class BindDll {
		public static Action<IntPtr>[] GetBindList() {
			Action<IntPtr>[] list= {
				Lua_ProtocolId.reg,
				Lua_Common_CustomTransform.reg,
				Lua_Common_Protocol_BaseProtocol.reg,
				Lua_Common_Protocol_JoinGameProtocol.reg,
				Lua_Common_Protocol_DieProtocol.reg,
				Lua_Common_Protocol_ExitGameProtocol.reg,
				Lua_Common_Protocol_FireProtocol.reg,
				Lua_Common_Protocol_LoginProtocol.reg,
				Lua_Common_Protocol_RegisterProtocol.reg,
				Lua_Common_Protocol_ResultProtocol.reg,
				Lua_Common_Protocol_ReturnUserDataProtocol.reg,
				Lua_Common_Protocol_ReviveProtocol.reg,
				Lua_Common_Protocol_StartGameProtocol.reg,
				Lua_Common_Protocol_SyncPositionProtocol.reg,
			};
			return list;
		}
	}
}
