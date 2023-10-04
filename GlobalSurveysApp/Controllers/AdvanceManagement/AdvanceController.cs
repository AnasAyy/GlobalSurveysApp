using AutoMapper;
using Azure.Core;
using GlobalSurveysApp.Data.Repo;
using GlobalSurveysApp.Dtos;
using GlobalSurveysApp.Dtos.AdvanceDtos;
using GlobalSurveysApp.Dtos.ApproveDtos;
using GlobalSurveysApp.Dtos.UserManagmentDtos.UserDtos;
using GlobalSurveysApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GlobalSurveysApp.Controllers.AdvanceManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvanceController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly IAdvanceRepo _advanceRepo;
        private readonly IMapper _mapper;

        public AdvanceController(IAdvanceRepo advanceRepo, IUserRepo userRepo, IMapper mapper)
        {
            _advanceRepo = advanceRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }


        [Authorize(Roles = "Normal user, Direct responsible, HR, Secretary"), HttpPost("AddAdvance")]
        public async Task<ActionResult<object>> AddAdvance(AddAdvanceRequestDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion



            #region Add Advance

            var advance = _mapper.Map<Advance>(request);
            int userIdValue;
            int.TryParse(userId.Value, out userIdValue);
            advance.UserId = userIdValue;
            advance.CreateAt = DateTime.Now;
            var advanceId = await _advanceRepo.CreateAdvance(advance);

            if (advanceId <= 0)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "عذراً، حدث خطأ ما. يرجى المحاولة مرة أخرى.",
                    MessageEn = "Oops, something went wrong. Please try again.",
                });
            }
            #endregion

            #region Add Approver

            List<Approver> approvers = new List<Approver>();
            FCMtokenResponseDto FC = new FCMtokenResponseDto();

            var user = await _advanceRepo.GetUserById(userIdValue);
            if (user != null && user.DirectResponsibleId != null)
            {
                approvers.Add(new Approver
                {
                    RequestId = advanceId,
                    ApproverType = 1, // 1 For DirectResponsible 
                    RequestType = 1, // 1 For Advance Request
                    CanViewed = true,
                    UserId = user.DirectResponsibleId
                });
                FC.FCMToken = await _advanceRepo.GetFCM(user.DirectResponsibleId);
            }

            int? hr = await _advanceRepo.GetIdByRole("HR").FirstOrDefaultAsync();
            if (hr != 0 && hr != userIdValue && user?.DirectResponsibleId != null)
            {
                approvers.Add(new Approver
                {
                    RequestId = advanceId,
                    ApproverType = 2,  // 2 For HR
                    RequestType = 1,  // 1 For Advance Request
                    UserId = hr,
                    CanViewed = false,

                });
            }
            if (hr != 0 && hr != userIdValue && user?.DirectResponsibleId == null)
            {
                approvers.Add(new Approver
                {
                    RequestId = advanceId,
                    ApproverType = 2,  // 2 For HR
                    RequestType = 1,  // 1 For Advance Request
                    UserId = hr,
                    CanViewed = true,

                });
                FC.FCMToken = await _advanceRepo.GetFCM(hr);

            }



            int? manager = await _advanceRepo.GetIdByRole("Manager").FirstOrDefaultAsync();
            if (manager != 0 && manager != userIdValue && hr != 0)
            {
                approvers.Add(new Approver
                {
                    RequestId = advanceId,
                    ApproverType = 3, // 3 For Manager
                    RequestType = 1, // 1 For Advance Request
                    UserId = manager,
                    CanViewed = false,
                });
                //FC.ManagerFCMToken = await _advanceRepo.GetFCM(manager);
            }
            if (manager != 0 && manager != userIdValue && hr == 0)
            {
                approvers.Add(new Approver
                {
                    RequestId = advanceId,
                    ApproverType = 3, // 3 For Manager
                    RequestType = 1, // 1 For Advance Request
                    UserId = manager,
                    CanViewed = true,
                });
                FC.FCMToken = await _advanceRepo.GetFCM(manager);
            }



            await _advanceRepo.CreateApprover(approvers);
            if (!_userRepo.SaveChanges())
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "عذراً، حدث خطأ ما. يرجى المحاولة مرة أخرى.",
                    MessageEn = "Oops, something went wrong. Please try again.",
                });
            }



            FC.MessageAR = "طلب سلفة جديد من " + user?.FirstName + " " + user?.LastName;
            FC.MessageEN = "New Advance request from " + user?.FirstName + " " + user?.LastName;

            return new JsonResult(new
            {
                code = 200,
                FC,
            });

            #endregion







        }

        [Authorize(Roles = "Normal user, Direct responsible, HR, Secretary"), HttpGet("ViewUserAdvance")]
        public async Task<IActionResult> ViewUserAdvance(GetUserAdvanceRequestDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion
            var result = await _advanceRepo.GetAllAdvanceForUser(Convert.ToInt32(userId.Value));
            if (result.Any())
            {
                var list = PagedList<GetAllAdvanceResponseDto>.ToPagedList(result, request.Page, 10);
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
        public async Task<IActionResult> FilterByDate(GetUserAdvanceByDateRequestDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            var result = await _advanceRepo.GetAdvanceForUserByDate(Convert.ToInt32(userId.Value), request.From, request.To);
            if (result.Any())
            {
                var list = PagedList<GetAllAdvanceResponseDto>.ToPagedList(result, request.Page, 10);
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

        [Authorize(Roles = "Normal user, Direct responsible, HR, Secretary"), HttpPut("UpdateAdvance")]
        public IActionResult UpdateAdvance(UpdateAdvanceRequestDto request)
        {
            var result = _advanceRepo.GetAdvanceById(request.Id);
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

            result.Amount = request.Amount;
            result.Currency = request.Currency;
            result.To = request.To;
            result.From = request.From;
            result.UpdatedAt = DateTime.Now;



            _advanceRepo.Update(result);
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
            var result = _advanceRepo.GetAdvanceById(request.Id);
            if (result == null)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "حاول مجددا",
                    MessageEn = "Try Again",
                });
            }
            var approvers = await _advanceRepo.GetApproversByRequestId(request.Id);
            if (approvers == null)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "حاول مجددا",
                    MessageEn = "Try Again",
                });
            }

            GetAdvanceDetailsResponseDto details = new GetAdvanceDetailsResponseDto();
            details.Id = result.Id;
            details.Currency = result.Currency;
            details.Amount = result.Amount;
            details.From = result.From;
            details.To = result.To;

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

        [Authorize(Roles = "Normal user, Direct responsible, HR, Manager, Secretary"), HttpGet("GetCurrency")]
        public async Task<IActionResult> GetCurrency()
        {
            var Currency = await _advanceRepo.GetCurrency();
            if (Currency == null)
            {
                return Ok(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "لا يوجد بيانات",
                    MessageEn = "No Data",
                });
            }

            return Ok(Currency);
        }

        [Authorize(Roles = "Manager, Direct responsible, HR, Secretary"), HttpGet("GetAdvanceForApprover")]
        public async Task<IActionResult> GetAdvanceForApprover(GetAdvanceForApproverRequestDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            var result = await _advanceRepo.GetAdvanceForApprover(Convert.ToInt32(userId.Value));

            if (result != null)
            {
                var list = PagedList<GetAdvanceForApproverResponseDto>.ToPagedList(result, request.Page, 10);
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


        [Authorize(Roles = "Manager, Direct responsible, HR, Secretary"), HttpGet("GetAddvanceForApproverByDate")]
        public async Task<IActionResult> GetAddvanceForApproverByDate(GetAdvanceForApproverByDateRequestDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            var result = await _advanceRepo.GetAdvanceForApproverByDate(Convert.ToInt32(userId.Value), request.From, request.To);

            if (result != null)
            {
                var list = PagedList<GetAdvanceForApproverResponseDto>.ToPagedList(result, request.Page, 10);
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
        [Authorize(Roles = "Manager, Direct responsible, HR, Secretary"), HttpGet("GetAddvanceForApproverByName")]
        public async Task<IActionResult> GetAddvanceForApproverByName(GetAdvanceForApproverByNameRequestDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            var result = await _advanceRepo.GetAdvanceForApproverByName(Convert.ToInt32(userId.Value), request.Name);

            if (result != null)
            {
                var list = PagedList<GetAdvanceForApproverResponseDto>.ToPagedList(result, request.Page, 10);
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
            var advance =  _advanceRepo.GetAdvanceById(request.RequestId);
           
            var result = await _advanceRepo.GetApprover(request.RequestId, Convert.ToInt32(userId.Value));
            if (result == null || advance == null)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "حاول مجددا",
                    MessageEn = "Try Again",
                });
            }

            if (userRole.Value == "Direct responsible")
            {
                if (request.Status == 1)
                {
                    result.Status = RequestStatus.Accepted;
                    
                }
                if (request.Status == 2)
                {
                    result.Status = RequestStatus.Rejected;
                }
                result.Note = request.Note;
                result.CanViewed = false;
                result.UpdatedAt = DateTime.Now;
                await _advanceRepo.UpdateApprover(result);

                int hr = await _advanceRepo.GetIdByRole("HR").FirstOrDefaultAsync();
                result = await _advanceRepo.GetApprover(request.RequestId, hr);


                if (result != null)
                {
                    result.CanViewed = true;
                    await _advanceRepo.UpdateApprover(result);
                    FC.MessageAR = "طلب سلفة جديد";
                    FC.MessageEN = "New Advance request ";
                    FC.FCMToken = await _advanceRepo.GetFCM(hr);
                }

                advance.IsUpdated = true;

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
                result.Note = request.Note;
                result.CanViewed = false;
                result.UpdatedAt = DateTime.Now;
                await _advanceRepo.UpdateApprover(result);

                int manager = await _advanceRepo.GetIdByRole("Manager").FirstOrDefaultAsync();
                result = await _advanceRepo.GetApprover(request.RequestId, manager);


                if (result != null)
                {
                    result.CanViewed = true;
                    await _advanceRepo.UpdateApprover(result);
                    FC.MessageAR = "طلب سلفة جديد  ";
                    FC.MessageEN = "New Advance request ";
                    FC.FCMToken = await _advanceRepo.GetFCM(manager);
                }
                advance.IsUpdated = true;

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
                    result.Note = request.Note;
                    result.CanViewed = false;
                    result.UpdatedAt = DateTime.Now;
                    await _advanceRepo.UpdateApprover(result);
                    var requestUser =  _advanceRepo.GetAdvanceById(result.RequestId);
                    if(requestUser == null)
                    {
                        return BadRequest();
                    }
                    advance.Status = RequestStatus.Accepted;
                    
                    FC.MessageAR = "تم قبول طلب السلفة الخاص بك ";
                    FC.MessageEN = "Your advance request has been accepted ";
                    FC.FCMToken = await _advanceRepo.GetFCM(Convert.ToInt32(requestUser.UserId));
                }
                if (request.Status == 2)
                {
                    result.Status = RequestStatus.Rejected;
                    result.Note = request.Note;
                    result.UpdatedAt = DateTime.Now;
                    await _advanceRepo.UpdateApprover(result);
                    var requestUser = _advanceRepo.GetAdvanceById(result.RequestId);
                    if (requestUser == null)
                    {
                        return BadRequest();
                    }
                    advance.Status = RequestStatus.Rejected;
                    
                    FC.MessageAR = "تم رفض طلب السلفة الخاص بك ";
                    FC.MessageEN = "Your advance request has been rejected ";
                    FC.FCMToken = await _advanceRepo.GetFCM(Convert.ToInt32(requestUser.UserId));
                }
                advance.IsUpdated = true;
            }
            _advanceRepo.Update(advance);
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
            });


        }

    }
}
