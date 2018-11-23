# Fastfill & Netverify SDK for iOS
Netverify SDK offers scanning and authentication of government issued IDs.

## Table of Content

- [Initialization](#initialization)
- [Configuration](#configuration)
- [Delegation](#delegation)
- [Callback](#callback)

## Initialization
Log into the Jumio Customer Portal. You will find your API token and API secret on the "Settings" page under "API credentials". We strongly recommend that you store your credentials outside your app. If the token and secret are not set in the `NetverifyConfiguration` object, an exception will be thrown. Please note that in Swift you need to catch the underlying exception and translate it into a `NSError` instance.
Whenever an exception is thrown, the `NetverifyViewController` instance will be null and the SDK is not usable. Make sure that all necessary configuration is set before the `NetverifyConfiguration` instance is passed to the initializer.

```
NetverifyConfiguration config = new NetverifyConfiguration();
config.MerchantApiToken = @"YOURAPITOKEN";
config.MerchantApiSecret = @"YOURAPISECRET";
config.DataCenter = JumioDataCenter.Eu; // Change this parameter if your account is in the EU data center. Default is US.
config.Delegate = this;

NetverifyViewController netverifyViewController;
try {
	netverifyViewController = new NetverifyViewController(config);
} catch (Exception exception) {
	// HANDLE EXCEPTION
}
```

It is possible to update parameters of the configuration when a scan is finished with an error and the user is required to perform another scan. This can only be used if the SDK is currently not presented.
```
netverifyViewController UpdateConfiguration(config);
```

Make sure initialization and presentation are timely within one minute. On iPads, the presentation style `UIModalPresentationFormSheet` is default and mandatory.
```
PresentViewController(netverifyViewController, true, null);
```

## Configuration

### ID verification
By default the Jumio SDK enables Netverify which performs a full ID verification in the default mode.

Use ID verification to receive a verification status and verified data positions. Ensure that your customer account is allowed to use this feature. A callback URL can be specified for individual transactions. This setting overrides any callback URL you have set in the Jumio Customer Portal.

__Note:__ Not available for accounts configured as Fastfill only.
```
config.CallbackUrl = "YOURCALLBACKURL";
```
Set the following setting to switch to Fastfill mode (which performs data extraction only):
```
config.RequireVerification = false;
```

Identity Verification is automatically enabled if it is activated for your account. Make sure to link the UMoove framework to your app project.

__Note:__ Identity Verification requires portrait orientation in your app.

Set the following setting to disable Identity Verification on a transaction level:
```
config.RequireFaceMatch = false;
```

### Preselection
You can specify issuing country ([ISO 3166-1 alpha-3](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-3) country code), ID type, and/or document variant. When all three parameters are preselected, the document selection screen in the SDK can be skipped entirely.

__Note:__ Fastfill does not support paper IDs, except German ID cards.
```
config.PreselectedCountry = "AUT";
config.PreselectedDocumentTypes = NetverifyDocumentType.Passport | NetverifyDocumentType.Visa;
config.PreselectedDocumentVariant = NetverifyDocumentVariant.Plastic;
```

### Transaction identifiers
The merchant scan reference allows you to specify your own unique identifier for the scan (max. 100 characters).

__Note:__ Must not contain sensitive data like PII (Personally Identifiable Information) or account
login.
```
config.MerchantScanReference = "YOURSCANREFERENCE";
```
Use the following property to identify the scan in your reports (max. 100 characters).
```
config.MerchantReportingCriteria = "YOURREPORTINGCRITERIA";
```
You can also set a unique identifier for each of your customers (max. 100 characters).

__Note:__ Must not contain sensitive data like PII (Personally Identifiable Information) or account
login.
```
config.CustomerId = "CUSTOMERID";
```

### Miscellaneous

When using Fastfill (requireVerification=false), you can limit data extraction to be done on the device only by enabling `DataExtractionOnMobileOnly`.
```
config.DataExtractionOnMobileOnly = true;
```

Use `CameraPosition` to set the default camera (front or back).
```
config.CameraPosition = JumioCameraPosition.Front;
```

The style of the status bar can be specified.
```
config.StatusBarStyle = UIStatusBarStyle.LightContent;
```

## Delegation
Implement the delegate methods of the `NetverifyViewControllerDelegate` protocol to be notified of successful initialization, successful scans, and error situations. Dismiss the `NetverifyViewController` instance in your app in case of success or error.

### Initialization
When this method is fired, the SDK has finished initialization and loading tasks, and is ready to use. The error object is only set when an error has occurred (e.g. wrong credentials are set or a network error occurred).
```
public override void DidFinishInitializingWithError(NetverifyViewController netverifyViewController, NetverifyError error)
{
	if (error != null) {
		var errorCode = error.Code;
		var errorMessage = error.Message
	}
}
```

### Success
Upon success, the extracted document data is returned, including its scan reference. Call clear on the document data object after processing the card information to make sure no sensitive data remains in the device's memory.
```
public override void DidFinishWithDocumentData(NetverifyViewController netverifyViewController, NetverifyDocumentData documentData, string scanReference)
{
	// YOURCODE
}
```

### Error
This method is fired when the user presses the cancel button during the workflow or in an error situation. The parameter `error` contains an error code and a message. The corresponding scan reference is also available.
```
public override void DidCancelWithError(NetverifyViewController netverifyViewController, NetverifyError error, string scanReference)
{
	var errorCode = error.Code;
	var errorMessage = error.Message
}
```

### Retrieving information
The data that is returned in the callback is explained in the sub-chapter of our native [Netverify SDK documentation](https://github.com/Jumio/mobile-sdk-ios/blob/master/docs/integration_netverify-fastfill.md#retrieving-information)

#### Clean up
After the SDK was dismissed and especially if you want to create a new instance of NetverifyUIController make sure to call `destroy` to ensure proper cleanup of the SDK.
```
netverifyViewController.Destroy();
netverifyViewController = null;
```

**Important:** only call `Destroy` after `DidFinishWithDocumentData(NetverifyViewController netverifyViewController, NetverifyDocumentData documentData, string scanReference)` or `DidCancelWithError(NetverifyViewController netverifyViewController, NetverifyError error, string scanReference)` was called to ensure that Netverify SDK is in a final state. Call `cancel` during the workflow, which will evoke `DidCancelWithError(NetverifyViewController netverifyViewController, NetverifyError error, string scanReference)`. Setting `NetverifyUIController` to null is essential to free memory as soon as possible.
