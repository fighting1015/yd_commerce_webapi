using System;
using Vapps.Configuration;
using Xunit;
using Abp.Reflection.Extensions;

namespace Vapps.Tests
{
    public sealed class MultiTenantTheoryAttribute : TheoryAttribute
    {
        public MultiTenantTheoryAttribute()
        {
            if (!VappsConsts.MultiTenancyEnabled)
            {
                //Skip = "MultiTenancy is disabled.";
            }
        }
    }
}