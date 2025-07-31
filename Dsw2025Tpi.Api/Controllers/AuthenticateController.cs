﻿using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Dsw2025Tpi.Application.Exceptions;

namespace Dsw2025Tpi.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticateController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly JwtTokenService _jwtTokenService;

    public AuthenticateController(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        JwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel request)
    {
        var user =  await _userManager.FindByNameAsync(request.Username);
        if (user == null) {
            return Unauthorized("Incorrect username or password");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            return Unauthorized("Incorrect username or password");
        }
        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? throw new Dsw2025Tpi.Application.Exceptions.ApplicationException("User has not assigned role");
        
        var token = _jwtTokenService.GenerateToken(request.Username, role);
        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        
        var user = new IdentityUser { UserName = model.Username, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);
        var role = await _userManager.AddToRoleAsync(user, "User");
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("User successfully registered.");
    }
}
