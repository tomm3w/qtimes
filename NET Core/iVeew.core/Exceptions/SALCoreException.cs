namespace iVeew.Core.Exceptions
{
    public enum ErrorType
    {
        validation = 0,
        authentication = 1,
        unknownerror = 2,
        data = 4
    }
    public enum ErrorCode
    {
        AccessDenied = 403,
        UnKnownError = 50000,
        InvalidParam = 50001,
        ErrorCodeLoginSocial = 10001,
        ErrorCodeInvalidFacebookToken = 10002,
        ErrorCodeInvalidUserNamePassword = 10003,
        ErrorCodeInvalidNewPassword = 10019,
        ErrorCodePermanentlyBannedUser=10004,
        ErrorCodeTemporaryBannedUser = 10005,
        ErrorCodeUserAlreadyExists = 10006,
        ErrorCodeInvalidUser = 10007,

        ErrorCodePhotoDoesnotExists = 10008,
        ErrorCodeInvalidPhotoSize = 10009,
        ErrorCodeInvalidPhotoFormat = 10010,

        ErrorCodeUserCannotSendInvitationHimself = 10011,
        ErrorCodeUserAlreadyFriend = 10012,
        ErrorCodeUserAlreadyInvited = 10013,
        ErrorCodeUserInvitationAlreadyRejected = 10014,
        ErrorCodeUserInvitationNotFound = 10015,
        ErrorCodeUserCannotInviteBannedUser = 10016,
        ErrorCodeUserNotFriend = 10017,

        ErrorCodeInvalidStatusKeyword = 10018,

        ErrorCodePassNotFound = 10500,
        ErrorCodeInvalidPassword = 10501,

        ErrorCodeNoReadyMessage = 10502,
        ErrorCodeRestaurantNoNotAssigned = 10503,
        ErrorCodeErrorSavingImage = 10504,
        ErrorCodeErrorInvalidSquareImage = 10505,

        ErrorCodeRestaurantNotFound=10506
    }
    public class CoreException : System.Exception
    {
        public CoreException()
        {
            ErrorType = ErrorType.validation;
            ErrorCode = ErrorCode.UnKnownError;
        }
        public string ErrorMessage { get; set; }
        public ErrorType ErrorType { get; set; }
        public ErrorCode ErrorCode { get; set; }
    }
}