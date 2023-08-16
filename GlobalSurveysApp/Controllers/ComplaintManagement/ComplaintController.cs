﻿using AutoMapper;
using Azure.Core;
using GlobalSurveysApp.Data.Repo;
using GlobalSurveysApp.Dtos;
using GlobalSurveysApp.Dtos.ComplaintDtos;
using GlobalSurveysApp.Dtos.TimeOffDtos;
using GlobalSurveysApp.Models;
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
    }
}
