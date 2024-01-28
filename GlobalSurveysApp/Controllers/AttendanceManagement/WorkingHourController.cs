using GlobalSurveysApp.Dtos.PublicListDtos;
using GlobalSurveysApp.Dtos;
using GlobalSurveysApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GlobalSurveysApp.Data.Repo;
using AutoMapper;
using GlobalSurveysApp.Dtos.AttendanceDtos.WorkingHourDto;

namespace GlobalSurveysApp.Controllers.AttendencManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkingHourController : ControllerBase
    {
        private readonly IAttendanceRepo _attendance;
        private readonly IMapper _mapper;

        public WorkingHourController(IAttendanceRepo attendance, IMapper mapper)
        {
            _attendance = attendance;
            _mapper = mapper;
            
        }
        #region Working Hour
        [Authorize(Roles = "Admin"), HttpPost("AddWorkingHoure")]
        public async Task<IActionResult> AddWorkingHoure(AddWorkingHourRequestDto request)
        {
            var WH = await _attendance.GetWorkingHourForAdd(request);
            if (WH)
            {
                return BadRequest(new ErrorDto()
                {
                    Code = 400,
                    MessageAr = "العنصر موجود مسبقا",
                    MessageEn = "The item already exists",
                });
            }


            var workinghour = _mapper.Map<WorkingHour>(request);
            
            await _attendance.CreateWorkingHour(workinghour);
            if (!await _attendance.SaveChangesAsync())
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

        [Authorize(Roles = "Admin"), HttpPut("updateWorkingHoure")]
        public async Task<IActionResult> updateWorkingHoure(UpdateWorkingHourRequestDto request)
        {
            
            var result = await _attendance.GetWorkingHourDyId(request.Id);
            if (result == null) return NotFound();

            var existingItem = await _attendance.GetWoorkingHourByParForUpdate(request);
            if (existingItem)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "العنصر موجود مسبقا",
                    MessageEn = "The Name already exits",
                });
            }



            var workinghour = _mapper.Map<WorkingHour>(request);
            workinghour.UpdateddAt = DateTime.Now;
            _attendance.UpdateWorkingHour(workinghour);
            if (!await _attendance.SaveChangesAsync())
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


        [Authorize(Roles = "Admin"), HttpGet("GetAllWorkingHours")]
        public async Task<IActionResult> GetAllWorkingHours(GetAllWorkingHoureRequestDto request)
        {
            var result = await _attendance.GetAllWorkingHoure();

            if (result != null)
            {
                var list = PagedList<GetAllWorkingHoureResponseDto>.ToPagedList(result, request.Page, 10);
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
        #endregion

        
    }
}
