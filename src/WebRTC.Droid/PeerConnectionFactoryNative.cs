using Android.Content;
using Android.OS;
using Android.Util;
using Java.IO;
using WebRTC.Abstraction;
using WebRTC.Abstraction.Extensions;
using Org.Webrtc;
using Org.Webrtc.Audio;
using WebRTC.Droid.Extensions;
using ICameraVideoCapturer = WebRTC.Abstraction.ICameraVideoCapturer;
using MediaConstraints = WebRTC.Abstraction.MediaConstraints;
using PeerConnectionFactory = Org.Webrtc.PeerConnectionFactory;

namespace WebRTC.Droid
{
    public interface IPeerConnectionFactoryAndroid : IPeerConnectionFactory
    {
        IEglBaseContext EglBaseContext { get; }
    }

    internal class PeerConnectionFactoryNative : NativeObjectBase, IPeerConnectionFactoryAndroid
    {
        private const string TAG = nameof(PeerConnectionFactoryNative);

        private readonly PeerConnectionFactory _factory;
        private readonly Context _context;

        public PeerConnectionFactoryNative(Context context)
        {
            _context = context;

            EglBase = EglBaseHelper.Create();

            _factory = CreateNativeFactory(context, EglBaseContext);
            NativeObject = _factory;
        }

        public IEglBase EglBase { get; }
        public IEglBaseContext EglBaseContext => EglBase.EglBaseContext;

        public override void Dispose()
        {
            base.Dispose();
            EglBase.Release();
        }

        public IPeerConnection CreatePeerConnection(RTCConfiguration configuration,
            IPeerConnectionListener peerConnectionListener)
        {
            var nativeConfiguration = configuration.ToNative();
            var peerConnection = _factory.CreatePeerConnection(nativeConfiguration,
                new PeerConnectionListenerProxy(peerConnectionListener));
            return peerConnection == null ? null : new PeerConnectionNative(peerConnection, configuration, this);
        }

        public IAudioSource CreateAudioSource(MediaConstraints mediaConstraints)
        {
            var audioSource = _factory.CreateAudioSource(mediaConstraints.ToNative());
            if (audioSource == null)
                return null;
            return new AudioSourceNative(audioSource);
        }

        public IAudioTrack CreateAudioTrack(string id, IAudioSource audioSource)
        {
            var audioTrack = _factory.CreateAudioTrack(id, audioSource.ToNative<AudioSource>());
            if (audioTrack == null)
                return null;
            return new AudioTrackNative(audioTrack);
        }

        public IVideoSource CreateVideoSource(bool isScreencast) =>
            new VideoSourceNative(_factory.CreateVideoSource(isScreencast));

        public IVideoTrack CreateVideoTrack(string id, IVideoSource videoSource)
        {
            var videoTrack = _factory.CreateVideoTrack(id, videoSource.ToNative<VideoSource>());
            if (videoTrack == null)
                return null;
            return new VideoTrackNative(videoTrack);
        }

        public ICameraVideoCapturer CreateCameraCapturer(IVideoSource videoSource, bool frontCamera) =>
            CreateCameraVideoCapturer(videoSource.ToNative<VideoSource>(), frontCamera);

        public ICameraVideoCapturer CreateCameraCapturer(IVideoSource videoSource, RTCCameraDevice cameraDevice)
        {
            var cameraEnumerator = _context.GetAllCameras();
            var videoCapturer = cameraEnumerator.CreateCapturer(cameraDevice.DeviceId, null);
            return videoCapturer == null
                ? null
                : new CameraVideoCapturerNative(videoCapturer, _context, videoSource.ToNative<VideoSource>(),
                    EglBaseContext);
        }

        public IFileVideoCapturer CreateFileCapturer(IVideoSource videoSource, string file)
        {
            var fileVideoCapturer = new FileVideoCapturer(file);
            return new FileVideoCapturerNative(fileVideoCapturer, _context, videoSource.ToNative<VideoSource>(),
                EglBaseContext);
        }

        public bool StartAecDump(string file, int fileSizeLimitBytes)
        {
            try
            {
                var aecDumpFileDescriptor = ParcelFileDescriptor.Open(new File(file),
                    ParcelFileMode.Create | ParcelFileMode.Truncate | ParcelFileMode.ReadWrite);

                return _factory.StartAecDump(aecDumpFileDescriptor.DetachFd(), fileSizeLimitBytes);
            }
            catch (IOException ex)
            {
                return false;
            }
        }

        public void StopAecDump()
        {
            _factory.StopAecDump();
        }


        private ICameraVideoCapturer CreateCameraVideoCapturer(VideoSource videoSource, bool frontCamera)
        {
            var videoCapturer = CreateCameraCapturer(_context.GetAllCameras(), frontCamera);
            return videoCapturer == null
                ? null
                : new CameraVideoCapturerNative(videoCapturer, _context, videoSource, EglBaseContext);
        }

        private Org.Webrtc.ICameraVideoCapturer CreateCameraCapturer(ICameraEnumerator cameraEnumerator,
            bool frontCamera)
        {
            var devicesNames = cameraEnumerator.GetDeviceNames();
            foreach (var devicesName in devicesNames)
            {
                if (cameraEnumerator.IsFrontFacing(devicesName) && frontCamera)
                {
                    var videoCapturer = cameraEnumerator.CreateCapturer(devicesName, null);
                    if (videoCapturer != null)
                        return videoCapturer;
                }
            }

            foreach (var devicesName in devicesNames)
            {
                var videoCapturer = cameraEnumerator.CreateCapturer(devicesName, null);
                if (videoCapturer != null)
                    return videoCapturer;
            }

            return null;
        }


        private static PeerConnectionFactory CreateNativeFactory(Context context, IEglBaseContext eglBaseContext)
        {
            var adm = CreateJavaAudioDevice(context);

            var encoderFactory = new DefaultVideoEncoderFactory(eglBaseContext, true, true);
            var decoderFactory = new DefaultVideoDecoderFactory(eglBaseContext);
            var factory = PeerConnectionFactory.InvokeBuilder()
                .SetAudioDeviceModule(adm)
                .SetVideoEncoderFactory(encoderFactory)
                .SetVideoDecoderFactory(decoderFactory)
                .CreatePeerConnectionFactory();

            adm.Release();

            return factory;
        }

        private static IAudioDeviceModule CreateJavaAudioDevice(Context context)
        {
            var audioErrorCallbacks = new AudioErrorCallbacks();
            return JavaAudioDeviceModule.InvokeBuilder(context)
                .SetAudioRecordErrorCallback(audioErrorCallbacks)
                .SetAudioRecordStateCallback(audioErrorCallbacks)
                .SetAudioTrackErrorCallback(audioErrorCallbacks)
                .SetAudioTrackStateCallback(audioErrorCallbacks)
                .CreateAudioDeviceModule();
        }

        private class AudioErrorCallbacks : Java.Lang.Object,
            JavaAudioDeviceModule.IAudioRecordErrorCallback,
            JavaAudioDeviceModule.IAudioRecordStateCallback,
            JavaAudioDeviceModule.IAudioTrackErrorCallback,
            JavaAudioDeviceModule.IAudioTrackStateCallback

        {
            public void OnWebRtcAudioRecordError(string p0)
            {
                Log.Error(TAG, $"OnWebRtcAudioRecordError: {p0}");
            }

            public void OnWebRtcAudioRecordInitError(string p0)
            {
                Log.Error(TAG, $"OnWebRtcAudioRecordInitError: {p0}");
            }

            public void OnWebRtcAudioRecordStartError(JavaAudioDeviceModule.AudioRecordStartErrorCode p0, string p1)
            {
                Log.Error(TAG, $"OnWebRtcAudioRecordStartError: errorCode {p0} {p1}");
            }

            public void OnWebRtcAudioTrackError(string p0)
            {
                Log.Error(TAG, $"OnWebRtcAudioTrackError: errorCode {p0}");
            }

            public void OnWebRtcAudioTrackInitError(string p0)
            {
                Log.Error(TAG, $"OnWebRtcAudioTrackInitError: errorCode {p0}");
            }

            public void OnWebRtcAudioTrackStartError(JavaAudioDeviceModule.AudioTrackStartErrorCode p0, string p1)
            {
                Log.Error(TAG, $"OnWebRtcAudioTrackStartError: errorCode {p0} {p1}");
            }

            public void OnWebRtcAudioRecordStart()
            {
                Log.Info(TAG, "Audio recording starts");
            }

            public void OnWebRtcAudioRecordStop()
            {
                Log.Info(TAG, "Audio recording stops");
            }

            public void OnWebRtcAudioTrackStart()
            {
                Log.Info(TAG, "Audio playout starts");
            }

            public void OnWebRtcAudioTrackStop()
            {
                Log.Info(TAG, "Audio playout stops");
            }
        }
    }
}