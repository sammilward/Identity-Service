using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityService.Data;
using Microsoft.AspNetCore.Identity;
using Prometheus;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

public class ProfileService : IProfileService
{
    protected UserManager<ApplicationUser> _userManager;
    private Counter tokenCounter = Metrics.CreateCounter("tokensIssued", "Tokens issues");

    public ProfileService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);

        var claims = new List<Claim>
        {
            new Claim("username", user.UserName),
        };

        tokenCounter.Inc();

        context.IssuedClaims.AddRange(claims);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);

        context.IsActive = (user != null);
    }
}