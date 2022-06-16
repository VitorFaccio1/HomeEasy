using AutoMapper;
using CrossCutting.Exceptions;
using Infrastructure.Data.Entities.User;
using Infrastructure.Data.Interfaces;
using MediatR;

namespace HomeEasy.Domain.Queries.v1.User.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<GetUsersQueryResponse>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IUserService userService, IMapper mapper)
        {
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<IEnumerable<GetUsersQueryResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await GetUsers();

            var response = _mapper.Map<IEnumerable<UserEntity>,
                IEnumerable<GetUsersQueryResponse>>(users.Where(u => u.IsActive.Equals(true)));

            return response;
        }

        private async Task<IEnumerable<UserEntity>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();

            if (!users.Any() || users == null)
                throw new UsersNotFoundException();

            return users;
        }
    }
}