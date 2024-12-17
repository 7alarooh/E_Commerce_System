using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using E_CommerceSystem.Services;
using E_CommerceSystem.Models.DTOs;
using E_CommerceSystem.Models;
using AutoMapper;

namespace E_CommerceSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Secures endpoints (only authenticated users can access)
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;

        public ReviewController(IReviewService reviewService, IMapper mapper)
        {
            _reviewService = reviewService;
            _mapper = mapper;
        }

        /// <summary>
        /// Add a new review for a product (Authenticated users only).
        /// </summary>
        [HttpPost("add")]
        public IActionResult AddReview([FromBody] AddReviewDTO input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _reviewService.AddReview(input.UserId, input.ProductId, input.Rating, input.Comment);
                return Ok(new { Message = "Review added successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Get all reviews for a specific product.
        /// </summary>
        [HttpGet("product/{productId}")]
        public IActionResult GetReviewsByProduct(int productId)
        {
            var reviews = _reviewService.GetReviewsByProductId(productId);
            var outputReviews = _mapper.Map<IEnumerable<OutputReviewDTO>>(reviews);
            return Ok(outputReviews);
        }

        /// <summary>
        /// Get a specific review by ID.
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetReviewById(int id)
        {
            var review = _reviewService.GetReviewById(id);
            if (review == null)
                return NotFound(new { Error = "Review not found." });

            var outputReview = _mapper.Map<OutputReviewDTO>(review);
            return Ok(outputReview);
        }
    }
}
