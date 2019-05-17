using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Shell : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_dpsd(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#else
			Profiler.BeginSample(methodName);
			#endif
			#endif
			Shell self=(Shell)checkSelf(l);
			Shell.DiePrtocolSendDelg v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.dpsd=v;
			else if(op==1) self.dpsd+=v;
			else if(op==2) self.dpsd-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
		#if DEBUG
		finally {
			#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.EndSample();
			#else
			Profiler.EndSample();
			#endif
		}
		#endif
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_tp(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#else
			Profiler.BeginSample(methodName);
			#endif
			#endif
			Shell self=(Shell)checkSelf(l);
			Shell.TestPrint v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.tp=v;
			else if(op==1) self.tp+=v;
			else if(op==2) self.tp-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
		#if DEBUG
		finally {
			#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.EndSample();
			#else
			Profiler.EndSample();
			#endif
		}
		#endif
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"Shell");
		addMember(l,"dpsd",null,set_dpsd,true);
		addMember(l,"tp",null,set_tp,true);
		createTypeMetatable(l,null, typeof(Shell),typeof(UnityEngine.MonoBehaviour));
	}
}
