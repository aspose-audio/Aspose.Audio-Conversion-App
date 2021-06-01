using FFmpeg.NET.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AudioVedio.Apis.Services
{
    public class ConstantsService
    {
        internal static string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = "", str = "";
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }
        public static string GenerateUrlFilename()
        {
            return GetRandomString(8, true, true, true, false, "");
        }
        public static AudioCodec GetAudioCodec(string audioCodec)
        {
            AudioCodec ret;
            if (Enum.TryParse(audioCodec, out ret))
            {
                return ret;
            }
            else
            {
                throw new Exception("Invalid audio codec");
            }
        }

        public static AudioFormat GetAudioFormat(string audioFormat)
        {
            AudioFormat ret;
            if (Enum.TryParse(audioFormat, out ret))
            {
                return ret;
            }
            else
            {
                throw new Exception("Invalid audio format");
            }
        }
    }
}
