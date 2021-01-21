using Org.Webrtc;
using WebRTC.Abstraction;

namespace WebRTC.Droid
{
    internal class VideoFrameNative : NativeObjectBase,IVideoFrame
    {
        public VideoFrameNative(VideoFrame videoFrame) : base(videoFrame)
        {
            
        }
    }
}