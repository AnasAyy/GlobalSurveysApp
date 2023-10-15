using Azure.Core;
using GlobalSurveysApp.Data.Repo;
using GlobalSurveysApp.Dtos.AdvanceDtos;
using GlobalSurveysApp.Dtos;
using GlobalSurveysApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GlobalSurveysApp.Dtos.HistoryDtos;
using System.Security.Claims;
using GlobalSurveysApp.Dtos.TimeOffDtos;
using GlobalSurveysApp.Dtos.ComplaintDtos;

namespace GlobalSurveysApp.Controllers.History
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryRepo _historyRepo;
        private readonly IAdvanceRepo _advanceRepo;
        private readonly ITimeOffRepo _timeOffRepo;
        private readonly IComplaintRepo _complaintRepo;


        public HistoryController(IHistoryRepo historyRepo, IComplaintRepo complaintRepo, IAdvanceRepo advanceRepo, ITimeOffRepo timeOffRepo)
        {
            _historyRepo = historyRepo;
            _complaintRepo = complaintRepo;
            _advanceRepo = advanceRepo;
            _timeOffRepo = timeOffRepo;
        }

        [Authorize(Roles = "Manager, HR"), HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers(GetListOfUsersRequestDto request)
        {
            var result = await _historyRepo.GetListOfUsers();
            if (result != null)
            {
                var list = PagedList<GetListOfUsersResponseDto>.ToPagedList(result, request.Page, 10);
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

        [Authorize(Roles = "Manager, HR"), HttpGet("GetAllUsersByName")]
        public async Task<IActionResult> GetAllUsersByName(GetListOfUsersByNameRequestDto request)
        {
            var result = await _historyRepo.GetListOfUsersByName(request.Name);
            if (result != null)
            {
                var list = PagedList<GetListOfUsersResponseDto>.ToPagedList(result, request.Page, 10);
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

        [Authorize(Roles = "Manager, HR"), HttpGet("GetTotal")]
        public async Task<IActionResult> GetTotal(GetTotalRequestDto request)
        {

            var result = await _historyRepo.GetTotal(request.UserId);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(new ErrorDto
            {
                Code = 400,
                MessageAr = "عذراً، حدث خطأ ما. يرجى المحاولة مرة أخرى.",
                MessageEn = "Oops, something went wrong. Please try again.",
            });
        }

        [Authorize(Roles = "Manager, HR"), HttpGet("GetTotalByDate")]
        public async Task<IActionResult> GetTotalByDate(GetTotalByDateRequestDto request)
        {

            var result = await _historyRepo.GetTotalByDate(request.UserId, request.From, request.To);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(new ErrorDto
            {
                Code = 400,
                MessageAr = "عذراً، حدث خطأ ما. يرجى المحاولة مرة أخرى.",
                MessageEn = "Oops, something went wrong. Please try again.",
            });
        }

        [Authorize (Roles = "Manager, HR"), HttpGet("GetAllAdvance")]
        public async Task<IActionResult> GetAllAdvance(GetAllRequestDto request)
        {
            var result = await _advanceRepo.GetAllAdvanceForUser(request.UserId);
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

        [Authorize (Roles = "Manager, HR"), HttpGet("GetAllAdvanceByDate")]
        public async Task<IActionResult> GetAllAdvanceByDate(GetAllRequestByDateDto request)
        {
            var result = await _advanceRepo.GetAdvanceForUserByDate(request.UserId, request.From, request.To);
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

        [Authorize(Roles = "HR, Manager"), HttpGet("GetAllTimeOff")]
        public async Task<IActionResult> GetAllTimeOff(GetAllRequestDto request)
        {
            
            var result = await _timeOffRepo.GetAllTimeOffForUser(request.UserId);
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

        [Authorize(Roles = "HR, Manager"), HttpGet("GetAllTimeOffByDate")]
        public async Task<IActionResult> GetAllTimeOffByDate(GetAllRequestByDateDto request)
        {
          
            var result = await _timeOffRepo.GetTimeForUserByDate(request.UserId, request.From, request.To);
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

        [Authorize(Roles = "HR, Manager"), HttpGet("GetAllComplaint")]
        public async Task<IActionResult> GetAllComplaint(GetAllRequestDto request)
        {
            
            var result = await _complaintRepo.GetAllComplaintForUser(request.UserId);
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

        [Authorize(Roles = "HR, Manager"), HttpGet("GetAllComplaintByDate")]
        public async Task<IActionResult> GetAllComplaintByDate(GetAllRequestByDateDto request)
        {
          
            var result = await _complaintRepo.GetComplaintForUserByDate(request.UserId, request.From, request.To);
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

        [Authorize(Roles = "HR, Manager"), HttpGet("GenaralFilter")]
        public async Task<IActionResult> GenaralFilter(GenaralFilterRequestDto request)
        {
            var result = await _historyRepo.GetUsers(request.type,request.From,request.To);
            if (result != null)
            {
                var list = PagedList<GenaralFilterResponseDto>.ToPagedList(result, request.Page, 10);
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

        [Authorize(Roles = "HR, Manager"), HttpGet("GetData")]
        public async Task<IActionResult> GetData(GetAllDataResponseDto request)
        {
            if(request.type == 1)
            {
                var result = await _historyRepo.GetAdvanceForUserByDate(request.Id, request.From, request.To);
                if (result != null)
                {
                    var list = PagedList<GetAllAdvanceResponseDto>.ToPagedList(result, request.Page, 10);
                    Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(list.Paganation));
                    return Ok(list);
                }
                
            }
            else if (request.type == 2)
            {
                var result = await _historyRepo.GetTimeForUserByDate(request.Id, request.From, request.To);
                if (result != null)
                {
                    var list = PagedList<GetAllTimeOffResponseDto>.ToPagedList(result, request.Page, 10);
                    Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(list.Paganation));
                    return Ok(list);
                }
                
            }
            else if(request.type == 3)
            {
                var result = await _historyRepo.GetComplaintForUserByDate(request.Id, request.From, request.To);
                if (result != null)
                {
                    var list = PagedList<GetAllComplaintResponseDto>.ToPagedList(result, request.Page, 10);
                    Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(list.Paganation));
                    return Ok(list);
                }
                
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


