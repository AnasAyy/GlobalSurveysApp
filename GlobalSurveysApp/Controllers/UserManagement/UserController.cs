using AutoMapper;
using GlobalSurveysApp.Data.Repo;
using GlobalSurveysApp.Data.Repos;
using GlobalSurveysApp.Dtos;
using GlobalSurveysApp.Dtos.PublicListDtos;
using GlobalSurveysApp.Dtos.UserManagmentDtos.UserDtos;
using GlobalSurveysApp.Dtos.UserManagmentDtos.UserRequest;
using GlobalSurveysApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GlobalSurveysApp.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IEncryptRepo _encrypt;
        private readonly IImageConvertRepo _imageConvert;

        public UserController(IUserRepo userRepo, IMapper mapper, IEncryptRepo encrypt, IImageConvertRepo imageConvert)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _encrypt = encrypt;
            _imageConvert = imageConvert;

        }


        [Authorize(Roles = "Admin, Manager"), HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromForm] AddUserRequestDto request)
        {
            var u = _userRepo.IsExits(request.PrivateMobile);
            if (u) return new JsonResult(new
            {
                errors = new
                {

                    Phone = new[] { "Private Mobail is exits" }
                }
            });

            var e = _userRepo.EmailIsExits(request.Email);
            if (e) return new JsonResult(new
            {
                errors = new
                {

                    Email = new[] { "Email is exits" },
                }
            });


            #region Check Photo
            if (request.IdCard == null || request.IdCard.Length == 0)
            {
                return BadRequest("No file selected.");
            }

            string fileExtention = Path.GetExtension(request.IdCard.FileName);
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", "pdf" };
            bool x = validExtensions.Contains(fileExtention.ToLower());
            if (!x) return BadRequest(new ErrorDto
            {
                Code = 400,
                MessageAr = "الرجاء صورة بصيغة صحيحة",
                MessageEn = "Please Choose a Photo With Rigth Extention "
            });
            #endregion


            var user = _mapper.Map<User>(request);
            user.CreatedAt = DateTime.Now;
            user.UserName = request.PrivateMobile;
            #region Encrypt Password
            var encryptedPassword = _encrypt.EncryptPassword(request.PrivateMobile);
            if (encryptedPassword == string.Empty)
            {
                return BadRequest();
            }

            #endregion
            #region Encrypt QRcode
            var encryptedQRcode = _encrypt.EncryptPassword(request.QRcode);
            if (encryptedQRcode == string.Empty)
            {
                return BadRequest();
            }
            user.QRcode = encryptedQRcode;
            #endregion
            user.Password = encryptedPassword;
            user.IdCard = await _imageConvert.ConvertToByte(request.IdCard);
            _userRepo.Create(user, request.WorkingDays);
            if (!_userRepo.SaveChanges())
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "عذراً، حدث خطأ ما. يرجى المحاولة مرة أخرى.",
                    MessageEn = "Oops, something went wrong. Please try again.",
                });
            }
            return Ok();


        }


        [Authorize(Roles = "Admin, Manager"), HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers(GetAllUsersRequestDto request)
        {
            var result = _userRepo.GetAllUsers();
            if (result != null)
            {
                var list = PagedList<GetAllUSersResponseDto>.ToPagedList(result, request.Page, 10);
                Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(list.Paganation));
                return Ok(list);
            }
            return Ok(new ErrorDto
            {
                Code = 400,
                MessageAr = "لا يوجد بيانات",
                MessageEn = "No Data",
            }); ;
        }


        [Authorize(Roles = "Admin, Manager"), HttpGet("GetUserByName")]
        public IActionResult GetUserByName(GetUserByNameRequestDto request)
        {
            var result = _userRepo.GetUserByName(request.Name);
            if (result != null)
            {
                var list = PagedList<GetAllUSersResponseDto>.ToPagedList(result, request.Page, 10);
                Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(list.Paganation));
                return Ok(list);
            }
            return Ok(new ErrorDto
            {
                Code = 400,
                MessageAr = "لا يوجد بيانات",
                MessageEn = "No Data",
            });
        }


        [Authorize(Roles = "Admin, Manager"), HttpGet("GetUserByNameActive")]
        public IActionResult GetUserByNameActive(GetUserByNameRequestDto request)
        {
            var result = _userRepo.GetUserByNameActive(request.Name);
            if (result != null)
            {
                var list = PagedList<GetAllUSersResponseDto>.ToPagedList(result, request.Page, 10);
                Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(list.Paganation));
                return Ok(list);
            }
            return Ok(new ErrorDto
            {
                Code = 400,
                MessageAr = "لا يوجد بيانات",
                MessageEn = "No Data",
            });
        }


        [Authorize(Roles = "Admin, Manager"), HttpGet("GetUserByNameDis_Active")]
        public IActionResult GetUserByNameDis_Active(GetUserByNameRequestDto request)
        {
            var result = _userRepo.GetUserByNameDis_Active(request.Name);
            if (result != null)
            {
                var list = PagedList<GetAllUSersResponseDto>.ToPagedList(result, request.Page, 10);
                Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(list.Paganation));
                return Ok(list);
            }
            return Ok(new ErrorDto
            {
                Code = 400,
                MessageAr = "لا يوجد بيانات",
                MessageEn = "No Data",
            });
        }


        [Authorize(Roles = "Admin, Manager"), HttpGet("GetUserByType")]
        public IActionResult GetUserByType(GetUserByTypeRequestDto request)
        {
            var result = _userRepo.GetUserByType(request.Type);
            if (result != null)
            {
                var list = PagedList<GetAllUSersResponseDto>.ToPagedList(result, request.Page, 10);
                Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(list.Paganation));
                return Ok(list);
            }
            return Ok(new ErrorDto
            {
                Code = 400,
                MessageAr = "لا يوجد بيانات",
                MessageEn = "No Data",
            }); ;
        }


        [Authorize(Roles = "Admin, Manager"), HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserRequestDto request)
        {
            var result = _userRepo.GetUserById(request.Id);
            if (result == null)
            {
                return new JsonResult(new
                {
                    errors = new
                    {
                        
                        Id = new[] { "Can not recognize ID" }
                    }
                });
            }
            if (result.PrivateMobile != request.PrivateMobile)
            {
                if (_userRepo.IsExits(request.PrivateMobile))
                    return new JsonResult(new
                    {
                        errors = new
                        {

                           
                            Phone = new[] { "Private Mobail is exits" }
                        }
                    });
            }

            if (result.Email != request.Email)
            {
                if (_userRepo.EmailIsExits(request.Email))
                    return new JsonResult(new
                    {
                        errors = new
                        {

                            Email = new[] { "Email is exits" },
                        }
                    });
            }

           if (result.SerialNumber != request.SerialNumber)
            {
                if (_userRepo.SerialNumberIsExits(request.SerialNumber))
                    return new JsonResult(new
                    {
                        errors = new
                        {

                            SerialNumber = new[] { "SerialNumber is exits" },
                        }
                    });
            }

            result.FirstName = request.FirstName;
            result.SecondName = request.SecondName;
            result.ThirdName = request.ThirdName;
            result.LastName = request.LastName;
            result.PrivateMobile = request.PrivateMobile;
            result.Email = request.Email;
            result.WorkMobile = request.WorkMobile;
            result.RoleId = request.RoleId;
            result.PassportNumber = request.PassportNumber;
            result.LocationId = request.LocationId;
            result.WorkingHourId = request.WorkingHourId;
            result.SerialNumber = request.SerialNumber;
            if (request.Password != null)
            {
                var Password = _encrypt.EncryptPassword(request.Password);
                result.Password = Password;

            }
            result.UpdatedAt = DateTime.Now;
            if (request.QRcode != null)
            {
                var QRcode = _encrypt.EncryptPassword(request.QRcode);
                result.QRcode = QRcode;
            }

            result.placeOfBirth = request.PlaceOfBirth;
            result.DateOfBirth = request.DateOfBirth;
            result.Department = request.Department;
            result.Location = request.Location;
            result.Gender = request.Gender;
            result.CertificateLevel = request.CertificateLevel;
            result.FieldOfStudy = request.FieldOfStudy;
            result.DirectResponsibleId = request.DirectResponsibleId;
            result.FirstContractDate = request.FirstContractDate;
            result.Postion = request.Postion;
            result.Nationality = request.Nationality;
            result.IsActive = request.Status;
            if (request.IdCard != null)
            {
                #region Check Photo
                if (request.IdCard == null || request.IdCard.Length == 0)
                {
                    return BadRequest("No file selected.");
                }

                string fileExtention = Path.GetExtension(request.IdCard.FileName);
                string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", "pdf" };
                bool x = validExtensions.Contains(fileExtention.ToLower());
                if (!x) return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "الرجاء صورة بصيغة صحيحة",
                    MessageEn = "Please Choose a Photo With Rigth Extention "
                });
                #endregion
                result.IdCard = await _imageConvert.ConvertToByte(request.IdCard);
            }
            _userRepo.Update(result,request.WorkingDays);
            if (!_userRepo.SaveChanges())
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "عذراً، حدث خطأ ما. يرجى المحاولة مرة أخرى.",
                    MessageEn = "Oops, something went wrong. Please try again.",
                });
            }
            return Ok();

        }

        [Authorize(Roles = "Admin, Manager"), HttpGet("ViewUser")]
        public IActionResult ViewUser(GetUserByIdRequestDto request)
        {
            var result = _userRepo.GetUserById(request.Id);
            if (result == null)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "لم يتم التعرف على ال ID",
                    MessageEn = "Can not recognize ID",
                });
            }
            var user = _userRepo.GetUserDetails(result.Id);
            return Ok(user);
        }

        [Authorize(Roles = "Admin, Manager"), HttpGet("GetVariables")]
        public IActionResult GetVariables()
        {
            var variables = _userRepo.GetPublicList();
            if (variables == null)
            {
                return Ok(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "لا يوجد بيانات",
                    MessageEn = "No Data",
                });
            }

            return Ok(variables);

        }


        [Authorize(Roles = "Admin, Manager"), HttpGet("GetAllDirectResponsible")]
        public IActionResult GetAllDirectResponsible()
        {
            var result = _userRepo.GetDirectResponsible();
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return Ok(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "لا يوجد بيانات",
                    MessageEn = "No Data",
                });
            }
        }

        [Authorize(Roles = "Admin, Manager"), HttpGet("ViewIdCard")]
        public async Task<IActionResult> ViewIdCard(GetUserByIdRequestDto request)
        {
            var user = await _userRepo.GetUserByIdAsync(request.Id);

            if (user == null)
            {
                return NotFound(new ErrorDto
                {
                    Code = 404,
                    MessageAr = "لا يوجد بيانات",
                    MessageEn = "No data found",
                });
            }

            return new JsonResult(new
            {
                Photo = user.IdCard
            });
        }

        [Authorize(Roles = "Admin, Manager"), HttpGet("GetType")]
        public async Task<IActionResult> GetTypes()
        {
            var Roles = await _userRepo.GetRole();
            if (Roles == null)
            {
                return Ok(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "لا يوجد بيانات",
                    MessageEn = "No Data",
                });
            }

            var type = Roles.Select(r => new
            {
                r.Id,
                r.Title
            }).ToList();

            return Ok(type);
        }


        [Authorize(Roles = "Admin, Manager"), HttpGet("GetLocations")]
        public async Task<IActionResult> GetLocations()
        {
            var Locations = await _userRepo.GetAlllocations();
            if (Locations == null)
            {
                return Ok(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "لا يوجد بيانات",
                    MessageEn = "No Data",
                });
            }

            

            return Ok(Locations);
        }
        
        [Authorize(Roles = "Admin, Manager"), HttpGet("GetWorkingHour")]
        public async Task<IActionResult> GetWorkingHour()
        {
            var WorkingHour = await _userRepo.GetAllWorkingHour();
            if (WorkingHour == null)
            {
                return Ok(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "لا يوجد بيانات",
                    MessageEn = "No Data",
                });
            }

            

            return Ok(WorkingHour);
        }
        
        [Authorize(Roles = "Admin, Manager"), HttpGet("GetWorkingDay")]
        public async Task<IActionResult> GetWorkingDay()
        {
            var WorkingDays = await _userRepo.GetAllWorkingDays();
            if (WorkingDays == null)
            {
                return Ok(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "لا يوجد بيانات",
                    MessageEn = "No Data",
                });
            }

            

            return Ok(WorkingDays);
        }



    }
}

