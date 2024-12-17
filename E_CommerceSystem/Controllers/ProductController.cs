using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using E_CommerceSystem.Models;
using E_CommerceSystem.Models.DTOs;
using E_CommerceSystem.Services;
using AutoMapper;

namespace E_CommerceSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Secures all endpoints by default
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        /// <summary>
        /// Add a new product (Admin Only).
        /// </summary>
        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddProduct([FromBody] InputProductDTO inputProduct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var product = _mapper.Map<Product>(inputProduct);
                _productService.AddProduct(product);
                return Ok(new { Message = "Product added successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Update product details (Admin Only).
        /// </summary>
        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateProduct(int id, [FromBody] InputProductDTO inputProduct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var product = _mapper.Map<Product>(inputProduct);
                _productService.UpdateProduct(id, product);
                return Ok(new { Message = "Product updated successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Get a list of products with pagination and optional filtering by name/price range.
        /// </summary>
        [HttpGet("list")]
        public IActionResult GetProducts([FromQuery] ProductFilterDTO filter)
        {
            var products = _productService.GetFilteredProducts(filter.Name, filter.MinPrice, filter.MaxPrice, filter.PageNumber, filter.PageSize);
            var outputProducts = _mapper.Map<IEnumerable<OutputProductDTO>>(products);
            return Ok(outputProducts);
        }

        /// <summary>
        /// Get product details by ID.
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
                return NotFound(new { Error = "Product not found." });

            var outputProduct = _mapper.Map<OutputProductDTO>(product);
            return Ok(outputProduct);
        }
    }
}
