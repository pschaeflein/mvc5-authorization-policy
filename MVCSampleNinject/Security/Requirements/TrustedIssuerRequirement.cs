// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Schaeflein.Community.MVC5AuthZPolicy;

namespace MVCSampleNinject.Security.Requirements
{
	public class TrustedIssuerRequirement : IAuthorizationRequirement
	{
		public TrustedIssuerRequirement() { }
	}
}
