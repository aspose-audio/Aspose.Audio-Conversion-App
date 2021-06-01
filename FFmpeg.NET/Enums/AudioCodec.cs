using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFmpeg.NET.Enums
{
    public enum AudioCodec
    {
        Default,
        copy,
        /// <summary>
        /// aac
        /// </summary>
        aac, //AAC (Advanced Audio Coding)
        /// <summary>
        /// aiff
        /// </summary>
        pcm_alaw, //PCM A-law, G.711 A-law
        pcm_mulaw, //PCM mu-law, G.711 mu-law
        pcm_s16be, //PCM signed 16-bit big-endian
        pcm_s16le, //PCM signed 16-bit little-endian
        pcm_s24be, //PCM signed 24-bit big-endian
        pcm_s24le, //PCM signed 24-bit little-endian
        pcm_s32be, //PCM signed 32-bit big-endian
        pcm_s32le, //PCM signed 32-bit little-endian
        /// <summary>
        /// flac
        /// </summary>
        flac, ///FLAC (Free Lossless Audio Codec)

        libmp3lame, //MP3 (MPEG audio layer 3)

        wmav1, //Windows Media Audio 1
        wmav2, //Windows Media Audio 2

        ac3,

        libopencore_amrnb,
        libvo_amrwbenc,

        libvorbis

    }
}
