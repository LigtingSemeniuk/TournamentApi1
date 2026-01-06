using HotChocolate.Types;
using TournamentApi.Domain;

namespace TournamentApi.GraphQL.Types;

public class MatchObjectType : ObjectType<Match>
{
    protected override void Configure(IObjectTypeDescriptor<Match> d)
    {
        
        d.Field(m => m.WinnerId).Type<IntType>();
        d.Field(m => m.Winner).Type<UserType>();
    }
}
