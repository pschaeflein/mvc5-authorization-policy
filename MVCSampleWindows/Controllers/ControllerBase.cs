// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Schaeflein.Community.MVC5AuthZPolicy;
using System.Web.Mvc;

namespace MVCSampleWindows.Controllers
{
	public class ControllerBase : Controller, IControllerAuthorizationService
	{
		public IAuthorizationService authorizationService { get; set; }

		public IAuthorizationService AuthorizationService
		{
			get
			{
				return authorizationService;
			}
		}

		public ControllerBase()
		{
			var options = new AuthorizationOptions();
			options.AddPolicy("Users", new AuthorizationPolicyBuilder()
																			.RequireAuthenticatedUser()
																			.AddRequirements(new WindowsLocalUserPolicyRequirement())
																			.Build());

			authorizationService = new AuthorizationService(
				new AuthorizationPolicyProvider(options),
				new IAuthorizationHandler[] { new PassThroughAuthorizationHandler() });
		}

		public ControllerBase(IAuthorizationService service)
		{
			authorizationService = service;
		}
	}
}