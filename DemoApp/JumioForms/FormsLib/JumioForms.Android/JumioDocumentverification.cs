using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Support.V4.App;
using Com.Jumio;
using Com.Jumio.Core.Enums;
using Com.Jumio.Core.Exceptions;
using Com.Jumio.DV;
using JumioForms.Abstractions;

namespace JumioForms
{
    public class JumioDocumentVerification : IDocumentverification
    {
        private static string TAG = "JumioMobileSDKNetverify";
        public static int PERMISSION_REQUEST_CODE_DOCUMENT_VERIFICATION = 302;

        static DocumentVerificationSDK _documentVerificationSDK;
        static TaskCompletionSource<DocumentResult> _jumioVerifyTask;

        #region IDocumentverify

        public void Init(string apiToken, string apiSecret, string dataCenter, IDictionary<string, object> options, IDictionary<string, object> customization = null)
        {
            if (!DocumentVerificationSDK.IsSupportedPlatform(CrossJumio.CurrentActivity))
            {
                throw new Exception("This platform is not supported.");
            }

            try
            {
                if (string.IsNullOrWhiteSpace(apiToken) || string.IsNullOrWhiteSpace(apiSecret) || string.IsNullOrWhiteSpace(dataCenter))
                {
                    throw new Exception("Missing required parameters apiToken, apiSecret or dataCenter.");
                }

                JumioDataCenter center = (dataCenter.Equals("eu", System.StringComparison.CurrentCultureIgnoreCase)) ? JumioDataCenter.Eu : JumioDataCenter.Us;
                _documentVerificationSDK = DocumentVerificationSDK.Create(CrossJumio.CurrentActivity, apiToken, apiSecret, center);

                ConfigureDocumentVerification(options);
            }
            catch (Java.Lang.Exception e)
            {
                throw new Exception("Error initializing the DocumentVerification SDK: " + e.LocalizedMessage);
            }
        }

        public Task<DocumentResult> VerifyAsync()
        {
            if (_jumioVerifyTask != null)
            {
                _jumioVerifyTask.TrySetCanceled();
            }

            _jumioVerifyTask = new TaskCompletionSource<DocumentResult>();

            StartDocumentVerification();

            return _jumioVerifyTask.Task;
        }

        #endregion

        #region Activity Methods

        public static void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == DocumentVerificationSDK.RequestCode)
            {
                if (data == null)
                {
                    return;
                }

                string scanReference = "", errorMessage = "", errorCode = "";
                if (data.HasExtra(DocumentVerificationSDK.ExtraScanReference))
                {
                    scanReference = data.GetStringExtra(DocumentVerificationSDK.ExtraScanReference);
                }
                if (data.HasExtra(DocumentVerificationSDK.ExtraErrorMessage))
                {
                    errorMessage = data.GetStringExtra(DocumentVerificationSDK.ExtraErrorMessage);
                }
                if (data.HasExtra(DocumentVerificationSDK.ExtraErrorCode))
                {
                    errorCode = data.GetStringExtra(DocumentVerificationSDK.ExtraErrorCode);
                }

                if (resultCode == Result.Ok)
                {
                    SendEvent(true, scanReference, errorMessage, errorCode);
                }
                else if (resultCode == Result.Canceled)
                {
                    SendEvent(false, scanReference, errorMessage, errorCode);
                }
                if (_documentVerificationSDK != null)
                {
                    _documentVerificationSDK.Destroy();
                }
            }
        }

        public static void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (PERMISSION_REQUEST_CODE_DOCUMENT_VERIFICATION == requestCode)
            {
                bool allGranted = true;
                foreach (var grantResult in grantResults)
                {
                    if (grantResult != Permission.Granted)
                    {
                        allGranted = false;
                        break;
                    }
                }

                if (allGranted)
                {
                    StartSdk(_documentVerificationSDK);
                }
                else
                {
                    if (_jumioVerifyTask != null)
                    {
                        _jumioVerifyTask.TrySetCanceled();
                    }
                    throw new Exception("You need to grant all required permissions to start the Jumio SDK");
                }
            }
        }

        #endregion

        private void ConfigureDocumentVerification(IDictionary<string, object> options)
        {
            foreach (var item in options)
            {
                string key = item.Key;

                if (key.Equals("type"))
                {
                    _documentVerificationSDK.SetType((string)item.Value);
                }
                else if (key.Equals("customDocumentCode"))
                {
                    _documentVerificationSDK.SetCallbackUrl((string)item.Value);
                }
                else if (key.Equals("country"))
                {
                    _documentVerificationSDK.SetCountry((string)item.Value);
                }
                else if (key.Equals("merchantReportingCriteria"))
                {
                    _documentVerificationSDK.SetMerchantReportingCriteria((string)item.Value);
                }
                else if (key.Equals("callbackUrl"))
                {
                    _documentVerificationSDK.SetCallbackUrl((string)item.Value);
                }
                else if (key.Equals("enableExtraction"))
                {
                    _documentVerificationSDK.SetEnableExtraction((bool)item.Value);
                }
                else if (key.Equals("merchantScanReference"))
                {
                    _documentVerificationSDK.SetMerchantScanReference((string)item.Value);
                }
                else if (key.Equals("customerId"))
                {
                    _documentVerificationSDK.SetCustomerId((string)item.Value);
                }
                else if (key.Equals("documentName"))
                {
                    _documentVerificationSDK.SetDocumentName((string)item.Value);
                }
                else if (key.Equals("cameraPosition"))
                {
                    JumioCameraPosition cameraPosition = (((string)item.Value).ToLower().Equals("front")) ? JumioCameraPosition.Front : JumioCameraPosition.Back;
                    _documentVerificationSDK.SetCameraPosition(cameraPosition);
                }
            }
        }

        private void StartDocumentVerification()
        {
            if (_documentVerificationSDK == null)
            {
                if (_jumioVerifyTask != null)
                {
                    _jumioVerifyTask.TrySetCanceled();
                }
                throw new Exception("The DocumentVerification SDK is not initialized yet. Call InitDocumentVerification() first.");
            }

            try
            {
                CheckPermissionsAndStart(_documentVerificationSDK);
            }
            catch (Java.Lang.Exception e)
            {
                if (_jumioVerifyTask != null)
                {
                    _jumioVerifyTask.TrySetCanceled();
                }
                throw new Exception("Error starting the Netverify SDK: " + e.LocalizedMessage);
            }
        }

        private void CheckPermissionsAndStart(MobileSDK sdk)
        {
            if (!MobileSDK.HasAllRequiredPermissions(CrossJumio.CurrentActivity))
            {
                //Acquire missing permissions.
                String[] mp = MobileSDK.GetMissingPermissions(CrossJumio.CurrentActivity);

                int code;
                if (sdk is DocumentVerificationSDK)
                {
                    code = PERMISSION_REQUEST_CODE_DOCUMENT_VERIFICATION;
                }
                else
                {
                    if (_jumioVerifyTask != null)
                    {
                        _jumioVerifyTask.TrySetCanceled();
                    }
                    throw new Exception("Invalid SDK instance");
                }

                ActivityCompat.RequestPermissions(CrossJumio.CurrentActivity, mp, code);
            }
            else
            {
                StartSdk(sdk);
            }
        }

        private static void StartSdk(MobileSDK sdk)
        {
            try
            {
                sdk.Start();
            }
            catch (MissingPermissionException e)
            {
                throw new Exception(e.LocalizedMessage);
            }
        }

        private static void SendEvent(bool isSuccess, string scanReference, string errorMessage, string errorCode)
        {
            if (_jumioVerifyTask != null)
            {
                _jumioVerifyTask.TrySetResult(new DocumentResult(isSuccess, scanReference, errorMessage, errorCode));
            }
        }
    }
}
