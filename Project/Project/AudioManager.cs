using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Project
{
    public class AudioManager
    {
        private static AudioEngine audioEngine;

        public static AudioEngine AudioEngine
        {
            get 
            {
                if (audioEngine == null)
                {
                    audioEngine = new AudioEngine("Content\\Audio\\MyGameAudio2.xgs");
                }
                return AudioManager.audioEngine; 
            }
        }
        private static WaveBank waveBank;

        public static WaveBank WaveBank
        {
            get 
            {
                if (waveBank == null)
                {
                    waveBank = new WaveBank(AudioEngine, "Content\\Audio\\Wave Bank2.xwb");
                }
                return AudioManager.waveBank; 
            }
        }
        private static SoundBank soundBank;

        public static SoundBank SoundBank
        {
            get 
            {
                if (soundBank == null)
                {
                    WaveBank wb = WaveBank;
                    soundBank = new SoundBank(AudioEngine, "Content\\Audio\\Sound Bank2.xsb");
                }
                return AudioManager.soundBank; 
            }
        }


        public static Cue GetCue(string name)
        {
            return SoundBank.GetCue(name);
        }
    }
}
