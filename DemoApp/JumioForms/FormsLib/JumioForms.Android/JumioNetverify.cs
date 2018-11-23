using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Widget;
using Com.Jumio;
using Com.Jumio.Core.Enums;
using Com.Jumio.Core.Exceptions;
using Com.Jumio.NV;
using Com.Jumio.NV.Data.Document;
using Com.Jumio.NV.Enums;
using JumioForms.Abstractions;

namespace JumioForms
{
    public class JumioNetverify : INetverify
    {

        private static string TAG = "JumioMobileSDKNetverify";
        public static int PERMISSION_REQUEST_CODE_NETVERIFY = 301;

        static NetverifySDK _netverifySDK;
        static TaskCompletionSource<JumioResult> _jumioVerifyTask;

        #region INetverify

        public void Init(string apiToken, string apiSecret, string dataCenter, IDictionary<string, object> options, IDictionary<string, object> customization = null)
        {
            if (!NetverifySDK.IsSupportedPlatform(CrossJumio.CurrentActivity))
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
                _netverifySDK = NetverifySDK.Create(CrossJumio.CurrentActivity, apiToken, apiSecret, center);

                this.ConfigureNetverify(options);
            }
            catch (Java.Lang.Exception e)
            {
                throw new Exception("Error initializing the Netverify SDK: " + e.LocalizedMessage);
            }
        }

        public Task<JumioResult> VerifyAsync()
        {
            if(_jumioVerifyTask != null)
            {
                _jumioVerifyTask.TrySetCanceled();
            }

            _jumioVerifyTask = new TaskCompletionSource<JumioResult>();

            StartNetverify();

            return _jumioVerifyTask.Task;
        }

        #endregion

        #region Activity Methods

        public static void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == NetverifySDK.RequestCode)
            {
                if (data == null)
                {
                    return;
                }
                var scanReference = data.GetStringExtra(NetverifySDK.ExtraScanReference) != null ? data.GetStringExtra(NetverifySDK.ExtraScanReference) : "";

                if (resultCode == Result.Ok)
                {
                    NetverifyDocumentData documentData = (NetverifyDocumentData)data.GetParcelableExtra(NetverifySDK.ExtraScanData);

                    var result = new Dictionary<string, object>();
                    result.Add("selectedCountry", documentData.SelectedCountry);
                    if (documentData.SelectedDocumentType == NVDocumentType.Passport)
                    {
                        result.Add("selectedDocumentType", "PASSPORT");
                    }
                    else if (documentData.SelectedDocumentType == NVDocumentType.DriverLicense)
                    {
                        result.Add("selectedDocumentType", "DRIVER_LICENSE");
                    }
                    else if (documentData.SelectedDocumentType == NVDocumentType.IdentityCard)
                    {
                        result.Add("selectedDocumentType", "IDENTITY_CARD");
                    }
                    else if (documentData.SelectedDocumentType == NVDocumentType.Visa)
                    {
                        result.Add("selectedDocumentType", "VISA");
                    }
                    result.Add("idNumber", documentData.IdNumber);
                    result.Add("personalNumber", documentData.PersonalNumber);
                    result.Add("issuingDate", documentData.IssuingDate != null ? documentData.IssuingDate.ToString() : "");
                    result.Add("expiryDate", documentData.ExpiryDate != null ? documentData.ExpiryDate.ToString() : "");
                    result.Add("issuingCountry", documentData.IssuingCountry);
                    result.Add("lastName", documentData.LastName);
                    result.Add("firstName", documentData.FirstName);
                    result.Add("dob", documentData.Dob != null ? documentData.Dob.ToString() : ""); // test format
                    if (documentData.Gender == NVGender.M)
                    {
                        result.Add("gender", "m");
                    }
                    else if (documentData.Gender == NVGender.F)
                    {
                        result.Add("gender", "f");
                    }
                    else if (documentData.Gender == NVGender.X)
                    {
                        result.Add("gender", "x");
                    }
                    result.Add("originatingCountry", documentData.OriginatingCountry);
                    result.Add("addressLine", documentData.AddressLine);
                    result.Add("city", documentData.City);
                    result.Add("subdivision", documentData.Subdivision);
                    result.Add("postCode", documentData.PostCode);
                    result.Add("optionalData1", documentData.OptionalData1);
                    result.Add("optionalData2", documentData.OptionalData2);
                    result.Add("placeOfBirth", documentData.PlaceOfBirth);
                    if (documentData.ExtractionMethod == NVExtractionMethod.Mrz)
                    {
                        result.Add("extractionMethod", "MRZ");
                    }
                    else if (documentData.ExtractionMethod == NVExtractionMethod.Ocr)
                    {
                        result.Add("extractionMethod", "OCR");
                    }
                    else if (documentData.ExtractionMethod == NVExtractionMethod.Barcode)
                    {
                        result.Add("extractionMethod", "BARCODE");
                    }
                    else if (documentData.ExtractionMethod == NVExtractionMethod.BarcodeOcr)
                    {
                        result.Add("extractionMethod", "BARCODE_OCR");
                    }
                    else if (documentData.ExtractionMethod == NVExtractionMethod.None)
                    {
                        result.Add("extractionMethod", "NONE");
                    }

                    result.Add("scanReference", scanReference);

                    //MRZ data if available
                    if (documentData.MrzData != null)
                    {
                        var mrzData = new Dictionary<string, object>();
                        if (documentData.MrzData.Format == NVMRZFormat.Mrp)
                        {
                            mrzData.Add("format", "MRP");
                        }
                        else if (documentData.MrzData.Format == NVMRZFormat.Td1)
                        {
                            mrzData.Add("format", "TD1");
                        }
                        else if (documentData.MrzData.Format == NVMRZFormat.Td2)
                        {
                            mrzData.Add("format", "TD2");
                        }
                        else if (documentData.MrzData.Format == NVMRZFormat.Cnis)
                        {
                            mrzData.Add("format", "CNIS");
                        }
                        else if (documentData.MrzData.Format == NVMRZFormat.MrvA)
                        {
                            mrzData.Add("format", "MRVA");
                        }
                        else if (documentData.MrzData.Format == NVMRZFormat.MrvB)
                        {
                            mrzData.Add("format", "MRVB");
                        }
                        else if (documentData.MrzData.Format == NVMRZFormat.Unknown)
                        {
                            mrzData.Add("format", "UNKNOWN");
                        }
                        mrzData.Add("line1", documentData.MrzData.MrzLine1);
                        mrzData.Add("line2", documentData.MrzData.MrzLine2);
                        mrzData.Add("line3", documentData.MrzData.MrzLine3);
                        mrzData.Add("idNumberValid", documentData.MrzData.IdNumberValid());
                        mrzData.Add("dobValid", documentData.MrzData.DobValid());
                        mrzData.Add("expiryDateValid", documentData.MrzData.ExpiryDateValid());
                        mrzData.Add("personalNumberValid", documentData.MrzData.PersonalNumberValid());
                        mrzData.Add("compositeValid", documentData.MrzData.CompositeValid());
                        result.Add("mrzData", mrzData);
                    }

                    // EMRTD data if available
                    if (documentData.EMRTDStatus != null)
                    {
                        result.Add("emrtdStatus", documentData.EMRTDStatus.ToString());
                    }

                    SendEvent("EventDocumentData", result);
                }
                else if (resultCode == Result.Canceled)
                {
                    var errorMessage = data.GetStringExtra(NetverifySDK.ExtraErrorMessage);
                    var errorCode = data.GetStringExtra(NetverifySDK.ExtraErrorCode);
                    SendErrorObject(errorCode, errorMessage, scanReference);
                }
                if (_netverifySDK != null)
                {
                    _netverifySDK.Destroy();
                }
            }
        }

        public static void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (PERMISSION_REQUEST_CODE_NETVERIFY == requestCode)
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
                    StartSdk(_netverifySDK);
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

        private void ConfigureNetverify(IDictionary<string, object> options)
        {
            foreach (var item in options)
            {
                string key = item.Key;

                if (key.Equals("requireVerification"))
                {
                    _netverifySDK.SetRequireVerification((bool)item.Value);
                }
                else if (key.Equals("callbackUrl"))
                {
                    _netverifySDK.SetCallbackUrl((string)item.Value);
                }
                else if (key.Equals("requireFaceMatch"))
                {
                    _netverifySDK.SetRequireFaceMatch((bool)item.Value);
                }
                else if (key.Equals("preselectedCountry"))
                {
                    _netverifySDK.SetPreselectedCountry((string)item.Value);
                }
                else if (key.Equals("merchantScanReference"))
                {
                    _netverifySDK.SetMerchantScanReference((string)item.Value);
                }
                else if (key.Equals("merchantReportingCriteria"))
                {
                    _netverifySDK.SetMerchantReportingCriteria((string)item.Value);
                }
                else if (key.Equals("customerID"))
                {
                    _netverifySDK.SetCustomerId((string)item.Value);
                }
                else if (key.Equals("enableEpassport"))
                {
                    _netverifySDK.SetEnableEMRTD((bool)item.Value);
                }
                else if (key.Equals("sendDebugInfoToJumio"))
                {
                    _netverifySDK.SendDebugInfoToJumio((bool)item.Value);
                }
                else if (key.Equals("dataExtractionOnMobileOnly"))
                {
                    _netverifySDK.SetDataExtractionOnMobileOnly((bool)item.Value);
                }
                else if (key.Equals("cameraPosition"))
                {
                    JumioCameraPosition cameraPosition = (((string)item.Value).ToLower().Equals("front")) ? JumioCameraPosition.Front : JumioCameraPosition.Back;
                    _netverifySDK.SetCameraPosition(cameraPosition);
                }
                else if (key.Equals("preselectedDocumentVariant"))
                {
                    NVDocumentVariant variant = (((string)item.Value).ToLower().Equals("paper")) ? NVDocumentVariant.Paper : NVDocumentVariant.Plastic;
                    _netverifySDK.SetPreselectedDocumentVariant(variant);
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

                    _netverifySDK.SetPreselectedDocumentTypes(documentTypes);
                }
            }
        }

        private void StartNetverify()
        {
            if (_netverifySDK == null)
            {
                if (_jumioVerifyTask != null)
                {
                    _jumioVerifyTask.TrySetCanceled();
                }
                throw new Exception("The Netverify SDK is not initialized yet. Call Init() first.");
            }

            try
            {
                CheckPermissionsAndStart(_netverifySDK);
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
                if (sdk is NetverifySDK)
                {
                    code = PERMISSION_REQUEST_CODE_NETVERIFY;
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

        private static void SendErrorObject(string errorCode, string errorMsg, string scanReference)
        {
            var errorResult = new Dictionary<string, object>();
            errorResult.Add("errorCode", errorCode ?? "");
            errorResult.Add("errorMessage", errorMsg ?? "");
            errorResult.Add("scanReference", scanReference ?? "");
            SendEvent("EventError", errorResult);
        }

        private static void SendEvent(string eventName, Dictionary<string, object> result)
        {
            if(_jumioVerifyTask !=  null)
            {
                _jumioVerifyTask.TrySetResult(new JumioResult(eventName, result));
            }
        }
    }
}
