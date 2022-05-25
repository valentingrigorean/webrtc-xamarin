using WebRTC.Abstraction;
using WebRTC.iOS.Extensions;
using WebRTC.iOS.Binding;


namespace WebRTC.iOS
{
    internal class RtpTransceiverNative :NativeObjectBase,IRtpTransceiver
    {
        private readonly IRTCRtpTransceiver _rtpTransceiver;
        private IRtpSender _rtpSender;
        private IRtpReceiver _rtpReceiver;

        public RtpTransceiverNative(IRTCRtpTransceiver rtpTransceiver) : base(rtpTransceiver)
        {
            _rtpTransceiver = rtpTransceiver;
        }

        public RtpMediaType MediaType => _rtpTransceiver.MediaType.ToNet();
        public string Mid => _rtpTransceiver.Mid;
        public bool IsStopped => _rtpTransceiver.IsStopped;
        public RtpTransceiverDirection Direction => _rtpTransceiver.Direction.ToNet();

        public IRtpSender Sender
        {
            get
            {
                if (_rtpTransceiver.Sender == null)
                    return null;
                return _rtpSender ?? (_rtpSender = new RtpSenderNative(_rtpTransceiver.Sender));
            }
        }

        public IRtpReceiver Receiver
        {
            get
            {
                if (_rtpTransceiver.Receiver == null)
                    return null;
                return _rtpReceiver ?? (_rtpReceiver = new RtpReceiverNative(_rtpTransceiver.Receiver));
            }
        }
        
        public void Stop()
        {
            _rtpTransceiver.Stop();
        }
    }
}