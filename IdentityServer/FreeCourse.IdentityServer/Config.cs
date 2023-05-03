// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FreeCourse.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("resource_catalog"){Scopes={ "catalog_fullPermission" } },
                new ApiResource("resource_photoStock"){Scopes={ "photoStock_fullPermission" } },
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName),
            };
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_fullPermission","Katalog API için full Erişim"),
                new ApiScope("photoStock_fullPermission","PhotoStock API için full Erişim"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client()
                {
                    ClientId="WebMVCClient",
                    ClientName="Asp.NET Core MVC",
                    ClientSecrets={new Secret("secret".Sha256()) },
                    AllowedGrantTypes={GrantType.ClientCredentials },
                    AllowedScopes={ "catalog_fullPermission", "photoStock_fullPermission" , IdentityServerConstants.LocalApi.ScopeName }
                }
            };
    }
}