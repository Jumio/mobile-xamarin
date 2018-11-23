using System;
namespace JumioForms.Abstractions
{
    public class DocumentResult
    {
        public DocumentResult(bool isSuccess, string scanReference, string errorMessage, string errorCode)
        {
            IsSuccess = isSuccess;
            ScanReference = scanReference;
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }

        public bool IsSuccess { get; }

        public string ScanReference { get; }

        public string ErrorMessage { get; }

        public string ErrorCode { get; }
    }
}
