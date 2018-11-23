using System;
using NativeLibrary;
using Foundation;
using UIKit;
using System.Diagnostics;
using System.Globalization;
using CoreGraphics;

namespace JumioBindingiOS
{
    public class JumioMobileSDKNetverify : NetverifyViewControllerDelegate
    {
        NetverifyViewController _netverifyViewController;
        NetverifyConfiguration _netverifyConfiguration;

        public Action<string, NSMutableDictionary> JumioEventHandler { get; set; }

        #region NetverifyViewControllerDelegate

        public override void DidCancelWithError(NetverifyViewController netverifyViewController, NetverifyError error, string scanReference)
        {
            SendNetverifyError(error, scanReference);
        }

        public override void DidFinishWithDocumentData(NetverifyViewController netverifyViewController, NetverifyDocumentData documentData, string scanReference)
        {
            NSMutableDictionary result = new NSMutableDictionary();
            try
            {
                NSDateFormatter formatter = new NSDateFormatter();
                formatter.DateFormat = "yyyy-MM-dd'T'HH:mm:ss.SSS";
                if (!string.IsNullOrEmpty(documentData.SelectedCountry))
                    result.SetValueForKey(new NSString(documentData.SelectedCountry), new NSString("selectedCountry"));
                if (documentData.SelectedDocumentType == NetverifyDocumentType.Passport)
                {
                    result.SetValueForKey(new NSString("PASSPORT"), new NSString("selectedDocumentType"));
                }
                else if (documentData.SelectedDocumentType == NetverifyDocumentType.DriverLicense)
                {
                    result.SetValueForKey(new NSString("DRIVER_LICENSE"), new NSString("selectedDocumentType"));
                }
                else if (documentData.SelectedDocumentType == NetverifyDocumentType.IdentityCard)
                {
                    result.SetValueForKey(new NSString("IDENTITY_CARD"), new NSString("selectedDocumentType"));
                }
                else if (documentData.SelectedDocumentType == NetverifyDocumentType.Visa)
                {
                    result.SetValueForKey(new NSString("VISA"), new NSString("selectedDocumentType"));
                }
                if (!string.IsNullOrEmpty(documentData.IdNumber))
                    result.SetValueForKey(new NSString(documentData?.IdNumber), new NSString("idNumber"));

                if (!string.IsNullOrEmpty(documentData.PersonalNumber))
                    result.SetValueForKey(new NSString(documentData?.PersonalNumber), new NSString("personalNumber"));
                if (documentData.IssuingDate != null)
                    result.SetValueForKey(new NSString(formatter?.StringFor(documentData.IssuingDate)),
                    new NSString("issuingDate"));
                if (documentData.ExpiryDate != null)
                    result.SetValueForKey(new NSString(formatter?.StringFor(documentData.ExpiryDate)),
                    new NSString("expiryDate"));
                if (!string.IsNullOrEmpty(documentData.IssuingCountry))
                    result.SetValueForKey(new NSString(documentData?.IssuingCountry), new NSString("issuingCountry"));
                if (!string.IsNullOrEmpty(documentData.LastName))
                    result.SetValueForKey(new NSString(documentData?.LastName), new NSString("lastName"));
                if (!string.IsNullOrEmpty(documentData.FirstName))
                    result.SetValueForKey(new NSString(documentData?.FirstName), new NSString("firstName"));
                if (documentData.Dob != null)
                    result.SetValueForKey(new NSString(formatter.StringFor(documentData?.Dob)), new NSString("dob"));
                if (documentData.Gender == NetverifyGender.M)
                {
                    result.SetValueForKey(new NSString("m"), new NSString("gender"));
                }
                else if (documentData.Gender == NetverifyGender.F)
                {
                    result.SetValueForKey(new NSString("f"), new NSString("gender"));
                }
                else if (documentData.Gender == NetverifyGender.X)
                {
                    result.SetValueForKey(new NSString("x"), new NSString("gender"));
                }
                if (!string.IsNullOrEmpty(documentData.OriginatingCountry))
                    result.SetValueForKey(new NSString(documentData?.OriginatingCountry),
                    new NSString("originatingCountry"));
                if (!string.IsNullOrEmpty(documentData.AddressLine))
                    result.SetValueForKey(new NSString(documentData?.AddressLine), new NSString("addressLine"));
                if (!string.IsNullOrEmpty(documentData.City))
                    result.SetValueForKey(new NSString(documentData?.City), new NSString("city"));
                if (!string.IsNullOrEmpty(documentData.Subdivision))
                    result.SetValueForKey(new NSString(documentData?.Subdivision), new NSString("subdivision"));
                if (!string.IsNullOrEmpty(documentData.PostCode))
                    result.SetValueForKey(new NSString(documentData?.PostCode), new NSString("postCode"));
                if (!string.IsNullOrEmpty(documentData.OptionalData1))
                    result.SetValueForKey(new NSString(documentData?.OptionalData1), new NSString("optionalData1"));
                if (!string.IsNullOrEmpty(documentData.OptionalData2))
                    result.SetValueForKey(new NSString(documentData?.OptionalData2), new NSString("optionalData2"));
                if (documentData.ExtractionMethod == NetverifyExtractionMethod.Mrz)
                {
                    result.SetValueForKey(new NSString("MRZ"), new NSString("extractionMethod"));
                }
                else if (documentData.ExtractionMethod == NetverifyExtractionMethod.Ocr)
                {
                    result.SetValueForKey(new NSString("OCR"), new NSString("extractionMethod"));
                }
                else if (documentData.ExtractionMethod == NetverifyExtractionMethod.Barcode)
                {
                    result.SetValueForKey(new NSString("BARCODE"), new NSString("extractionMethod"));
                }
                else if (documentData.ExtractionMethod == NetverifyExtractionMethod.BarcodeOCR)
                {
                    result.SetValueForKey(new NSString("BARCODE_OCR"), new NSString("extractionMethod"));
                }
                else if (documentData.ExtractionMethod == NetverifyExtractionMethod.None)
                {
                    result.SetValueForKey(new NSString("NONE"), new NSString("extractionMethod"));
                }

                // MRZ data if available
                if (documentData.MrzData != null)
                {
                    NSMutableDictionary mrzData = new NSMutableDictionary();
                    if (documentData.MrzData.Format == NetverifyMRZFormat.Mrp)
                    {
                        mrzData.SetValueForKey(new NSString("MRP"), new NSString("format"));
                    }
                    else if (documentData.MrzData.Format == NetverifyMRZFormat.Td1)
                    {
                        mrzData.SetValueForKey(new NSString("TD1"), new NSString("format"));
                    }
                    else if (documentData.MrzData.Format == NetverifyMRZFormat.Td2)
                    {
                        mrzData.SetValueForKey(new NSString("TD2"), new NSString("format"));
                    }
                    else if (documentData.MrzData.Format == NetverifyMRZFormat.Cnis)
                    {
                        mrzData.SetValueForKey(new NSString("CNIS"), new NSString("format"));
                    }
                    else if (documentData.MrzData.Format == NetverifyMRZFormat.Mrva)
                    {
                        mrzData.SetValueForKey(new NSString("MRVA"), new NSString("format"));
                    }
                    else if (documentData.MrzData.Format == NetverifyMRZFormat.Mrvb)
                    {
                        mrzData.SetValueForKey(new NSString("MRVB"), new NSString("format"));
                    }
                    else if (documentData.MrzData.Format == NetverifyMRZFormat.Unknown)
                    {
                        mrzData.SetValueForKey(new NSString("UNKNOWN"), new NSString("format"));
                    }

                    if (!string.IsNullOrEmpty(documentData.MrzData.Line1))
                        mrzData.SetValueForKey(new NSString(documentData.MrzData.Line1), new NSString("line1"));
                    if (!string.IsNullOrEmpty(documentData.MrzData.Line2))
                        mrzData.SetValueForKey(new NSString(documentData.MrzData.Line2), new NSString("line2"));
                    if (!string.IsNullOrEmpty(documentData.MrzData.Line3))
                        mrzData.SetValueForKey(new NSString(documentData.MrzData.Line3), new NSString("line3"));
                    mrzData.SetValueForKey(NSNumber.FromBoolean(documentData.MrzData.IdNumberValid),
                        new NSString("idNumberValid"));
                    mrzData.SetValueForKey(NSNumber.FromBoolean(documentData.MrzData.DobValid),
                        new NSString("dobValid"));
                    mrzData.SetValueForKey(NSNumber.FromBoolean(documentData.MrzData.ExpiryDateValid),
                        new NSString("expiryDateValid"));
                    mrzData.SetValueForKey(NSNumber.FromBoolean(documentData.MrzData.PersonalNumberValid),
                        new NSString("personalNumberValid"));
                    mrzData.SetValueForKey(NSNumber.FromBoolean(documentData.MrzData.CompositeValid),
                        new NSString("compositeValid"));
                    mrzData.SetValueForKey(mrzData, new NSString("mrzData"));
                }

                result.SetValueForKey(new NSString(scanReference), new NSString("scanReference"));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                AppDelegate del = (AppDelegate)UIApplication.SharedApplication.Delegate;
                del.Window.RootViewController.DismissViewController(true, () => {
                    JumioEventHandler?.Invoke("EventDocumentData", result);
                });
            }


        }

        #endregion

        void SendNetverifyError(NetverifyError error, string scanReference)
        {
            NSMutableDictionary result = new NSMutableDictionary();
            result.SetValueForKey(new NSString(error.Code), new NSString("errorCode"));
            result.SetValueForKey(new NSString(error.Message), new NSString("errorMessage"));
            if (scanReference != null)
            {
                result.SetValueForKey(new NSString(scanReference), new NSString("scanReference"));
            }

            AppDelegate del = (AppDelegate)UIApplication.SharedApplication.Delegate;
            del.Window.RootViewController.DismissViewController(true, () => {
                JumioEventHandler?.Invoke("EventError", result);
            });
        }

        public void InitNetverify(string apiToken, string apiSecret, string dataCenter, NSDictionary options = null, NSDictionary customization = null)
        {
            if (_netverifyViewController != null)
            {
                _netverifyViewController.Destroy();
            }

            // Initialization
            _netverifyConfiguration = new NetverifyConfiguration();
            _netverifyConfiguration.Delegate = this;
            _netverifyConfiguration.MerchantApiToken = apiToken;
            _netverifyConfiguration.MerchantApiSecret = apiSecret;
            _netverifyConfiguration.DataCenter = dataCenter.ToLower() == "eu" ? JumioDataCenter.Eu : JumioDataCenter.Us;

            // Configuration
            if (options != null)
            {
                foreach (NSString key in options.Keys)
                {
                    if (key == "requireVerification")
                    {
                        _netverifyConfiguration.RequireVerification = GetBoolValue(options.ObjectForKey(key));
                    }
                    else if (key == "callbackUrl")
                    {
                        _netverifyConfiguration.CallbackUrl = options.ObjectForKey(key).ToString();
                    }
                    else if (key == "requireFaceMatch")
                    {
                        _netverifyConfiguration.RequireFaceMatch = GetBoolValue(options.ObjectForKey(key));
                    }
                    else if (key == "preselectedCountry")
                    {
                        _netverifyConfiguration.PreselectedCountry = options.ObjectForKey(key).ToString();
                    }
                    else if (key == "merchantScanReference")
                    {
                        _netverifyConfiguration.MerchantScanReference = options.ObjectForKey(key).ToString();
                    }
                    else if (key == "merchantReportingCriteria")
                    {
                        _netverifyConfiguration.MerchantReportingCriteria = options.ObjectForKey(key).ToString();
                    }
                    else if (key == "customerId")
                    {
                        _netverifyConfiguration.CustomerId = options.ObjectForKey(key).ToString();
                    }
                    else if (key == "sendDebugInfoToJumio")
                    {
                        _netverifyConfiguration.SendDebugInfoToJumio = GetBoolValue(options.ObjectForKey(key));
                    }
                    else if (key == "dataExtractionOnMobileOnly")
                    {
                        _netverifyConfiguration.DataExtractionOnMobileOnly = GetBoolValue(options.ObjectForKey(key));
                    }
                    else if (key == "cameraPosition")
                    {
                        var cameraString = options.ObjectForKey(key).ToString().ToLower();
                        JumioCameraPosition cameraPosition = cameraString == "front" ? JumioCameraPosition.Front : JumioCameraPosition.Back;
                        _netverifyConfiguration.CameraPosition = cameraPosition;
                    }
                    else if (key == "preselectedDocumentVariant")
                    {
                        var variantString = options.ObjectForKey(key).ToString().ToLower();
                        NetverifyDocumentVariant variant = variantString == "paper" ? NetverifyDocumentVariant.Paper : NetverifyDocumentVariant.Plastic;
                        _netverifyConfiguration.PreselectedDocumentVariant = variant;
                    }
                    else if (key == "documentTypes")
                    {
                        NSMutableArray jsonTypes = (NSMutableArray)options.ObjectForKey(key);
                        NetverifyDocumentType documentTypes = 0;

                        nuint i;
                        for (i = 0; i < jsonTypes.Count; i++)
                        {
                            string type = jsonTypes.GetItem<NSString>(i);

                            if (type.ToLower() == "passport")
                            {
                                documentTypes = documentTypes | NetverifyDocumentType.Passport;
                            }
                            else if (type.ToLower() == "driver_license")
                            {
                                documentTypes = documentTypes | NetverifyDocumentType.DriverLicense;
                            }
                            else if (type.ToLower() == "identity_card")
                            {
                                documentTypes = documentTypes | NetverifyDocumentType.IdentityCard;
                            }
                            else if (type.ToLower() == "visa")
                            {
                                documentTypes = documentTypes | NetverifyDocumentType.Visa;
                            }
                        }

                        _netverifyConfiguration.PreselectedDocumentTypes = documentTypes;
                    }
                }
            }

            _netverifyViewController = new NetverifyViewController(_netverifyConfiguration);
        }

        public void StartNetverify()
        {
            if (_netverifyViewController == null)
            {
                Debug.WriteLine(@"The Netverify SDK is not initialized yet. Call InitNetverify() first.");
                return;
            }
            AppDelegate del = (AppDelegate)UIApplication.SharedApplication.Delegate;
            del.Window.RootViewController.PresentViewController(_netverifyViewController, true, null);
        }

        public static bool GetBoolValue(NSObject value)
        {
            if (value != null && value is NSNumber)
            {
                return ((NSNumber)value).BoolValue;
            }
            return (bool)ToObject<bool>(value);
        }

        public static object ToObject<T>(NSObject nsO)
        {
            var targetType = typeof(T);

            if (nsO is NSString)
            {
                return nsO.ToString();
            }

            if (nsO is NSDate)
            {
                var nsDate = (NSDate)nsO;
                return DateTime.SpecifyKind((DateTime)nsDate, DateTimeKind.Unspecified);
            }

            if (nsO is NSDecimalNumber)
            {
                return decimal.Parse(nsO.ToString(), CultureInfo.InvariantCulture);
            }

            if (nsO is NSNumber)
            {
                var x = (NSNumber)nsO;

                switch (Type.GetTypeCode(targetType))
                {
                    case TypeCode.Boolean:
                        return x.BoolValue;
                    case TypeCode.Char:
                        return Convert.ToChar(x.ByteValue);
                    case TypeCode.SByte:
                        return x.SByteValue;
                    case TypeCode.Byte:
                        return x.ByteValue;
                    case TypeCode.Int16:
                        return x.Int16Value;
                    case TypeCode.UInt16:
                        return x.UInt16Value;
                    case TypeCode.Int32:
                        return x.Int32Value;
                    case TypeCode.UInt32:
                        return x.UInt32Value;
                    case TypeCode.Int64:
                        return x.Int64Value;
                    case TypeCode.UInt64:
                        return x.UInt64Value;
                    case TypeCode.Single:
                        return x.FloatValue;
                    case TypeCode.Double:
                        return x.DoubleValue;
                }
            }

            if (nsO is NSValue)
            {
                var v = (NSValue)nsO;

                if (targetType == typeof(IntPtr))
                {
                    return v.PointerValue;
                }

                if (targetType == typeof(CGSize))
                {
                    return v.SizeFValue;
                }

                if (targetType == typeof(CGRect))
                {
                    return v.RectangleFValue;
                }

                if (targetType == typeof(CGPoint))
                {
                    return v.PointFValue;
                }
            }

            return nsO;
        }
    }
}