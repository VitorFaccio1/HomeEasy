using MediatR;

namespace HomeEasy.Domain.Queries.v1.User.GetUsers
{
    public class GetUsersQuery : IRequest<List<GetUsersQueryResponse>>
    {
    }
}
