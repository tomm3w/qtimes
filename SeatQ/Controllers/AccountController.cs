using Core.Attributes;
using Core.Exceptions;
using Core.Helpers;
using Core.Web;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using SeatQ.core.dal.Enums;
using SeatQ.core.dal.Infrastructure.Repositories;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace SeatQ.Controllers
{
    [Authorize]
    public class AccountController : WebServiceBaseController
    {
        private readonly IAccountInfoRepository _userRepository;

        public AccountController(IAccountInfoRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && Authentication.Web.Security.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                //Get the user in seatq
                var userId = Authentication.Web.Security.CurrentUserIdByName(model.UserName);
                var usr = _userRepository.GetUserInfo(userId);
                string[] roles = Roles.GetRolesForUser(model.UserName);
                if (usr == null)
                {
                    //insert userinfo in seatq for some additional information
                    usr = _userRepository.RegisterUserInfo(userId);
                }
                else
                {
                    //check the restaurant is confirmed
                    //check restaurant chain is active or not
                    //check user is blocked or not

                    string errorMsg = null;
                    bool success = true;

                    if (usr.RestaurantChain == null)
                    {
                        errorMsg = "The user name or password provided is incorrect.";
                        success = false;
                    }
                    else if (usr.RestaurantChain != null && (bool)usr.RestaurantChain.Restaurant.isConfirmed == false)
                    {
                        errorMsg = "Your account is not confirmed.";
                        success = false;
                    }
                    else if (usr.RestaurantChain != null && (bool)usr.RestaurantChain.IsActive == false)
                    {
                        errorMsg = "Your restaurant is not active.";
                        success = false;
                    }
                    else if ((bool)usr.isActive == false)
                    {
                        errorMsg = "You account is blocked.";
                        success = false;
                    }
                    else if ((bool)usr.isDeleted == true)
                    {
                        errorMsg = "Your account is deleted.";
                        success = false;
                    }

                    if (success == false)
                    {
                        LogOff();
                        ModelState.AddModelError("", errorMsg);
                        return View(model);
                    }

                }

                _userRepository.UpdateLastAccess(usr.UserId);

                if (roles.Contains("User"))
                {
                    if (usr.StaffTypeId == (int)StaffTypeEnum.Hostess)
                    { 
                        return RedirectToAction("Index", "Waitlist", new { area = "Admin" });
                        //return RedirectToAction("Index", "Waitlist/hostess", new { area = "Admin" });
                    }
                    else
                        return RedirectToAction("Index", "seating", new { area = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "Waitlist", new { area = "Admin" });
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            LogOff();
            return View(model);
        }

        //
        // POST: /Account/LogOff

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Authentication.Web.Security.Logout();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        //Payment
        public ActionResult Payment()
        {
            return View();
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult SuccessSignup()
        {
            UserProfile data = (UserProfile)TempData["signupuser"];
            return View(data);
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(/*RegisterModel*/ SignupModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    UserProfile usr = _userRepository.RegisterAccount(model);

                    //Send email for confirmation
                    new SeatQ.Helpers.EmailHelper().SendConfirmationEmailMail(usr);

                    TempData["signupuser"] = usr;
                    return RedirectToAction("SuccessSignup", "Account");

                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Confirmation(Guid? id)
        {
            if (ModelState.IsValid)
            {
                _userRepository.ConfirmRegistration(id);
            }

            // If we got this far, something failed, redisplay form
            return RedirectToAction("Index", "Home");
        }

        [WebserviceExecption]
        [AllowAnonymous]
        public JsonResult ResendConfirmation(string Email, Guid? UserId)
        {
            ErrorCode errorCode = ErrorCode.UnKnownError;
            if (UserId != null)
            {
                var usr = _userRepository.GetUserByUserId((Guid)UserId);

                if (usr != null)
                {
                    //Send email for confirmation
                    new SeatQ.Helpers.EmailHelper().SendConfirmationEmailMail(usr);

                    return GetJSON(new { status = "ok" });
                }
                else
                {
                    errorCode = ErrorCode.ErrorCodeInvalidUser;
                    ModelState.AddModelError("User", "Invalid email address");
                }

            }
            else
            {
                errorCode = ErrorCode.ErrorCodeInvalidUser;
                ModelState.AddModelError("User", "Email address required");
            }

            throw new InvalidPostDataException { ErrorCode = errorCode, ModelState = ModelState };

        }

        [HttpPost]
        [AllowAnonymous]
        [WebserviceExecption]
        public JsonResult ForgotPassword(ResetPasswordModel model)
        {
            //return null;
            ErrorCode errorCode = ErrorCode.UnKnownError;
            if (ModelState.IsValid)
            {

                using (SeatQEntities context = new SeatQEntities())
                {
                    UserProfile usr = context.UserProfiles.FirstOrDefault(u => u.UserName == model.UserName /*&& u.Email == model.Email*/);
                    if (usr == null)
                    {
                        errorCode = ErrorCode.ErrorCodeInvalidUser;
                        ModelState.AddModelError("User", "User not found.");
                    }
                    else
                    {
                        //generate random password
                        string newpassword = PasswordHelper.GenerateNumericPassword(6);
                        //reset password
                        var userForUpdate = System.Web.Security.Membership.GetUser(model.UserName);
                        userForUpdate.ChangePassword(userForUpdate.ResetPassword(), newpassword);
                        System.Web.Security.Membership.UpdateUser(userForUpdate);


                        {
                            UserInfo ui = context.UserInfoes.FirstOrDefault(x => x.UserId == usr.UserId);
                            if (ui != null)
                            {
                                ui.IsPasswordChanged = true;
                                context.SaveChanges();
                            }

                            //Send email for confirmation
                            new SeatQ.Helpers.EmailHelper().SendResetPassword(usr, newpassword);

                            return GetJSON(new { status = "ok" });
                        }
                    }
                }
            }
            throw new InvalidPostDataException { ErrorCode = errorCode, ModelState = ModelState };
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            var externalLogins = (from account in accounts
                                  let clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider)
                                  select new ExternalLogin
                                  {
                                      Provider = account.Provider,
                                      ProviderDisplayName = clientData.DisplayName,
                                      ProviderUserId = account.ProviderUserId,
                                  }).ToList();

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(Authentication.Web.Security.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
