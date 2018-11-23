using System;
using NativeLibrary;
using Foundation;
using UIKit;
using System.Diagnostics;

namespace JumioBindingiOS
{
    public class JumioMobileSDKDocumentVerification : DocumentVerificationViewControllerDelegate
    {
        DocumentVerificationViewController _documentVerificationViewController;
        DocumentVerificationConfiguration _documentVerifcationConfiguration;

        public Action<string, NSMutableDictionary> EventWithNameHandler { get; set; }

        #region DocumentVerificationViewControllerDelegate

        public override void DidFinishWithError(DocumentVerificationViewController documentVerificationViewController, DocumentVerificationError error)
        {
            SendDocumentVerificationError(error);
        }

        public override void DidFinishWithScanReference(DocumentVerificationViewController documentVerificationViewController, string scanReference)
        {
            NSMutableDictionary result = new NSMutableDictionary();
            result.SetValueForKey(new NSString(scanReference), new NSString("scanReference"));

            AppDelegate del = (AppDelegate)UIApplication.SharedApplication.Delegate;
            del.Window.RootViewController.DismissViewController(true, () => {
                EventWithNameHandler?.Invoke("EventDocumentData", result);
            });
        }

        #endregion

        public void InitDocumentVerification(string apiToken, string apiSecret, string dataCenter, NSDictionary options = null, NSDictionary customization = null)
        {
            // Initialization
            _documentVerifcationConfiguration = new DocumentVerificationConfiguration();
            _documentVerifcationConfiguration.Delegate = this;
            _documentVerifcationConfiguration.MerchantApiToken = apiToken;
            _documentVerifcationConfiguration.MerchantApiSecret = apiSecret;
            _documentVerifcationConfiguration.DataCenter = dataCenter.ToLower() == "eu" ? JumioDataCenter.Eu : JumioDataCenter.Us;

            // Configuration
            if (options != null)
            {
                foreach (NSString key in options.Keys)
                {
                    if (key == "type")
                    {
                        _documentVerifcationConfiguration.Type = options.ValueForKey(key).ToString();
                    }
                    else if (key == "customDocumentCode")
                    {
                        _documentVerifcationConfiguration.CustomDocumentCode = options.ValueForKey(key).ToString();
                    }
                    else if (key == "country")
                    {
                        _documentVerifcationConfiguration.Country = options.ValueForKey(key).ToString();
                    }
                    else if (key == "merchantReportingCriteria")
                    {
                        _documentVerifcationConfiguration.MerchantReportingCriteria = options.ValueForKey(key).ToString();
                    }
                    else if (key == "callbackUrl")
                    {
                        _documentVerifcationConfiguration.CallbackUrl = options.ValueForKey(key).ToString();
                    }
                    else if (key == "merchantScanReference")
                    {
                        _documentVerifcationConfiguration.MerchantScanReference = options.ValueForKey(key).ToString();
                    }
                    else if (key == "customerId")
                    {
                        _documentVerifcationConfiguration.CustomerId = options.ValueForKey(key).ToString();
                    }
                    else if (key == "documentName")
                    {
                        _documentVerifcationConfiguration.DocumentName = options.ValueForKey(key).ToString();
                    }
                    else if (key == "enableExtraction")
                    {
                        _documentVerifcationConfiguration.EnableExtraction = JumioMobileSDKNetverify.GetBoolValue(options.ValueForKey(key));
                    }
                    else if (key == "cameraPosition")
                    {
                        var cameraString = options.ObjectForKey(key).ToString().ToLower();
                        JumioCameraPosition cameraPosition = cameraString == "front" ? JumioCameraPosition.Front : JumioCameraPosition.Back;
                        _documentVerifcationConfiguration.CameraPosition = cameraPosition;
                    }
                }
            }

            _documentVerificationViewController = new DocumentVerificationViewController(_documentVerifcationConfiguration);
        }

        public void StartDocumentVerification()
        {
            if (_documentVerificationViewController == null)
            {
                Debug.WriteLine(@"The Document Verification SDK is not initialized yet. Call initDocumentVerification() first.");
                return;
            }

            AppDelegate del = (AppDelegate)UIApplication.SharedApplication.Delegate;
            del.Window.RootViewController.PresentViewController(_documentVerificationViewController, true, null);

        }

        void SendDocumentVerificationError(DocumentVerificationError error)
        {
            NSMutableDictionary result = new NSMutableDictionary();
            result.SetValueForKey(new NSString(error.Code), new NSString("errorCode"));
            result.SetValueForKey(new NSString(error.Message), new NSString("errorMessage"));

            AppDelegate del = (AppDelegate)UIApplication.SharedApplication.Delegate;
            del.Window.RootViewController.DismissViewController(true, () => {
                EventWithNameHandler?.Invoke("EventError", result);
            });
        }
    }
}