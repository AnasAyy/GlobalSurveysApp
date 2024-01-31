using Azure.Core;
using GlobalSurveysApp.Data.Repo;
using GlobalSurveysApp.Dtos;
using GlobalSurveysApp.Dtos.AttendanceDtos.AttendanceDto;
using GlobalSurveysApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GlobalSurveysApp.Controllers.AttendanceManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceRepo _attendence;
        private const double AttendanceRadiusInKm = 0.04;
        private const double EarthRadiusKm = 6371.0;

        public AttendanceController(IAttendanceRepo attendence)
        {
            _attendence = attendence;
        }

        [Authorize, HttpPost("CheckIn")]
        public async Task<IActionResult> CheckIn(CheckRequestDto request)
        {
            var yemenTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");
            var yemenDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, yemenTimeZone);


            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            #region Check user location
            var result = await _attendence.GetUserLocation(Convert.ToInt32(userId.Value));
            if (result == null)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "عذراً، حدث خطأ ما. يرجى المحاولة مرة أخرى.",
                    MessageEn = "Oops, something went wrong. Please try again.",
                });

            }

            if (!await _attendence.CheckSerialNumber(Convert.ToInt32(userId.Value), request.SerialNumber))
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "عذراً،الرجاء التحقق من استخدام هاتفك.",
                    MessageEn = "Sorry, please verify the use of your phone.",
                });
            }
            #endregion

            #region Check Attendence
            var i = await _attendence.IsExits(Convert.ToInt32(userId.Value), yemenDate);
            if (i) return new JsonResult(new
            {
                errors = new
                {

                    Attendance = new[] { "Attendance is exits" }
                }
            });
            #endregion

            #region Check distance
            var distance = CalculateDistance(request.DesignatedLat, request.DesignatedLon, result.Latitude, result.Longitude);

            if (distance <= AttendanceRadiusInKm)
            {
                
                var attendance = new Attendenc()
                {
                    CheckIn = TimeSpan.FromHours(yemenDate.Hour) + TimeSpan.FromMinutes(yemenDate.Minute),
                    UserId = Convert.ToInt32(userId.Value),
                    Date = yemenDate,


                };

                await _attendence.CreateAttendance(attendance);
                if (!await _attendence.SaveChangesAsync())
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
            #endregion

            return BadRequest(new ErrorDto
            {
                Code = 400,
                MessageAr = "عذراً، تم رفض طلب الحضور أنت خارج نطاق الحضور.",
                MessageEn = "Sorry, Attendance request rejected. You are outside the attendance range.",
            });
        }


        [Authorize, HttpPut("CheckOut")]
        public async Task<IActionResult> CheckOut(CheckRequestDto request)
        {
            var yemenTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Aden");
            var yemenDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, yemenTimeZone);


            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            #region Check user location
            var result = await _attendence.GetUserLocation(Convert.ToInt32(userId.Value));
            if (result == null)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "عذراً، حدث خطأ ما. يرجى المحاولة مرة أخرى.",
                    MessageEn = "Oops, something went wrong. Please try again.",
                });

            }

            if (!await _attendence.CheckSerialNumber(Convert.ToInt32(userId.Value), request.SerialNumber))
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "عذراً،الرجاء التحقق من استخدام هاتفك.",
                    MessageEn = "Sorry, please verify the use of your phone.",
                });
            }
            #endregion

            #region Check Attendence
            var attendance = await _attendence.GetAttendance(Convert.ToInt32(userId.Value), yemenDate);
            if (attendance == null) return new JsonResult(new
            {
                errors = new
                {

                    Attendance = new[] { "لم تقم بتسجل الحضور بعد.!" }
                }
            });
            #endregion

            #region Check distance
            var distance = CalculateDistance(request.DesignatedLat, request.DesignatedLon, result.Latitude, result.Longitude);

            if (distance <= AttendanceRadiusInKm)
            {


                attendance.CheckOut = TimeSpan.FromHours(yemenDate.Hour) + TimeSpan.FromMinutes(yemenDate.Minute);
                _attendence.UpdateAttendance(attendance);
                if (!await _attendence.SaveChangesAsync())
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
            #endregion

            return BadRequest(new ErrorDto
            {
                Code = 400,
                MessageAr = "عذراً، تم رفض طلب الانصراف أنت خارج نطاق الحضور.",
                MessageEn = "Sorry, Attendance request rejected. You are outside the attendance range.",
            });
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var dLat = DegreesToRadians(lat2 - lat1);
            var dLon = DegreesToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = EarthRadiusKm * c;
            return distance;
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }



        #region Reports
        [Authorize, HttpGet("GetAttendanceReport")]
        public async Task<IActionResult> GetAttendanceReport(AttendanceReportRequestDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }

            DateTime fromDateTime = DateTime.Parse("2024/1/1");
            if (request.From < fromDateTime)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "عذراً، الرجاء تحديد التاريخ بدقه.",
                    MessageEn = "Sorry, please specify the date accurately.",
                });

            }

            #endregion
            var x = await _attendence.GetAttendanceRecords(Convert.ToInt32(userId.Value), request.From, request.To);
            return Ok(x);
        }
        #endregion

    }
}
