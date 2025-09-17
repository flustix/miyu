namespace Miyu.UI.Android;

[Activity(ConfigurationChanges = DEFAULT_CONFIG_CHANGES, Exported = true, LaunchMode = DEFAULT_LAUNCH_MODE, MainLauncher = true)]
internal class MainActivity : AndroidGameActivity
{
    protected override Game CreateGame()
    {
        return new AndroidApp(Window?.Context?.Resources?.DisplayMetrics?.Density ?? 0);
    }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        if (Window is null)
            return;

        Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
        Window.AddFlags(WindowManagerFlags.KeepScreenOn | WindowManagerFlags.DrawsSystemBarBackgrounds);
    }
}
