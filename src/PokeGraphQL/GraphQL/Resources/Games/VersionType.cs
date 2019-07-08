﻿namespace PokeGraphQL.GraphQL.Resources.Games
{
    using HotChocolate.Types;
    using PokeAPI;

    internal sealed class VersionType : BaseNamedApiObjectType<GameVersion>
    {
        /// <inheritdoc/>
        protected override void ConcreteConfigure(IObjectTypeDescriptor<GameVersion> descriptor)
        {
            descriptor.Description("Versions of the games, e.g., Red, Blue or Yellow.");
            descriptor.Field(x => x.VersionGroup)
                .Description("The version group this version belongs to.")
                .Type<VersionGroupType>()
                .Resolver((ctx, token) => ctx.Service<GameResolver>().GetVersionGroupAsync(ctx.Parent<GameVersion>().VersionGroup.Name, token));
        }
    }
}
