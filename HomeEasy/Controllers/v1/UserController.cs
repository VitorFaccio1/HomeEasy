using CrossCutting.Exceptions;
using HomeEasy.Domain.Commands.v1.User.AddUser;
using HomeEasy.Domain.Queries.v1.User.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HomeEasy.Controllers.v1
{
    [ApiController]
    [Route("api/homeasy/v1")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {   
            _mediator = mediator;
        }

        /// <summary>
        /// Add user async v1.
        /// </summary>
        /// <p>
        /// <b>Description: </b><br />
        /// This method add a user to the system.<br />
        /// </p>
        /// <br />
        [HttpPost("user")]
        public async Task<IActionResult> AddUserAsync(AddUserCommand addUserCommand)
        {
            var validationResult = await new AddUserCommandValidator().ValidateAsync(addUserCommand);

            return await GetActionResultAsync(addUserCommand, validationResult);
        }


        /// <summary>
        /// Get users async v1.
        /// </summary>
        /// <p>
        /// <b>Description: </b><br />
        /// This method gets all the users registered in the system.<br />
        /// </p>
        /// <br />
        [HttpGet("users")]
        public async Task<IActionResult> GetUsersAsync()
        {
            return await GetActionResultAsync(new GetUsersQuery());
        }

        public async Task<IActionResult> GetActionResultAsync(object request, FluentValidation.Results.ValidationResult? validatorErrors = null)
        {
            try
            {
                if (validatorErrors == null || !validatorErrors.Errors.Any())
                {
                    var response = await _mediator.Send(request);

                    return Ok(response);
                }
                else
                {
                    var errors = new List<string>();

                    foreach (var validationError in validatorErrors.Errors)
                        errors.Add(validationError.ErrorMessage);

                    return BadRequest(errors);
                }
            }
            catch (HttpCustomException ex)
            {
                return StatusCode((int)ex.ResponseCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, ex.Message);
            }
        }
    }
}