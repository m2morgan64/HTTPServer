using System;
using NAudio.CoreAudioApi;

namespace VolumeController
{
    public class VolumeManager
    {
        public class VolumeSettings
        {
            public int? MaxVolume { get; set; }
            public int? Volume { get; set; }
            public bool? VolumeLimitEnabled { get; set; }
            public bool? Mute { get; set; }
        }

        private readonly MMDeviceEnumerator _mmde = new MMDeviceEnumerator();
        private readonly MMDevice _mmDevice;
        private bool _suppressSetVolume;

        private int _maxVolume = 100;
        public int MaxVolume
        {
            get => _maxVolume;
            set
            {
                _maxVolume = value;
                SetVolume();
            }
        }

        private bool _volumeLimitEnabled;
        public bool VolumeLimitEnabled
        {
            get => _volumeLimitEnabled;
            set
            {
                _volumeLimitEnabled = value;
                if (VolumeLimitEnabled)
                {
                    SetVolume();
                }
            }
        }

        public int Volume
        {
            get => Convert.ToInt16(_mmDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
            set
            {
                if (Volume != value)
                {
                    _mmDevice.AudioEndpointVolume.MasterVolumeLevelScalar = value / 100f;
                }
            } 
        }

        public bool Mute
        {
            get => _mmDevice.AudioEndpointVolume.Mute;
            set => _mmDevice.AudioEndpointVolume.Mute = value;
        }
        public VolumeManager()
        {
            _mmDevice = _mmde.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            _mmDevice.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
        }

        public void SetVolume(VolumeSettings settings)
        {
            _suppressSetVolume = true;
            MaxVolume = settings.MaxVolume ?? MaxVolume;
            VolumeLimitEnabled = settings.VolumeLimitEnabled ?? VolumeLimitEnabled;
            Mute = settings.Mute ?? Mute;
            _suppressSetVolume = false;
            if (!Mute)
            {
                SetVolume(settings.Volume ?? Volume);
            }
        }

        public void SetVolume()
        {
            SetVolume((int)Math.Round(_mmDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100f, MidpointRounding.AwayFromZero));
        }

        public void SetVolume(int newVolume)
        {
            if (_suppressSetVolume)
            {
                return;
            }

            if (VolumeLimitEnabled
                && newVolume > MaxVolume)
            {
                Volume = MaxVolume;
            }
            else
            {
                Volume = newVolume;
            }
        }

        private void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data)
        {
            SetVolume();
        }

        public VolumeSettings GetVolumeSettings()
        {
            return new VolumeSettings()
            {
                MaxVolume = MaxVolume,
                Volume = Volume,
                VolumeLimitEnabled = VolumeLimitEnabled,
                Mute = Mute
            };
        }
    }
}
