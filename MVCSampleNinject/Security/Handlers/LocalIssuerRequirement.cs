﻿// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Schaeflein.Community.MVC5AuthZPolicy;
using System.Security.Claims;
using MVCSampleNinject.Security.Requirements;

namespace MVCSampleNinject.Security.Handlers
{
	public class LocalIssuerHandler : AuthorizationHandler<TrustedIssuerRequirement>
	{
		public LocalIssuerHandler() { }

		public string Issuer
		{
			get
			{
				return "ASP.NET Identity";
			}
		}
		public string ClaimType
		{
			get
			{
				return "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider";
			}
		}



		protected override void Handle(AuthorizationContext context, TrustedIssuerRequirement requirement)
		{
			Claim issuerClaim = context.User.FindFirst(c => c.Type == ClaimType);

			if (issuerClaim.Value == Issuer)
			{
				context.Succeed(requirement);
			}
		}
	}
}