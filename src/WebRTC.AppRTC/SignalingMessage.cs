using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using WebRTC.Abstraction;

namespace WebRTC.AppRTC
{
    public enum SignalingMessageType
    {
        Unknown,

        [EnumMember(Value = "new-app-connection")]
        Register,

        [EnumMember(Value = "app-start-video")]
        Registered,
        [EnumMember(Value = "amk-send-offer")] Offer,
        [EnumMember(Value = "receive-answer")] ReceivedAnswer,

        [EnumMember(Value = "receive-candidate")]
        ReceiveCandidate,

        [EnumMember(Value = "amk-send-candidate")]
        SendCandidate,
    }

    public class SignalingMessage
    {
        public const string Register = "new-app-connection";
        public const string Registered = "app-start-video";
        public const string SendOffer = "amk-send-offer";
        public const string ReceivedAnswer = "receive-answer";
        public const string ReceiveCandidate = "receive-candidate";
        public const string SendCandidate = "amk-send-candidate";
        
        [JsonProperty("type")] public SignalingMessageType MessageType { get; set; }

        [JsonProperty("amkSocketId")] public string SocketId { get; set; }

        public static SignalingMessage FromJson(string json)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (values.ContainsKey("type"))
            {
                switch (values["type"].ToString())
                {
                    case Registered:
                        return JsonConvert.DeserializeObject<RegisteredMessage>(json);
                    case SendOffer:
                        break;
                    case ReceivedAnswer:
                        return GetAnswerSessionDescription(values["answer"].ToString());
                    case ReceiveCandidate:
                        return JsonConvert.DeserializeObject<IceCandidateMessage>(json);
                }
            }

            return null;
        }

        private static SessionDescriptionMessage GetAnswerSessionDescription(string json)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string,string>>(json);

            return new SessionDescriptionMessage
            {
                MessageType = SignalingMessageType.ReceivedAnswer,
                Description = new SessionDescription(SdpType.Answer, values["sdp"])
            };
        }
    }

    public class RegisterMessage : SignalingMessage
    {
        public RegisterMessage(string phoneNumber, double longitude, double latitude)
        {
            PhoneNumber = phoneNumber;
            Longitude = longitude;
            Latitude = latitude;
            MessageType = SignalingMessageType.Register;
        }


        [JsonProperty("phoneNumber")] public string PhoneNumber { get; }

        [JsonProperty("longitude")] public double Longitude { get; }

        [JsonProperty("latitude")] public double Latitude { get; }
    }

    public class RegisteredMessage : SignalingMessage
    {
        public RegisteredMessage()
        {
            MessageType = SignalingMessageType.Registered;
        }

        [JsonProperty("rtcServer")] public RTCServers RTCServer { get; set; }

        [JsonProperty("stun")] public string StunServer { get; set; }

        public class RTCServers
        {
            [JsonProperty("turn1")] public RTCServerEx Turn1 { get; set; }

            [JsonProperty("turn2")] public RTCServerEx Turn2 { get; set; }
        }

        public class RTCServerEx
        {
            [JsonProperty("urls")] public string Url { get; set; }

            [JsonProperty("username")] public string Username { get; set; }

            [JsonProperty("credential")] public string Password { get; set; }

            [JsonProperty("credentialType")] public string CredentialType { get; set; }
        }
    }

    public class IceCandidateMessage : SignalingMessage
    {
        public IceCandidateMessage()
        {
            MessageType = SignalingMessageType.ReceiveCandidate;
        }

        public IceCandidateMessage(IceCandidate iceCandidate)
        {
            MessageType = SignalingMessageType.SendCandidate;
            IceCandidate = new IceCandidateEx
            {
                Sdp = iceCandidate.Sdp,
                SdpMid = iceCandidate.SdpMid,
                SdpMLineIndex = iceCandidate.SdpMLineIndex
            };
        }

        [JsonProperty("candidate")] public IceCandidateEx IceCandidate { get; set; }

        public class IceCandidateEx
        {
            [JsonProperty("candidate")]
            public string Sdp { get; set; }
        
            [JsonProperty("sdpMid")]
            public string SdpMid { get; set; }
        
            [JsonProperty("sdpMlineIndex")]
            public int SdpMLineIndex { get; set; }
        }
    }

    public class SessionDescriptionMessage : SignalingMessage
    {
        public SessionDescriptionMessage()
        {
            MessageType = SignalingMessageType.ReceivedAnswer;
        }

        public SessionDescriptionMessage(SessionDescription description)
        {
            MessageType = SignalingMessageType.Offer;
            Description = description;
        }

        [JsonProperty("offer")] 
        public SessionDescription Description { get; set; }
    }
}