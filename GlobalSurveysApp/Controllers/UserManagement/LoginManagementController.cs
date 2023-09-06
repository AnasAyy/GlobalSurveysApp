using AutoMapper;
using GlobalSurveysApp.Data.Repo;
using GlobalSurveysApp.Data.Repos;
using GlobalSurveysApp.Dtos;
using GlobalSurveysApp.Dtos.UserManagmentDtos.LoginManagement;
using GlobalSurveysApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GlobalSurveysApp.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginManagementController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEncryptRepo _encryptRepo;
        private readonly ILoginRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IRoleRepo _role;

        public LoginManagementController(IConfiguration configuration, IEncryptRepo encryptRepo, ILoginRepo userRepo,IRoleRepo roleRepo, IMapper mapper)
        {
            _configuration = configuration;
            _encryptRepo = encryptRepo;
            _userRepo = userRepo;
            _mapper = mapper;
            _role = roleRepo;
        }

        [AllowAnonymous, HttpGet("test")]
        public IActionResult test()
        {
            return Ok(new
            {
                message = "HI"
            });
        }

        [AllowAnonymous, HttpPost("LoginViaUsername")]
        public ActionResult<LoginResponseDto> LoginViaUsername(LoginViaUsernameRequestDto request)
        {
            #region Encrypt Password
            var encryptedPassword = _encryptRepo.EncryptPassword(request.Password);
            if (encryptedPassword == string.Empty)
            {
                return BadRequest();
            }
            request.Password = encryptedPassword;
            #endregion

            #region Check user
            var user = _userRepo.LoginViaUserName(request);
            if (user == null)
            {
                return Unauthorized();
            }
            if (!user.IsActive)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "الحساب غير مفعل",
                    MessageEn = "Account is disactive",
                });
            }
            #endregion

            #region Get User Role
            string role = _role.GetRoleById(user.RoleId);
            #endregion

             #region Get User Department
            string ?department = _userRepo.GetDep(user.Department);
            
            #endregion

            #region Last Login
            user.LastLogin= DateTime.Now;
            _userRepo.Update(user);
            #endregion

            #region Create Token
            var subject = _configuration["Jwt:Subject"];
            var keyhash = _configuration["Jwt:Key"];
            if (subject == null || keyhash == null)
            {
                return BadRequest();
            }


            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, role),


                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyhash));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(48),
                signingCredentials: signIn);

            var stringToken = new JwtSecurityTokenHandler().WriteToken(token);




            return Ok(new LoginResponseDto()
            {
                Token = stringToken,
                User = new UserResponceDto()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.WorkMobile,
                    UserRole = role,
                    IsVerified = user.IsVerified,
                    Department = department!
                }



            }) ;
            #endregion
        }


        [AllowAnonymous, HttpPost("LoginViaQRcode")]
        public ActionResult<LoginResponseDto> LoginViaQRcode(LoginViaQRcodeRequestDto request)
        {

            #region Encrypt QRcode
            var encryptedQRcode = _encryptRepo.EncryptPassword(request.QRcode);
            if (encryptedQRcode == string.Empty)
            {
                return BadRequest();
            }
            request.QRcode = encryptedQRcode;
            #endregion

            #region Check user
            var user = _userRepo.LoginViaQRcode(request);
            if (user == null)
            {
                return Unauthorized();
            }
            if (!user.IsActive)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "الحساب غير مفعل",
                    MessageEn = "Account is disactive",
                });
            }
            #endregion

            #region Get User Role
            string role = _role.GetRoleById(user.RoleId);

            #endregion

            #region Get User Department
            string? department = _userRepo.GetDep(user.Department);

            #endregion

            #region Create Token
            var subject = _configuration["Jwt:Subject"];
            var keyhash = _configuration["Jwt:Key"];
            if (subject == null || keyhash == null)
            {
                return BadRequest();
            }

            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, role),
                        

                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyhash));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(48),
                signingCredentials: signIn);

            var stringToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new LoginResponseDto()
            {
                Token = stringToken,
                User = new UserResponceDto()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.WorkMobile,
                    UserRole = role,
                    IsVerified = user.IsVerified,
                    Department = department!
                }
            });
            #endregion
        }

        

        [Authorize, HttpPut("AddFCMtoken")]
        public IActionResult AddFCMtoken(FCMtokenRequestDto request)
        {

            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            #region Check if User Exist
            var result = _userRepo.GetUserById(Convert.ToInt32(userId.Value));
            if (result == null)
            {
                return NotFound(new ErrorDto()
                {
                    Code = 400,
                    MessageAr = "عذراً، حدث خطأ ما. يرجى المحاولة مرة أخرى.",
                    MessageEn = "Oops, something went wrong. Please try again.",
                });
            }
            #endregion

            #region Delete Exits FCM
            _userRepo.DeleteFCM(result.Id);
            #endregion

            #region Add FCMtoken
            FCMtoken FCMtoken = new FCMtoken();
            FCMtoken.Token = request.FCMtoken;
            FCMtoken.UserId = result.Id;
            _userRepo.CreateFCM(FCMtoken);
            if (!_userRepo.SaveChanges())
            {
                return BadRequest(new ErrorDto()
                {
                    Code = 400,
                    MessageAr = "",
                    MessageEn = ""
                });
            }

            return Ok();
            #endregion
        }

        [Authorize, HttpPut("UpdatePassword")]
        public IActionResult UpdatePassword(VerifiyUserPasswordRequestDto request)
        {
            #region Check Password
            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest(new ErrorDto()
                {
                    Code = 400,
                    MessageAr = "كلمات المرور التي أدخلتها غير متطابقة. يرجى المحاولة مرة أخرى",
                    MessageEn = "The passwords you entered do not match. Please try again",
                });
            }
            #endregion

            #region Encrypt Password
            var encryptedPassword = _encryptRepo.EncryptPassword(request.Password);
            if (encryptedPassword == string.Empty)
            {
                return BadRequest();
            }
            request.Password = encryptedPassword;
            #endregion

            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            #region Check if User Exist
            var result = _userRepo.GetUserById(Convert.ToInt32(userId.Value));
            if (result == null)
            {
                return NotFound(new ErrorDto()
                {
                    Code = 404,
                    MessageAr = "",
                    MessageEn = ""
                });
            }
            #endregion

            #region Update Password  
            result.Password = encryptedPassword;
            result.IsVerified = true;
            _userRepo.Update(result);
            if (!_userRepo.SaveChanges())
            {
                return BadRequest(new ErrorDto()
                {
                    Code = 400,
                    MessageAr = "",
                    MessageEn = ""
                });
            }

            return Ok();
            #endregion

        }

        [Authorize, HttpPut("TempAPI")]
        public IActionResult TempAPI()
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            #region Check if User Exist
            var result = _userRepo.GetUserById(Convert.ToInt32(userId.Value));
            if (result == null)
            {
                return NotFound(new ErrorDto()
                {
                    Code = 404,
                    MessageAr = "",
                    MessageEn = ""
                });
            }
            #endregion

            result.IsVerified = false;
            _userRepo.Update(result);

            if (!_userRepo.SaveChanges())
            {
                return BadRequest(new ErrorDto()
                {
                    Code = 400,
                    MessageAr = "",
                    MessageEn = ""
                });
            }

            return Ok();
        }



    }
}
