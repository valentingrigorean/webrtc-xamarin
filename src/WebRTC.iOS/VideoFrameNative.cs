using WebRTC.Abstraction;
using WebRTC.iOS.Binding;

namespace WebRTC.iOS
{
    internal class VideoFrameNative : NativeObjectBase, IVideoFrame
    {
        public VideoFrameNative(RTCVideoFrame videoFrame) : base(videoFrame)
        {
            
        }
    }
}