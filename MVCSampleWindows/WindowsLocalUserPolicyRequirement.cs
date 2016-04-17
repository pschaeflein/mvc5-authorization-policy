// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using Schaeflein.Community.MVC5AuthZPolicy;

namespace MVCSampleWindows
{
	public class WindowsLocalUserPolicyRequirement : AuthorizationHandler<WindowsLocalUserPolicyRequirement>, IAuthorizationRequirement
	{
		public WindowsLocalUserPolicyRequirement() { }

		public string ClaimType
		{
			get
			{
				return ClaimTypes.GroupSid;
			}
		}

		public string GroupSid
		{
			get
			{
				// Well-known security identifiers in Windows operating systems
				// https://support.microsoft.com/en-us/kb/243330
				return "S-1-5-32-545";
			}
		}

		protected override void Handle(AuthorizationContext context, WindowsLocalUserPolicyRequirement requirement)
		{
			bool succeeded = false;

			foreach (var claim in context.User.FindAll(c => c.Type == requirement.ClaimType))
			{
				if (claim.Value == GroupSid)
				{
					succeeded = true;
					break;
				}
			}

			if (succeeded)
			{
				context.Succeed(requirement);
			}
		}

	}
}