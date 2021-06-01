using AudioVedio.Apis.Services;
using FFmpeg.NET;
using System;
using System.Collections.Generic;
using System.Text;

namespace AudioVedio.Apis.Models.App
{
    public class ConvertParam
    {
        public string AdjustVolumn { get; set; }
        public string AudioCodec { get; set; }

        public string AudioFormat { get; set; }
        public string AudioBitRate { get; set; }

        public string AudioQuality { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public ConversionOptions ToConversionOptions()
        {
            ConversionOptions ret = new ConversionOptions();
            if(!string.IsNullOrEmpty(AdjustVolumn))
            {
                try
                {
                    ret.AdjustVolumn = Int32.Parse(AdjustVolumn);
                }
                catch
                {

                }
            }
            if (!string.IsNullOrEmpty(AudioQuality))
            {
                try
                {
                    ret.AudioQuality = Int32.Parse(AudioQuality);
                }
                catch
                {

                }
            }
            if (!string.IsNullOrEmpty(AudioBitRate))
            {
                try
                {
                    ret.AudioBitRate = Int32.Parse(AudioBitRate);
                }
                catch
                {

                }
            }
            if (!string.IsNullOrEmpty(AudioCodec))
            {
                ret.AudioCodec = ConstantsService.GetAudioCodec(AudioCodec);
            }

            if (!string.IsNullOrEmpty(AudioFormat))
            {
                ret.AudioFormat = ConstantsService.GetAudioFormat(AudioFormat);
            }

            if(!string.IsNullOrEmpty(StartTime))
            {
                try
                {
                    int seconds = this.GetSecondsFromTimeString(StartTime);
                    ret.Seek = TimeSpan.FromSeconds(seconds);
                }
                catch(Exception)
                {

                }
            }
            if(!string.IsNullOrEmpty(EndTime))
            {
                try
                {
                    int duration = this.GetSecondsFromTimeString(EndTime);

                    if(ret.Seek.HasValue)
                    {
                        duration -= (int)ret.Seek.Value.TotalSeconds;
                    }
                    ret.EndDuration = TimeSpan.FromSeconds(duration);
                }
                catch (Exception)
                {

                }
            }
            return ret;
        }

        private int GetSecondsFromTimeString(string timeStr)
        {
            string[] timeArr = timeStr.Split(':');
            int hour = int.Parse(timeArr[0]);
            int minute = int.Parse(timeArr[1]);
            int second = int.Parse(timeArr[2]);
            return hour * 60 * 60 + minute * 60 + second;
        }
    }
}
