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

namespace GlobalSurveysApp.Controllers.History
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryRepo _HistoryRepo;

        public HistoryController(IHistoryRepo historyRepo)
        {
            _HistoryRepo = historyRepo;
        }

        [Authorize(Roles = "Manager, HR"), HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers(GetListOfUsersRequestDto request)
        {
            var result = await _HistoryRepo.GetListOfUsers();
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
            var result = await _HistoryRepo.GetListOfUsersByName(request.Name);
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

        [ HttpGet("GetTotal")]
        public async Task<IActionResult> GetTotal(GetTotalRequestDto request)
        {

            var result = await _HistoryRepo.GetTotal(request.UserId);
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
    }
    }


