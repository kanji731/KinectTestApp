using SpeechLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KinectMusic
{
    public class Speak
    {
        private SpVoice sv1;

        public Speak()
        {
            sv1 = new SpVoice();

            sv1.Volume = 100;  // 音量の設定
            sv1.Rate = 0;     // 速度の設定
        }

        public void PlaySpeak(string text)
        {
            SpeechVoiceSpeakFlags flg = SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak;
            sv1.Speak(text, flg);
        }
    }
}
