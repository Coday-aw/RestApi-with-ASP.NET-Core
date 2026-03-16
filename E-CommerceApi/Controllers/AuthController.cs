using E_CommerceApi.DTOs.Customer;
using E_CommerceApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using E_CommerceApi.Interfaces;

namespace E_CommerceApi.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    
    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto user)
    {
        try
        {
            //1 validate DTO modell
            if (!ModelState.IsValid)
                return BadRequest();

            //2 Create a new user/user
            var newUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.Email,
                Email = user.Email,
            };

            //3 Create user/user in Identity
            var result = await _userManager.CreateAsync(newUser, user.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            //4 Add user/user role
            await _userManager.AddToRoleAsync(newUser, "Customer");
    
            return Ok(new { Message = "Registration Successful" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
    {
        //1 validate DTO modell
        if (!ModelState.IsValid)
            return BadRequest();
        
        // find user in database 
        var user = await _userManager.FindByEmailAsync(userDto.Email);
        if (user == null)
            return Unauthorized("Invalid Email or Password");
        
        // check so password matches
        var result = await _signInManager.CheckPasswordSignInAsync(user, userDto.Password, false);
        if(!result.Succeeded)
            return Unauthorized("Invalid Email or Password");

        // generate token 
        var token = await _tokenService.GenerateJwtToken(user);
        
        // send back token
        return Ok(new {message = "Login Successful", token});

    }
}