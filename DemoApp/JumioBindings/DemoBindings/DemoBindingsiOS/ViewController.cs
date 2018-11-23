using System;
using Foundation;
using NativeLibrary;
using UIKit;
using System.Diagnostics;
using CoreFoundation;
using System.Globalization;
using CoreGraphics;

namespace JumioBindingiOS
{
    public partial class ViewController : UIViewController
    {
        protected readonly JumioMobileSDKNetverify _NvManager = new JumioMobileSDKNetverify();
        protected readonly JumioMobileSDKDocumentVerification _DvManager = new JumioMobileSDKDocumentVerification();

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void DemoButton_Clicked(UIButton sender)
        {
            var settings = new NSMutableDictionary();
            settings.SetValueForKey(NSNumber.FromBoolean(true), new NSString("requireVerification"));
            _NvManager.InitNetverify("API_TOKEN", "API_SECRET", "DATACENTER", settings);
            _NvManager.StartNetverify();

            _NvManager.JumioEventHandler -= Manager_JumioEventHandler;
            _NvManager.JumioEventHandler += Manager_JumioEventHandler;
        }

        void Manager_JumioEventHandler(string message, NSMutableDictionary result)
        {
            _NvManager.JumioEventHandler -= Manager_JumioEventHandler;
            _DvManager.EventWithNameHandler -= Manager_JumioEventHandler;

            if (message == "EventError")
            {
                //TODO: Review Result
            }
            else if (message == "EventDocumentData")
            {
               
            }
        }

        partial void Document_Button_Clicked(UIButton sender)
        {
            var settings = new NSMutableDictionary();
            //Mandatory Fields https://github.com/Jumio/mobile-react#document-verification
            settings.SetValueForKey(new NSString("BC"), new NSString("type"));
            settings.SetValueForKey(new NSString("CUSTOMER ID"), new NSString("customerId"));
            settings.SetValueForKey(new NSString("USA"), new NSString("country"));
            settings.SetValueForKey(new NSString("YOURSCANREFERENCE"), new NSString("merchantScanReference"));

            _DvManager.InitDocumentVerification("API_TOKEN", "API_SECRET", "DATACENTER", settings);
            _DvManager.StartDocumentVerification();

            _DvManager.EventWithNameHandler -= Manager_JumioEventHandler;
            _DvManager.EventWithNameHandler += Manager_JumioEventHandler;
        }
    }
}
