/*
SpawnMini - Rust Harmony Mod

Author: Kxrse
Patching: HarmonyLib (0Harmony.dll)
Target: Assembly-CSharp.dll (.NET Framework 4.8)
*/

using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RustMods
{
    [HarmonyPatch]
    public class SpawnMini
    {
        private static Harmony _harmony;
        private static Dictionary<ulong, BaseEntity> _playerMinis = new Dictionary<ulong, BaseEntity>();
        private const string MINI_PREFAB = "assets/content/vehicles/minicopter/minicopter.entity.prefab";

        public static void Initialize()
        {
            _harmony = new Harmony("com.kxrse.spawnmini");
            _harmony.PatchAll();
            Debug.Log("[SpawnMini] Initialized");
        }

        public static void Shutdown()
        {
            _harmony?.UnpatchAll("com.kxrse.spawnmini");
            Debug.Log("[SpawnMini] Shutdown");
        }

        [HarmonyPatch(typeof(ConVar.Chat), "sayImpl")]
        public static class Chat_sayImpl_Patch
        {
            static bool Prefix(ConVar.Chat.ChatChannel targetChannel, ConsoleSystem.Arg arg)
            {
                try
                {
                    BasePlayer player = arg.Player();
                    if (player == null)
                        return true;

                    string message = arg.GetString(0, "text");
                    if (message == null)
                        return true;

                    string lower = message.ToLower().Trim();

                    if (lower == "/mini")
                    {
                        HandleMiniCommand(player);
                        return false;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[SpawnMini] Error in chat prefix: {ex.Message}");
                    return true;
                }
            }
        }

        private static void HandleMiniCommand(BasePlayer player)
        {
            ulong uid = player.userID.Get();

            KillPreviousMini(uid);

            Vector3 position = player.transform.position + (player.eyes.BodyForward() * 3f) + Vector3.up * 1f;

            BaseEntity entity = GameManager.server.CreateEntity(MINI_PREFAB, position, Quaternion.identity);
            if (entity == null)
            {
                player.ChatMessage("Failed to spawn minicopter.");
                return;
            }

            entity.OwnerID = uid;
            entity.Spawn();

            FillFuel(entity);

            _playerMinis[uid] = entity;
            player.ChatMessage("Minicopter spawned!");
        }

        private static void KillPreviousMini(ulong uid)
        {
            BaseEntity oldMini;
            if (_playerMinis.TryGetValue(uid, out oldMini))
            {
                if (oldMini != null && !oldMini.IsDestroyed)
                {
                    oldMini.Kill();
                }
                _playerMinis.Remove(uid);
            }
        }

        private static void FillFuel(BaseEntity entity)
        {
            if (entity.children == null)
                return;

            for (int i = 0; i < entity.children.Count; i++)
            {
                StorageContainer storage = entity.children[i] as StorageContainer;
                if (storage != null)
                {
                    Item fuel = ItemManager.CreateByName("lowgradefuel", 500);
                    if (fuel != null)
                        fuel.MoveToContainer(storage.inventory);
                    break;
                }
            }
        }
    }
}