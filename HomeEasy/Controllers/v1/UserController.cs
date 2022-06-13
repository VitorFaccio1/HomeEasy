using HomeEasy.Domain.Commands.v1.User.AddUser;
using HomeEasy.Domain.Queries.v1.User.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<Unit> AddUserAsync(AddUserCommand addUserCommand)
        {
            return await _mediator.Send(addUserCommand);
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
        public async Task<List<GetUsersQueryResponse>> GetUsersAsync()
        {
            return await _mediator.Send(new GetUsersQuery());
        }
    }
}