using AutoMapper;
using CrossCutting.Exceptions;
using Infrastructure.Data.Entities.User;
using Infrastructure.Data.Interfaces;
using MediatR;

namespace HomeEasy.Domain.Queries.v1.User.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserQueryResponse>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IUserService userService, IMapper mapper)
        {
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<GetUserQueryResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            UserEntity user = await GetUser(request);

            var response = _mapper.Map<UserEntity, GetUserQueryResponse>(user);

            return response;
        }

        private async Task<UserEntity> GetUser(GetUserQuery request)
        {
            var user = await _userService.GetUserAsync(request.Id);

            if (user == null || user.IsActive == false)
                throw new UserNotFoundException();

            return user;
        }
    }
}