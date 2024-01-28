using AutoMapper;
using GlobalSurveysApp.Data.Repo;
using GlobalSurveysApp.Dtos.AttendanceDtos.WorkingHourDto;
using GlobalSurveysApp.Dtos;
using GlobalSurveysApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GlobalSurveysApp.Dtos.AttendanceDtos.LocationDto;

namespace GlobalSurveysApp.Controllers.AttendanceManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IAttendanceRepo _attendance;
        private readonly IMapper _mapper;

        public LocationController(IAttendanceRepo attendance, IMapper mapper)
        {
            _attendance = attendance;
            _mapper = mapper;
        }

        #region Location
        [Authorize(Roles = "Admin"), HttpPost("AddLocation")]
        public async Task<IActionResult> AddLocation(AddLocationRequestDto request)
        {
            var L = await _attendance.GetLocationForAdd(request);
            if (L)
            {
                return BadRequest(new ErrorDto()
                {
                    Code = 400,
                    MessageAr = "العنصر موجود مسبقا",
                    MessageEn = "The item already exists",
                });
            }


            var location = _mapper.Map<Location>(request);

            await _attendance.CreateLocation(location);
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

        [Authorize(Roles = "Admin"), HttpPut("updateLocation")]
        public async Task<IActionResult> updateLocation(UpdateLocationRequestDto request)
        {

            var result = await _attendance.GetLocationDyId(request.Id);
            if (result == null) return NotFound();

            var existingItem = await _attendance.GetLocationByParForUpdate(request);
            if (existingItem)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "العنصر موجود مسبقا",
                    MessageEn = "The Name already exits",
                });
            }



            var location = _mapper.Map<Location>(request);
            location.UpdateddAt = DateTime.Now;
            _attendance.UpdateLocation(location);
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


        [Authorize(Roles = "Admin"), HttpGet("GetAllLocations")]
        public async Task<IActionResult> GetAllLocations(GetAllLocationsRequestDto request)
        {
            var result = await _attendance.GetAllLocations();

            if (result != null)
            {
                var list = PagedList<GetAllLocationResponseDto>.ToPagedList(result, request.Page, 10);
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
