﻿using System;
using System.Collections.Generic;
namespace SLua {
	[LuaBinder(3)]
	public class BindCustom {
		public static Action<IntPtr>[] GetBindList() {
			Action<IntPtr>[] list= {
				Lua_NetworkManager.reg,
				Lua_EasyTouch.reg,
				Lua_Shell.reg,
				Lua_System_Collections_Generic_List_1_int.reg,
				Lua_System_String.reg,
			};
			return list;
		}
	}
}
