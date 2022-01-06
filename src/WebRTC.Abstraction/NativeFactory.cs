using System;

namespace WebRTC.Abstraction
{
    internal static class NativeFactory
    {
        private static INativeFactory _factory = null!;

        public static void Init(INativeFactory factory)
        {
            _factory = factory;
            RTCCameraDevice.SupportedDevices = factory.CameraDevices;
        }

        internal static RTCCertificate GenerateCertificate(EncryptionKeyType keyType, long expires)
        {
            CheckIfInit();
            return _factory.GenerateCertificate(keyType, expires);
        }

        internal static IPeerConnectionFactory CreatePeerConnectionFactory()
        {
            CheckIfInit();
            return _factory.CreatePeerConnectionFactory();
        }

        internal static void StopInternalTracingCapture()
        {
            CheckIfInit();
            _factory.StopInternalTracingCapture();
        }

        internal static void ShutdownInternalTracer()
        {
            CheckIfInit();
            _factory.ShutdownInternalTracer();
        }

        private static void CheckIfInit()
        {
            if (_factory == null)
                throw new Exception("App.Init was not called");
        }
    }
}