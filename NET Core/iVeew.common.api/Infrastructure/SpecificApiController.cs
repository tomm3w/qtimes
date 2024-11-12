using iVeew.common.api.Infrastructure.Exeptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;

namespace iVeew.common.api.Infrastructure
{
    /// <summary>
    /// Base class for all api controllers
    /// </summary>
    public class SpecificApiController : ControllerBase
    {
        protected IApplicationFacade Facade;
        protected ClaimsPrincipal Principal;
        public SpecificApiController(IApplicationFacade facade)
        {
            Facade = facade;

        }

        private void InitializePrincipal()
        {
            if (Principal == null && Request != null)
                Principal = User.Identity as ClaimsPrincipal;
        }

        public bool Administrator
        {
            get
            {
                InitializePrincipal();
                return Principal.IsInRole("Administrator");
            }
        }
        public bool RegionalManager
        {
            get
            {
                InitializePrincipal();
                return Principal.IsInRole("Regional Manager");
            }
        }

        public bool BusinessAdmin
        {
            get
            {
                InitializePrincipal();
                return Principal.IsInRole("Business Admin");
            }
        }

        public bool IsUserRole
        {
            get
            {
                InitializePrincipal();
                return Principal.IsInRole("User");
            }
        }

        public string Username
        {
            get
            {
                InitializePrincipal();
                return User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            }
        }

        public Guid UserId
        {
            get
            {
                InitializePrincipal();
                return Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid").Value);
                //throw new Exception("Implement in scope of security"); // Security.CurrentUserIdByName(Principal.Identity.Name);
            }
        }
        protected OkObjectResult CreateHttpResponse(object data)
        {
            return new OkObjectResult(new { status = "ok".ToLower(), type = "data", data = data });
        }
        protected OkObjectResult CreateOkHttpResponse()
        {
            return new OkObjectResult(new { status = "ok".ToLower(), type = "data", data = new { } });
        }
        protected IActionResult CreateBadRequestHttpResponse(string message)
        {
            return new BadRequestObjectResult(new { status = "badRequest".ToLower(), type = "data", data = message });
        }
    }
}