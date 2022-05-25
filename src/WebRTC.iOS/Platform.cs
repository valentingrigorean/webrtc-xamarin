using System.Collections.Generic;
using System.Linq;
using WebRTC.iOS.Binding;


namespace WebRTC.iOS
{
    public static class Platform
    {
        public static void Init(IDictionary<string, string> trialsFields = null, bool enableInternalTracer = true,
            bool mockSimulatorCamera = true)
        {
            if (trialsFields?.Any() ?? false)
            {
                RTCFieldTrials.InitFieldTrialDictionary(trialsFields);
            }

            if (enableInternalTracer)
            {
                RTCTracing.RTCStartInternalCapture("log.txt");
            }

            RTCSSLAdapter.RTCInitializeSSL();
            Abstraction.NativeFactory.Init(new NativeFactory(mockSimulatorCamera));
        }

        public static void Cleanup()
        {
            RTCTracing.RTCShutdownInternalTracer();
            RTCSSLAdapter.RTCCleanupSSL();
        }
    }
}