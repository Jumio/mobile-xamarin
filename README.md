# Plugin for Xamarin

Official Jumio Mobile SDK plugin for Xamarin

## Compatibility

We only ensure compatibility with Xamarin.Android SDK 9.0.0.19 and Xamarin.iOS SDK 12.0.0.15

## Setup

The SDK can be used in a native Xamarin.Android or Xamarin.iOS project, as well as in a Xamarin.Forms project.
There are demo applications for both the [native Android and iOS binding project](https://github.com/Jumio/mobile-xamarin/tree/master/DemoApp/JumioBindings) and the [Xamarin.Forms project](https://github.com/Jumio/mobile-xamarin/tree/master/DemoApp/JumioForms)

To set it up in your own project, first copy the .dll-files from the `Ref` folder for a [native project Android/iOS](https://github.com/Jumio/mobile-xamarin/tree/master/DemoApp/JumioForms/Ref).
Set up the .dll files as references in your project.

## Integration

The native Jumio Netverify and DocumentVerification SDKs from Android and iOS can be accessed in Xamarin via BindingProjects for [Android ](https://developer.xamarin.com/guides/android/advanced_topics/binding-a-java-library/binding-an-aar/) and [iOS](https://docs.microsoft.com/en-us/xamarin/ios/platform/binding-objective-c/). The specific documentations for Android and iOS below explain the usage and all methods that can be called in the native Binding projects for both products and both platforms.

### iOS
- [Integration Netverify & Fastfill SDK](ios/integration_netverify-fastfill.md)
- [Integration Document Verification SDK](ios/integration_document-verification.md)

### Android
- [Integration Netverify & Fastfill SDK](android/integration_netverify-fastfill.md)
- [Integration Document Verification SDK](android/integration_document-verification.md)

Initialize the SDK with the following call.
```
JumioMobileSDKNetverify.initNetverify(<API_TOKEN>, <API_SECRET>, <DATACENTER>, {settings});
JumioMobileSDKDocumentVerification.initDocumentVerification(<API_TOKEN>, <API_SECRET>, <DATACENTER>, {settings});
```
Datacenter can either be **US** or **EU**.

## Usage

The following chapters explain the usage of the plugin within a Xamarin.Forms application. The detailed implementation for this code can be found in the [JumioForms demo project](https://github.com/Jumio/mobile-xamarin/tree/master/DemoApp/JumioForms)

### Netverify

```csharp
CrossJumio.CurrentNetverify.Init(<API_TOKEN>, <API_SECRET>, <DATACENTER>, {settings}, {customization});
```

Datacenter can either be **US** or **EU**.

Configure the SDK with the *settings*-Object.

| Configuration | Datatype | Description |
| ------ | -------- | ----------- |
| requireVerification | Boolean | Enable ID verification |
| callbackUrl | String | Specify an URL for individual transactions |
| requireFaceMatch | Boolean | Enable face match during the ID verification for a specific transaction |
| preselectedCountry | Boolean | Specify the issuing country (ISO 3166-1 alpha-3 country code) |
| merchantScanReference | String | Allows you to identify the scan (max. 100 characters) |
| merchantReportingCriteria | String | Use this option to identify the scan in your reports (max. 100 characters) |
| customerId | String | Set a customer identifier (max. 100 characters) |
| sendDebugInfoToJumio | Boolean | Send debug information to Jumio. |
| dataExtractionOnMobileOnly | Boolean | Limit data extraction to be done on device only |
| cameraPosition | String | Which camera is used by default. Can be **FRONT** or **BACK**. |
| preselectedDocumentVariant | String | Which types of document variants are available. Can be **PAPER** or **PLASTIC** |
| documentTypes | String-Array | An array of accepted document types: Available document types: **PASSPORT**, **DRIVER_LICENSE**, **IDENTITY_CARD**, **VISA** |


Initialization example with configuration.

```csharp
var settings = new Dictionary<string, object>
      {
          {"requireVerification", true}
          ,{"callbackUrl", "URL"}
          ,{"preselectedCountry", "USA"}
          ,{"customerId", "CUSTOMER ID"}
          ,{"cameraPosition", "back"}
          ,{"documentTypes", new List<string>() {"passport", "driver_license", "identity_card", "visa"}}
      };

CrossJumio.CurrentNetverify.Init("API_TOKEN", "API_SECRET", "US", settings, customization)
```

As soon as the sdk is initialized, you can verify if all required permissions are set and start the SDK with this method:
```csharp
CrossJumio.CurrentNetverify.VerifyAsync()
```

### Document Verification

To initialize the SDK, perform the following call.

```csharp
CrossJumio.CurrentDocumentVerification.Init(<API_TOKEN>, <API_SECRET>, <DATACENTER>, {settings});
```

Datacenter can either be **US** or **EU**.

Configure the SDK with the *settings*-Object. **(settings marked with * are mandatory)**

| Configuration | Datatype | Description |
| ------ | -------- | ----------- |
| **type*** | String | See the list below |
| **customerId*** | String | Set a customer identifier (max. 100 characters) |
| **country*** | String | Set the country (ISO-3166-1 alpha-3 code) |
| **merchantScanReference*** | String | Allows you to identify the scan (max. 100 characters) |
| merchantReportingCriteria | String | Use this option to identify the scan in your reports (max. 100 characters) |
| callbackUrl | String | Specify an URL for individual transactions |
| documentName | String | Override the document label on the help screen |
| customDocumentCode | String | Set your custom document code (set in the merchant backend under "Settings" - "Multi Documents" - "Custom" |
| cameraPosition | String | Which camera is used by default. Can be **FRONT** or **BACK**. |

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

Initialization example with configuration.

```csharp
var settings = new Dictionary<string, object>
{
    {"type", "BC"}
    ,{"customerId", "CUSTOMER ID"}
    ,{"country", "USA"}
    ,{"merchantScanReference", "YOURSCANREFERENCE"}
    ,{"cameraPosition", "back"}
};

CrossJumio.CurrentDocumentVerification.Init("API_TOKEN", "API_SECRET", settings, customization)
```

As soon as the sdk is initialized, you can verify if all required permissions are set and start the SDK with this method:
```csharp
CrossJumio.CurrentDocumentVerification.VerifyAsync()
```

## Customization

### Android

#### Netverify
The Netverify SDK can be customized to the respective needs. The [native Android recommendation](https://developer.android.com/guide/topics/ui/look-and-feel/themes#Customize) of customizing an application using Themes is applied.
This chapter in the Android SDK explains how it works using our Jumio Surface tool: [customization chapter](https://github.com/Jumio/mobile-sdk-android/blob/v2.14.0/docs/integration_netverify-fastfill.md#customization).
The theme can be changed in the plugin by applying the customized Theme in the `Styles.xml` file of your project located in `YOURPROJECT/Resources/values/Styles.xml`. The name of the Theme has to be applied in the the AndroidManifest file (located at `YOURPROJECT/Properties/AndroidManifest.xml)`) `android:theme` property in the NetverifyActivity tag.


#### Document Verification
As done for Netverify, define a custom Theme that uses the `Theme.DocumentVerification` theme as parent theme and overrides attributes to your needs.
The theme can be changed in the plugin by applying the customized theme in the `Styles.xml` file of your project located in `YOURPROJECT/Resources/values/Styles.xml`. The name of the Theme has to be applied in the the AndroidManifest file (located at `YOURPROJECT/Properties/AndroidManifest.xml)`) `android:theme` property in the DocumentVerificationActivity tag.

Example for a customized theme in `Styles.xml`:
```
<style name="CustomDocumentVerification" parent="Theme.DocumentVerification">
    <item name="colorPrimary">#97be0d</item>
    <item name="colorPrimaryDark">#79980a</item>
    <item name="colorControlNormal">#ffffff</item>
</style>
```

### iOS

The SDK can be customized to the respective needs by using the **customization** parameter within the **Init() method** of the iOS bindings
```csharp
CrossJumio.CurrentNetverify.Init(<API_TOKEN>, <API_SECRET>, <DATACENTER>, {settings}, {customization});
CrossJumio.CurrentDocumentVerification.Init(<API_TOKEN>, <API_SECRET>, <DATACENTER>, {settings}, {customization});
```

You can pass the following customization options to the initializer:

| Customization key | Type | Description |
|:------------------|:-----|:------------|
| disableBlur       | BOOL | Deactivate the blur effect |
| backgroundColor   | STRING | Change base view's background color |
| foregroundColor   | STRING | Change base view's foreground color |
| tintColor         | STRING | Change the tint color of the navigation bar |
| barTintColor      | STRING | Change the bar tint color of the navigation bar |
| textTitleColor    | STRING | Change the text title color of the navigation bar |
| documentSelectionHeaderBackgroundColor | STRING | Change the background color of the document selection header |
| documentSelectionHeaderTitleColor | STRING | Change the title color of the document selection header |
| documentSelectionHeaderIconColor | STRING | Change the icon color of the document selection header |
| documentSelectionButtonBackgroundColor | STRING | Change the background color of the document selection button |
| documentSelectionButtonTitleColor | STRING | Change the title color of the document selection button |
| documentSelectionButtonIconColor | STRING | Change the icon color of the document selection button |
| fallbackButtonBackgroundColor | STRING | Change the background color of the fallback button |
| fallbackButtonBorderColor | STRING | Change the border color of the fallback button |
| fallbackButtonTitleColor | STRING | Change the title color of the fallback button |
| positiveButtonBackgroundColor | STRING | Change the background color of the positive button |
| positiveButtonBorderColor | STRING | Change the border color of the positive button |
| positiveButtonTitleColor | STRING | Change the title color of the positive button |
| negativeButtonBackgroundColor | STRING | Change the background color of the negative button |
| negativeButtonBorderColor | STRING | Change the border color of the negative button |
| negativeButtonTitleColor | STRING | Change the title color of the negative button |
| scanOverlayStandardColor (NV only) | STRING | Change the standard color of the scan overlay |
| scanOverlayValidColor (NV only) | STRING | Change the valid color of the scan overlay |
| scanOverlayInvalidColor (NV only) | STRING | Change the invalid color of the scan overlay |

All colors are provided with a HEX string with the following format: #ff00ff.

**Customization example**
```csharp
var customization = new Dictionary<string, object>
    {
        {"tintColor", "#ff0000"}
        ,{"disableBlur", true}
        ,{"backgroundColor", "#00ff00"}
    };

CrossJumio.CurrentNetverify.Init("API_TOKEN", "API_SECRET", "US", settings, customization);
```

## Localization

### Android
The SDK strings are localized using the native Android localization feature which is described here for the Android SDK: https://github.com/Jumio/mobile-sdk-android#localizing-labels.
Override a string by defining the respective attribute in the the `Strings.xml` file of your project located in `YOURPROJECT/Resources/values/Strings.xml`


# Support

## Contact

If you have any questions regarding our implementation guide please contact Jumio Customer Service at support@jumio.com or https://support.jumio.com. The Jumio online helpdesk contains a wealth of information regarding our service including demo videos, product descriptions, FAQs and other things that may help to get you started with Jumio. Check it out at: https://support.jumio.com.

## Copyright
&copy; Jumio Corp. 268 Lambert Avenue, Palo Alto, CA 94306

The source code and software available on this website (“Software”) is provided by Jumio Corp. or its affiliated group companies (“Jumio”) "as is” and any express or implied warranties, including, but not limited to, the implied warranties of merchantability and fitness for a particular purpose are disclaimed. In no event shall Jumio be liable for any direct, indirect, incidental, special, exemplary, or consequential damages (including but not limited to procurement of substitute goods or services, loss of use, data, profits, or business interruption) however caused and on any theory of liability, whether in contract, strict liability, or tort (including negligence or otherwise) arising in any way out of the use of this Software, even if advised of the possibility of such damage.
In any case, your use of this Software is subject to the terms and conditions that apply to your contractual relationship with Jumio. As regards Jumio’s privacy practices, please see our privacy notice available here: [Privacy Policy](https://www.jumio.com/legal-information/privacy-policy/).
