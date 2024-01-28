using GlobalSurveysApp.Dtos.AttendanceDtos.LocationDto;
using GlobalSurveysApp.Dtos;
using GlobalSurveysApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GlobalSurveysApp.Data.Repo;
using AutoMapper;
using GlobalSurveysApp.Dtos.AttendanceDtos.WorkingHourDto;

namespace GlobalSurveysApp.Controllers.AttendanceManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkingDayController : ControllerBase
    {
        private readonly IAttendanceRepo _attendance;
        private readonly IMapper _mapper;

        public WorkingDayController(IAttendanceRepo attendanceRepo, IMapper mapper)
        {
            _attendance = attendanceRepo;
            _mapper = mapper;
        }

        #region Working Day
        [HttpPost("AddWorkingDay")]
        public async Task<IActionResult> AddWorkingDay(AddWorkingDayRequestDto request)
        {
            var WD = await _attendance.GetWorkingDayForAdd(request);
            if (WD)
            {
                return BadRequest(new ErrorDto()
                {
                    Code = 400,
                    MessageAr = "العنصر موجود مسبقا",
                    MessageEn = "The item already exists",
                });
            }


            var workingDay = _mapper.Map<WorkingDay>(request);

            await _attendance.CreateWorkingDay(workingDay);
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

        [HttpPut("updateWorkingDay")]
        public async Task<IActionResult> updateWorkingDay(UpdateWorkingDayRequestDto request)
        {

            var result = await _attendance.GetWorkingDayDyId(request.Id);
            if (result == null) return NotFound();

            var existingItem = await _attendance.GetWorkingDayByParForUpdate(request);
            if (existingItem)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "العنصر موجود مسبقا",
                    MessageEn = "The Name already exits",
                });
            }



            var workingDay = _mapper.Map<WorkingDay>(request);
            workingDay.UpdateddAt = DateTime.Now;
            _attendance.UpdateWorkingDay(workingDay);
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


        [HttpGet("GetAllWorkingDay")]
        public async Task<IActionResult> GetAllWorkingDay(GetAllWorkingDayRequestDto request)
        {
            var result = await _attendance.GetAllWorkingDays();

            if (result != null)
            {
                var list = PagedList<GetAllWorkingDayResponseDto>.ToPagedList(result, request.Page, 10);
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
