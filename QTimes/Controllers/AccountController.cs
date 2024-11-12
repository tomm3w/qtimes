using Core.Attributes;
using Core.Exceptions;
using Core.Helpers;
using Core.Web;
using SeatQ.core.dal.Infrastructure.Repositories;
using SeatQ.core.dal.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace QTimes.Controllers
{
    public class AccountController : WebServiceBaseController
    {
        private readonly IAccountInfoRepository _userRepository;

        public AccountController(IAccountInfoRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && Authentication.Web.Security.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                var usrLogged = System.Web.Security.Membership.GetUser(model.UserName);
                if (!usrLogged.IsApproved)
                {
                    ModelState.AddModelError("", "You must have a confirmed email to log on.");
                    Authentication.Web.Security.Logout();
                    return View(model);
                }

                //Get the user in seatq
                var userId = new Guid(usrLogged.ProviderUserKey.ToString());
                var usr = _userRepository.GetUserInfo(userId);
                string[] roles = Roles.GetRolesForUser(model.UserName);

                if (usr == null)
                {

                }
                else
                {
                    string errorMsg = null;
                    bool success = true;

                    if (usr.ReservationBusinessId == null)
                    {
                        errorMsg = "Account not found or active.";
                        success = false;
                    }
                    if (usr.IsConfirmed == false)
                    {
                        errorMsg = "Account not confirmed by email.";
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
                }
                else
                {
                    //return RedirectToAction("Index", "Reservation", new { area = "Admin" });
                    return RedirectToAction("Index", "Accounts", new { area = "Concert" });
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            LogOff();
            return View(model);
        }


        public ActionResult LogOff()
        {
            Authentication.Web.Security.Logout();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

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
                    UserProfile usr = _userRepository.RegisterReservationAccount(model);

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
                _userRepository.ConfirmReservationRegistration(id);
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

        [AllowAnonymous]
        public ActionResult SuccessSignup()
        {
            UserProfile data = (UserProfile)TempData["signupuser"];
            return View(data);
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
    }
}