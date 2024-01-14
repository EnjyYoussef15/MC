using Azure.Messaging;
using MCSHiPPERS_Task.DTO;
using MCSHiPPERS_Task.Models;
using MCSHiPPERS_Task.Repository.IRepository;
using MCSHiPPERS_Task.Repository.Repository;
using MCSHiPPERS_Task.Utilites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace MCSHiPPERS_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _AcountRepository;
        //private readonly IMapper _mapper;
        private readonly UserManager<User> _userManger;
        private readonly IEmailService _emailService;
        private SignInManager<IdentityUser> _signInManager { get; }
        public AccountController(IAccountRepository AcountRepository, UserManager<User> userManager, IEmailService emailService)
        {
            _AcountRepository = AcountRepository;
            ////_mapper = mapper;
            _userManger = userManager;
            _emailService = emailService;

        }

        [AllowAnonymous]
        [HttpPost("UserLogin")]
        public async Task<APIResponse> LoginWithTokin(LoginDTO user)
        {
           
            var userData = await _userManger.FindByEmailAsync(user.Email);
            if (userData is not null)
            {
               
                if (await _userManger.CheckPasswordAsync(userData , user.Password))
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Secret_Key_235K2K1a23"));

                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var data = new List<Claim>();

                    data.Add(new Claim("Email", userData.Email));
                    data.Add(new Claim("username", userData.UserName));

                    if (userData.UserName == "Admin22")
                    {
                        data.Add(new Claim(ClaimTypes.Role, "Admin"));
                    }
                    else
                    {
                        data.Add(new Claim(ClaimTypes.Role, "defaultUser"));

                    }
                    var token = new JwtSecurityToken(
                    claims: data,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: credentials);

                 
                    return new APIResponse
                    {
                        StatusCode = 200,
                        Data = new JwtSecurityTokenHandler().WriteToken(token),
                        Messages = { "User Logged succssefly" }
                    };
                }
                else
                {
                    return new APIResponse()
                    {
                        StatusCode = 400,
                        Messages = { "password is wrong" }
                    };
                }

            }
            else
            {

                return new APIResponse
                {
                    StatusCode = 400,
                    Messages = new List<string> { $"user with {user.Email} cannot be found" }
                };
                //return Unauthorized();

            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<APIResponse> Registration (UserDTO userdto)
        {

            
            var userData = await _userManger.FindByEmailAsync(userdto.Email);
            if (userData is not null)
            {
                return new APIResponse()
                {
                    StatusCode = 500,
                    Messages = { "Email already Exist" }
                };
            }

            if (ModelState.IsValid)
            {
                User newUser = new()
                {
                    UserName = userdto.UserName,
                    Email = userdto.Email,

                };

                IdentityResult result = await _userManger.CreateAsync(newUser, userdto.Password);
                if (!result.Succeeded)
                {
                    List<string>err= new List<string>();
                    foreach(var error in result.Errors) 
                    {
                        err.Add(error.Description);
                    }

                    return new APIResponse
                    {
                        StatusCode = 400,
                        Messages = err
                    };
                }
                else
                {
                    
                    return new APIResponse()
                    {
                        Data = newUser,
                    };

                }

            }
            else
            {
                return new APIResponse
                {
                    StatusCode = 400,
                    Messages = { "Model is not valid" }
                };
            }
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<APIResponse> ForgotPassword(string Email)
        {
            var userData = await _userManger.FindByEmailAsync(Email);
            if (userData == null)
            {
                return new APIResponse
                {
                    StatusCode = 400,
                    Messages = new List<string> { $"user with {Email} cannot be found" }
                };
            }

            var token = await _userManger.GeneratePasswordResetTokenAsync(userData);

            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var link = Url.Action(nameof(ResetPassword), "Account", new { token, email = userData.Email }, Request.Scheme);



            EmailService emailHelper = new EmailService();
            bool emailResponse = await emailHelper.SendPasswordResetEmail(userData.Email, link);

            if (emailResponse)

            { 
                return new APIResponse()
                {
                    StatusCode = 200,
                   
                    Messages = new List<string> { "Password reset email sent" }
                };
              }
            else
            {
                return new APIResponse()
                {
                    StatusCode = 500,
                    
                    Messages = new List<string> { "Error sending email" }
                };
             }



  

}

        [HttpGet("ResetPassword")]
        public async Task<APIResponse> ResetPassword(string token , string email)
        {
            var user = new newpasswordDTO { Token = token, email = email };

            return new APIResponse()
            {
                StatusCode = 200,
                Data = user,
            };
          
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<APIResponse> ResetPassword(newpasswordDTO dto)
        {
            var user = await _userManger.FindByEmailAsync(dto.email);
            if (user != null)
            {
              
                dto.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(dto.Token));
                var result = await _userManger.ResetPasswordAsync(user, dto.Token, dto.password);
                if (result.Succeeded)
                {
                    //await _signInManager.RefreshSignInAsync(user);
                    return new APIResponse()
                    {
                       
                        Data = user,
                        Messages ={ " User Updated Succfully"}
                    };
                }
                else
                {
                    return new APIResponse()
                    {
                        StatusCode = 500,
                        Messages = result.Errors.Select(a => a.Description).ToList()
                    };
                }

            }
            return new APIResponse()
            {
                StatusCode = 400,
                Messages = new List<string> { "User cannot be found" }
            };
        }

        [HttpGet]
        [Route("GetallUsers")]
        public async Task<APIResponse> Getregistration()
        {
           var users= await _AcountRepository.GetAllUsers();
            if (users != null)
            {
                
                return new APIResponse()
                {
                    StatusCode = 200,
                    Data = users,
                };
            }
            else
            {
                return new APIResponse()
                {
                    StatusCode = 400,
                    Messages = { "No Users" }
                };
            }

        }

        [HttpGet]
        [Route("getOneUser/{id}")]
        public async Task<APIResponse> Getregistration(string id)
        {
            User user = await _AcountRepository.GetUserById(id);

            if (user != null)
            {

                return new APIResponse()
                {
                    StatusCode = 200,
                    Data = user,
                };
            }
            else
            {
                return new APIResponse()
                {
                    StatusCode = 400,
                    Messages = { "No Users" }
                };
            }
        }

          
    }
}
