using System;
using System.Collections.Generic;
using WebRTC.Abstraction;
using Org.Webrtc;

namespace WebRTC.Droid
{
    public class VideoRendererProxy : Java.Lang.Object, IVideoSink, IVideoRenderer
    {
        private readonly List<IVideoRendererListener> _videoRendererListeners = new List<IVideoRendererListener>();

        private IVideoSink _renderer;
        public object NativeObject => this;

        public IVideoSink Renderer
        {
            get => _renderer;
            set
            {
                if (_renderer == this)
                    throw new InvalidOperationException("You can set renderer to self");
                _renderer = value;
            }
        }

        public virtual void OnFrame(VideoFrame p0)
        {
            Renderer?.OnFrame(p0);

            var videoRendererListeners = _videoRendererListeners.ToArray();
            var frame = new VideoFrameNative(p0);
            foreach (var rendererListener in videoRendererListeners)
            {
                rendererListener.RenderFrame(frame);
            }

            p0?.Dispose();
        }

        public void AddVideoRendererListener(IVideoRendererListener videoRendererListener)
        {
            if (!_videoRendererListeners.Contains(videoRendererListener))
                _videoRendererListeners.Add(videoRendererListener);
        }

        public void RemoveVideoRendererListener(IVideoRendererListener videoRendererListener)
        {
            _videoRendererListeners.Remove(videoRendererListener);
        }
    }
}