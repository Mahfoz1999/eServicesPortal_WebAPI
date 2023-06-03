using eServicesPortal_Models.Models;
using MediatR;

namespace eServicesPortal_Commends.Commends.UserCommends.Query;

public record GetCurrentUserQuery() : IRequest<User>;
public record GetAllUsersQuery() : IRequest<IEnumerable<User>>;
