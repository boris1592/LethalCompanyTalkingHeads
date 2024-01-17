using System.Collections.Generic;
using Dissonance;
using HarmonyLib;
using UnityEngine;

namespace TalkingHeads.VoiceChatPatch
{
    [HarmonyPatch(typeof(StartOfRound))]
    public class VoicePatchPatch
    {
        private readonly static List<TalkingPlayer> _talkingPlayers = new();
        private static DissonanceComms _voiceChatModule = null;

        [HarmonyPatch(nameof(StartOfRound.OnEnable))]
        [HarmonyPostfix]
        private static void OnEnable(ref DissonanceComms ___voiceChatModule)
        {
            _voiceChatModule = ___voiceChatModule;
            _voiceChatModule.OnPlayerStartedSpeaking += OnPlayerStartedSpeaking;
            _voiceChatModule.OnPlayerStoppedSpeaking += OnPlayerStoppedSpeaking;
            Plugin.Log.LogInfo("StartOfRound patch subscribed to voiceChatModule events");
        }

        [HarmonyPatch(nameof(StartOfRound.OnDisable))]
        [HarmonyPostfix]
        private static void OnDisable()
        {
            _voiceChatModule.OnPlayerStartedSpeaking -= OnPlayerStartedSpeaking;
            _voiceChatModule.OnPlayerStoppedSpeaking -= OnPlayerStoppedSpeaking;
            _voiceChatModule = null;
        }

        private static void OnPlayerStartedSpeaking(VoicePlayerState state)
        {
            var player = TalkingPlayer.CreateVoicePlayer(state);

            if (player is null) return;

            _talkingPlayers.Add(player);
        }

        private static void OnPlayerStoppedSpeaking(VoicePlayerState state)
        {
            var player = TalkingPlayer.CreateVoicePlayer(state);

            if (player is null) return;

            _talkingPlayers.RemoveAll(p => p.Player == player.Player);
            player.ResetScale();
        }

        // LateUpdate here because animations override the scale in Update
        [HarmonyPatch(nameof(StartOfRound.LateUpdate))]
        [HarmonyPostfix]
        private static void LateUpdate()
        {
            foreach (var player in _talkingPlayers)
                player.UpdateScale(Time.deltaTime);
        }
    }
}
