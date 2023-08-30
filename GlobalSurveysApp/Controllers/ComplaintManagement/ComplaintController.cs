using AutoMapper;
using Azure.Core;
using GlobalSurveysApp.Data.Repo;
using GlobalSurveysApp.Dtos;
using GlobalSurveysApp.Dtos.AdvanceDtos;
using GlobalSurveysApp.Dtos.ApproveDtos;
using GlobalSurveysApp.Dtos.ComplaintDtos;
using GlobalSurveysApp.Dtos.TimeOffDtos;
using GlobalSurveysApp.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GlobalSurveysApp.Controllers.ComplaintManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private readonly IComplaintRepo _complaintRepo;
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public ComplaintController(IComplaintRepo complaintRepo, IUserRepo userRepo, IMapper mapper)
        {
            _complaintRepo = complaintRepo;
            _userRepo = userRepo;
            _mapper = mapper;

        }

        [Authorize(Roles = "Normal user, Direct responsible"), HttpPost("AddComplaint")]
        public async Task<IActionResult> AddComplaint(AddComplaintRequestDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            #region Add Complaint

            var complaint = _mapper.Map<Complaint>(request);
            int userIdValue;
            int.TryParse(userId.Value, out userIdValue);
            complaint.UserId = userIdValue;
            complaint.CreatedAt = DateTime.Now;
            var complaintId = await _complaintRepo.CreateComplaint(complaint);

            if (complaintId <= 0)
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

            var user = await _complaintRepo.GetUserById(userIdValue);
            FCMtokenResponseDto FC = new FCMtokenResponseDto();
            Approver approvers = new Approver();

            int? hr = await _complaintRepo.GetIdByRole("HR").FirstOrDefaultAsync();
            if (hr != 0 && hr != userIdValue)
            {
                approvers.RequestId = complaintId;
                approvers.ApproverType = 2;  // 2 For HR
                approvers.RequestType = 3;  // 3 For Complaint Request
                approvers.UserId = hr;
                approvers.CanViewed = true;
            }

            await _complaintRepo.CreateApprover(approvers);
            FC.FCMToken = await _complaintRepo.GetFCM(hr);

            if (!_userRepo.SaveChanges())
            {
                

                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "عذراً، حدث خطأ ما. يرجى المحاولة مرة أخرى.",
                    MessageEn = "Oops, something went wrong. Please try again.",
                });
            }

            var recurringJobId = "complaintJobId";
            var recurringJobOptions = new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Local
            };

            RecurringJob.AddOrUpdate(recurringJobId,  () =>  _complaintRepo.Complaint_BS(), Cron.Daily, recurringJobOptions);

            FC.MessageAR = "شكوى جديد من " + user?.FirstName + " " + user?.LastName;
            FC.MessageEN = "New Complaint from " + user?.FirstName + " " + user?.LastName;

            return new JsonResult(new
            {
                code = 200,
                FC,
            });
            #endregion
        }

        [Authorize(Roles = "Normal user, Direct responsible"), HttpGet("ViewUserCopmalint")]
        public async Task<IActionResult> ViewUserCopmalint(GetUserComplaintRequestDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion
            var result = await _complaintRepo.GetAllComplaintForUser(Convert.ToInt32(userId.Value));
            if (result.Any())
            {
                var list = PagedList<GetAllComplaintResponseDto>.ToPagedList(result, request.Page, 10);
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

        [Authorize(Roles = "Normal user, Direct responsible"), HttpGet("FilterByDate")]
        public async Task<IActionResult> FilterByDate(GetUserComplaintByDateRequestDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            var result = await _complaintRepo.GetComplaintForUserByDate(Convert.ToInt32(userId.Value), request.From, request.To);
            if (result.Any())
            {
                var list = PagedList<GetAllComplaintResponseDto>.ToPagedList(result, request.Page, 10);
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

        [Authorize(Roles = "Normal user, Direct responsible"), HttpPut("UpdateComplaint")]
        public IActionResult UpdateComplaint(UpdateComplaintRequestDto request)
        {
            var result = _complaintRepo.GetComplaintById(request.Id);
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

            result.Title = request.Title;
            result.Description = request.Description;
            result.Against = request.Against;
            result.UpdatedAt = DateTime.Now;



            _complaintRepo.UpdateComplaint(result);
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

        [Authorize(Roles = "Normal user, Direct responsible, Manager, HR"), HttpGet("ViewDetails")]
        public async Task<IActionResult> ViewDetails(ViewDetailsRequestDto request)
        {
            var result = _complaintRepo.GetComplaintById(request.Id);
            if (result == null)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "حاول مجددا",
                    MessageEn = "Try Again",
                });
            }
            var approvers = await _complaintRepo.GetLastApproverByRequestId(request.Id);
            if (approvers == null)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "حاول مجددا",
                    MessageEn = "Try Again",
                });
            }

            GetComplaintDetailsResponseDto details = new GetComplaintDetailsResponseDto();
            details.Id = result.Id;
            details.Title = result.Title;
            details.Details = result.Description;
            details.Against = result.Against;
            details.Status = approvers.Status;
            details.Note = approvers.Note;

            return new JsonResult(new
            {
                code = 200,
                details,
            });

        }

        [Authorize(Roles = "Normal user, Direct responsible, Manager, HR"), HttpGet("GetTitle")]
        public async Task<IActionResult> GetTitle()
        {
            var types = await _complaintRepo.GetTitles();
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

        [Authorize(Roles = "Manager, HR"), HttpGet("GetComplaintForApprover")]
        public async Task<IActionResult> GetComplaintForApprover(GetComplaintForApproverRequestDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            var result = await _complaintRepo.GetComplaintForApprover(Convert.ToInt32(userId.Value));

            if (result != null)
            {
                var list = PagedList<GetComplaintForApproverResponseDto>.ToPagedList(result, request.Page, 10);
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

        [Authorize(Roles = "Manager, HR"), HttpGet("GetComplaintForApproverByDate")]
        public async Task<IActionResult> GetComplaintForApproverByDate(GetComplaintForApproverByDAteRequestDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            var result = await _complaintRepo.GetComplaintForApproverByDate(Convert.ToInt32(userId.Value), request.From, request.To);

            if (result != null)
            {
                var list = PagedList<GetComplaintForApproverResponseDto>.ToPagedList(result, request.Page, 10);
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

        [Authorize(Roles = "Manager, HR"), HttpGet("GetComplaintForApproverByName")]
        public async Task<IActionResult> GetComplaintForApproverByName(GetComplaintForApproverByNameDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            var result = await _complaintRepo.GetComplaintForApproverByName(Convert.ToInt32(userId.Value), request.Name);

            if (result != null)
            {
                var list = PagedList<GetComplaintForApproverResponseDto>.ToPagedList(result, request.Page, 10);
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


        [Authorize(Roles = "Manager, HR"), HttpPut("Approve")]
        public async Task<IActionResult> Approve(ApproveComplaintRequestDto request)
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

            var complaint = _complaintRepo.GetComplaintById(request.RequestId);

            var result = await _complaintRepo.GetApprover(request.RequestId, Convert.ToInt32(userId.Value));

            if (result == null || complaint == null)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "حاول مجددا",
                    MessageEn = "Try Again",
                });
            }


            result.Status = RequestStatus.Accepted;
            result.Note = request.Note;
            result.CanViewed = false;
            result.UpdatedAt = DateTime.Now;
            await _complaintRepo.UpdateApprover(result);



            
            complaint.IsUpdated = true;
            complaint.Status = RequestStatus.Accepted;

            FC.MessageAR = "تم التعامل مع الشكوى التي قدمتها ضد " + complaint.Against;
            FC.MessageEN = "Your complaint against " + complaint.Against + " has been dealt with";
            FC.FCMToken = await _complaintRepo.GetFCM(Convert.ToInt32(complaint.UserId));

            _complaintRepo.UpdateComplaint(complaint);
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
