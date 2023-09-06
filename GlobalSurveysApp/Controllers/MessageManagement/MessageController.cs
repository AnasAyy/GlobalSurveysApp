using AutoMapper;
using GlobalSurveysApp.Data.Repo;
using GlobalSurveysApp.Dtos;
using GlobalSurveysApp.Dtos.MessageDtos;
using GlobalSurveysApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GlobalSurveysApp.Controllers.MessageManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepo _messageRepo;
        private readonly IAdvanceRepo _advanceRepo;
        private readonly IMapper _mapper;

        public MessageController(IMessageRepo messageRepo, IMapper mapper, IAdvanceRepo advanceRepo)
        {
            _messageRepo = messageRepo;
            _mapper = mapper;
            _advanceRepo = advanceRepo;
        }

        [Authorize(Roles = "Direct responsible, Normal user, HR, Manager"), HttpGet("GetDepartments")]
        public async Task<IActionResult> GetDepartments()
        {
            var types = await _messageRepo.GeTDepartments();
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

        [Authorize(Roles = "Direct responsible, Normal user, HR, Manager"), HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            var users = await _messageRepo.GetAllUsers(Convert.ToInt32(userId.Value));
            if (users == null)
            {
                return Ok(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "لا يوجد بيانات",
                    MessageEn = "No Data",
                });
            }

            return Ok(users);
        }

        [Authorize(Roles = "Direct responsible, Normal user, HR, Manager"), HttpGet("GetMessageType")]
        public async Task<IActionResult> GetMessageType()
        {
            var types = await _messageRepo.GetMessageType();
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



        [Authorize(Roles = "HR, Manager"), HttpPost("CreateMessage")]
        public async Task<IActionResult> CreateMessage(AddMessageRequestDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            var message = _mapper.Map<Message>(request);
            message.CreatedAt = DateTime.Now;
            message.UserId = Convert.ToInt32(userId.Value);

            await _messageRepo.CreateMessage(message);
            
            
            if (!await _messageRepo.SaveChanges())
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "عذراً، حدث خطأ ما. يرجى المحاولة مرة أخرى.",
                    MessageEn = "Oops, something went wrong. Please try again.",
                });
            }
            if(request.Type == 1038)
            {
                string fcm = await _advanceRepo.GetFCM(request.ToWhom);
                return new JsonResult(new { FCM = fcm });
            }
            return Ok();
            

        }


        [Authorize(Roles = "HR, Manager"), HttpGet("GetMessagesForTeller")]
        public async Task<IActionResult> GetMessagesForTeller(GetMessagesRequestDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion
            var result = await _messageRepo.GetMessagesForTeller(Convert.ToInt32(userId.Value));
            if (result.Any())
            {
                var list = PagedList<GetMessagesResponseDto>.ToPagedList(result, request.Page, 10);
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

        [Authorize(Roles = "HR, Manager"), HttpPut("UpdateMessage")]
        public async Task<IActionResult> UpdateMessage(UpdateMessageRequestDto request)
        {
            var result = _messageRepo.GetMessageById(request.Id);
            if (result == null)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "حاول مجددا",
                    MessageEn = "Try Again",
                });
            }

            result.Title = request.Title;
            result.ToWhom = request.ToWhom;
            result.Type = request.Type;
            result.Body = request.Body;


            _messageRepo.UpdateMessage(result);
            if (!await _messageRepo.SaveChanges())
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

        [Authorize(Roles = "HR, Manager"), HttpGet("ViewMessagesDetalisforTeller")]
        public async Task<IActionResult> ViewMessagesDetalisforTeller(GetMessageDetalisRequestDto request)
        {
            var result = await _messageRepo.ViewMessagesDetalisforTeller(request.Id);
            if (result == null)
            {
                return Ok(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "لا يوجد بيانات",
                    MessageEn = "No Data",
                });
            }
            return Ok(result);
        }

        [Authorize(Roles = "Direct responsible, Normal user, HR, Manager"), HttpGet("GetMessagesForReciver")]
        public async Task<IActionResult> GetMessagesForReciver(GetMessagesRequestDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion
            var result = await _messageRepo.GetMessagesForReciver(Convert.ToInt32(userId.Value));
            if (result.Any())
            {
                var list = PagedList<GetMessagesResponseDto>.ToPagedList(result, request.Page, 10);
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

        [Authorize(Roles = "Direct responsible, Normal user, HR, Manager"), HttpGet("GetMessagesDetailsForReciver")]
        public async Task<IActionResult> GetMessagesDetailsForReciver(GetMessageDetalisRequestDto request)
        {
            #region Check Token Data
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (userId == null)
            {
                return Unauthorized();
            }
            #endregion

            var result = await _messageRepo.ViewMessagesDetalisforReciver(Convert.ToInt32(userId.Value), request.Id);
            if (result == null)
            {
                return Ok(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "لا يوجد بيانات",
                    MessageEn = "No Data",
                });
            }
            return Ok(result);

        }

    }
}
