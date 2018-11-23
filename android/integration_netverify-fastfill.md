# Fastfill & Netverify SDK for Android
Netverify & Fastfill SDK offers scanning and authentication of governmental issued IDs.

## Table of Content

- [Setup](#setup)
- [Dependencies](#dependencies)
- [Initialization](#integration)
- [Configuration](#configuration)

## Setup

Using the SDK requires an activity declaration in your `AndroidManifest.xml`.

```
<activity
	android:theme="@style/Theme.Netverify"
	android:hardwareAccelerated="true"
	android:name="com.jumio.nv.NetverifyActivity"
	android:configChanges="orientation|screenSize|screenLayout|keyboardHidden"/>
```

The orientation can be sensor based or locked with the attribute `android:screenOrientation`.

## Dependencies

|Dependency| Version        | Mandatory           | Description
| ---------------------------- |:-------------:|:-----------------|:-----------------|
|Xamarin.Android.Arch.Core.Common|	1.0.0.1	| x | Android native library|
|Xamarin.Android.Arch.Lifecycle.Common|	1.0.3.1	| x | Android native library|
|Xamarin.Android.Arch.Lifecycle.Runtime|	1.0.3.1	| x | Android native library|
|Xamarin.Android.Support.Animated.Vector.Drawable|	27.0.2.1	| x | Android native library|
|Xamarin.Android.Support.Annotations|	27.0.2.1	| x | Android native library|
|Xamarin.Android.Support.Compat|	27.0.2.1	| x | Android native library|
|Xamarin.Android.Support.Core.UI|	27.0.2.1	| x | Android native library|
|Xamarin.Android.Support.Core.Utils|	27.0.2.1	| x | Android native library|
|Xamarin.Android.Support.Design|	27.0.2.1	| x | Android native library|
|Xamarin.Android.Support.Fragment|	27.0.2.1	| x | Android native library|
|Xamarin.Android.Support.Media.Compat|	27.0.2.1	| x | Android native library|
|Xamarin.Android.Support.Transition|	27.0.2.1	| x | Android native library|
|Xamarin.Android.Support.v4|	27.0.2.1	| x | Android native library|
|Xamarin.Android.Support.v7.AppCompat|	27.0.2.1	| x | Android native library|
|Xamarin.Android.Support.v7.CardView|	27.0.2.1	| x | Android native library|
|Xamarin.Android.Support.v7.MediaRouter|	27.0.2.1	| x | Android native library|
|Xamarin.Android.Support.v7.Palette|	27.0.2.1	| x | Android native library|
|Xamarin.Android.Support.v7.RecyclerView|	27.0.2.1	| x | Android native library|
|Xamarin.Android.Support.Vector.Drawable|	27.0.2.1	| x | Android native library|
|Xamarin.Build.Download|	0.4.9	| x | Android native library|
|Xamarin.GooglePlayServices.Base|	60.1142.1	| x | Android native library|
|Xamarin.GooglePlayServices.Basement|	60.1142.1	| x | Android native library|
|Xamarin.GooglePlayServices.Tasks|	60.1142.1	| x | Android native library|
|Xamarin.GooglePlayServices.Vision|	60.1142.1	| x | Android native library|
|Xamarin.GooglePlayServices.Vision.Common|	60.1142.1	| x | Android native library|

### Others

#### Root detection
Applications implementing the SDK shall not run on rooted devices. Use either the below method or a self-devised check to prevent usage of SDK scanning functionality on rooted devices.
```
NetverifySDK.IsRooted(Context context);
```

#### Device supported check
Call the method `IsSupportedPlatform` to check if the device platform is supported by the SDK.

```
NetverifySDK.IsSupportedPlatform();
```

## Initialization
To create an instance for the SDK, perform the following call as soon as your activity is initialized.

```
private static String YOURAPITOKEN = "";
private static String YOURAPISECRET = "";

NetverifySDK netverifySDK = NetverifySDK.Create(yourActivity, YOURAPITOKEN, YOURAPISECRET, JumioDataCenter.Us);
```
Make sure that your customer API token and API secret are correct, specify an instance
of your activity and provide a reference to identify the scans in your reports (max. 100 characters or `null`). If your customer account is in the EU data center, use `JumioDataCenter.Eu` instead.

__Note:__ Log into your Jumio customer portal, and you can find your customer API token and API secret on the "Settings" page under "API credentials". We strongly recommend you to store credentials outside your app.

## Configuration

### ID verification

By default, the SDK is used in Fastfill mode which means it is limited to data extraction only. No verification of the ID is performed.

Enable ID verification to receive a verification status and verified data positions. A callback URL can be specified for individual transactions (constraints see chapter. Ensure that your customer account is allowed to use this feature.

__Note:__ Not possible for accounts configured as Fastfill only.
```
netverifySDK.SetRequireVerification(true);
netverifySDK.SetCallbackUrl("YOURCALLBACKURL");
```
You can enable Identity Verification during the ID verification for a specific transaction (if it is enabled for your account).
```
netverifySDK.SetRequireFaceMatch(true);
```

### Preselection

You can specify issuing country  ([ISO 3166-1 alpha-3](https://en.wikipedia.org/wiki/ISO_3166-1_alpha-3) country code), ID type(s) and/or document variant to skip their selection during the scanning process.<br />
__Note:__ Fastfill does not support paper IDs, except German ID cards.
```
netverifySDK.SetPreselectedCountry("AUT");
netverifySDK.SetPreselectedDocumentVariant(NVDocumentVariant.Plastic);

var documentTypes = new List<NVDocumentType>();
documentTypes.Add(NVDocumentType.Passport);
documentTypes.Add(NVDocumentType.DriverLicense);
netverifySDK.SetPreselectedDocumentTypes(documentTypes);
```

### Transaction identifiers

The merchant scan reference allows you to identify the scan (max. 100 characters).

__Note:__ Must not contain sensitive data like PII (Personally Identifiable Information) or account login.
```
netverifySDK.SetMerchantScanReference("YOURSCANREFERENCE");
```
Use the following property to identify the scan in your reports (max. 100 characters).
```
netverifySDK.SetMerchantReportingCriteria("YOURREPORTINGCRITERIA");
```
You can also set a customer identifier (max. 100 characters).

__Note:__ The customer ID must not contain sensitive data like PII (Personally Identifiable Information) or account login.
```
netverifySDK.SetCustomerId("CUSTOMERID");
```

### eMRTD

Use `SetEnableEMRTD` to read the NFC chip of an eMRTD.
```
netverifySDK.SetEnableEMRTD (true);
```
__Note:__ Not available for Fastfill as it is a Netverify feature.

### Miscellaneous

In case Fastfill is used (requireVerification=NO), data extraction can be limited to be executed on device only by enabling `SetDataExtractionOnMobileOnly`
```
netverifySDK.SetDataExtractionOnMobileOnly(true);
```

Use `SetCameraPosition` to configure the default camera (front or back).
```
netverifySDK.SetCameraPosition(JumioCameraPosition.Front);
```

### Starting the SDK

Use the Initiate method to preload the SDK and avoid the loading spinner after the SDK start.
```
netverifySDK.Initiate(INetverifyInitiateCallback);
```
To show the SDK, call the respective method below within your activity or fragment.

Activity: `netverifySDK.Start();` <br/>
Fragment: `StartActivityForResult(netverifySDK.Intent, NetverifySDK.RequestCode);`

__Note:__ The default request code is 200. To use another code, override the public static variable `NetverifySDK.RequestCode` before displaying the SDK.


### Retrieving information (Fastfill)

Implement the standard `OnActivityResult` method in your activity or fragment for successful scans (`Result.Ok`) and user cancellation notifications (`Result.Canceled`). Call `netverifySDK.Destroy()` once you received the result.

```
protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
{
	if (requestCode == NetverifySDK.RequestCode) {
		if (resultCode == Result.Ok) {
			// OBTAIN PARAMETERS HERE
			// YOURCODE
		} else if (resultCode ==  Result.Canceled) {
			// string scanReference = data.GetStringExtra(NetverifySDK.ExtraScanReference);
			// string errorMessage = data.GetStringExtra(NetverifySDK.ExtraErrorMessage);
			// string errorCode = data.GetStringExtra(NetverifySDK.ExtraErrorCode);
			// YOURCODE
		}
		// CLEANUP THE SDK AFTER RECEIVING THE RESULT
		// if (netverifySDK != null) {
		// 	netverifySDK.Destroy();
		// 	netverifySDK = null;
		// }
	}
}
```
#### Callback data structure
The data that is returned in the callback is explained in the sub-chapter of our native [Netverify SDK documentation](https://github.com/Jumio/mobile-sdk-android/blob/master/docs/integration_netverify-fastfill.md#retrieving-information-fastfill)

#### Clean up
After handling the result, it is very important to clean up the SDK by calling  `netverifyCustomSDKController.Destroy()` and `netverifySDK.Destroy()`.
