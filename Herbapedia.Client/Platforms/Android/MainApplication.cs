using Android.App;
using Android.Runtime;

namespace Herbapedia.Client
{
    [Application(UsesCleartextTraffic = true, AllowBackup = false, Debuggable = true)]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
