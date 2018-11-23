# Document Verification SDK for iOS
Document Verification is a powerful solution to enable scanning various types (Utility Bill, Bank statement and many others) of your customer's documents in your mobile application within seconds, also supporting data extraction on documents like Utility Bills and Bank Statements (see [Supported documents for data extraction](https://github.com/Jumio/implementation-guides/blob/master/netverify/document-verification.md#supported-documents))

## Table of Content

- [Initialization](#initialization)
- [Configuration](#configuration)
- [Delegation](#delegation)
- [Callback](#callback)

## Initialization
Log into your Jumio Customer Portal and you can find your API token and API secret on the "Settings" page under "API credentials". We strongly recommend to store credentials outside your app. In case the token and secret are not set in the `DocumentVerificationConfiguration` object, an exception will be thrown. Please note that in Swift you need to catch the underlying exception and translate it into a `NSError` instance. Whenever an exception is thrown, the `DocumentVerificationViewController` instance will be null and the SDK is not usable. Make sure that all necessary configuration is set before the `DocumentVerificationConfiguration` instance is passed to the initializer.

```
DocumentVerificationViewController config = new DocumentVerificationConfiguration();
config.MerchantApiToken = "YOURAPITOKEN";
config.MerchantApiSecret = "YOURAPISECRET";
config.DataCenter = JumioDataCenter.Eu; // Change this parameter if your account is in the EU data center. Default is US.
config.Delegate = this;

DocumentVerificationViewController documentVerificationViewController;
try {
	documentVerificationViewController = new DocumentVerificationViewController(config);
} catch (Exception exception) {
	// HANDLE EXCEPTION
}
```

Make sure initialization and presentation are timely within one minute. On iPads, the presentation style _UIModalPresentationFormSheet_ is default and mandatory.

```
PresentViewController(documentVerificationViewController, true, null);
```
## Configuration

### Document type
Set the parameter `Type` in the `DocumentVerificationConfiguration` object to pass the document type.
```
config.Type = "BC";
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
*  SSC (Social security card)
*  CUSTOM (Custom document type)

#### Custom Document Type
Use the following method to pass your custom document code. Maintain your custom document code within your Jumio Customer Portal under "Settings" - "Document Verifications" -
"Custom".
```
config.CustomDocumentCode = "YOURCUSTOMDOCUMENTCODE";
```

### Country

The country needs to be in format [ISO-3166-1 alpha 3](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-3) or XKX for Kosovo.
```
config.Country= "USA";
```

### Transaction identifiers
Specify your reporting criteria to identify each scan attempt in your reports (max. 100 characters).
```
config.MerchantReportingCriteria = "YOURREPORTINGCRITERIA";
```

__Note:__ Must not contain sensitive data like PII (Personally Identifiable Information) or account login.

A callback URL can be specified for individual transactions. This setting overrides your Jumio merchant settings.
```
config.CallbackUrl = "YOURCALLBACKURL";
```

### Data Extraction

Data extraction is automatically enabled when it is activated for your account. Use the following setting to disable the extraction on a transaction level:

```
config.EnableExtraction = false;
```

__Note:__ If you want to activate data extraction for your account in general, please contact your Account Manager, or reach out to Jumio Support.

### Miscellaneous
Use the following property to identify the scan in your reports (max. 100 characters) and set a customer identifier (max. 100 characters).
```
config.MerchantScanReference = "YOURSCANREFERENCE";
config.CustomerId = "CUSTOMERID";
```
__Note:__ Must not contain sensitive data like PII (Personally Identifiable Information) or account login.

Use cameraPosition to configure the default camera (front or back).
```
config.CameraPosition = JumioCameraPositionFront;
```

The style of the status bar can be specified.
```
config.StatusBarStyle = UIStatusBarStyle.LightContent;
```

## Delegation
Implement the delegate methods of the `DocumentVerificationViewControllerDelegate` protocol to be notified of successful initialisation, successful scans and error situations. Dismiss the SDK view in your app in case of success or error.

### Success
Upon success, the scan reference is returned.
```
public override void DidFinishWithScanReference(DocumentVerificationViewController documentVerificationViewController, string scanReference)
{
	// YOURCODE
}
```

### Error
This method is fired when the user presses the cancel button during the workflow or in an error situation. The parameter `error` contains an error code and a message.
```
public override void DidFinishWithError(DocumentVerificationViewController documentVerificationViewController, DocumentVerificationError error)
{
	var errorCode = error.code;
	var errorMessage = error.message;
}
```

**_Error codes_** that are available via the `code` property of the DocumentVerificationError object:

| Code | Message | Description |
| :-------------: |:----------|:-------------|
| A10000 | We have encountered a network communication problem | Retry possible, user decided to cancel |
| B10000 | Authentication failed | Secure connection could not be established, retry impossible |
| C10401 | Authentication failed | API credentials invalid, retry impossible |
| D10403 | Authentication failed | Wrong API credentials used, retry impossible |
| E10000 | No Internet connection available | Retry possible, user decided to cancel |
| G00000 | Cancelled by end-user | No error occurred |
| H00000 | The camera is currently not available | Camera cannot be initialized, retry impossible |
| I00000 | Certificate not valid anymore. Please update your application | End-to-end encryption key not valid anymore, retry impossible |
| K10400 | Unsupported document code defined. Please contact Jumio support | An unsupported document code has been set, retry impossible |
