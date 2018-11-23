using Android.App;

[assembly: UsesFeature("android.hardware.camera", Required = false)]

[assembly: UsesPermission(Android.Manifest.Permission.Internet)]
[assembly: UsesPermission(Android.Manifest.Permission.Vibrate)]