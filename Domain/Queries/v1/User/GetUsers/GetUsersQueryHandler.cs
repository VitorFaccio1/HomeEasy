using AutoMapper;
using CrossCutting.Exceptions;
using Infrastructure.Data.Entities.User;
using Infrastructure.Data.Interfaces;
using MediatR;

namespace HomeEasy.Domain.Queries.v1.User.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<GetUsersQueryResponse>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IUserService userService, IMapper mapper)
        {
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<List<GetUsersQueryResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userService.GetUsersAsync();

            if (!users.Any())
                throw new UsersNotFoundException();

            var response = _mapper.Map<List<UserEntity>, List<GetUsersQueryResponse>>(users);

            return response;
        }
    }
}