using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using VehicleParkingSystem.Services;

namespace VehicleParkingSystem.Filters
{
    public class JwtAuthorizeAttribute : AuthorizationFilterAttribute
    {
        private readonly string requiredRole;


        public JwtAuthorizeAttribute()
        {
        }


        public JwtAuthorizeAttribute(string role)
        {
            requiredRole = role;
        }


        public override void OnAuthorization(
            HttpActionContext actionContext)
        {
            var authorization =
                actionContext.Request.Headers.Authorization;


            
            if (authorization == null ||
                !authorization.Scheme.Equals(
                    "Bearer",
                    StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrWhiteSpace(
                    authorization.Parameter))
            {
                actionContext.Response =
                    actionContext.Request.CreateResponse(
                        HttpStatusCode.Unauthorized,
                        new
                        {
                            message =
                                "Authentication token is required."
                        }
                    );

                return;
            }


            try
            {
                string token =
                    authorization.Parameter;


                var jwtService =
                    new JwtService();


                var principal =
                    jwtService.ValidateToken(token);


               
                if (!string.IsNullOrWhiteSpace(requiredRole) &&
                    !principal.IsInRole(requiredRole))
                {
                    actionContext.Response =
                        actionContext.Request.CreateResponse(
                            HttpStatusCode.Forbidden,
                            new
                            {
                                message =
                                    "You are not authorized to access this resource."
                            }
                        );

                    return;
                }


               
                actionContext.RequestContext.Principal =
                    principal;
            }
            catch
            {
                actionContext.Response =
                    actionContext.Request.CreateResponse(
                        HttpStatusCode.Unauthorized,
                        new
                        {
                            message =
                                "Invalid or expired token."
                        }
                    );
            }
        }
    }
}