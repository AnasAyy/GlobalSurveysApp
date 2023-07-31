using AutoMapper;
using GlobalSurveysApp.Data.Repo;
using GlobalSurveysApp.Dtos;
using GlobalSurveysApp.Dtos.PublicListDtos;
using GlobalSurveysApp.Migrations;
using GlobalSurveysApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlobalSurveysApp.Controllers.PublicListManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicListController : ControllerBase
    {
        private readonly IPublicList _publicList;
        private readonly IMapper _mapper;

        public PublicListController(IPublicList publicList, IMapper mapper)
        {
            _publicList = publicList;
            _mapper = mapper;
        }


        [Authorize(Roles = "Admin"), HttpPost("AddItem")]
        public IActionResult AddItem(AddItemRequestDto request)
        {
            if (request.NameEN == "" || request.NameEN == null || request.NameAR == "" || request.NameAR == null || request.Type == 0 || request.Type < 0)
            {
                return BadRequest(new ErrorDto { Code = 400, MessageAr = "القيمة فارغه", MessageEn = "Empty Value" });

            }
            var item = _publicList.GetITemByName(request.NameAR, request.NameEN);
            if (item)
            {
                return BadRequest(new ErrorDto()
                {
                    Code = 400,
                    MessageAr = "العنصر موجود مسبقا",
                    MessageEn = "The item already exists",
                });
            }
            if (!_publicList.CheckType(request.Type))
            {
                return BadRequest(new ErrorDto()
                {
                    Code = 400,
                    MessageAr = "النوع غير موجود",
                    MessageEn = "The type not exists",
                });
            }
            var p = _mapper.Map<PublicList>(request);
            _publicList.Create(p);
            if (!_publicList.SaveChanges())
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

        [Authorize(Roles = "Admin"), HttpPut("UpdateItem")]
        public IActionResult UpdateItem(UpdateItemRequestDto request)
        {
            if (request.NameEN == "" || request.NameEN == null || request.NameAR == "" || request.NameAR == null)
            {
                return BadRequest(new ErrorDto { Code = 400, MessageAr = "القيمة فارغه", MessageEn = "Empty Value" });

            }
            var result = _publicList.GetById(request.Id);
            if (result == null) return NotFound();

            var existingItem = _publicList.GetITemByName(request.NameAR, request.NameAR);
            if (existingItem)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    MessageAr = "العنصر موجود مسبقا",
                    MessageEn = "The Name already exits",
                });
            }

            var item = _publicList.GetITemByName(request.NameAR, request.NameEN);
            if (item)
            {
                return BadRequest(new ErrorDto()
                {
                    Code = 400,
                    MessageAr = "العنصر موجود مسبقا",
                    MessageEn = "The item already exists",
                });
            }

            var m = _mapper.Map<PublicList>(request);
            m.UpdatedAt = DateTime.Now;
            _publicList.Update(m);
            if (!_publicList.SaveChanges())
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

        [Authorize(Roles = "Admin"), HttpGet("GetAllItems")]
        public IActionResult GetAllItems(GetAllItemsRequestDto request)
        {
            var result = _publicList.GetAll();
            if (result.Any())
            {
                var list = PagedList<GetAllItemsResponseDto>.ToPagedList(result, request.page, 10);
                Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(list.Paganation));
                return Ok(list);
            }
            return NotFound();

        }



        [Authorize(Roles = "Admin"), HttpGet("GetAllMainItems")]
        public IActionResult GetAllMainItems()
        {
            var result = _publicList.GetMainItem();
            if (result != null && result.Any())
            {
                var list = result.Select(r => new GetAllMainItemsResponseDto
                {
                    Id = r.Id,
                    NameAR = r.NameAR,
                    NameEN = r.NameEN,
                }).ToList();
                return Ok(list);
            }
            return NotFound();
        }


    }
}
