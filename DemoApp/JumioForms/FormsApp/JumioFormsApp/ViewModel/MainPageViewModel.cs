using System;
using System.Collections.Generic;
using System.Windows.Input;
using JumioForms;
using Xamarin.Forms;
namespace JumioFormsApp.ViewModel
{
    public class MainPageViewModel
    {
        public MainPageViewModel()
        {
            NetverifyCommand = new Command(InvokeNetverifyCommand);

            DocumentVerificationCommand = new Command(InvokeDocumentVerificationCommand);
        }

        public ICommand NetverifyCommand { get; }

        public ICommand DocumentVerificationCommand { get; }

        async void InvokeNetverifyCommand()
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

            //Customization for iOS only
            var customization = new Dictionary<string, object>
            {
                //{"tintColor", "#ff0000"}
                //,{"disableBlur", true}
                //,{"backgroundColor", "#00ff00"}
                //,... please see further options in JumioNetverify.cs
            };

            CrossJumio.CurrentNetverify.Init("API_TOKEN", "API_SECRET", "DATACENTER", settings, customization);
            var result = await CrossJumio.CurrentNetverify.VerifyAsync();

            var resultText = "";
            if(result.Result != null)
            {
                foreach(var item in result.Result)
                {
                    resultText += $"\n{item.Key}={item.Value?.ToString()}";
                }
            }

            var message = $"{nameof(result.Message)}={result.Message}\n---{nameof(result.Result)}---{resultText}";
            await Application.Current.MainPage.DisplayAlert("Jumio", message, "Ok");
        }

        async void InvokeDocumentVerificationCommand()
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

            //Customization for iOS only
            var customization = new Dictionary<string, object>
            {
                //{"tintColor", "#ff0000"}
                //,{"disableBlur", true}
                //,{"backgroundColor", "#00ff00"}
                //,... please see further options in JumioDocumentVerification.cs
            };
            CrossJumio.CurrentDocumentVerification.Init("API_TOKEN", "API_SECRET", "DATACENTER", settings, customization);
            var result = await CrossJumio.CurrentDocumentVerification.VerifyAsync();

            var message = $"{nameof(result.ErrorCode)}={result.ErrorCode}\n{nameof(result.ErrorMessage)}={result.ErrorMessage}\n{nameof(result.IsSuccess)}={result.IsSuccess}\n{nameof(result.ScanReference)}={result.ScanReference}";
            await Application.Current.MainPage.DisplayAlert("Jumio", message, "Ok");
        }
    }
}
