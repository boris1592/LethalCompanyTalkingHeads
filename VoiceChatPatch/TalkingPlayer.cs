using System.Linq;
using Dissonance;
using Dissonance.Audio.Playback;
using GameNetcodeStuff;
using UnityEngine;
using TalkingHeads.Util;

namespace TalkingHeads.VoiceChatPatch
{
    public class TalkingPlayer
    {
        public readonly PlayerControllerB Player;

        private readonly VoicePlayerState _voice;
        private readonly Transform _targetTransform;
        private float _currentScale = 0;

        private TalkingPlayer(PlayerControllerB player, VoicePlayerState voice)
        {
            Player = player;
            _voice = voice;
            _targetTransform = Player.lowerSpine.FindComponentInChildren("spine.004");
        }

        public void UpdateScale(float deltaTime)
        {
            var targetScale = _voice.Amplitude * Plugin.Config.AmplitudeMultiplier;
            _currentScale = _currentScale.MoveTowards(targetScale, Plugin.Config.ScaleSpeed * deltaTime);
            _targetTransform.localScale = (1 + _currentScale) * Vector3.one;
        }

        public void ResetScale()
        {
            _targetTransform.localScale = Vector3.one;
        }

        public static TalkingPlayer CreateVoicePlayer(VoicePlayerState state)
        {
            if (state.Playback is not VoicePlayback) return null;

            var playback = state.Playback as VoicePlayback;
            var player = StartOfRound.Instance.allPlayerScripts
                .FirstOrDefault(player => player.currentVoiceChatAudioSource == playback.AudioSource);

            return player is null ? null : new TalkingPlayer(player, state);
        }
    }
}
