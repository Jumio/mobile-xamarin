using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using JumioForms.Abstractions;
using JumioForms.iOS.Extensions;
using NativeLibrary;
using UIKit;

namespace JumioForms
{
    public class JumioNetverify : NetverifyViewControllerDelegate, INetverify
    {
        NetverifyViewController _netverifyViewController;
        NetverifyConfiguration _netverifyConfiguration;

        TaskCompletionSource<JumioResult> _jumioVerifyTask;

        public JumioNetverify()
        {
        }

        #region NetverifyViewControllerDelegate

        public override void DidCancelWithError(NetverifyViewController netverifyViewController, NetverifyError error, string scanReference)
        {
            SendNetverifyError(error, scanReference);
        }

        public override void DidFinishWithDocumentData(NetverifyViewController netverifyViewController, NetverifyDocumentData documentData, string scanReference)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                NSDateFormatter formatter = new NSDateFormatter
                {
                    DateFormat = "yyyy-MM-dd'T'HH:mm:ss.SSS"
                };
                if (!string.IsNullOrEmpty(documentData.SelectedCountry))
                {
                    result.Add("selectedCountry", documentData.SelectedCountry);
                }
                if (documentData.SelectedDocumentType == NetverifyDocumentType.Passport)
                {
                    result.Add("selectedDocumentType","PASSPORT");
                }
                else if (documentData.SelectedDocumentType == NetverifyDocumentType.DriverLicense)
                {
                    result.Add("selectedDocumentType", "DRIVER_LICENSE");
                }
                else if (documentData.SelectedDocumentType == NetverifyDocumentType.IdentityCard)
                {
                    result.Add("selectedDocumentType", "IDENTITY_CARD");
                }
                else if (documentData.SelectedDocumentType == NetverifyDocumentType.Visa)
                {
                    result.Add("selectedDocumentType", "VISA");
                }
                if (!string.IsNullOrEmpty(documentData.IdNumber))
                {
                    result.Add("idNumber", documentData?.IdNumber);
                }

                if (!string.IsNullOrEmpty(documentData.PersonalNumber))
                {
                    result.Add("personalNumber", documentData?.PersonalNumber);
                }
                if (documentData.IssuingDate != null)
                {
                    result.Add("issuingDate", formatter?.StringFor(documentData.IssuingDate));
                }
                if (documentData.ExpiryDate != null)
                {
                    result.Add("expiryDate", formatter?.StringFor(documentData.ExpiryDate));
                }
                if (!string.IsNullOrEmpty(documentData.IssuingCountry))
                {
                    result.Add("issuingCountry", documentData?.IssuingCountry);
                }
                if (!string.IsNullOrEmpty(documentData.LastName))
                {
                    result.Add("lastName", documentData?.LastName);
                }
                if (!string.IsNullOrEmpty(documentData.FirstName))
                {
                    result.Add("firstName", documentData?.FirstName);
                }
                if (documentData.Dob != null)
                {
                    result.Add("dob", documentData?.Dob);
                }
                if (documentData.Gender == NetverifyGender.M)
                {
                    result.Add("gender", "m");
                }
                else if (documentData.Gender == NetverifyGender.F)
                {
                    result.Add("gender", "f");
                }
                else if (documentData.Gender == NetverifyGender.X)
                {
                    result.Add("gender", "x");
                }
                if (!string.IsNullOrEmpty(documentData.OriginatingCountry))
                {
                    result.Add("originatingCountry", documentData?.OriginatingCountry);
                }
                if (!string.IsNullOrEmpty(documentData.AddressLine))
                {
                    result.Add("addressLine", documentData?.AddressLine);
                }
                if (!string.IsNullOrEmpty(documentData.City))
                {
                    result.Add("city", documentData?.City);
                }
                if (!string.IsNullOrEmpty(documentData.Subdivision))
                {
                    result.Add("subdivision", documentData?.Subdivision);
                }
                if (!string.IsNullOrEmpty(documentData.PostCode))
                {
                    result.Add("postCode", documentData?.PostCode);
                }
                if (!string.IsNullOrEmpty(documentData.OptionalData1))
                {
                    result.Add("optionalData1", documentData?.OptionalData1);
                }
                if (!string.IsNullOrEmpty(documentData.OptionalData2))
                {
                    result.Add("optionalData2", documentData?.OptionalData2);
                }
                if (documentData.ExtractionMethod == NetverifyExtractionMethod.Mrz)
                {
                    result.Add("extractionMethod", "MRZ");
                }
                else if (documentData.ExtractionMethod == NetverifyExtractionMethod.Ocr)
                {
                    result.Add("extractionMethod", "OCR");
                }
                else if (documentData.ExtractionMethod == NetverifyExtractionMethod.Barcode)
                {
                    result.Add("extractionMethod", "BARCODE");
                }
                else if (documentData.ExtractionMethod == NetverifyExtractionMethod.BarcodeOCR)
                {
                    result.Add("extractionMethod", "BARCODE_OCR");
                }
                else if (documentData.ExtractionMethod == NetverifyExtractionMethod.None)
                {
                    result.Add("extractionMethod", "NONE");
                }

                // MRZ data if available
                if (documentData.MrzData != null)
                {
                    Dictionary<string, object> mrzData = new Dictionary<string, object>();
                    if (documentData.MrzData.Format == NetverifyMRZFormat.Mrp)
                    {
                        mrzData.Add("format", "MRP");
                    }
                    else if (documentData.MrzData.Format == NetverifyMRZFormat.Td1)
                    {
                        mrzData.Add("format", "TD1");
                    }
                    else if (documentData.MrzData.Format == NetverifyMRZFormat.Td2)
                    {
                        mrzData.Add("format", "TD2");
                    }
                    else if (documentData.MrzData.Format == NetverifyMRZFormat.Cnis)
                    {
                        mrzData.Add("format", "CNIS");
                    }
                    else if (documentData.MrzData.Format == NetverifyMRZFormat.Mrva)
                    {
                        mrzData.Add("format", "MRVA");
                    }
                    else if (documentData.MrzData.Format == NetverifyMRZFormat.Mrvb)
                    {
                        mrzData.Add("format", "MRVB");
                    }
                    else if (documentData.MrzData.Format == NetverifyMRZFormat.Unknown)
                    {
                        mrzData.Add("format", "UNKNOWN");
                    }

                    if (!string.IsNullOrEmpty(documentData.MrzData.Line1))
                    {
                        mrzData.Add("line1", documentData.MrzData.Line1);
                    }
                    if (!string.IsNullOrEmpty(documentData.MrzData.Line2))
                    {
                        mrzData.Add("line2", documentData.MrzData.Line2);
                    }
                    if (!string.IsNullOrEmpty(documentData.MrzData.Line3))
                        {
                            mrzData.Add("line3", documentData.MrzData.Line3);
                        }

                    mrzData.Add("idNumberValid", documentData.MrzData.IdNumberValid);
                    mrzData.Add("dobValid", documentData.MrzData.DobValid);
                    mrzData.Add("expiryDateValid", documentData.MrzData.ExpiryDateValid);
                    mrzData.Add("personalNumberValid", documentData.MrzData.PersonalNumberValid);
                    mrzData.Add("compositeValid", documentData.MrzData.CompositeValid);


                    result.Add("mrzData", mrzData);
                }
                result.Add("scanReference", scanReference);
            }
            catch (Exception ex)
            {
                if (_jumioVerifyTask != null)
                {
                    _jumioVerifyTask.TrySetCanceled();
                }
                throw new Exception(ex.ToString());
            }
            finally
            {
                var top = GetTopViewController(UIApplication.SharedApplication.KeyWindow);
                top.DismissViewController(true, () => {
                    if (_jumioVerifyTask != null)
                    {
                        _jumioVerifyTask.TrySetResult(new JumioResult("EventDocumentData", result));
                    }
                });
            }
        }

        #endregion

        #region INetverify

        public void Init(string apiToken, string apiSecret, string dataCenter, IDictionary<string, object> options, IDictionary<string, object> customization = null)
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
                foreach (var item in options)
                {
                    if (item.Key == "requireVerification")
                    {
                        _netverifyConfiguration.RequireVerification = GetBoolValue(item.Value);
                    }
                    else if (item.Key == "callbackUrl")
                    {
                        _netverifyConfiguration.CallbackUrl = item.Value.ToString();
                    }
                    else if (item.Key == "requireFaceMatch")
                    {
                        _netverifyConfiguration.RequireFaceMatch = GetBoolValue(item.Value);
                    }
                    else if (item.Key == "preselectedCountry")
                    {
                        _netverifyConfiguration.PreselectedCountry = item.Value.ToString();
                    }
                    else if (item.Key == "merchantScanReference")
                    {
                        _netverifyConfiguration.MerchantScanReference = item.Value.ToString();
                    }
                    else if (item.Key == "merchantReportingCriteria")
                    {
                        _netverifyConfiguration.MerchantReportingCriteria = item.Value.ToString();
                    }
                    else if (item.Key == "customerId")
                    {
                        _netverifyConfiguration.CustomerId = item.Value.ToString();
                    }
                    else if (item.Key == "sendDebugInfoToJumio")
                    {
                        _netverifyConfiguration.SendDebugInfoToJumio = GetBoolValue(item.Value);
                    }
                    else if (item.Key == "dataExtractionOnMobileOnly")
                    {
                        _netverifyConfiguration.DataExtractionOnMobileOnly = GetBoolValue(item.Value);
                    }
                    else if (item.Key == "cameraPosition")
                    {
                        var cameraString = item.Value.ToString().ToLower();
                        JumioCameraPosition cameraPosition = cameraString == "front" ? JumioCameraPosition.Front : JumioCameraPosition.Back;
                        _netverifyConfiguration.CameraPosition = cameraPosition;
                    }
                    else if (item.Key == "preselectedDocumentVariant")
                    {
                        var variantString = item.Value.ToString().ToLower();
                        NetverifyDocumentVariant variant = variantString == "paper" ? NetverifyDocumentVariant.Paper : NetverifyDocumentVariant.Plastic;
                        _netverifyConfiguration.PreselectedDocumentVariant = variant;
                    }
                    else if (item.Key == "documentTypes")
                    {
                        IList<string> jsonTypes = (IList<string>)item.Value;
                        NetverifyDocumentType documentTypes = 0;

                        int i;
                        for (i = 0; i < jsonTypes.Count; i++)
                        {
                            string type = jsonTypes[i];

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

            // Customization
            if (customization != null)
            {
                foreach (var item in customization)
                {
                    if (item.Key == "disableBlur")
                    {
                        NetverifyBaseView.NetverifyAppearance().DisableBlur = true;
                    }
                    else
                    {
                        UIColor color = UIColor.Clear.FromHexString(item.Value.ToString());

                        if (item.Key == "backgroundColor")
                        {
                            NetverifyBaseView.NetverifyAppearance().BackgroundColor = color;
                        }
                        else if (item.Key == "tintColor")
                        {
                            (null as UINavigationBar).NetverifyAppearance().TintColor = color;
                        }
                        else if (item.Key == "barTintColor")
                        {
                            (null as UINavigationBar).NetverifyAppearance().TintColor = color;
                        }
                        else if (item.Key == "textTitleColor")
                        {
                            (null as UINavigationBar).NetverifyAppearance().TitleTextAttributes = new UIStringAttributes { ForegroundColor = color };
                        }
                        else if (item.Key == "foregroundColor")
                        {
                            NetverifyBaseView.NetverifyAppearance().ForegroundColor = color;
                        }
                        else if (item.Key == "documentSelectionHeaderBackgroundColor")
                        {
                            NetverifyDocumentSelectionHeaderView.NetverifyAppearance().BackgroundColor = color;
                        }
                        else if (item.Key == "documentSelectionHeaderTitleColor")
                        {
                            NetverifyDocumentSelectionHeaderView.NetverifyAppearance().TitleColor = color;
                        }
                        else if (item.Key == "documentSelectionHeaderIconColor")
                        {
                            NetverifyDocumentSelectionHeaderView.NetverifyAppearance().IconColor = color;
                        }
                        else if (item.Key == "documentSelectionButtonBackgroundColor")
                        {
                            NetverifyDocumentSelectionButton.NetverifyAppearance().SetBackgroundColor(color, UIControlState.Normal);
                        }
                        else if (item.Key == "documentSelectionButtonTitleColor")
                        {
                            NetverifyDocumentSelectionButton.NetverifyAppearance().SetTitleColor(color, UIControlState.Normal);
                        }
                        else if (item.Key == "documentSelectionButtonIconColor")
                        {
                            NetverifyDocumentSelectionButton.NetverifyAppearance().SetIconColor(color, UIControlState.Normal);
                        }
                        else if (item.Key == "fallbackButtonBackgroundColor")
                        {
                            NetverifyFallbackButton.NetverifyAppearance().SetBackgroundColor(color, UIControlState.Normal);
                        }
                        else if (item.Key == "fallbackButtonBorderColor")
                        {
                            NetverifyFallbackButton.NetverifyAppearance().BorderColor = color;
                        }
                        else if (item.Key == "fallbackButtonTitleColor")
                        {
                            NetverifyFallbackButton.NetverifyAppearance().SetTitleColor(color, UIControlState.Normal);
                        }
                        else if (item.Key == "positiveButtonBackgroundColor")
                        {
                            NetverifyPositiveButton.NetverifyAppearance().SetBackgroundColor(color, UIControlState.Normal);
                        }
                        else if (item.Key == "positiveButtonBorderColor")
                        {
                            NetverifyPositiveButton.NetverifyAppearance().BorderColor = color;
                        }
                        else if (item.Key == "positiveButtonTitleColor")
                        {
                            NetverifyPositiveButton.NetverifyAppearance().SetTitleColor(color, UIControlState.Normal);
                        }
                        else if (item.Key == "negativeButtonBackgroundColor")
                        {
                            NetverifyNegativeButton.NetverifyAppearance().SetBackgroundColor(color, UIControlState.Normal);
                        }
                        else if (item.Key == "negativeButtonBorderColor")
                        {
                            NetverifyNegativeButton.NetverifyAppearance().BorderColor = color;
                        }
                        else if (item.Key == "negativeButtonTitleColor")
                        {
                            NetverifyNegativeButton.NetverifyAppearance().SetTitleColor(color, UIControlState.Normal);
                        }
                    }
                }
            }

            _netverifyViewController = new NetverifyViewController(_netverifyConfiguration);
        }

        public Task<JumioResult> VerifyAsync()
        {
            if (_netverifyViewController == null)
            {
                throw new Exception(@"The Netverify SDK is not initialized yet. Call InitNetverifyHelper() first.");
            }

            if (_jumioVerifyTask != null)
            {
                _jumioVerifyTask.TrySetCanceled();
            }

            _jumioVerifyTask = new TaskCompletionSource<JumioResult>();

            var top = GetTopViewController(UIApplication.SharedApplication.KeyWindow);
            top.PresentViewController(_netverifyViewController, true, null);

            return _jumioVerifyTask.Task;
        }

        #endregion

        void SendNetverifyError(NetverifyError error, string scanReference)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("errorCode", error.Code);
            result.Add("errorMessage", error.Message);
            if (scanReference != null)
            {
                result.Add("scanReference", scanReference);
            }

            var top = GetTopViewController(UIApplication.SharedApplication.KeyWindow);
            top.DismissViewController(true, () => {
                if (_jumioVerifyTask != null)
                {
                    _jumioVerifyTask.TrySetResult(new JumioResult("EventError", result));
                }
            });
        }

        UIViewController GetTopViewController(UIWindow window)
        {
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
            {
                vc = vc.PresentedViewController;
            }
            var navController = vc as UINavigationController;
            if (navController != null)
            {
                vc = navController.ViewControllers.Last();
            }
            return vc;
        }

        public static bool GetBoolValue(object value)
        {
            if (value != null && value is NSNumber)
            {
                return ((NSNumber)value).BoolValue;
            }
            return (bool)ToObject<bool>(value);
        }

        public static object ToObject<T>(object nsO)
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
