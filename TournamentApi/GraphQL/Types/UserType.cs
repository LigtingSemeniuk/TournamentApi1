using HotChocolate.Types;
using TournamentApi.Domain;

namespace TournamentApi.GraphQL.Types;

public class UserType : ObjectType<User>
{
    protected override void Configure(IObjectTypeDescriptor<User> d)
    {
        d.Field(x => x.PasswordHash).Ignore();
    }
}
