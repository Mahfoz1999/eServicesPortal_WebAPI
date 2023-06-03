using eServicesPortal_Commends.Commends.UserCommends.Query;
using eServicesPortal_Database.DatabaseConnection;
using eServicesPortal_Models.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eServicesPortal_Commends.Commends.UserCommends.QueryHandler;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<User>>
{
    private readonly eServicesPortalDbContext Context;
    private readonly IMediator mediator;
    public GetAllUsersQueryHandler(eServicesPortalDbContext Context, IMediator mediator)
    {
        this.Context = Context;
        this.mediator = mediator;
    }
    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await Context.Users.ToListAsync();
    }
}
