using System;
using Android.OS;
using Java.Lang;
using Square.OkHttp3;
using WebRTC.AppRTC;
using Exception = System.Exception;

namespace WebRTC.Droid.Demo
{
    public class AppRTCFactory : IAppRTCFactory
    {
        public IWebSocketConnection CreateWebSocketConnection()=> new WebSocketConnection();

        public IScheduler CreateDefaultScheduler() => new Scheduler();

        public ILogger CreateLogger() => new ConsoleLogger();

        private class Scheduler : IScheduler
        {
            private readonly Handler _handler = new Handler(Looper.MainLooper);

            public void Schedule(Action action)
            {
                _handler.Post(action);
            }
        }
        

        private class WebSocketConnection : IWebSocketConnection
        {
            private readonly WebSocketListenerEx _listener;
            private IWebSocket _webSocket;
            
            public WebSocketConnection()
            {
                _listener = new WebSocketListenerEx(this);
            }
            

            public event EventHandler OnOpened;
            public event EventHandler OnClosed;
            public event EventHandler<Exception> OnError;
            public event EventHandler<string> OnMessage;
            public bool IsOpen => _listener.IsOpen;
            
            public void Dispose()
            {
                _webSocket.Dispose();
                _listener.Dispose();
            }
            public void Open(string url, string protocol = null, string authToken = null)
            {
                var request = new Request.Builder()
                    .Url($"{{url}}?{protocol}")
                    .Build();

                var client = new OkHttpClient();
               
                _webSocket = client.NewWebSocket(request, _listener);
            }

            public void Close()
            {
                _webSocket.Close(1000,null);
            }

            public void Send(string message)
            {
                _webSocket.Send(message);
            }
            
            private void SendOnOpened()
            {
                OnOpened?.Invoke(this,EventArgs.Empty);
            }

            private void SendOnClose()
            {
                OnClosed?.Invoke(this,EventArgs.Empty);
            }

            private void SendOnError(Exception error)
            {
                OnError?.Invoke(this,error);
            }

            private void SendOnMessage(string message)
            {
                OnMessage?.Invoke(this,message);
            }

            private class WebSocketListenerEx : WebSocketListener
            {
                private readonly WebSocketConnection _webSocketConnection;
                
                public bool IsOpen { get; private set; }

                public WebSocketListenerEx(WebSocketConnection webSocketConnection)
                {
                    _webSocketConnection = webSocketConnection;
                }

                public override void OnOpen(IWebSocket webSocket, Response response)
                {
                    base.OnOpen(webSocket, response);
                    _webSocketConnection.SendOnOpened();
                    IsOpen = true;
                }

                public override void OnClosing(IWebSocket webSocket, int code, string reason)
                {
                    base.OnClosing(webSocket, code, reason);
                    IsOpen = false;
                }

                public override void OnClosed(IWebSocket webSocket, int code, string reason)
                {
                    base.OnClosed(webSocket, code, reason);
                    _webSocketConnection.SendOnClose();
                }

                public override void OnFailure(IWebSocket webSocket, Throwable t, Response response)
                {
                    base.OnFailure(webSocket, t, response);
                    _webSocketConnection.SendOnError(new Exception(t.Message));
                    IsOpen = false;
                }

                public override void OnMessage(IWebSocket webSocket, string text)
                {
                    base.OnMessage(webSocket, text);
                    _webSocketConnection.SendOnMessage(text);
                }
            }
        }
    }
}