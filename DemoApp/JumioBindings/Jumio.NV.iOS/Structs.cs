using System;
using ObjCRuntime;

namespace NativeLibrary
{
    [Native]
    public enum NetverifyMRZFormat : ulong
    {
        Unknown = 0,
        Mrp,
        Mrva,
        Mrvb,
        Td1,
        Td2,
        Cnis
    }

    [Native]
    public enum NetverifyDocumentType : ulong
    {
        All = 0,
        Passport = 1 << 0,
        DriverLicense = 1 << 1,
        IdentityCard = 1 << 2,
        Visa = 1 << 3
    }

    [Native]
    public enum NetverifyGender : ulong
    {
        Unknown,
        M,
        F,
        X
    }

    [Native]
    public enum NetverifyDocumentVariant : ulong
    {
        Unknown = 0,
        Paper,
        Plastic
    }

    [Native]
    public enum NetverifyExtractionMethod : ulong
    {
        Mrz,
        Ocr,
        Barcode,
        BarcodeOCR,
        None
    }

    public enum JumioDataCenter : uint
    {
        Us,
        Eu
    }

    public enum JumioCameraPosition : uint
    {
        Back,
        Front
    }

    [Native]
    public enum JMNavigationControllerTransitionType : ulong
    {
        PushPop,
        Custom
    }

    [Native]
    public enum NetverifyScanMode : ulong
    {
        Undefined = 0,
        Mrz,
        Barcode,
        Ocr,
        OCR_Template,
        Face,
        Manual
    }

    [Native]
    public enum NetverifyScanSide : ulong
    {
        Front = 0,
        Back,
        Face
    }
}