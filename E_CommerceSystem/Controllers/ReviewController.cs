using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using E_CommerceSystem.Services;
using E_CommerceSystem.Models.DTOs;
using E_CommerceSystem.Models;
using AutoMapper;
using System.Security.Claims;

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
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }

            if (!int.TryParse(userIdString, out int userId))
            {
                return BadRequest(new { message = "Invalid User ID format." });
            }

            if (userId != input.UserId)
            {
                return Forbid(new { message = "You can only add reviews for your own purchases." });
            }

            try
            {
                _reviewService.AddReview(userId, input.ProductId, input.Rating, input.Comment);
                return Ok(new { Message = "Review added successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }


        /// <summary>
        /// Get all reviews for a specific product.
        /// </summary>
        [HttpGet("product/{productId}")]
        [AllowAnonymous]
        public IActionResult GetReviewsByProduct(int productId)
        {
            var reviews = _reviewService.GetReviewsByProductId(productId);
            var outputReviews = reviews.Select(r => new OutputReviewDTO
            {
                Id = r.Id,
                UserId = r.UserId,
                ProductId = r.ProductId,
                Rating = r.Rating,
                Comment = r.Comment,
                ReviewDate = r.ReviewDate
            }).ToList();

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
