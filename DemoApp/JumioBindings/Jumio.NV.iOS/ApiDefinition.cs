using System;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace NativeLibrary
{
    // The first step to creating a binding is to add your native library ("libNativeLibrary.a")
    // to the project by right-clicking (or Control-clicking) the folder containing this source
    // file and clicking "Add files..." and then simply select the native library (or libraries)
    // that you want to bind.
    //
    // When you do that, you'll notice that MonoDevelop generates a code-behind file for each
    // native library which will contain a [LinkWith] attribute. VisualStudio auto-detects the
    // architectures that the native library supports and fills in that information for you,
    // however, it cannot auto-detect any Frameworks or other system libraries that the
    // native library may depend on, so you'll need to fill in that information yourself.
    //
    // Once you've done that, you're ready to move on to binding the API...
    //
    //
    // Here is where you'd define your API definition for the native Objective-C library.
    //
    // For example, to bind the following Objective-C class:
    //
    //     @interface Widget : NSObject {
    //     }
    //
    // The C# binding would look like this:
    //
    //     [BaseType (typeof (NSObject))]
    //     interface Widget {
    //     }
    //
    // To bind Objective-C properties, such as:
    //
    //     @property (nonatomic, readwrite, assign) CGPoint center;
    //
    // You would add a property definition in the C# interface like so:
    //
    //     [Export ("center")]
    //     CGPoint Center { get; set; }
    //
    // To bind an Objective-C method, such as:
    //
    //     -(void) doSomething:(NSObject *)object atIndex:(NSInteger)index;
    //
    // You would add a method definition to the C# interface like so:
    //
    //     [Export ("doSomething:atIndex:")]
    //     void DoSomething (NSObject object, int index);
    //
    // Objective-C "constructors" such as:
    //
    //     -(id)initWithElmo:(ElmoMuppet *)elmo;
    //
    // Can be bound as:
    //
    //     [Export ("initWithElmo:")]
    //     IntPtr Constructor (ElmoMuppet elmo);
    //
    // For more information, see http://docs.xamarin.com/ios/advanced_topics/binding_objective-c_types
    //


    // @interface NetverifyMrzData : NSObject
    [BaseType(typeof(NSObject))]
    interface NetverifyMrzData
    {
        // @property (nonatomic, strong) NSString * line1;
        [Export("line1", ArgumentSemantic.Strong)]
        string Line1 { get; set; }

        // @property (nonatomic, strong) NSString * line2;
        [Export("line2", ArgumentSemantic.Strong)]
        string Line2 { get; set; }

        // @property (nonatomic, strong) NSString * line3;
        [Export("line3", ArgumentSemantic.Strong)]
        string Line3 { get; set; }

        // @property (assign, nonatomic) NetverifyMRZFormat format;
        [Export("format", ArgumentSemantic.Assign)]
        NetverifyMRZFormat Format { get; set; }

        // -(BOOL)idNumberValid;
        [Export("idNumberValid")]
        bool IdNumberValid { get; }

        // -(BOOL)dobValid;
        [Export("dobValid")]
        bool DobValid { get; }

        // -(BOOL)expiryDateValid;
        [Export("expiryDateValid")]
        bool ExpiryDateValid { get; }

        // -(BOOL)personalNumberValid;
        [Export("personalNumberValid")]
        bool PersonalNumberValid { get; }

        // -(BOOL)compositeValid;
        [Export("compositeValid")]
        bool CompositeValid { get; }
    }

    // @interface NetverifyDocumentData : NSObject
    [BaseType(typeof(NSObject))]
    interface NetverifyDocumentData
    {
        // @property (nonatomic, strong) NSString * _Nonnull selectedCountry;
        [Export("selectedCountry", ArgumentSemantic.Strong)]
        string SelectedCountry { get; set; }

        // @property (assign, nonatomic) NetverifyDocumentType selectedDocumentType;
        [Export("selectedDocumentType", ArgumentSemantic.Assign)]
        NetverifyDocumentType SelectedDocumentType { get; set; }

        // @property (nonatomic, strong) NSString * _Nullable idNumber;
        [NullAllowed, Export("idNumber", ArgumentSemantic.Strong)]
        string IdNumber { get; set; }

        // @property (nonatomic, strong) NSString * _Nullable personalNumber;
        [NullAllowed, Export("personalNumber", ArgumentSemantic.Strong)]
        string PersonalNumber { get; set; }

        // @property (nonatomic, strong) NSDate * _Nullable issuingDate;
        [NullAllowed, Export("issuingDate", ArgumentSemantic.Strong)]
        NSDate IssuingDate { get; set; }

        // @property (nonatomic, strong) NSDate * _Nullable expiryDate;
        [NullAllowed, Export("expiryDate", ArgumentSemantic.Strong)]
        NSDate ExpiryDate { get; set; }

        // @property (nonatomic, strong) NSString * _Nullable issuingCountry;
        [NullAllowed, Export("issuingCountry", ArgumentSemantic.Strong)]
        string IssuingCountry { get; set; }

        // @property (nonatomic, strong) NSString * _Nullable optionalData1;
        [NullAllowed, Export("optionalData1", ArgumentSemantic.Strong)]
        string OptionalData1 { get; set; }

        // @property (nonatomic, strong) NSString * _Nullable optionalData2;
        [NullAllowed, Export("optionalData2", ArgumentSemantic.Strong)]
        string OptionalData2 { get; set; }

        // @property (nonatomic, strong) NSString * _Nullable lastName;
        [NullAllowed, Export("lastName", ArgumentSemantic.Strong)]
        string LastName { get; set; }

        // @property (nonatomic, strong) NSString * _Nullable firstName;
        [NullAllowed, Export("firstName", ArgumentSemantic.Strong)]
        string FirstName { get; set; }

        // @property (nonatomic, strong) NSDate * _Nullable dob;
        [NullAllowed, Export("dob", ArgumentSemantic.Strong)]
        NSDate Dob { get; set; }

        // @property (assign, nonatomic) NetverifyGender gender;
        [Export("gender", ArgumentSemantic.Assign)]
        NetverifyGender Gender { get; set; }

        // @property (nonatomic, strong) NSString * _Nullable originatingCountry;
        [NullAllowed, Export("originatingCountry", ArgumentSemantic.Strong)]
        string OriginatingCountry { get; set; }

        // @property (nonatomic, strong) NSString * _Nullable addressLine;
        [NullAllowed, Export("addressLine", ArgumentSemantic.Strong)]
        string AddressLine { get; set; }

        // @property (nonatomic, strong) NSString * _Nullable city;
        [NullAllowed, Export("city", ArgumentSemantic.Strong)]
        string City { get; set; }

        // @property (nonatomic, strong) NSString * _Nullable subdivision;
        [NullAllowed, Export("subdivision", ArgumentSemantic.Strong)]
        string Subdivision { get; set; }

        // @property (nonatomic, strong) NSString * _Nullable postCode;
        [NullAllowed, Export("postCode", ArgumentSemantic.Strong)]
        string PostCode { get; set; }

        // @property (assign, nonatomic) NetverifyExtractionMethod extractionMethod;
        [Export("extractionMethod", ArgumentSemantic.Assign)]
        NetverifyExtractionMethod ExtractionMethod { get; set; }

        // @property (nonatomic, strong) NetverifyMrzData * _Nullable mrzData;
        [NullAllowed, Export("mrzData", ArgumentSemantic.Strong)]
        NetverifyMrzData MrzData { get; set; }

        // @property (nonatomic, strong) UIImage * _Nullable frontImage;
        [NullAllowed, Export("frontImage", ArgumentSemantic.Strong)]
        UIImage FrontImage { get; set; }

        // @property (nonatomic, strong) UIImage * _Nullable backImage;
        [NullAllowed, Export("backImage", ArgumentSemantic.Strong)]
        UIImage BackImage { get; set; }

        // @property (nonatomic, strong) UIImage * _Nullable faceImage;
        [NullAllowed, Export("faceImage", ArgumentSemantic.Strong)]
        UIImage FaceImage { get; set; }
    }

    // @protocol NetverifyViewControllerDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface NetverifyViewControllerDelegate
    {
        // @optional -(void)netverifyViewController:(NetverifyViewController * _Nonnull)netverifyViewController didFinishInitializingWithError:(NetverifyError * _Nullable)error;
        [Export("netverifyViewController:didFinishInitializingWithError:")]
        void DidFinishInitializingWithError(NetverifyViewController netverifyViewController, [NullAllowed] NetverifyError error);

        // @required -(void)netverifyViewController:(NetverifyViewController * _Nonnull)netverifyViewController didFinishWithDocumentData:(NetverifyDocumentData * _Nonnull)documentData scanReference:(NSString * _Nonnull)scanReference;
        [Abstract]
        [Export("netverifyViewController:didFinishWithDocumentData:scanReference:")]
        void DidFinishWithDocumentData(NetverifyViewController netverifyViewController, NetverifyDocumentData documentData, string scanReference);

        // @required -(void)netverifyViewController:(NetverifyViewController * _Nonnull)netverifyViewController didCancelWithError:(NetverifyError * _Nullable)error scanReference:(NSString * _Nullable)scanReference;
        [Abstract]
        [Export("netverifyViewController:didCancelWithError:scanReference:")]
        void DidCancelWithError(NetverifyViewController netverifyViewController, [NullAllowed] NetverifyError error, [NullAllowed] string scanReference);
    }

    // @protocol NetverifyUIControllerDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface NetverifyUIControllerDelegate
    {
        // @required -(void)netverifyUIController:(NetverifyUIController * _Nonnull)netverifyUIController didFinishInitializingWithError:(NetverifyError * _Nullable)error;
        [Abstract]
        [Export("netverifyUIController:didFinishInitializingWithError:")]
        void NetverifyUIController(NetverifyUIController netverifyUIController, [NullAllowed] NetverifyError error);

        // @required -(void)netverifyUIController:(NetverifyUIController * _Nonnull)netverifyUIController didDetermineAvailableCountries:(NSArray * _Nonnull)countries suggestedCountry:(NetverifyCountry * _Nullable)country;
        [Abstract]
        [Export("netverifyUIController:didDetermineAvailableCountries:suggestedCountry:")]
        void NetverifyUIController(NetverifyUIController netverifyUIController, NSObject[] countries, [NullAllowed] NetverifyCountry country);

        // @required -(void)netverifyUIController:(NetverifyUIController * _Nonnull)netverifyUIController didDetermineNextScanViewController:(NetverifyCustomScanViewController * _Nonnull)scanViewController isFallback:(BOOL)isFallback;
        [Abstract]
        [Export("netverifyUIController:didDetermineNextScanViewController:isFallback:")]
        void NetverifyUIController(NetverifyUIController netverifyUIController, NetverifyCustomScanViewController scanViewController, bool isFallback);

        // @required -(void)netverifyUIControllerDidCaptureAllParts:(NetverifyUIController * _Nonnull)netverifyUIController;
        [Abstract]
        [Export("netverifyUIControllerDidCaptureAllParts:")]
        void NetverifyUIControllerDidCaptureAllParts(NetverifyUIController netverifyUIController);

        // @required -(void)netverifyUIController:(NetverifyUIController * _Nonnull)netverifyUIController didDetermineError:(NetverifyError * _Nonnull)error retryPossible:(BOOL)retryPossible;
        [Abstract]
        [Export("netverifyUIController:didDetermineError:retryPossible:")]
        void NetverifyUIController(NetverifyUIController netverifyUIController, NetverifyError error, bool retryPossible);

        // @required -(void)netverifyUIController:(NetverifyUIController * _Nonnull)netverifyUIController didFinishWithDocumentData:(NetverifyDocumentData * _Nonnull)documentData scanReference:(NSString * _Nonnull)scanReference;
        [Abstract]
        [Export("netverifyUIController:didFinishWithDocumentData:scanReference:")]
        void NetverifyUIController(NetverifyUIController netverifyUIController, NetverifyDocumentData documentData, string scanReference);

        // @required -(void)netverifyUIController:(NetverifyUIController * _Nonnull)netverifyUIController didCancelWithError:(NetverifyError * _Nullable)error scanReference:(NSString * _Nullable)scanReference;
        [Abstract]
        [Export("netverifyUIController:didCancelWithError:scanReference:")]
        void NetverifyUIController(NetverifyUIController netverifyUIController, [NullAllowed] NetverifyError error, [NullAllowed] string scanReference);
    }

    //[Static]
    //partial interface Constants
    //{
    //    // extern NSString *const kJMSDKBundleShortVersionKey;
    //    [Field("kJMSDKBundleShortVersionKey", "__Internal")]
    //    NSString kJMSDKBundleShortVersionKey { get; }

    //    // extern NSString *const kJMSDKBundleVersionKey;
    //    [Field("kJMSDKBundleVersionKey", "__Internal")]
    //    NSString kJMSDKBundleVersionKey { get; }
    //}

    // @interface JMSDK : NSObject
    [BaseType(typeof(NSObject))]
    interface JMSDK
    {
        // @property (nonatomic, strong) NSBundle * bundle;
        [Export("bundle", ArgumentSemantic.Strong)]
        NSBundle Bundle { get; set; }

        // @property (readonly, nonatomic, strong) NSDictionary * plistDictionary;
        [Export("plistDictionary", ArgumentSemantic.Strong)]
        NSDictionary PlistDictionary { get; }

        // -(NSDictionary *)mobileDeviceDetails;
        [Export("mobileDeviceDetails")]
        NSDictionary MobileDeviceDetails { get; }

        // -(NSString *)shortVersionString;
        [Export("shortVersionString")]
        string ShortVersionString { get; }

        // -(NSString *)versionString;
        [Export("versionString")]
        string VersionString { get; }

        // -(NSString *)sdkVersionString;
        [Export("sdkVersionString")]
        string SdkVersionString { get; }

        // -(NSString *)sdkVersionStringFull;
        [Export("sdkVersionStringFull")]
        string SdkVersionStringFull { get; }

        // -(NSString *)bundleValueForKey:(NSString *const)key;
        [Export("bundleValueForKey:")]
        string BundleValueForKey(string key);
    }

    // @interface NetverifyConfiguration : NSObject <NSCopying>
    [BaseType(typeof(NSObject))]
    interface NetverifyConfiguration : INSCopying
    {
        // @property (nonatomic, strong) NSString * _Nonnull merchantApiToken;
        [Export("merchantApiToken", ArgumentSemantic.Strong)]
        string MerchantApiToken { get; set; }

        // @property (nonatomic, strong) NSString * _Nonnull merchantApiSecret;
        [Export("merchantApiSecret", ArgumentSemantic.Strong)]
        string MerchantApiSecret { get; set; }

        // @property (nonatomic, strong) NSString * _Nullable merchantScanReference;
        [NullAllowed, Export("merchantScanReference", ArgumentSemantic.Strong)]
        string MerchantScanReference { get; set; }

        // @property (nonatomic, strong) NSString * _Nullable merchantReportingCriteria;
        [NullAllowed, Export("merchantReportingCriteria", ArgumentSemantic.Strong)]
        string MerchantReportingCriteria { get; set; }

        // @property (assign, nonatomic) JumioDataCenter dataCenter;
        [Export("dataCenter", ArgumentSemantic.Assign)]
        JumioDataCenter DataCenter { get; set; }

        // @property (nonatomic, strong) NSString * _Nullable offlineToken;
        [NullAllowed, Export("offlineToken", ArgumentSemantic.Strong)]
        string OfflineToken { get; set; }

        [Wrap("WeakDelegate")]
        [NullAllowed]
        NetverifyViewControllerDelegate Delegate { get; set; }

        // @property (nonatomic, weak) id<NetverifyViewControllerDelegate> _Nullable delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        NSObject WeakDelegate { get; set; }

        [Wrap("WeakCustomUIDelegate")]
        [NullAllowed]
        NetverifyUIControllerDelegate CustomUIDelegate { get; set; }

        // @property (nonatomic, weak) id<NetverifyUIControllerDelegate> _Nullable customUIDelegate;
        [NullAllowed, Export("customUIDelegate", ArgumentSemantic.Weak)]
        NSObject WeakCustomUIDelegate { get; set; }

        // @property (nonatomic, strong) NSString * _Nullable preselectedCountry;
        [NullAllowed, Export("preselectedCountry", ArgumentSemantic.Strong)]
        string PreselectedCountry { get; set; }

        // @property (assign, nonatomic) NetverifyDocumentType preselectedDocumentTypes;
        [Export("preselectedDocumentTypes", ArgumentSemantic.Assign)]
        NetverifyDocumentType PreselectedDocumentTypes { get; set; }

        // @property (assign, nonatomic) NetverifyDocumentVariant preselectedDocumentVariant;
        [Export("preselectedDocumentVariant", ArgumentSemantic.Assign)]
        NetverifyDocumentVariant PreselectedDocumentVariant { get; set; }

        // @property (assign, nonatomic) BOOL requireFaceMatch;
        [Export("requireFaceMatch")]
        bool RequireFaceMatch { get; set; }

        // @property (assign, nonatomic) BOOL requireVerification;
        [Export("requireVerification")]
        bool RequireVerification { get; set; }

        // @property (nonatomic, strong) NSString * _Nullable customerId;
        [NullAllowed, Export("customerId", ArgumentSemantic.Strong)]
        string CustomerId { get; set; }

        // @property (assign, nonatomic) JumioCameraPosition cameraPosition;
        [Export("cameraPosition", ArgumentSemantic.Assign)]
        JumioCameraPosition CameraPosition { get; set; }

        // @property (nonatomic, strong) NSString * _Nullable callbackUrl;
        [NullAllowed, Export("callbackUrl", ArgumentSemantic.Strong)]
        string CallbackUrl { get; set; }

        // @property (assign, nonatomic) BOOL dataExtractionOnMobileOnly;
        [Export("dataExtractionOnMobileOnly")]
        bool DataExtractionOnMobileOnly { get; set; }

        // @property (assign, nonatomic) BOOL sendDebugInfoToJumio;
        [Export("sendDebugInfoToJumio")]
        bool SendDebugInfoToJumio { get; set; }

        // @property (assign, nonatomic) UIStatusBarStyle statusBarStyle;
        [Export("statusBarStyle", ArgumentSemantic.Assign)]
        UIStatusBarStyle StatusBarStyle { get; set; }
    }

    // @protocol JMAnimator <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface JMAnimator
    {
        // @required -(JMNavigationControllerTransitionType)transitionType;
        [Abstract]
        [Export("transitionType")]
        JMNavigationControllerTransitionType TransitionType { get; }

        // @required -(NSTimeInterval)offTransitionDuration;
        [Abstract]
        [Export("offTransitionDuration")]
        double OffTransitionDuration { get; }

        // @required -(NSTimeInterval)viewOffTransitionDuration;
        [Abstract]
        [Export("viewOffTransitionDuration")]
        double ViewOffTransitionDuration { get; }

        // @required -(void)performOffTransition:(BOOL)isPop toFullScreenBlur:(BOOL)toFullScreen;
        [Abstract]
        [Export("performOffTransition:toFullScreenBlur:")]
        void PerformOffTransition(bool isPop, bool toFullScreen);

        // @required -(NSTimeInterval)onTransitionDuration;
        [Abstract]
        [Export("onTransitionDuration")]
        double OnTransitionDuration { get; }

        // @required -(NSTimeInterval)viewOnTransitionDuration;
        [Abstract]
        [Export("viewOnTransitionDuration")]
        double ViewOnTransitionDuration { get; }

        // @required -(void)performOnTransition:(BOOL)isPop;
        [Abstract]
        [Export("performOnTransition:")]
        void PerformOnTransition(bool isPop);

        // @required -(void)prepareForCustomTransition:(BOOL)isPop;
        [Abstract]
        [Export("prepareForCustomTransition:")]
        void PrepareForCustomTransition(bool isPop);

        // @required -(BOOL)startsWithFullScreenBlur;
        [Abstract]
        [Export("startsWithFullScreenBlur")]
        bool StartsWithFullScreenBlur { get; }
    }

    // @interface JMNavigationController : UINavigationController
    [BaseType(typeof(UINavigationController))]
    interface JMNavigationController
    {
        // @property (nonatomic, strong) AVCaptureVideoPreviewLayer * previewLayer;
        [Export("previewLayer", ArgumentSemantic.Strong)]
        AVCaptureVideoPreviewLayer PreviewLayer { get; set; }
    }

    // @interface NetverifyViewController : JMNavigationController
    [BaseType(typeof(JMNavigationController))]
    interface NetverifyViewController
    {
        // -(instancetype _Nonnull)initWithConfiguration:(NetverifyConfiguration * _Nonnull)configuration;
        [Export("initWithConfiguration:")]
        IntPtr Constructor(NetverifyConfiguration configuration);

        // -(BOOL)updateConfiguration:(NetverifyConfiguration * _Nonnull)configuration;
        [Export("updateConfiguration:")]
        bool UpdateConfiguration(NetverifyConfiguration configuration);

        // -(NSString * _Nonnull)sdkVersion;
        [Export("sdkVersion")]
        string SdkVersion { get; }

        // -(NSUUID * _Nullable)debugID;
        [NullAllowed, Export("debugID")]
        NSUuid DebugID { get; }

        // -(void)destroy;
        [Export("destroy")]
        void Destroy();
    }

    // @interface NetverifyError : NSObject
    [BaseType(typeof(NSObject))]
    interface NetverifyError
    {
        // @property (readonly, nonatomic, strong) NSString * code;
        [Export("code", ArgumentSemantic.Strong)]
        string Code { get; }

        // @property (readonly, nonatomic, strong) NSString * message;
        [Export("message", ArgumentSemantic.Strong)]
        string Message { get; }
    }

    // @interface NetverifyUIController : NSObject
    [BaseType(typeof(NSObject))]
    interface NetverifyUIController
    {
        // -(instancetype _Nonnull)initWithConfiguration:(NetverifyConfiguration * _Nonnull)configuration;
        [Export("initWithConfiguration:")]
        IntPtr Constructor(NetverifyConfiguration configuration);

        // -(void)setupWithDocument:(NetverifyDocument * _Nonnull)document;
        [Export("setupWithDocument:")]
        void SetupWithDocument(NetverifyDocument document);

        // -(void)cancel;
        [Export("cancel")]
        void Cancel();

        // -(void)retryAfterError;
        [Export("retryAfterError")]
        void RetryAfterError();

        // -(NSString * _Nonnull)sdkVersion;
        [Export("sdkVersion")]
        string SdkVersion { get; }

        // -(NSUUID * _Nullable)debugID;
        [NullAllowed, Export("debugID")]
        NSUuid DebugID { get; }

        // -(void)destroy;
        [Export("destroy")]
        void Destroy();
    }

    // @interface NetverifyDocument : NSObject
    [BaseType(typeof(NSObject))]
    interface NetverifyDocument
    {
        // @property (readonly, nonatomic, strong) NSString * countryCode;
        [Export("countryCode", ArgumentSemantic.Strong)]
        string CountryCode { get; }

        // @property (readonly, assign, nonatomic) NetverifyDocumentType type;
        [Export("type", ArgumentSemantic.Assign)]
        NetverifyDocumentType Type { get; }

        // @property (assign, nonatomic) NetverifyDocumentVariant selectedVariant;
        [Export("selectedVariant", ArgumentSemantic.Assign)]
        NetverifyDocumentVariant SelectedVariant { get; set; }

        // -(BOOL)supportsPaperVariant;
        [Export("supportsPaperVariant")]
        bool SupportsPaperVariant { get; }

        // -(BOOL)supportsPlasticVariant;
        [Export("supportsPlasticVariant")]
        bool SupportsPlasticVariant { get; }

        // -(BOOL)hasMultipleVariants;
        [Export("hasMultipleVariants")]
        bool HasMultipleVariants { get; }
    }

    // @interface NetverifyCountry : NSObject
    [BaseType(typeof(NSObject))]
    interface NetverifyCountry
    {
        // @property (readonly, nonatomic, strong) NSString * _Nonnull code;
        [Export("code", ArgumentSemantic.Strong)]
        string Code { get; }

        // @property (readonly, nonatomic, strong) NSString * _Nonnull name;
        [Export("name", ArgumentSemantic.Strong)]
        string Name { get; }

        // @property (readonly, nonatomic, strong) NSArray<NetverifyDocument *> * _Nonnull documents;
        [Export("documents", ArgumentSemantic.Strong)]
        NetverifyDocument[] Documents { get; }
    }

    // @protocol NetverifyCustomScanViewControllerDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface NetverifyCustomScanViewControllerDelegate
    {
        // @required -(void)netverifyCustomScanViewController:(NetverifyCustomScanViewController * _Nonnull)customScanView shouldDisplayLegalAdvice:(NSString * _Nonnull)message completion:(void (^ _Nonnull)(void))completion;
        [Abstract]
        [Export("netverifyCustomScanViewController:shouldDisplayLegalAdvice:completion:")]
        void ShouldDisplayLegalAdvice(NetverifyCustomScanViewController customScanView, string message, Action completion);

        // @required -(void)netverifyCustomScanViewController:(NetverifyCustomScanViewController * _Nonnull)customScanView shouldDisplayConfirmationWithImageView:(NetverifyConfirmationImageView * _Nonnull)view text:(NSString * _Nonnull)text confirmation:(void (^ _Nullable)(void))confirmation retake:(void (^ _Nullable)(void))retake;
        [Abstract]
        [Export("netverifyCustomScanViewController:shouldDisplayConfirmationWithImageView:text:confirmation:retake:")]
        void ShouldDisplayConfirmationWithImageView(NetverifyCustomScanViewController customScanView, NetverifyConfirmationImageView view, string text, [NullAllowed] Action confirmation, [NullAllowed] Action retake);

        // @required -(void)netverifyCustomScanViewController:(NetverifyCustomScanViewController * _Nonnull)customScanView shouldDisplayNoUSAddressFoundHint:(NSString * _Nonnull)message confirmation:(void (^ _Nonnull)(void))confirmation;
        [Abstract]
        [Export("netverifyCustomScanViewController:shouldDisplayNoUSAddressFoundHint:confirmation:")]
        void ShouldDisplayNoUSAddressFoundHint(NetverifyCustomScanViewController customScanView, string message, Action confirmation);

        // @required -(void)netverifyCustomScanViewController:(NetverifyCustomScanViewController * _Nonnull)customScanView shouldDisplayFlipDocumentHint:(NSString * _Nonnull)message confirmation:(void (^ _Nonnull)(void))confirmation;
        [Abstract]
        [Export("netverifyCustomScanViewController:shouldDisplayFlipDocumentHint:confirmation:")]
        void ShouldDisplayFlipDocumentHint(NetverifyCustomScanViewController customScanView, string message, Action confirmation);

        // @optional -(void)netverifyCustomScanViewController:(NetverifyCustomScanViewController * _Nonnull)customScanView shouldDisplayBlurHint:(NSString * _Nonnull)message;
        [Export("netverifyCustomScanViewController:shouldDisplayBlurHint:")]
        void ShouldDisplayBlurHint(NetverifyCustomScanViewController customScanView, string message);
    }

    // @interface NetverifyCustomScanViewController : UIViewController
    [BaseType(typeof(UIViewController))]
    interface NetverifyCustomScanViewController
    {
        [Wrap("WeakCustomScanViewControllerDelegate")]
        NetverifyCustomScanViewControllerDelegate CustomScanViewControllerDelegate { get; set; }

        // @property (assign, nonatomic) id<NetverifyCustomScanViewControllerDelegate> _Nonnull customScanViewControllerDelegate;
        [NullAllowed, Export("customScanViewControllerDelegate", ArgumentSemantic.Assign)]
        NSObject WeakCustomScanViewControllerDelegate { get; set; }

        // @property (nonatomic, strong) UIView * _Nonnull customOverlayLayer;
        [Export("customOverlayLayer", ArgumentSemantic.Strong)]
        UIView CustomOverlayLayer { get; set; }

        // -(BOOL)isFallbackAvailable;
        [Export("isFallbackAvailable")]
        bool IsFallbackAvailable { get; }

        // -(void)switchToFallback;
        [Export("switchToFallback")]
        void SwitchToFallback();

        // -(BOOL)hasFlash;
        [Export("hasFlash")]
        bool HasFlash { get; }

        // -(BOOL)hasMultipleCameras;
        [Export("hasMultipleCameras")]
        bool HasMultipleCameras { get; }

        // -(BOOL)isFlashOn;
        [Export("isFlashOn")]
        bool IsFlashOn { get; }

        // -(id)currentCameraPosition;
        [Export("currentCameraPosition")]
        NSObject CurrentCameraPosition { get; }

        // -(BOOL)isImagePicker;
        [Export("isImagePicker")]
        bool IsImagePicker { get; }

        // -(void)takeImage;
        [Export("takeImage")]
        void TakeImage();

        // -(BOOL)canToggleFlash;
        [Export("canToggleFlash")]
        bool CanToggleFlash { get; }

        // -(void)toggleFlash;
        [Export("toggleFlash")]
        void ToggleFlash();

        // -(BOOL)canSwitchCamera;
        [Export("canSwitchCamera")]
        bool CanSwitchCamera { get; }

        // -(void)switchCamera;
        [Export("switchCamera")]
        void SwitchCamera();

        // -(void)retryScan;
        [Export("retryScan")]
        void RetryScan();

        // -(void)pauseScan;
        [Export("pauseScan")]
        void PauseScan();

        // -(CGRect)overlayFrame;
        [Export("overlayFrame")]
        CGRect OverlayFrame { get; }

        // -(NSString * _Nonnull)localizedShortHelpText;
        [Export("localizedShortHelpText")]
        string LocalizedShortHelpText { get; }

        // -(NSString * _Nonnull)localizedLongHelpText;
        [Export("localizedLongHelpText")]
        string LocalizedLongHelpText { get; }

        // -(NSUInteger)currentStep;
        [Export("currentStep")]
        nuint CurrentStep { get; }

        // -(NSUInteger)totalSteps;
        [Export("totalSteps")]
        nuint TotalSteps { get; }

        // -(NetverifyScanMode)currentScanMode;
        [Export("currentScanMode")]
        NetverifyScanMode CurrentScanMode { get; }

        // @property (assign, nonatomic) CGFloat verticalRoiOffset;
        [Export("verticalRoiOffset")]
        nfloat VerticalRoiOffset { get; set; }

        // -(CGRect)roiFrame;
        [Export("roiFrame")]
        CGRect RoiFrame { get; }
    }

    // @interface NetverifyConfirmationImageView : UIView
    [BaseType(typeof(UIView))]
    interface NetverifyConfirmationImageView
    {
        // @property (readonly, assign, nonatomic) CGSize imageSize;
        [Export("imageSize", ArgumentSemantic.Assign)]
        CGSize ImageSize { get; }
    }

    // @interface NetverifyBaseView : UIView <NetverifyAppearance>
    [BaseType(typeof(UIView))]
    interface NetverifyBaseView
    {
        // @property (nonatomic, strong) NSNumber * disableBlur __attribute__((annotate("ui_appearance_selector")));
        [Export("disableBlur", ArgumentSemantic.Strong)]
        NSNumber DisableBlur { get; set; }

        // @property (nonatomic, strong) UIColor * foregroundColor __attribute__((annotate("ui_appearance_selector")));
        [Export("foregroundColor", ArgumentSemantic.Strong)]
        UIColor ForegroundColor { get; set; }

        // @property (nonatomic, strong) NSString * customLightFontName __attribute__((annotate("ui_appearance_selector")));
        [Export("customLightFontName", ArgumentSemantic.Strong)]
        string CustomLightFontName { get; set; }

        // @property (nonatomic, strong) NSString * customRegularFontName __attribute__((annotate("ui_appearance_selector")));
        [Export("customRegularFontName", ArgumentSemantic.Strong)]
        string CustomRegularFontName { get; set; }

        // @property (nonatomic, strong) NSString * customMediumFontName __attribute__((annotate("ui_appearance_selector")));
        [Export("customMediumFontName", ArgumentSemantic.Strong)]
        string CustomMediumFontName { get; set; }

        // @property (nonatomic, strong) NSString * customBoldFontName __attribute__((annotate("ui_appearance_selector")));
        [Export("customBoldFontName", ArgumentSemantic.Strong)]
        string CustomBoldFontName { get; set; }

        // @property (nonatomic, strong) NSString * customItalicFontName __attribute__((annotate("ui_appearance_selector")));
        [Export("customItalicFontName", ArgumentSemantic.Strong)]
        string CustomItalicFontName { get; set; }
    }

    // @interface DocumentVerificationConfiguration : NSObject <NSCopying>
    [BaseType(typeof(NSObject))]
    interface DocumentVerificationConfiguration : INSCopying
    {
        // @property (nonatomic, strong) NSString * _Nonnull merchantApiToken;
        [Export("merchantApiToken", ArgumentSemantic.Strong)]
        string MerchantApiToken { get; set; }

        // @property (nonatomic, strong) NSString * _Nonnull merchantApiSecret;
        [Export("merchantApiSecret", ArgumentSemantic.Strong)]
        string MerchantApiSecret { get; set; }

        // @property (assign, nonatomic) JumioDataCenter dataCenter;
        [Export("dataCenter", ArgumentSemantic.Assign)]
        JumioDataCenter DataCenter { get; set; }

        // @property (nonatomic, strong) NSString * _Nonnull type;
        [Export("type", ArgumentSemantic.Strong)]
        string Type { get; set; }

        // @property (nonatomic, strong) NSString * _Nonnull country;
        [Export("country", ArgumentSemantic.Strong)]
        string Country { get; set; }

        // @property (nonatomic, strong) NSString * _Nonnull merchantScanReference;
        [Export("merchantScanReference", ArgumentSemantic.Strong)]
        string MerchantScanReference { get; set; }

        [Wrap("WeakDelegate")]
        [NullAllowed]
        DocumentVerificationViewControllerDelegate Delegate { get; set; }

        // @property (nonatomic, weak) id<DocumentVerificationViewControllerDelegate> _Nullable delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        NSObject WeakDelegate { get; set; }

        // @property (nonatomic, strong) NSString * _Nonnull customerId;
        [Export("customerId", ArgumentSemantic.Strong)]
        string CustomerId { get; set; }

        // @property (nonatomic, strong) NSString * _Nullable callbackUrl;
        [NullAllowed, Export("callbackUrl", ArgumentSemantic.Strong)]
        string CallbackUrl { get; set; }

        // @property (nonatomic, strong) NSString * _Nullable merchantReportingCriteria;
        [NullAllowed, Export("merchantReportingCriteria", ArgumentSemantic.Strong)]
        string MerchantReportingCriteria { get; set; }

        // @property (nonatomic, strong) NSString * _Nullable customDocumentCode;
        [NullAllowed, Export("customDocumentCode", ArgumentSemantic.Strong)]
        string CustomDocumentCode { get; set; }

        // @property (assign, nonatomic) JumioCameraPosition cameraPosition;
        [Export("cameraPosition", ArgumentSemantic.Assign)]
        JumioCameraPosition CameraPosition { get; set; }

        // @property (nonatomic, strong) NSString * _Nullable documentName;
        [NullAllowed, Export("documentName", ArgumentSemantic.Strong)]
        string DocumentName { get; set; }

        // @property (assign, nonatomic) BOOL enableExtraction;
        [Export("enableExtraction")]
        bool EnableExtraction { get; set; }

        // @property (assign, nonatomic) UIStatusBarStyle statusBarStyle;
        [Export("statusBarStyle", ArgumentSemantic.Assign)]
        UIStatusBarStyle StatusBarStyle { get; set; }
    }

    // @protocol DocumentVerificationViewControllerDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface DocumentVerificationViewControllerDelegate
    {
        // @required -(void)documentVerificationViewController:(DocumentVerificationViewController * _Nonnull)documentVerificationViewController didFinishWithScanReference:(NSString * _Nullable)scanReference;
        [Abstract]
        [Export("documentVerificationViewController:didFinishWithScanReference:")]
        void DidFinishWithScanReference(DocumentVerificationViewController documentVerificationViewController, [NullAllowed] string scanReference);

        // @required -(void)documentVerificationViewController:(DocumentVerificationViewController * _Nonnull)documentVerificationViewController didFinishWithError:(DocumentVerificationError * _Nonnull)error;
        [Abstract]
        [Export("documentVerificationViewController:didFinishWithError:")]
        void DidFinishWithError(DocumentVerificationViewController documentVerificationViewController, DocumentVerificationError error);
    }

    // @interface DocumentVerificationViewController : JMNavigationController
    [BaseType(typeof(JMNavigationController))]
    interface DocumentVerificationViewController
    {
        // -(instancetype _Nonnull)initWithConfiguration:(DocumentVerificationConfiguration * _Nonnull)configuration;
        [Export("initWithConfiguration:")]
        IntPtr Constructor(DocumentVerificationConfiguration configuration);

        // -(NSString * _Nonnull)sdkVersion;
        [Export("sdkVersion")]
        string SdkVersion { get; }
    }

    // @interface DocumentVerificationError : NSObject
    [BaseType(typeof(NSObject))]
    interface DocumentVerificationError
    {
        // @property (readonly, nonatomic, strong) NSString * code;
        [Export("code", ArgumentSemantic.Strong)]
        string Code { get; }

        // @property (readonly, nonatomic, strong) NSString * message;
        [Export("message", ArgumentSemantic.Strong)]
        string Message { get; }
    }
}