using MediatR;

namespace HomeEasy.Domain.Queries.v1.User.GetUser
{
    public class GetUserQuery : IRequest<GetUserQueryResponse>
    {
        public GetUserQuery(long id) 
        {
            Id = id;
        }

        /// <summary>
        /// The user ID.
        /// </summary>
        public long Id { get; set; }
    }
}
