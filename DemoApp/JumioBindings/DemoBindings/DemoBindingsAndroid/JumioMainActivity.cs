using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Widget;
using Com.Jumio;
using Com.Jumio.Core.Exceptions;
using Com.Jumio.DV;
using Com.Jumio.NV;
using Com.Jumio.NV.Data.Document;
using Com.Jumio.NV.Enums;
using System.Collections.Generic;
using System.Linq;
using Android.Util;
using Java.IO;
using DemoBindingsAndroid;

namespace JumioBindingAndroid
{
    [Activity(Label = "JumioBindingAndroid",
              ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout | ConfigChanges.KeyboardHidden,
              MainLauncher = true,
              Theme = "@style/MyTheme",
              Icon = "@mipmap/icon")]
    public class JumioMainActivity : AppCompatActivity
    {
        protected JumioModuleNetverify _NVmanager;
        protected JumioModuleDocumentVerification _DVmanager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _NVmanager = new JumioModuleNetverify(this);
            _DVmanager = new JumioModuleDocumentVerification(this);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button NvButton = FindViewById<Button>(Resource.Id.myButton);
            Button DvButton = FindViewById<Button>(Resource.Id.DBButton);

            NvButton.Click += NvButton_Click;
            DvButton.Click += DvButton_Click;
        }

        void NvButton_Click(object sender, System.EventArgs e)
        {
            var settings = new Dictionary<string, object>
            {
                {"requireVerification", true}
               //,{"callbackUrl", "URL"}
               //,{"preselectedCountry", "USA"}
               //,{"merchantScanReference", "123456789"}
               //,{"merchantReportingCriteria", "Criteria"}
               //,{"customerId", "ID"}
               //,{"sendDebugInfoToJumio", true}
               //,{"dataExtractionOnMobileOnly", false}
               //,{"cameraPosition", "back"}
               //,{"preselectedDocumentVariant", "plastic"}
               //,{"documentTypes", new List<string>() {"passport", "driver_license", "identity_card", "visa"}}
            };

            _NVmanager.InitNetverify("API_TOKEN", "API_SECRET", "DATACENTER", settings);
            _NVmanager.StartNetverify();

            _NVmanager.EventWithNameHandler -= Manager_EventWithNameHandler;
            _NVmanager.EventWithNameHandler += Manager_EventWithNameHandler;
        }
        void DvButton_Click(object sender, System.EventArgs e)
        {
            var settings = new Dictionary<string, object>
            {
                {"type", "BC"}
                ,{"customerId", "123456789"}
                ,{"country", "USA"}
                ,{"merchantScanReference", "123456789"}
                //,{"merchantScanReportingCriteria", "Criteria"}
                //,{"callbackUrl", "URL"}
                //,{"documentName", "Name"}
                //,{"customDocumentCode", "Custom"}
                //,{"cameraPosition", "back"}
                //,{"enableExtraction", true}
            };

            _DVmanager.InitDocumentVerification("API_TOKEN", "API_SECRET", "DATACENTER", settings);
            _DVmanager.StartDocumentVerification(); ;

            _DVmanager.EventWithNameHandler -= Manager_EventWithNameHandler;
            _DVmanager.EventWithNameHandler += Manager_EventWithNameHandler;
        }



        void Manager_EventWithNameHandler(string message, Dictionary<string, object> result)
        {
            _NVmanager.EventWithNameHandler -= Manager_EventWithNameHandler;

            if (message == "EventError")
            {
                //TODO: Review Result
            }
            else if (message == "EventDocumentData")
            {
                string stringResult = string.Join(";", result.Select(x => x.Key + "=" + x.Value).ToArray());
                Log.Info("Result",stringResult);
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

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

                    _NVmanager.SendEvent("EventDocumentData", result);
                }
                else if (resultCode == Result.Canceled)
                {
                    var errorMessage = data.GetStringExtra(NetverifySDK.ExtraErrorMessage);
                    var errorCode = data.GetStringExtra(NetverifySDK.ExtraErrorCode);
                    SendErrorObject(errorCode, errorMessage, scanReference);
                }
                if (JumioModuleNetverify.netverifySDK != null)
                {
                    JumioModuleNetverify.netverifySDK.Destroy();
                }
            }
            else if (requestCode == DocumentVerificationSDK.RequestCode)
            {
                if (data == null)
                {
                    return;
                }

                if (resultCode == Result.Ok)
                {
                    //TODO: Do something
                }
                else if (resultCode == Result.Canceled)
                {
                    var scanReference = data.GetStringExtra(DocumentVerificationSDK.ExtraScanReference);
                    var errorMessage = data.GetStringExtra(DocumentVerificationSDK.ExtraErrorMessage);
                    var errorCode = data.GetStringExtra(DocumentVerificationSDK.ExtraErrorCode);
                }
                if (JumioModuleNetverify.netverifySDK != null)
                {
                    JumioModuleNetverify.netverifySDK.Destroy();
                }
            }
        }

        private void SendErrorObject(string errorCode, string errorMsg, string scanReference)
        {
            var errorResult = new Dictionary<string, object>();
            errorResult.Add("errorCode", errorCode != null ? errorCode : "");
            errorResult.Add("errorMessage", errorMsg != null ? errorMsg : "");
            errorResult.Add("scanReference", scanReference != null ? scanReference : "");
            _NVmanager.SendEvent("EventError", errorResult);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
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
                if (requestCode == JumioModuleNetverify.PERMISSION_REQUEST_CODE_NETVERIFY)
                {
                    StartSdk(JumioModuleNetverify.netverifySDK);
                }
                else
                {
                    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                }
            }
            else
            {
                Toast.MakeText(this, "You need to grant all required permissions to start the Jumio SDK", ToastLength.Long).Show();
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }

        public void StartSdk(MobileSDK sdk)
        {
            try
            {
                sdk.Start();
            }
            catch (MissingPermissionException e)
            {
                _NVmanager.ShowErrorMessage(e.LocalizedMessage);
            }
        }
    }
}

