﻿using AutoMapper;
using GlobalSurveysApp.Data.Repo;
using GlobalSurveysApp.Dtos.AdvanceDtos;
using GlobalSurveysApp.Dtos;
using GlobalSurveysApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GlobalSurveysApp.Dtos.TimeOffDtos;
using Microsoft.EntityFrameworkCore;
using GlobalSurveysApp.Dtos.ApproveDtos;

namespace GlobalSurveysApp.Controllers.TimeOffManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeOffController : ControllerBase
    {
        private readonly ITimeOffRepo _timeOffRepo;
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public TimeOffController(ITimeOffRepo timeOffRepo, IUserRepo userRepo, IMapper mapper)
        {
            _timeOffRepo = timeOffRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }


        [Authorize(Roles = "Normal user, Direct responsible, HR, Secretary"), HttpPost("AddTimeOff")]
        public async Task<ActionResult<object>> AddTimeOff(AddTimeOffRequestDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            #region Add Time Off

            var timeOff = _mapper.Map<TimeOff>(request);
            int userIdValue;
            int.TryParse(userId.Value, out userIdValue);
            timeOff.UserId = userIdValue;
            timeOff.CreatedAt = DateTime.Now;
            timeOff.SubEmpStatus = RequestStatus.Pending;
            var timeOffId = await _timeOffRepo.CreateTimeOff(timeOff);

            if (timeOffId <= 0)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "عذراً، حدث خطأ ما. يرجى المحاولة مرة أخرى.",
                    MessageEn = "Oops, something went wrong. Please try again.",
                });
            }
            #endregion

            FCMtokenResponseDto FC = new FCMtokenResponseDto();
            FC.FCMToken = await _timeOffRepo.GetFCM(request.SubstituteEmployeeId);
            var user = await _timeOffRepo.GetUserById(userIdValue);

            FC.MessageAR = "طلب اجازة جديد من " + user?.FirstName + " " + user?.LastName;
            FC.MessageEN = "New Time Off request from " + user?.FirstName + " " + user?.LastName;

            return new JsonResult(new
            {
                code = 200,
                FC,
            });
        }

        [Authorize(Roles = "Normal user, Direct responsible, HR, Manager, Secretary"), HttpGet("ViewUserTimeOff")]
        public async Task<IActionResult> ViewUserTimeOff(GetUserTimeOffRequestDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion
            var result = await _timeOffRepo.GetAllTimeOffForUser(Convert.ToInt32(userId.Value));
            if (result.Any())
            {
                var list = PagedList<GetAllTimeOffResponseDto>.ToPagedList(result, request.Page, 10);
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

        [Authorize(Roles = "Normal user, Direct responsible, HR, Secretary"), HttpGet("FilterByDate")]
        public async Task<IActionResult> FilterByDate(GetUserTimeOffByDateRequestDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            var result = await _timeOffRepo.GetTimeForUserByDate(Convert.ToInt32(userId.Value), request.From, request.To);
            if (result.Any())
            {
                var list = PagedList<GetAllTimeOffResponseDto>.ToPagedList(result, request.Page, 10);
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

        [Authorize(Roles = "Normal user, Direct responsible, HR, Secretary"), HttpPut("UpdateTimeOff")]
        public IActionResult UpdateTimeOff(UpdateTimeOffRequestDto request)
        {
            var result = _timeOffRepo.GetTimeOffById(request.Id);
            if (result == null)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "حاول مجددا",
                    MessageEn = "Try Again",
                });
            }
            if (result.IsUpdated == true)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "ليس لديك صلاحيات تعديل الطلب.",
                    MessageEn = "You Do Not Have Permission To Update The Request.",
                });
            }

            result.Type = request.Type;
            result.Number = request.Number;
            result.EmergencyNumber = request.EmergencyNumber;
            result.SubstituteEmployeeId = request.SubstituteEmployeeId;
            result.To = request.To;
            result.From = request.From;
            result.UpdatedAt = DateTime.Now;



            _timeOffRepo.UpdateTimeOff(result);
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

        [Authorize(Roles = "Normal user, Direct responsible, HR, Manager, Secretary"), HttpGet("ViewDetails")]
        public async Task<IActionResult> ViewDetails(ViewDetailsRequestDto request)
        {
            var result = _timeOffRepo.GetTimeOffById(request.Id);
            if (result == null)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "حاول مجددا",
                    MessageEn = "Try Again",
                });
            }
            var approvers = await _timeOffRepo.GetApproversByRequestId(request.Id);
            if (approvers == null)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "حاول مجددا",
                    MessageEn = "Try Again",
                });
            }

            GetTimeOffDetailsResponseDto details = new GetTimeOffDetailsResponseDto();
            details.Id = result.Id;
            details.Type = result.Type;
            details.Number = result.Number;
            details.From = result.From;
            details.To = result.To;
            details.EmergencyNumber = result.EmergencyNumber;
            details.SubsituteEmployeeId = result.SubstituteEmployeeId;
            details.SubEmpStatus = result.SubEmpStatus;

            if (approvers.Count == 3)
            {
                for (int i = 0; i < approvers.Count; i++)
                {
                    if (approvers[i].ApproverType == 1)
                    {
                        details.DirectResponsibleStatus = approvers[i].Status;

                        if (approvers[i].Note != null && approvers[i].Note != "")
                        {
                            details.StatusNote = "Direct Responsible Note: " + approvers[i].Note;
                        }

                    }
                }
            }
            if (approvers.Count >= 2)
            {
                for (int i = 0; i < approvers.Count; i++)
                {
                    if (approvers[i].ApproverType == 2)
                    {
                        details.HRStatus = approvers[i].Status;
                        if (approvers[i].Note != null && approvers[i].Note != "")
                        {
                            details.StatusNote += " HR Note: " + approvers[i].Note;
                        }
                    }
                }
            }
            if (approvers.Count >= 1)
            {
                for (int i = 0; i < approvers.Count; i++)
                {


                    if (approvers[i].ApproverType == 3)
                    {
                        details.ManagerStatus = approvers[i].Status;
                        if (approvers[i].Note != null && approvers[i].Note != "")
                        {
                            details.StatusNote += " Manager Note: " + approvers[i].Note;
                        }
                    }
                }

            }

            return new JsonResult(new
            {
                code = 200,
                details,
            });

        }

        [Authorize(Roles = "Normal user, Direct responsible, HR, Manager, Secretary"), HttpGet("GetSubsituteEmployee")]
        public async Task<IActionResult> GetSubsituteEmployee()
        {

            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion
            var subsituteEmployees = await _timeOffRepo.testGetFilteredUsersAsync(Convert.ToInt32(userId.Value));
            if (subsituteEmployees == null)
            {
                return Ok(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "لا يوجد بيانات",
                    MessageEn = "No Data",
                });
            }

            return Ok(subsituteEmployees);


            //#region Check Token Data
            //var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            //if (userId == null)
            //{
            //    return Unauthorized();
            //}
            //#endregion
            //var subsituteEmployees = await _timeOffRepo.GetSubsituteEmployee(Convert.ToInt32(userId.Value));
            //if (subsituteEmployees == null)
            //{
            //    return Ok(new ErrorDto
            //    {
            //        Code = 400,
            //        MessageAr = "لا يوجد بيانات",
            //        MessageEn = "No Data",
            //    });
            //}

            //return Ok(subsituteEmployees);
        }

        //[HttpPost("testGetSubsituteEmployee")]
        //public async Task<IActionResult> testGetSubsituteEmployee()
        //{
        //    #region Check Token Data
        //    var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
        //    if (userId == null)
        //    {
        //        return Unauthorized();
        //    }
        //    #endregion
        //    var subsituteEmployees = await _timeOffRepo.testGetFilteredUsersAsync(Convert.ToInt32(userId.Value));
        //    if (subsituteEmployees == null)
        //    {
        //        return Ok(new ErrorDto
        //        {
        //            Code = 400,
        //            MessageAr = "لا يوجد بيانات",
        //            MessageEn = "No Data",
        //        });
        //    }

        //    return Ok(subsituteEmployees);
        //}

        [Authorize(Roles = "Normal user, Direct responsible, HR, Manager, Secretary"), HttpGet("GetTypes")]
        public async Task<IActionResult> GetTypes()
        {
            var types = await _timeOffRepo.GetTypes();
            if (types == null)
            {
                return Ok(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "لا يوجد بيانات",
                    MessageEn = "No Data",
                });
            }

            return Ok(types);
        }

        [Authorize(Roles = "Manager, Direct responsible, HR, Secretary"), HttpGet("GetTimeOffForApprover")]
        public async Task<IActionResult> GetTimeOffForApprover(GetTimeOffForApproverRequestDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            var result = await _timeOffRepo.GetTimeOffForApprover(Convert.ToInt32(userId.Value));

            if (result != null)
            {
                var list = PagedList<GetTimeOffForApproverResponseDto>.ToPagedList(result, request.Page, 10);
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

        [Authorize(Roles = "Manager, Direct responsible, HR, Secretary"), HttpGet("GetTimeOffForApproverByDate")]
        public async Task<IActionResult> GetTimeOffForApproverByDate(GetTimeOffForApproverByDAteRequestDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            var result = await _timeOffRepo.GetTimeOffForApproverByDate(Convert.ToInt32(userId.Value), request.From, request.To);

            if (result != null)
            {
                var list = PagedList<GetTimeOffForApproverResponseDto>.ToPagedList(result, request.Page, 10);
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

        [Authorize(Roles = "Manager, Direct responsible, HR, Secretary"), HttpGet("GetTimeOffForApproverByName")]
        public async Task<IActionResult> GetTimeOffForApproverByName(GetTimeOffForApproverByNameDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            var result = await _timeOffRepo.GetTimeOffForApproverByName(Convert.ToInt32(userId.Value), request.Name);

            if (result != null)
            {
                var list = PagedList<GetTimeOffForApproverResponseDto>.ToPagedList(result, request.Page, 10);
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

        [Authorize(Roles = "Manager, Direct responsible, HR, Secretary"), HttpPut("Approve")]
        public async Task<IActionResult> Approve(ApproveRequestDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            var userRole = HttpContext.User.FindFirst(ClaimTypes.Role);
            if (userId == null || userRole == null)
            {
                return Unauthorized();
            }
            #endregion
            FCMtokenResponseDto FC = new FCMtokenResponseDto();
            FCMtokenResponseDto FCForHR = new FCMtokenResponseDto();
            var timeOff = _timeOffRepo.GetTimeOffById(request.RequestId);

            var result = await _timeOffRepo.GetApprover(request.RequestId, Convert.ToInt32(userId.Value));
            if (result == null || timeOff == null)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "حاول مجددا",
                    MessageEn = "Try Again",
                });
            }

            if (userRole.Value == "Direct responsible" || userRole.Value == "Secretary")
            {
                if (request.Status == 1)
                {
                    result.Status = RequestStatus.Accepted;

                }
                if (request.Status == 2)
                {
                    result.Status = RequestStatus.Rejected;
                }
                if (request.Note != null)
                {
                    result.Note = request.Note;
                }
                result.CanViewed = false;
                result.UpdatedAt = DateTime.Now;
                await _timeOffRepo.UpdateApprover(result);

                int hr = await _timeOffRepo.GetIdByRole("HR").FirstOrDefaultAsync();
                result = await _timeOffRepo.GetApprover(request.RequestId, hr);


                if (result != null)
                {
                    result.CanViewed = true;
                    await _timeOffRepo.UpdateApprover(result);
                    FC.MessageAR = "طلب اجازة جديد";
                    FC.MessageEN = "New Time Off request ";
                    FC.FCMToken = await _timeOffRepo.GetFCM(hr);
                }

                timeOff.IsUpdated = true;

            }

            if (result == null)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "حاول مجددا",
                    MessageEn = "Try Again",
                });
            }
            if (userRole.Value == "HR")
            {
                if (request.Status == 1)
                {
                    result.Status = RequestStatus.Accepted;
                }
                if (request.Status == 2)
                {
                    result.Status = RequestStatus.Rejected;
                }
                if (request.Note != null)
                {
                    result.Note = request.Note;
                }

                result.CanViewed = false;
                result.UpdatedAt = DateTime.Now;
                await _timeOffRepo.UpdateApprover(result);

                int manager = await _timeOffRepo.GetIdByRole("Manager").FirstOrDefaultAsync();
                result = await _timeOffRepo.GetApprover(request.RequestId, manager);


                if (result != null)
                {
                    result.CanViewed = true;
                    await _timeOffRepo.UpdateApprover(result);
                    FC.MessageAR = "طلب أجازة جديد  ";
                    FC.MessageEN = "New Time Off request ";
                    FC.FCMToken = await _timeOffRepo.GetFCM(manager);
                }
                timeOff.IsUpdated = true;

            }
            if (result == null)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "حاول مجددا",
                    MessageEn = "Try Again",
                });
            }


            if (userRole.Value == "Manager")
            {
                if (request.Status == 1)
                {
                    result.Status = RequestStatus.Accepted;
                    if (request.Note != null)
                    {
                        result.Note = request.Note;
                    }
                    result.UpdatedAt = DateTime.Now;
                    result.CanViewed = false;
                    await _timeOffRepo.UpdateApprover(result);
                    var requestUser = _timeOffRepo.GetTimeOffById(result.RequestId);
                    if (requestUser == null)
                    {
                        return BadRequest();
                    }
                    timeOff.Status = RequestStatus.Accepted;

                    FC.MessageAR = "تم قبول طلب الاجازة الخاص بك ";
                    FC.MessageEN = "Your Time Off request has been accepted ";
                    FC.FCMToken = await _timeOffRepo.GetFCM(Convert.ToInt32(requestUser.UserId));

                    var user = _userRepo.GetUserById(requestUser.UserId);
                    int hr = await _timeOffRepo.GetIdByRole("HR").FirstOrDefaultAsync();
                    FCForHR.MessageAR = " تم قبول طلب الاجازة الخاص ب " + user?.FirstName + " " + user?.LastName;
                    FCForHR.MessageEN = "Time Off request For " + user?.FirstName + " " + user?.LastName +
                        " has been accepted ";
                    FCForHR.FCMToken = await _timeOffRepo.GetFCM(hr);

                }
                if (request.Status == 2)
                {
                    result.Status = RequestStatus.Rejected;
                    if (request.Note != null)
                    {
                        result.Note = request.Note;
                    }
                    result.UpdatedAt = DateTime.Now;
                    result.CanViewed = false;
                    await _timeOffRepo.UpdateApprover(result);
                    var requestUser = _timeOffRepo.GetTimeOffById(result.RequestId);
                    if (requestUser == null)
                    {
                        return BadRequest();
                    }
                    timeOff.Status = RequestStatus.Rejected;

                    FC.MessageAR = "تم رفض طلب الاجازة الخاص بك ";
                    FC.MessageEN = "Your Time Off request has been rejected ";
                    FC.FCMToken = await _timeOffRepo.GetFCM(Convert.ToInt32(requestUser.UserId));

                    var user = _userRepo.GetUserById(requestUser.UserId);
                    int hr = await _timeOffRepo.GetIdByRole("HR").FirstOrDefaultAsync();
                    FCForHR.MessageAR = "تم رفض طلب الاجازة الخاص ب " + user?.FirstName + " " + user?.LastName;
                    FCForHR.MessageEN = "Time Off request For " + user?.FirstName + " " + user?.LastName +
                        " has been rejected ";
                    FCForHR.FCMToken = await _timeOffRepo.GetFCM(hr);
                }
                timeOff.IsUpdated = true;
            }
            _timeOffRepo.UpdateTimeOff(timeOff);
            if (!_userRepo.SaveChanges())
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "عذراً، حدث خطأ ما. يرجى المحاولة مرة أخرى.",
                    MessageEn = "Oops, something went wrong. Please try again.",
                });
            }
            return new JsonResult(new
            {
                code = 200,
                FC,
                FCForHR
            });


        }


        [Authorize(Roles = "Manager, Direct responsible, HR, Secretary, Normal user"), HttpPost("ApproveBySubEmp")]
        public async Task<IActionResult> ApproveBySumEmp(ApproveBySumEmpRequestDtos request)
        {
            var timeOff = _timeOffRepo.GetTimeOffById(request.RequestId);
            if (request.Status == 2)
            {

                if (timeOff != null)
                {
                    timeOff.SubEmpStatus = RequestStatus.Rejected;
                    timeOff.IsUpdated = true;
                    timeOff.Status = RequestStatus.Rejected;
                    _timeOffRepo.UpdateTimeOff(timeOff);
                    if (!_userRepo.SaveChanges())
                    {
                        return BadRequest(new ErrorDto
                        {
                            Code = 400,
                            MessageAr = "عذراً، حدث خطأ ما. يرجى المحاولة مرة أخرى.",
                            MessageEn = "Oops, something went wrong. Please try again.",
                        });
                    }

                    FCMtokenResponseDto FC = new FCMtokenResponseDto();
                    FC.FCMToken = await _timeOffRepo.GetFCM(timeOff.UserId);
                    FC.MessageAR = "تم رفض طلب الاجازة الخاص بك ";
                    FC.MessageEN = "Your Time Off request has been rejected ";

                    return new JsonResult(new
                    {
                        code = 200,
                        FC,
                    });
                }
            }
            if (request.Status == 1)
            {
                if (timeOff != null)
                {
                    

                    #region Add Approver

                    List<Approver> approvers = new List<Approver>();
                    FCMtokenResponseDto FC = new FCMtokenResponseDto();

                    if (timeOff != null)
                    {

                        timeOff.SubEmpStatus = RequestStatus.Accepted;
                        timeOff.IsUpdated = true;
                        _timeOffRepo.UpdateTimeOff(timeOff);
                        if (!_userRepo.SaveChanges())
                        {
                            return BadRequest(new ErrorDto
                            {
                                Code = 400,
                                MessageAr = "عذراً، حدث خطأ ما. يرجى المحاولة مرة أخرى.",
                                MessageEn = "Oops, something went wrong. Please try again.",
                            });
                        }

                        var user = await _timeOffRepo.GetUserById(timeOff.UserId);
                        if (user != null && user.DirectResponsibleId != null)
                        {
                            approvers.Add(new Approver
                            {
                                RequestId = request.RequestId,
                                ApproverType = 1, // 1 For DirectResponsible 
                                RequestType = 2, // 2 For Time Request
                                CanViewed = true,
                                UserId = user.DirectResponsibleId
                            });
                            FC.FCMToken = await _timeOffRepo.GetFCM(user.DirectResponsibleId);
                        }

                        int? hr = await _timeOffRepo.GetIdByRole("HR").FirstOrDefaultAsync();
                        if (hr != 0 && hr != timeOff.UserId && user?.DirectResponsibleId != null)
                        {
                            approvers.Add(new Approver
                            {
                                RequestId = request.RequestId,
                                ApproverType = 2,  // 2 For HR
                                RequestType = 2,  // 2 For TimeOff Request
                                UserId = hr,
                                CanViewed = false,

                            });
                        }
                        if (hr != 0 && hr != timeOff.UserId && user?.DirectResponsibleId == null)
                        {
                            approvers.Add(new Approver
                            {
                                RequestId = timeOff.Id,
                                ApproverType = 2,  // 2 For HR
                                RequestType = 2,  // 2 For TimeOff Request
                                UserId = hr,
                                CanViewed = true,

                            });
                            FC.FCMToken = await _timeOffRepo.GetFCM(hr);

                        }



                        int? manager = await _timeOffRepo.GetIdByRole("Manager").FirstOrDefaultAsync();
                        if (manager != 0 && manager != timeOff.UserId && hr != 0 && hr != timeOff.UserId)
                        {
                            approvers.Add(new Approver
                            {
                                RequestId = timeOff.Id,
                                ApproverType = 3, // 3 For Manager
                                RequestType = 2, // 1 For Time Request
                                UserId = manager,
                                CanViewed = false,
                            });
                            //FC.ManagerFCMToken = await _advanceRepo.GetFCM(manager);
                        }
                        if (manager != 0 && manager != timeOff.UserId && hr == timeOff.UserId)
                        {
                            approvers.Add(new Approver
                            {
                                RequestId = timeOff.Id,
                                ApproverType = 3, // 3 For Manager
                                RequestType = 2,  // 2 For TimeOff Request
                                UserId = manager,
                                CanViewed = true,
                            });
                            FC.FCMToken = await _timeOffRepo.GetFCM(manager);
                        }



                        await _timeOffRepo.CreateApprover(approvers);
                        if (!_userRepo.SaveChanges())
                        {
                            return BadRequest(new ErrorDto
                            {
                                Code = 400,
                                MessageAr = "عذراً، حدث خطأ ما. يرجى المحاولة مرة أخرى.",
                                MessageEn = "Oops, something went wrong. Please try again.",
                            });
                        }



                        FC.MessageAR = "طلب اجازة جديد من " + user?.FirstName + " " + user?.LastName;
                        FC.MessageEN = "New Time Off request from " + user?.FirstName + " " + user?.LastName;

                        return new JsonResult(new
                        {
                            code = 200,
                            FC,
                        });
                    }
                    #endregion
                }
            }

            return null!;
        }

        [Authorize(Roles = "Manager, Direct responsible, HR, Secretary, Normal user"), HttpGet("TimeoffForSubEmp")]
        public async Task<IActionResult> TimeoffForSubEmp()
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            var timeoffs = await _timeOffRepo.GetTimeOffsForSubEmp(Convert.ToInt32(userId.Value.ToString()));
            if (timeoffs.Count == 0)
            {
                return Ok(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "لا يوجد بيانات",
                    MessageEn = "No Data",
                });
            }

            return new JsonResult(new
            {
                code = 200,
                timeoffs,
            });

        }

        [Authorize(Roles = "Manager, Direct responsible, HR, Secretary, Normal user"), HttpGet("GetTimeOffsCountForSubEmp")]
        public async Task<IActionResult> TimeoffCountForSubEmp()
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            var timeoffs = await _timeOffRepo.GetTimeOffsCountForSubEmp(Convert.ToInt32(userId.Value.ToString()));
            if (timeoffs == 0)
            {
                return new JsonResult(new
                {
                    Code = 200,
                    timeoffs = 0,
                });
            }

            return new JsonResult(new
            {
                code = 200,
                timeoffs,
            });

        }
    }
}
