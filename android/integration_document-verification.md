# Document Verification SDK for Android
Document Verification is a powerful solution to enable scanning various types (Utility Bill, Bank statement and many others) of your customer's documents in your mobile application within seconds, also supporting data extraction on documents like Utility Bills and Bank Statements

## Table of Content

- [Setup](#setup)
- [Integration](#integration)
- [Configuration](#configuration)
- [SDK Workflow](#sdk-workflow)
- [Javadoc](https://jumio.github.io/mobile-sdk-android/)

## Setup
Using the SDK requires an activity declaration in your AndroidManifest.xml.

```
<activity
  android:name="com.jumio.dv.DocumentVerificationActivity"
	android:theme="@style/Theme.DocumentVerification"
	android:hardwareAccelerated="true"
	android:configChanges="orientation|screenSize|screenLayout|keyboardHidden"/>
```

The orientation can be sensor based or locked with the attribute `android:screenOrientation`.


## Integration

### Permissions
The next permisions need to be added into your AndroidManifest.xml

```
<uses-permission android:name="android.permission.INTERNET" />
<uses-permission android:name="android.permission.VIBRATE" />
<uses-feature android:name="android.hardware.camera" android:required="false" />
```

### Dependencies

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

Applications implementing the SDK shall not run on rooted devices. Use either the below method or a self-devised check to prevent usage of SDK scanning functionality on rooted devices.
```
DocumentVerificationSDK.IsRooted(Context context);
```

Call the method `IsSupportedPlatform` to check if the device is supported.
```
DocumentVerificationSDK.IsSupportedPlatform();
```

Check the Android Studio sample projects to learn the most common use.

## Initialization
To create an instance for the SDK, perform the following call as soon as your activity is initialized.

```
private static String YOURAPITOKEN = "";
private static String YOURAPISECRET = "";

DocumentVerificationSDK documentVerificationSDK = DocumentVerificationSDK.Create(yourActivity, YOURAPITOKEN, YOURAPISECRET, JumioDataCenter.Us);
```
Make sure that your customer API token and API secret are correct, specify an instance
of your activity and provide a reference to identify the scans in your reports (max. 100 characters or `null`). If your customer account is in the EU data center, use `JumioDataCenter.Eu` instead.

__Note:__ Log into your Jumio customer portal, and you can find your customer API token and API secret on the "Settings" page under "API credentials". We strongly recommend you to store credentials outside your app.

## Configuration

### Document type
Use `SetType` to pass the document type.
```
documentVerificationSDK.SetType("DOCUMENTTYPE");
```

Possible types:

*  BS (Bank statement)
*  IC (Insurance card)
*  UB (Utility bill, front side)
*  CAAP (Cash advance application)
*  CRC (Corporate resolution certificate)
*  CCS (Credit card statement)
*  LAG (Lease agreement)
*  LOAP (Loan application)
*  MOAP (Mortgage application)
*  TR (Tax return)
*  VT (Vehicle title)
*  VC (Voided check)
*  STUC (Student card)
*  HCC (Health care card)
*  CB (Council bill)
*  SENC (Seniors card)
*  MEDC (Medicare card)
*  BC (Birth certificate)
*  WWCC (Working with children check)
*  SS (Superannuation statement)
*  TAC (Trade association card)
*  SEL (School enrolment letter)
*  PB (Phone bill)
*  USSS (US social security card)
*  SSC (Social security card)
*  CUSTOM (Custom document type)

#### Custom Document Type
Use the following method to pass your custom document code. Maintain your custom document code within your Jumio customer portal under "Settings" - "Multi Document" - "Custom".
```
documentVerificationSDK.SetCustomDocumentCode("YOURCUSTOMDOCUMENTCODE");
```

### Country
The country needs to be in format [ISO-3166-1 alpha 3](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-3) or XKX for Kosovo.
```
documentVerificationSDK.SetCountry("USA");
```

### Transaction identifiers

Use the following property to identify the scan in your reports (max. 100 characters).
```
documentVerificationSDK.SetMerchantReportingCriteria("YOURREPORTINGCRITERIA");
```

A callback URL can be specified for individual transactions constraints see chapter [Callback URL](#callback-url)). This setting overrides your Jumio customer settings.
```
documentVerificationSDK.SetCallbackUrl("YOURCALLBACKURL");
```

### Data Extraction

When data extraction should be used, set the following parameter that enables or disables extraction for each transaction. It is mandatory to be set to `true` if extraction is activated.

```
documentVerificationSDK.SetEnableExtraction(true);
```

__Note:__ If you want to enable extraction for your account in general, please contact your Account Manager, or reach out to Jumio Support.

### Miscellaneous
Use the following property to identify the scan in your reports (max. 100 characters).
```
documentVerificationSDK.SetMerchantScanReference("YOURSCANREFERENCE");
```

You can also set a customer identifier (max. 100 characters).
```
documentVerificationSDK.SetCustomerId("CUSTOMERID");
```

__Note:__ The customer ID and merchant scan reference must not contain sensitive data like PII (Personally Identifiable Information) or account login.

Use SetCameraPosition to configure the default camera (front or back).
```
documentVerificationSDK.SetCameraPosition(JumioCameraPosition.Front);
```

Use SetDocumentName to override the document label on Help screen.
```
documentVerificationSDK.SetDocumentName(“YOURDOCNAME”);
```

## SDK Workflow

### Starting the SDK

To show the SDK, call the respective method below within your activity or fragment.

Activity: `DocumentVerificationSDK.Start();` <br/>
Fragment: `StartActivityForResult(documentVerificationSDK.Intent,DocumentVerificationSDK.RequestCode);`

__Note:__ The default request code is 300. To use another code, override the public static variable `DocumentVerificationSDK.RequestCode` before displaying the SDK.

### Retrieving information

Implement the standard `OnActivityResult` method in your activity or fragment for successful scans (`Result.Ok`) and user cancellation notifications (`Result.Canceled`). Call `documentVerificationSDK.Destroy()` once you received the result.

```
protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data) {
	if (requestCode == DocumentVerificationSDK.REQUEST_CODE) {
		if (resultCode == Result.RequestCode) {
			// OBTAIN PARAMETERS HERE
			// YOURCODE
		} else if (resultCode == Result.Canceled) {
			var scanReference = data.GetStringExtra(DocumentVerificationSDK.ExtraScanReference);
			var errorMessage = data.GetStringExtra(DocumentVerificationSDK.ExtraErrorMessage);
			var errorCode = data.GetStringExtra(DocumentVerificationSDK.ExtraErrorCode);
			// YOURCODE
		}
        // CLEANUP THE SDK AFTER RECEIVING THE RESULT
         if (documentVerificationSDK != null) {
           documentVerificationSDK.Destroy();
           documentVerificationSDK = null;
         }
	}
}
```

#### Error codes

|Code        			| Message   | Description    |
| :--------------:|:----------|:---------------|
|A10000| We have encountered a network communication problem | Retry possible, user decided to cancel |
|B10000| Authentication failed | Secure connection could not be established, retry impossible |
|C10401| Authentication failed | API credentials invalid, retry impossible |
|D10403| Authentication failed | Wrong API credentials used, retry impossible|
|E20000| No Internet connection available | Retry possible, user decided to cancel |
|F00000| Scanning not available this time, please contact the app vendor | Resources cannot be loaded, retry impossible |
|G00000| Cancelled by end-user | No error occurred |
|H00000| The camera is currently not available | Camera cannot be initialized, retry impossible |
|I00000| Certificate not valid anymore. Please update your application | End-to-end encryption key not valid anymore, retry impossible |
|K10400| Unsupported document code defined. Please contact Jumio support | An unsupported document code has been set, retry impossible |
