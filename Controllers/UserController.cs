using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Enums;
using AutoMapper;

namespace InsurancePolicyManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;

        public UserController(IUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]   
        public async Task<ActionResult> GetAll()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        [HttpPost]
      
        public async Task<ActionResult> Create([FromBody] UserRegistrationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var user = await _service.CreateAsync(dto);
            return user == null ? BadRequest("Email already exists") : Ok(user);
        }

        [HttpGet("email/{email}")]
     
        public async Task<ActionResult> GetByEmail(string email)
        {
            var user = await _service.GetByEmailAsync(email);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpGet("{id}")]
       
        public async Task<ActionResult> GetById(string id)
        {
            var user = await _service.GetByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPatch("{id}")]
      
        public async Task<ActionResult> Update(string id, [FromBody] UserUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var updated = await _service.UpdateAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }



        [HttpDelete("{id}")]
      
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _service.DeleteAsync(id);
            return !result ? NotFound() : Ok(new { Message = "User deleted successfully" });
        }

        [HttpPatch("{customerId}/assign-agent")]
        public async Task<ActionResult> AssignAgent(string customerId, [FromBody] AgentAssignmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _service.AssignAgentAsync(customerId, dto.AgentId, dto.UnderwriterId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("unassigned-customers")]
        public async Task<ActionResult> GetUnassignedCustomers()
        {
            var customers = await _service.GetUnassignedCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("agents")]
        public async Task<ActionResult> GetAgents([FromQuery] string? location = null)
        {
            var agents = await _service.GetAgentsAsync(location);
            return Ok(agents);
        }

        [HttpPost("agents-by-location")]
        public async Task<ActionResult> GetAgentsByLocation([FromBody] AgentsByLocationRequest request)
        {
            var agents = await _service.GetAgentsAsync(request.Location);
            return Ok(agents);
        }

        [HttpPost("by-email")]
        public async Task<ActionResult> GetByEmailSecure([FromBody] UserByEmailRequest request)
        {
            var user = await _service.GetByEmailAsync(request.Email);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost("by-id")]
        public async Task<ActionResult> GetByIdSecure([FromBody] UserByIdRequest request)
        {
            var user = await _service.GetByIdAsync(request.UserId);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPatch("update")]
        public async Task<ActionResult> UpdateSecure([FromBody] UserUpdateSecureRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var dto = new UserUpdateDto
            {
                FullName = request.FullName,
                Email = request.Email,
                Role = (UserRole)request.Role,
                Mobile = request.Mobile,
                DateOfBirth = DateTime.Parse(request.DateOfBirth),
                Gender = (Gender)request.Gender,
                AadhaarNumber = request.AadhaarNumber,
                PANNumber = request.PanNumber,
                Address = request.Address,
                KYCStatus = (KYCStatus)request.KycStatus
            };
            var updated = await _service.UpdateAsync(request.UserId, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteSecure([FromBody] UserDeleteRequest request)
        {
            var result = await _service.DeleteAsync(request.UserId);
            return !result ? NotFound() : Ok(new { Message = "User deleted successfully" });
        }

        [HttpPatch("assign-agent")]
        public async Task<ActionResult> AssignAgentSecure([FromBody] AgentAssignmentSecureRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _service.AssignAgentAsync(request.CustomerId, request.AgentId, request.UnderwriterId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("underwriters")]
        public async Task<ActionResult> GetUnderwriters()
        {
            var underwriters = await _service.GetUnderwritersAsync();
            return Ok(underwriters);
        }

        [HttpGet("customer-assignments")]
        public async Task<ActionResult> GetCustomerAssignments()
        {
            var assignments = await _service.GetCustomerAssignmentsAsync();
            return Ok(assignments);
        }

        [HttpGet("management-data")]
        public async Task<ActionResult> GetManagementData()
        {
            var users = await _service.GetAllAsync();
            var agents = await _service.GetAgentsAsync();
            var underwriters = await _service.GetUnderwritersAsync();
            var unassignedCustomers = await _service.GetUnassignedCustomersAsync();
            var customerAssignments = await _service.GetCustomerAssignmentsAsync();

            var result = new
            {
                users,
                agents,
                underwriters,
                unassignedCustomers,
                customerAssignments
            };

            Response.Headers.Add("Cache-Control", "public, max-age=120");
            return Ok(result);
        }

        [HttpPost("upload-profile-image")]
        public async Task<ActionResult> UploadProfileImage([FromForm] ProfileImageUploadDto dto)
        {
            if (string.IsNullOrEmpty(dto.UserId) || dto.ProfileImage == null)
            {
                return BadRequest("User ID and profile image are required");
            }

            try
            {
                var result = await _service.UploadProfileImageAsync(dto.UserId, dto.ProfileImage);
                return Ok(new { ProfileImagePath = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error uploading profile image: {ex.Message}");
            }
        }
    }
}
