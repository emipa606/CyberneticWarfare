using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace CyberneticWarfare
{
    // Token: 0x02000007 RID: 7
    internal static class Logging
    {
        //// Token: 0x02000013 RID: 19
        //[HarmonyPatch(typeof(GenAttribute), "TryGetAttribute<T>", new Type[] { typeof(MemberInfo)})]
        //private static class Patch_PrimaryVerb
        //{
        //    // Token: 0x0600002B RID: 43 RVA: 0x00002A60 File Offset: 0x00000C60
        //    private static void Prefix(MemberInfo memberInfo)
        //    {
        //        Log.Message("TryGetAttribute: Name: " + memberInfo.Name + " DeclaringType:" + memberInfo.DeclaringType + " MemberType:" + memberInfo.MemberType);
        //    }
        //}
    }
}
