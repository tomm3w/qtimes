using iVeew.common.api.Queries;

namespace QTimes.api.Queries
{
    public class ParseDLResponse : IQueryResponse
    {
        public string RefId { get; set; }
        public string RequestId { get; set; }
        public int DocumentType { get; set; }
        public DVSDocument Document { get; set; }
        public string FacePhotoFromDocument { get; set; }
        public string PassUrl { get; internal set; }
    }

    public class ParseImageResultResponse
    {
        public ParseImageResult ParseImageResult { get; set; }
    }
    public class DVSDocument
    {
        public string ID { get; set; }
        public string IDType { get; set; }
        public string Country { get; set; }
        public string AbbrCountry { get; set; }
        public string Abbr3Country { get; set; }
        public string Issued { get; set; }
        public string Expires { get; set; }
        public string DOB { get; set; }
        public string FullName { get; set; }
        public string PrivateName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string FamilyName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Address { get; set; }
        public string Class { get; set; }
        public string Gender { get; set; }
        public string Height { get; set; }
        public string Eyes { get; set; }
        public string Hair { get; set; }
        public string Weight { get; set; }
    }

    public class ParseImageResult
    {
        public DriverLicense DriverLicense { get; set; }
        public string ErrorMessage { get; set; }
        public string Reference { get; set; }
        public bool Success { get; set; }
        public ValidationCode ValidationCode { get; set; }
    }

    public class DriverLicense
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Birthdate { get; set; }
        public object CardRevisionDate { get; set; }
        public string City { get; set; }
        public string ClassificationCode { get; set; }
        public string ComplianceType { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string EndorsementCodeDescription { get; set; }
        public string EndorsementsCode { get; set; }
        public object ExpirationDate { get; set; }
        public string EyeColor { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public object HAZMATExpDate { get; set; }
        public string HairColor { get; set; }
        public string Height { get; set; }
        public string IIN { get; set; }
        public string IssueDate { get; set; }
        public string IssuedBy { get; set; }
        public string JurisdictionCode { get; set; }
        public string LastName { get; set; }
        public string LicenseNumber { get; set; }
        public object LimitedDurationDocument { get; set; }
        public string MiddleName { get; set; }
        public string NamePrefix { get; set; }
        public string NameSuffix { get; set; }
        public object OrganDonor { get; set; }
        public string PostalCode { get; set; }
        public string Race { get; set; }
        public string RestrictionCode { get; set; }
        public string RestrictionCodeDescription { get; set; }
        public string VehicleClassCode { get; set; }
        public string VehicleClassCodeDescription { get; set; }
        public object Veteran { get; set; }
        public string WeightKG { get; set; }
        public string WeightLBS { get; set; }
        public string Base64Picture { get; set; }
    }
    public class ValidationCode
    {
        public bool? IsValid { get; set; }
        public string[] Errors { get; set; }
    }

    public class ParseFrontDL
    {
        public int ImageId { get; set; }
        public string FileName { get; set; }
        public Document Document { get; set; }
        public string Status { get; set; }
        public string ErrorMsg { get; set; }
    }

    public class Document
    {
        public FieldValue ID { get; set; }
        public FieldValue IDType { get; set; }
        public FieldValue Country { get; set; }
        public FieldValue AbbrCountry { get; set; }
        public FieldValue Abbr3Country { get; set; }
        public FieldValue Issued { get; set; }
        public FieldValue Expires { get; set; }
        public FieldValue DOB { get; set; }
        public FieldValue FullName { get; set; }
        public FieldValue PrivateName { get; set; }
        public FieldValue FirstName { get; set; }
        public FieldValue MiddleName { get; set; }
        public FieldValue FamilyName { get; set; }
        public FieldValue City { get; set; }
        public FieldValue State { get; set; }
        public FieldValue Zip { get; set; }
        public FieldValue Address { get; set; }
        public FieldValue Class { get; set; }
        public FieldValue Gender { get; set; }
        public FieldValue Height { get; set; }
        public FieldValue Eyes { get; set; }
        public FieldValue Hair { get; set; }
        public FieldValue Weight { get; set; }
        public FieldValue Template { get; set; }
        public ImageValue Picture { get; set; }
        public ImageValue Signature { get; set; }
    }

    public class FieldValue
    {
        public string Value { get; set; }
        //public string ValueMRZ { get; set; }
        public int Confidence { get; set; }
        //public string VerifiedMRZ { get; set; }
        //public string VisualAndMRZCompare { get; set; }
    }

    public class ImageValue
    {
        public string Value { get; set; }
        public int Confidence { get; set; }
        public string ImageBase64 { get; set; }
    }
}