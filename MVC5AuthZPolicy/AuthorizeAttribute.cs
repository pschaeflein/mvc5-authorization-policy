// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Schaeflein.Community.MVC5AuthZPolicy
{
	/// <summary>
	/// Specifies that the class or method that this attribute is applied to requires the specified authorization.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class AuthorizeAttribute : FilterAttribute, IAuthorizationFilter, IAuthorizeData
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AuthorizeAttribute"/> class. 
		/// </summary>
		public AuthorizeAttribute() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthorizeAttribute"/> class with the specified policy. 
		/// </summary>
		/// <param name="policy">The name of the policy to require for authorization.</param>
		public AuthorizeAttribute(string policy)
		{
			Policy = policy;
		}

		private IAuthorizationService _service;
		public AuthorizeAttribute(string policy, IAuthorizationService service)
		{
			_service = service;
		}

		/// <inheritdoc />
		public string Policy { get; set; }

		/// <inheritdoc />
		// REVIEW: can we get rid of the , deliminated in Roles/AuthTypes
		public string Roles { get; set; }

		/// <inheritdoc />
		public string ActiveAuthenticationSchemes { get; set; }


		async void IAuthorizationFilter.OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
		{
			IAuthorizationService service;
			var controller = filterContext.Controller as IControllerAuthorizationService;
			if (controller != null)
			{
				service = controller.AuthorizationService;
			}
			else
			{
				service = (IAuthorizationService)filterContext.HttpContext.GetOwinContext().Environment["MVC5AuthZPolicy"];
			}

			var execute = await service.AuthorizeAsync((ClaimsPrincipal)filterContext.HttpContext.User, Policy);

			if (!execute)
			{
				filterContext.Result = new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
			}

			return;
		}
	}
}
