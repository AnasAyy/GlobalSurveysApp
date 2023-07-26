using AutoMapper;
using GlobalSurveysApp.Data.Repo;
using GlobalSurveysApp.Data.Repos;
using GlobalSurveysApp.Dtos;
using GlobalSurveysApp.Dtos.UserManagmentDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GlobalSurveysApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEncryptRepo _encryptRepo;
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public UserManagementController(IConfiguration configuration, IEncryptRepo encryptRepo, IUserRepo userRepo, IMapper mapper)
        {
            _configuration = configuration;
            _encryptRepo = encryptRepo;
            _userRepo = userRepo;
            _mapper = mapper;
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
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    UserRole = "Admin"
                }

                

            });
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
            request.QRcode= encryptedQRcode;
            #endregion

            #region Check user
            var user = _userRepo.LoginViaQRcode(request);
            if (user == null)
            {
                return Unauthorized();
            }
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
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    UserRole = "Admin"
                }
            });
            #endregion
        }

        //[Authorize, HttpGet("IsVerified")]
        //public IActionResult IsVerified()
        //{
        //    #region Check Token Data
        //    var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
        //    if (userId == null)
        //    {
        //        return Unauthorized();
        //    }
        //    #endregion


        //    #region Check User Status
        //    if (!_userRepo.IsVerified(Convert.ToInt32(userId.Value)))
        //        return BadRequest(new ErrorDto
        //        {
        //            Code = 400,
        //            MessageAr = "",
        //            MessageEn = "",
        //        });
        //    #endregion


        //    return Ok("OK");
        //}

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
                    Code = 404,
                    MessageAr = "",
                    MessageEn = ""
                });
            }
            #endregion

            #region Update FCMtoken
            result.UpdatedAt = DateTime.Now;
            result.FCMtoken = request.FCMtoken;
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
        //[Authorize, HttpPut("VerifiyUser")]
        //public IActionResult VerifiyUser()
        //{
        //    #region Check if User Exist
        //    var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
        //    if (userId == null)
        //    {
        //        return Unauthorized();
        //    }
        //    var result = _userRepo.GetUserById(Convert.ToInt32(userId.Value));
        //    if (result == null)
        //    {
        //        return NotFound(new ErrorDto()
        //        {
        //            Code = 404,
        //            MessageAr = "",
        //            MessageEn = ""
        //        });
        //    }
        //    #endregion


        //}
    }
}
