using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using WebRTC.Abstraction;
using WebRTC.iOS.Binding;

namespace WebRTC.iOS
{
    public class VideoRendererProxy : NSObject, IRTCVideoRenderer, IVideoRenderer
    {
        private readonly List<IVideoRendererListener> _videoRendererListeners = new List<IVideoRendererListener>();

        private IRTCVideoRenderer _renderer;

        public object NativeObject => this;

        public IRTCVideoRenderer Renderer
        {
            get => _renderer;
            set
            {
                if (Equals(_renderer, this))
                    throw new InvalidOperationException("You can set renderer to self");
                _renderer = value;
            }
        }

        public Action OnFirstFrame { get; set; }

        public void RenderFrame(RTCVideoFrame frame)
        {
            Renderer?.RenderFrame(frame);
            OnFirstFrame?.Invoke();
            OnFirstFrame = null;

            var videoRendererListeners = _videoRendererListeners.ToArray();
            var frameNative = new VideoFrameNative(frame);
            foreach (var rendererListener in videoRendererListeners)
            {
                rendererListener.RenderFrame(frameNative);
            }

            frame.Dispose();
        }

        public void SetSize(CGSize size)
        {
            Renderer?.SetSize(size);
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