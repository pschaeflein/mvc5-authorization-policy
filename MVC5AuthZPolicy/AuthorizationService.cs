// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Schaeflein.Community.MVC5AuthZPolicy
{
	public class AuthorizationService : IAuthorizationService
	{
		private readonly IAuthorizationPolicyProvider _policyProvider;
		private readonly IList<IAuthorizationHandler> _handlers;

		public AuthorizationService(IAuthorizationPolicyProvider policyProvider, IEnumerable<IAuthorizationHandler> handlers)
		{
			if (policyProvider == null)
			{
				throw new ArgumentNullException(nameof(policyProvider));
			}
			if (handlers == null)
			{
				throw new ArgumentNullException(nameof(handlers));
			}

			_handlers = handlers.ToArray();
			_policyProvider = policyProvider;
		}


		public async Task<bool> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
		{
			if (requirements == null)
			{
				throw new ArgumentNullException(nameof(requirements));
			}

			var authContext = new AuthorizationContext(requirements, user, resource);
			foreach (var handler in _handlers)
			{
				await handler.HandleAsync(authContext);
			}

			if (authContext.HasSucceeded)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private string GetUserNameForLogging(ClaimsPrincipal user)
		{
			var identity = user?.Identity;
			if (identity != null)
			{
				var name = identity.Name;
				if (name != null)
				{
					return name;
				}
				return GetClaimValue(identity, "sub")
						?? GetClaimValue(identity, ClaimTypes.Name)
						?? GetClaimValue(identity, ClaimTypes.NameIdentifier);
			}
			return null;
		}

		private static string GetClaimValue(IIdentity identity, string claimsType)
		{
			return (identity as ClaimsIdentity)?.FindFirst(claimsType)?.Value;
		}

		public async Task<bool> AuthorizeAsync(ClaimsPrincipal user, object resource, string policyName)
		{
			if (policyName == null)
			{
				throw new ArgumentNullException(nameof(policyName));
			}

			var policy = await _policyProvider.GetPolicyAsync(policyName);
			if (policy == null)
			{
				throw new InvalidOperationException($"No policy found: {policyName}.");
			}
			return await this.AuthorizeAsync(user, resource, policy);
		}

		public static IAuthorizationService Create(AuthorizationOptions options)
		{
			AuthorizationService authorizationService = new AuthorizationService(
				new AuthorizationPolicyProvider(options),
				new IAuthorizationHandler[] { new PassThroughAuthorizationHandler() });

			return authorizationService;
		}
	}
}