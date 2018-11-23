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
    public class JumioDocumentVerification : DocumentVerificationViewControllerDelegate, IDocumentverification
    {
        DocumentVerificationViewController _documentVerificationViewController;
        DocumentVerificationConfiguration _documentVerifcationConfiguration;

        TaskCompletionSource<DocumentResult> _jumioVerifyTask;

        public JumioDocumentVerification()
        {
        }

        #region DocumentVerificationViewControllerDelegate

        public override void DidFinishWithError(DocumentVerificationViewController documentVerificationViewController, DocumentVerificationError error)
        {
            var top = GetTopViewController(UIApplication.SharedApplication.KeyWindow);
            top.DismissViewController(true, () => {
                if (_jumioVerifyTask != null)
                {
                    _jumioVerifyTask.TrySetResult(new DocumentResult(false, "", error.Message, error.Code));
                }
            });
        }

        public override void DidFinishWithScanReference(DocumentVerificationViewController documentVerificationViewController, string scanReference)
        {
            var top = GetTopViewController(UIApplication.SharedApplication.KeyWindow);
            top.DismissViewController(true, () => {
                if (_jumioVerifyTask != null)
                {
                    _jumioVerifyTask.TrySetResult(new DocumentResult(true, scanReference, "", ""));
                }
            });
        }

        #endregion

        #region IDocumentverify

        public void Init(string apiToken, string apiSecret, string dataCenter, IDictionary<string, object> options, IDictionary<string, object> customization = null)
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
                        _documentVerifcationConfiguration.Type = options[key].ToString();
                    }
                    else if (key == "customDocumentCode")
                    {
                        _documentVerifcationConfiguration.CustomDocumentCode = options[key].ToString();
                    }
                    else if (key == "country")
                    {
                        _documentVerifcationConfiguration.Country = options[key].ToString();
                    }
                    else if (key == "merchantReportingCriteria")
                    {
                        _documentVerifcationConfiguration.MerchantReportingCriteria = options[key].ToString();
                    }
                    else if (key == "callbackUrl")
                    {
                        _documentVerifcationConfiguration.CallbackUrl = options[key].ToString();
                    }
                    else if (key == "merchantScanReference")
                    {
                        _documentVerifcationConfiguration.MerchantScanReference = options[key].ToString();
                    }
                    else if (key == "customerId")
                    {
                        _documentVerifcationConfiguration.CustomerId = options[key].ToString();
                    }
                    else if (key == "documentName")
                    {
                        _documentVerifcationConfiguration.DocumentName = options[key].ToString();
                    }
                    else if (key == "enableExtraction")
                    {
                        _documentVerifcationConfiguration.EnableExtraction = GetBoolValue(options[key]);
                    }
                    else if (key == "cameraPosition")
                    {
                        var cameraString = options[key].ToString().ToLower();
                        JumioCameraPosition cameraPosition = cameraString == "front" ? JumioCameraPosition.Front : JumioCameraPosition.Back;
                        _documentVerifcationConfiguration.CameraPosition = cameraPosition;
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
                            (null as UINavigationBar).NetverifyAppearance().BarTintColor = color;
                        }
                        else if (item.Key == "textTitleColor")
                        {
                            (null as UINavigationBar).NetverifyAppearance().TitleTextAttributes = new UIStringAttributes { ForegroundColor = color };
                        }
                        else if (item.Key == "foregroundColor")
                        {
                            NetverifyBaseView.NetverifyAppearance().ForegroundColor = color;
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

            _documentVerificationViewController = new DocumentVerificationViewController(_documentVerifcationConfiguration);
        }

        public Task<DocumentResult> VerifyAsync()
        {
            if (_documentVerificationViewController == null)
            {
                throw new Exception(@"The Netverify SDK is not initialized yet. Call InitNetverifyHelper() first.");
            }

            if (_jumioVerifyTask != null)
            {
                _jumioVerifyTask.TrySetCanceled();
            }

            _jumioVerifyTask = new TaskCompletionSource<DocumentResult>();

            var top = GetTopViewController(UIApplication.SharedApplication.KeyWindow);
            top.PresentViewController(_documentVerificationViewController, true, null);

            return _jumioVerifyTask.Task;
        }

        #endregion

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
