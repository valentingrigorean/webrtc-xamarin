using WebRTC.Abstraction;
using WebRTC.iOS.Extensions;
using WebRTC.iOS.Binding;

namespace WebRTC.iOS
{
    internal abstract class MediaStreamTrackNative : NativeObjectBase, IMediaStreamTrack
    {
        private readonly RTCMediaStreamTrack _track;

        protected MediaStreamTrackNative(RTCMediaStreamTrack track) : base(track)
        {
            _track = track;
        }

        public string Kind => _track.Kind;
        public string Label => _track.TrackId;

        public bool IsEnabled
        {
            get => _track.IsEnabled;
            set => _track.IsEnabled = value;
        }

        public MediaStreamTrackState State => _track.ReadyState.ToNet();
    }
}