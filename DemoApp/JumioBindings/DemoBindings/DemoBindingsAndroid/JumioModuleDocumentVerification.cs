using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Jumio;
using Com.Jumio.Core.Enums;
using Com.Jumio.Core.Exceptions;
using Com.Jumio.DV;

namespace JumioBindingAndroid
{
    public class JumioModuleDocumentVerification
    {
        public Action<string, Dictionary<string, object>> EventWithNameHandler { get; set; }

        private static string TAG = "JumioMobileSDKDocumentVerification";
        public static int PERMISSION_REQUEST_CODE_DOCUMENT_VERIFICATION = 301;

        public static DocumentVerificationSDK DocumentVerificationSDK;

        private Activity _context;

        public JumioModuleDocumentVerification(Activity context)
        {
            _context = context;
        }

        public void InitDocumentVerification(string apiToken, string apiSecret, string dataCenter, Dictionary<string, object> options)
        {
            if (!DocumentVerificationSDK.IsSupportedPlatform(_context))
            {
                ShowErrorMessage("This platform is not supported.");
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(apiToken) || string.IsNullOrWhiteSpace(apiSecret) || string.IsNullOrWhiteSpace(dataCenter))
                {
                    ShowErrorMessage("Missing required parameters apiToken, apiSecret or dataCenter.");
                    return;
                }

                JumioDataCenter center = (dataCenter.Equals("eu", System.StringComparison.CurrentCultureIgnoreCase)) ? JumioDataCenter.Eu : JumioDataCenter.Us;
                DocumentVerificationSDK = DocumentVerificationSDK.Create(_context, apiToken, apiSecret, center);

                this.ConfigureDocumentVerification(options);
            }
            catch (Java.Lang.Exception e)
            {
                ShowErrorMessage("Error initializing the DocumentVerification SDK: " + e.LocalizedMessage);
            }
        }

        private void ConfigureDocumentVerification(Dictionary<string, object> options)
        {
            foreach (var item in options)
            {
                string key = item.Key;

                if (key.Equals("type"))
                {
                    DocumentVerificationSDK.SetType((string)item.Value);
                }
                else if (key.Equals("customDocumentCode"))
                {
                    DocumentVerificationSDK.SetCallbackUrl((string)item.Value);
                }
                else if (key.Equals("country"))
                {
                    DocumentVerificationSDK.SetCountry((string)item.Value);
                }
                else if (key.Equals("merchantReportingCriteria"))
                {
                    DocumentVerificationSDK.SetMerchantReportingCriteria((string)item.Value);
                }
                else if (key.Equals("callbackUrl"))
                {
                    DocumentVerificationSDK.SetCallbackUrl((string)item.Value);
                }
                else if (key.Equals("enableExtraction"))
                {
                    DocumentVerificationSDK.SetEnableExtraction((bool)item.Value);
                }
                else if (key.Equals("merchantScanReference"))
                {
                    DocumentVerificationSDK.SetMerchantScanReference((string)item.Value);
                }
                else if (key.Equals("customerId"))
                {
                    DocumentVerificationSDK.SetCustomerId((string)item.Value);
                }
                else if (key.Equals("documentName"))
                {
                    DocumentVerificationSDK.SetDocumentName((string)item.Value);
                }
                else if (key.Equals("cameraPosition"))
                {
                    JumioCameraPosition cameraPosition = (((string)item.Value).ToLower().Equals("front")) ? JumioCameraPosition.Front : JumioCameraPosition.Back;
                    DocumentVerificationSDK.SetCameraPosition(cameraPosition);
                }
            }
        }

        public void StartDocumentVerification()
        {
            if (DocumentVerificationSDK == null)
            {
                ShowErrorMessage("The DocumentVerification SDK is not initialized yet. Call InitDocumentVerification() first.");
                return;
            }

            try
            {
                CheckPermissionsAndStart(DocumentVerificationSDK);
            }
            catch (Java.Lang.Exception e)
            {
                ShowErrorMessage("Error starting the DocumentVerification SDK: " + e.LocalizedMessage);
            }
        }

        private void CheckPermissionsAndStart(MobileSDK sdk)
        {
            if (!MobileSDK.HasAllRequiredPermissions(_context))
            {
                //Acquire missing permissions.
                String[] mp = MobileSDK.GetMissingPermissions(_context);

                int code;
                if (sdk is DocumentVerificationSDK)
                    code = PERMISSION_REQUEST_CODE_DOCUMENT_VERIFICATION;
                else
                {
                    ShowErrorMessage("Invalid SDK instance");
                    return;
                }

                ActivityCompat.RequestPermissions(_context, mp, code);
                //The result is received in JumioMainActivity::onRequestPermissionsResult.
            }
            else
            {
                StartSdk(sdk);
            }
        }

        protected void StartSdk(MobileSDK sdk)
        {
            try
            {
                sdk.Start();
            }
            catch (MissingPermissionException e)
            {
                ShowErrorMessage(e.LocalizedMessage);
            }
        }

        public void ShowErrorMessage(string msg)
        {
            Log.Error("Error", msg);
            Dictionary<string, object> errorResult = new Dictionary<string, object>();
            errorResult.Add("errorMessage", msg != null ? msg : "");
            SendEvent("EventError", errorResult);
        }

        // Helper methods

        public void SendEvent(string eventName, Dictionary<string, object> result)
        {
            EventWithNameHandler?.Invoke(eventName, result);
            foreach (KeyValuePair<string, object> kvp in result)
            {
                Console.WriteLine(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
            }
        }
    }
}