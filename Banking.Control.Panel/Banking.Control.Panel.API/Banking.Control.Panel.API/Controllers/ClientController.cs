using Banking.Control.Panel.Model;
using Banking.Control.Panel.Model.Model.Account;
using Banking.Control.Panel.Model.Model.Client;
using Banking.Control.Panel.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Banking.Control.Panel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _client;
        private readonly string _imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        public ClientController(IClientService client)
        {
            _client = client;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel request)
        {
            var response = await _client.Authenticate(request);
            if (response == null)
            {
                return Unauthorized();
            }
            else
            {
                return Ok(response);
            }
        }

     
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ActionResult<Client>> Register(Client client)
        {
            try
            {
                // Check if the client object is null
                if (client == null)
                {
                    // Return a BadRequest if the client object is null
                    return BadRequest("Invalid client object");
                }

                // Assign a default role to the client (in case it's not provided)
                client.Role = "User";

                // Try to add the client to the system by calling the AddClient method
                var response = await _client.AddClient(client);

                // Check if the response is null, indicating the client was not added successfully
                if (response == null)
                {
                    // Return NotFound if the client could not be added (e.g., due to database or validation issues)
                    return NotFound("Client could not be added.");
                }
                else
                {
                    // Return the client object with a 200 OK status if the client was successfully added
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                // Log the exception message (optional, should be logged for further investigation)
                var message = ex.Message;
             
                // Throwing the exception again after logging it (optional depending on the requirement)
                // Ideally, you should return an error response, not just rethrow
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClientById(int id)
        {
            try
            {


                var response = await _client.GetClientById(id);
                if (response == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Client>> AddClient(Client client)
        {
            try
            {
                var response = await _client.AddClient(client);
                if (response == null) { return NotFound(); }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
        }

        [HttpPut]

        public async Task<ActionResult<Client>> UpdateClient(UpdateClientRequest client)
        {
            try
            {
                var response = await _client.UpdateClient(client);
                if (response == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Client>> DeleteClient(int id)
        {
            try
            {
                var response = await _client.DeleteClient(id);
                if (response == null)
                {
                    return NotFound();
                }

                else
                {

                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Client>>> GetAllClient(int userId, string? searchText = null,
        string sortBy = "FirstName", // default sort by FirstName
        bool ascending = true, // default sort order ascending
        int pageNumber = 1,
        int pageSize = 10)
        {
            try
            {
                var response = await _client.GetClients(userId, searchText, sortBy, ascending, pageNumber, pageSize);
                if (response == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(int clientId, IFormFile file)
        {
            try
            {


                // Ensure the directory exists
                if (!Directory.Exists(_imageDirectory))
                {
                    Directory.CreateDirectory(_imageDirectory);
                }
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }

                // Validate file extension (Optional)
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("Invalid file type.");
                }

                // Generate a unique file name
                var fileName = Guid.NewGuid().ToString() + fileExtension;
                var filePath = Path.Combine(_imageDirectory, fileName);

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return the file path or URL to access the image
                var fileUrl = $"{Request.Scheme}://{Request.Host}/images/{fileName}";
                await _client.UpdateClientProfilePath(clientId, fileUrl);
                return Ok(new { FileUrl = fileUrl });
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
        }

    }
}