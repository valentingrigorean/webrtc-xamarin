using Foundation;
using WebRTC.iOS.Extensions;
using WebRTC.Abstraction;

namespace WebRTC.iOS
{
    internal class NativeFactory : INativeFactory
    {
        public IPeerConnectionFactory CreatePeerConnectionFactory() => new PeerConnectionFactoryNative();

        public Abstraction.RTCCertificate GenerateCertificate(EncryptionKeyType keyType, long expires)
        {
            return WebRTC.iOS.Binding.RTCCertificate.GenerateCertificateWithParams(new NSDictionary<NSString, NSObject>(
                new[] {"expires".ToNative(), "name".ToNative()},
                new NSObject[] {new NSNumber(expires), keyType.ToStringNative()}
            )).ToNet();
        }
    }
}