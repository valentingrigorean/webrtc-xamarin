using System;
using Org.Webrtc;
using WebRTC.Abstraction;
using SessionDescription = Org.Webrtc.SessionDescription;

namespace WebRTC.Droid.Extensions
{
    public static class EnumExtensions
    {
        public static PeerConnection.IceTransportsType ToNative(this IceTransportPolicy self)
        {
            switch (self)
            {
                case IceTransportPolicy.None:
                   return PeerConnection.IceTransportsType.None;
                case IceTransportPolicy.Relay:
                    return PeerConnection.IceTransportsType.Relay;
                case IceTransportPolicy.NoHost:
                    return PeerConnection.IceTransportsType.Nohost;
                case IceTransportPolicy.All:
                    return PeerConnection.IceTransportsType.All;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static PeerConnection.BundlePolicy ToNative(this BundlePolicy self)
        {
            switch (self)
            {
                case BundlePolicy.Balanced:
                     return PeerConnection.BundlePolicy.Balanced;
                case BundlePolicy.MaxCompat:
                    return PeerConnection.BundlePolicy.Maxcompat;
                case BundlePolicy.MaxBundle:
                    return PeerConnection.BundlePolicy.Maxbundle;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static PeerConnection.RtcpMuxPolicy ToNative(this RtcpMuxPolicy self)
        {
            switch (self)
            {
                case RtcpMuxPolicy.Negotiate:
                    return PeerConnection.RtcpMuxPolicy.Negotiate;
                case RtcpMuxPolicy.Require:
                    return PeerConnection.RtcpMuxPolicy.Require;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static PeerConnection.TcpCandidatePolicy ToNative(this TcpCandidatePolicy self)
        {
            switch (self)
            {
                case TcpCandidatePolicy.Enabled:
                     return PeerConnection.TcpCandidatePolicy.Enabled;
                case TcpCandidatePolicy.Disabled:
                    return PeerConnection.TcpCandidatePolicy.Disabled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static PeerConnection.CandidateNetworkPolicy ToNative(this CandidateNetworkPolicy self)
        {
            switch (self)
            {
                case CandidateNetworkPolicy.All:
                    return PeerConnection.CandidateNetworkPolicy.All;
                case CandidateNetworkPolicy.LowCost:
                    return PeerConnection.CandidateNetworkPolicy.LowCost;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static PeerConnection.ContinualGatheringPolicy ToNative(this ContinualGatheringPolicy self)
        {
            switch (self)
            {
                case ContinualGatheringPolicy.Once:
                    return PeerConnection.ContinualGatheringPolicy.GatherOnce;
                case ContinualGatheringPolicy.Continually:
                    return PeerConnection.ContinualGatheringPolicy.GatherContinually;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static PeerConnection.KeyType ToNative(this EncryptionKeyType self)
        {
            switch (self)
            {
                case EncryptionKeyType.Rsa:
                    return PeerConnection.KeyType.Rsa;
                case EncryptionKeyType.Ecdsa:
                    return PeerConnection.KeyType.Ecdsa;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static PeerConnection.SdpSemantics ToNative(this SdpSemantics self)
        {
            switch (self)
            {
                case SdpSemantics.PlanB:
                    return PeerConnection.SdpSemantics.PlanB;
                case SdpSemantics.UnifiedPlan:
                    return PeerConnection.SdpSemantics.UnifiedPlan;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static PeerConnection.TlsCertPolicy ToNative(this TlsCertPolicy self)
        {
            switch (self)
            {
                case TlsCertPolicy.Secure:
                    return PeerConnection.TlsCertPolicy.TlsCertPolicySecure;
                case TlsCertPolicy.InsecureNoCheck:
                    return PeerConnection.TlsCertPolicy.TlsCertPolicyInsecureNoCheck;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }
        
        public static SessionDescription.Type ToNative(this SdpType self)
        {
            switch (self)
            {
                case SdpType.Answer:
                    return SessionDescription.Type.Answer;
                case SdpType.Offer:
                    return SessionDescription.Type.Offer;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }
        
        public static SdpType ToNet(this SessionDescription.Type self)
        {
            if (self == SessionDescription.Type.Answer)
                return SdpType.Answer;
            if (self == SessionDescription.Type.Offer)
                return SdpType.Offer;
            throw new ArgumentOutOfRangeException(nameof(self),self,null);
        }

        public static SourceState ToNet(this MediaStreamTrack.State self)
        {
            if (self == MediaStreamTrack.State.Live)
                return SourceState.Live;
            if (self == MediaStreamTrack.State.Ended)
                return SourceState.Ended;
            throw new ArgumentOutOfRangeException(nameof(self),self,null);
        }

        public static IceConnectionState ToNet(this PeerConnection.IceConnectionState self)
        {
            if (self == PeerConnection.IceConnectionState.Checking)
                return IceConnectionState.Checking;
            if (self == PeerConnection.IceConnectionState.Closed)
                return IceConnectionState.Closed;
            if (self == PeerConnection.IceConnectionState.Completed)
                return IceConnectionState.Completed;
            if (self == PeerConnection.IceConnectionState.Connected)
                return IceConnectionState.Connected;
            if (self == PeerConnection.IceConnectionState.Disconnected)
                return IceConnectionState.Disconnected;
            if (self == PeerConnection.IceConnectionState.Failed)
                return IceConnectionState.Failed;
            if (self == PeerConnection.IceConnectionState.New)
                return IceConnectionState.New;
            throw new ArgumentOutOfRangeException(nameof(self),self,null);
        }

        public static IceGatheringState ToNet(this PeerConnection.IceGatheringState self)
        {
            if (self == PeerConnection.IceGatheringState.Complete)
                return IceGatheringState.Complete;
            if (self == PeerConnection.IceGatheringState.Gathering)
                return IceGatheringState.Gathering;
            if (self == PeerConnection.IceGatheringState.New)
                return IceGatheringState.New;
            throw new ArgumentOutOfRangeException(nameof(self),self,null);
        }

        public static SignalingState ToNet(this PeerConnection.SignalingState self)
        {
            if (self == PeerConnection.SignalingState.Closed)
                return SignalingState.Closed;
            if (self == PeerConnection.SignalingState.Stable)
                return SignalingState.Stable;
            if (self == PeerConnection.SignalingState.HaveLocalOffer)
                return SignalingState.HaveLocalOffer;
            if (self == PeerConnection.SignalingState.HaveLocalPranswer)
                return SignalingState.HaveLocalPrAnswer;
            if (self == PeerConnection.SignalingState.HaveRemoteOffer)
                return SignalingState.HaveRemoteOffer;
            if (self == PeerConnection.SignalingState.HaveRemotePranswer)
                return SignalingState.HaveRemotePrAnswer;
            throw new ArgumentOutOfRangeException(nameof(self),self,null);
        }

        public static DataChannelState ToNet(this DataChannel.State self)
        {
            if (self == DataChannel.State.Closed)
                return DataChannelState.Closed;
            if (self == DataChannel.State.Closing)
                return DataChannelState.Closing;
            if (self == DataChannel.State.Connecting)
                return DataChannelState.Connecting;
            if (self == DataChannel.State.Open)
                return DataChannelState.Open;
            throw new ArgumentOutOfRangeException(nameof(self),self,null);
        }
    }
}