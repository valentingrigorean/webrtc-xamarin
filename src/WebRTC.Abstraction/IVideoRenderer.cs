namespace WebRTC.Abstraction
{
    public interface IVideoRendererListener
    {
        void RenderFrame(IVideoFrame videoFrame);
    }
    
    public interface IVideoRenderer : INativeObject
    {
        void AddVideoRendererListener(IVideoRendererListener videoRendererListener);
        void RemoveVideoRendererListener(IVideoRendererListener videoRendererListener);
    }
}