// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Schaeflein.Community.MVC5AuthZPolicy
{
	public class AuthorizationPolicy
	{
		public AuthorizationPolicy(string name, IAuthorizationRequirement requirement)
		{
			Requirements = new List<IAuthorizationRequirement>();
			AuthenticationSchemes = new List<string>();

			this.Name = name;
			this.Requirements.Add(requirement);
		}

		public AuthorizationPolicy(IEnumerable<IAuthorizationRequirement> requirements, IEnumerable<string> authenticationSchemes)
		{
			if (requirements == null)
			{
				throw new ArgumentNullException(nameof(requirements));
			}

			if (authenticationSchemes == null)
			{
				throw new ArgumentNullException(nameof(authenticationSchemes));
			}

			if (requirements.Count() == 0)
			{
				throw new InvalidOperationException(Resources.Exception_AuthorizationPolicyEmpty);
			}
			Requirements = new List<IAuthorizationRequirement>(requirements);
			AuthenticationSchemes = new List<string>(authenticationSchemes);
		}

		public string Name { get; }
		public List<IAuthorizationRequirement> Requirements { get; }
		public List<string> AuthenticationSchemes { get; }

	}
}