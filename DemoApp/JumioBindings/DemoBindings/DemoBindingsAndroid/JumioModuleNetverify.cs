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
using Com.Jumio.NV;
using Com.Jumio.NV.Data.Document;

namespace JumioBindingAndroid
{

    public class JumioModuleNetverify
    {
        public Action<string, Dictionary<string, object>> EventWithNameHandler { get; set; }

        private static string TAG = "JumioMobileSDKNetverify";
        public static int PERMISSION_REQUEST_CODE_NETVERIFY = 301;

        public static NetverifySDK netverifySDK;

        private Activity _context;

        public JumioModuleNetverify(Activity context)
        {
            _context = context;
        }

        public void InitNetverify(string apiToken, string apiSecret, string dataCenter, Dictionary<string, object> options)
        {
            if (!NetverifySDK.IsSupportedPlatform(_context))
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
                netverifySDK = NetverifySDK.Create(_context, apiToken, apiSecret, center);

                this.ConfigureNetverify(options);
            }
            catch (Java.Lang.Exception e)
            {
                ShowErrorMessage("Error initializing the Netverify SDK: " + e.LocalizedMessage);
            }
        }

        private void ConfigureNetverify(Dictionary<string, object> options)
        {
            foreach (var item in options)
            {
                string key = item.Key;

                if (key.Equals("requireVerification"))
                {
                    netverifySDK.SetRequireVerification((bool)item.Value);
                }
                else if (key.Equals("callbackUrl"))
                {
                    netverifySDK.SetCallbackUrl((string)item.Value);
                }
                else if (key.Equals("requireFaceMatch"))
                {
                    netverifySDK.SetRequireFaceMatch((bool)item.Value);
                }
                else if (key.Equals("preselectedCountry"))
                {
                    netverifySDK.SetPreselectedCountry((string)item.Value);
                }
                else if (key.Equals("merchantScanReference"))
                {
                    netverifySDK.SetMerchantScanReference((string)item.Value);
                }
                else if (key.Equals("merchantReportingCriteria"))
                {
                    netverifySDK.SetMerchantReportingCriteria((string)item.Value);
                }
                else if (key.Equals("customerID"))
                {
                    netverifySDK.SetCustomerId((string)item.Value);
                }
                else if (key.Equals("enableEpassport"))
                {
                    netverifySDK.SetEnableEMRTD((bool)item.Value);
                }
                else if (key.Equals("sendDebugInfoToJumio"))
                {
                    netverifySDK.SendDebugInfoToJumio((bool)item.Value);
                }
                else if (key.Equals("dataExtractionOnMobileOnly"))
                {
                    netverifySDK.SetDataExtractionOnMobileOnly((bool)item.Value);
                }
                else if (key.Equals("cameraPosition"))
                {
                    JumioCameraPosition cameraPosition = (((string)item.Value).ToLower().Equals("front")) ? JumioCameraPosition.Front : JumioCameraPosition.Back;
                    netverifySDK.SetCameraPosition(cameraPosition);
                }
                else if (key.Equals("preselectedDocumentVariant"))
                {
                    NVDocumentVariant variant = (((string)item.Value).ToLower().Equals("paper")) ? NVDocumentVariant.Paper : NVDocumentVariant.Plastic;
                    netverifySDK.SetPreselectedDocumentVariant(variant);
                }
                else if (key.Equals("documentTypes"))
                {
                    var types = (IEnumerable<string>)item.Value;

                    IList<NVDocumentType> documentTypes = new List<NVDocumentType>();
                    foreach (String type in types)
                    {
                        if (type.ToLower().Equals("passport"))
                        {
                            documentTypes.Add(NVDocumentType.Passport);
                        }
                        else if (type.ToLower().Equals("driver_license"))
                        {
                            documentTypes.Add(NVDocumentType.DriverLicense);
                        }
                        else if (type.ToLower().Equals("identity_card"))
                        {
                            documentTypes.Add(NVDocumentType.IdentityCard);
                        }
                        else if (type.ToLower().Equals("visa"))
                        {
                            documentTypes.Add(NVDocumentType.Visa);
                        }
                    }

                    netverifySDK.SetPreselectedDocumentTypes(documentTypes);
                }
            }
        }

        public void StartNetverify()
        {
            if (netverifySDK == null)
            {
                ShowErrorMessage("The Netverify SDK is not initialized yet. Call initNetverify() first.");
                return;
            }

            try
            {
                CheckPermissionsAndStart(netverifySDK);
            }
            catch (Java.Lang.Exception e)
            {
                ShowErrorMessage("Error starting the Netverify SDK: " + e.LocalizedMessage);
            }
        }

        public void EnableEMRTD()
        {
            if (netverifySDK == null)
            {
                ShowErrorMessage("The Netverify SDK is not initialized yet. Call initNetverify() first.");
                return;
            }
            netverifySDK.SetEnableEMRTD(true);
        }

        private void CheckPermissionsAndStart(MobileSDK sdk)
        {
            if (!MobileSDK.HasAllRequiredPermissions(_context))
            {
                //Acquire missing permissions.
                String[] mp = MobileSDK.GetMissingPermissions(_context);

                int code;
                if (sdk is NetverifySDK)
                    code = PERMISSION_REQUEST_CODE_NETVERIFY;
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