using BepInEx;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace WandererUnguarded
{
    [BepInPlugin("Aidanamite.WandererUnguarded", "WandererUnguarded", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        internal static Assembly modAssembly = Assembly.GetExecutingAssembly();
        internal static string modName = $"{modAssembly.GetName().Name}";
        internal static string modDir = $"{Environment.CurrentDirectory}\\BepInEx\\{modName}";

        void Awake()
        {
            new Harmony($"com.Aidanamite.{modName}").PatchAll(modAssembly);
            Logger.LogInfo($"{modName} has loaded");
        }
    }

    [HarmonyPatch(typeof(Wanderer), "Init")]
    public class Patch_Wanderer_Init
    {
        static void Postfix(Wanderer __instance)
        {
            __instance.enemy.enemyStats.Invencible = false;
            var h = 2000 + 1000 * GameManager.Instance.GetCurrentGamePlusLevel();
            __instance.enemy.enemyStats._baseStats.maxHealth = h;
            __instance.enemy.enemyStats._baseStats.defense = 90;
            __instance.enemy.enemyStats._currentStats.maxHealth = h;
            __instance.enemy.enemyStats._currentStats.defense = 90;
            __instance.enemy.enemyStats._currentHealth = h;
        }
    }

    [HarmonyPatch(typeof(Enemy), "CalcHitDamage")]
    public class Patch_Enemy_CalcHitDamage
    {
        static void Prefix(Enemy __instance, ref float hitStrength)
        {
            if (__instance.enemyBehaviour is Wanderer)
                hitStrength = Mathf.Max(0, hitStrength - (100 + 20 * GameManager.Instance.GetCurrentGamePlusLevel()));
        }
    }
}