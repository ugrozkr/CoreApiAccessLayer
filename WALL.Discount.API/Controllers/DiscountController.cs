using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Wall.Discount.Repository.Interface;

namespace WALL.Discount.API.Controllers
{
    [ApiController]
    [Route("api/discount")]
    public class DiscountController : Controller
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;

        public DiscountController(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetDiscountForCode(string code)
        {
            var discount = await _discountRepository.GetDiscount(code);
            if (discount == null)
                return NotFound();
            return Ok(discount);
        }

        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("{code}")]
        public async Task<IActionResult> GetDiscountForCode(Guid Id)
        {
            var discount = await _discountRepository.GetDiscount(Id);
            if (discount == null)
                return NotFound();
            return Ok(discount);
        }

        //[HttpPut("use/{Id}")]
        //public async Task<IActionResult> UseDiscount(Guid Id)
        //{
        //    return Ok();
        //}
    }
}
