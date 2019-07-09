﻿namespace PokeGraphQL.GraphQL.Resources.Games
{
    using System.Linq;
    using System.Threading.Tasks;
    using HotChocolate.Types;
    using PokeAPI;
    using PokeGraphQL.GraphQL.Resources.Locations;

    internal sealed class VersionGroupType : BaseNamedApiObjectType<VersionGroup>
    {
        /// <inheritdoc/>
        protected override void ConcreteConfigure(IObjectTypeDescriptor<VersionGroup> descriptor)
        {
            // TODO implement ignored fields
            descriptor.Description("Version groups categorize highly similar versions of the games.");
            descriptor.Field(x => x.Order)
                .Description("Order for sorting. Almost by date of release, except similar versions are grouped together.");
            descriptor.Field(x => x.Generation)
                .Description("The generation this version was introduced in.")
                .Type<GenerationType>()
                .Resolver((ctx, token) => ctx.Service<GameResolver>().GetGenerationAsync(ctx.Parent<VersionGroup>().Generation.Name, token));
            descriptor.Field(x => x.MoveLearnMethods)
                .Description("A list of methods in which pokemon can learn moves in this version group.")
                .Ignore();
            descriptor.Field(x => x.Pokedices)
                .Name("pokedexes")
                .Description("A list of pokedexes introduced in this version group.")
                .Type<ListType<PokedexType>>()
                .Resolver(async (ctx, token) =>
                {
                    var resolver = ctx.Service<GameResolver>();
                    var resourceTasks = ctx.Parent<VersionGroup>()
                        .Pokedices
                        .Select(pokedex => resolver.GetPokedexAsync(pokedex.Name, token));
                    return await Task.WhenAll(resourceTasks);
                });
            descriptor.Field(x => x.Regions)
                .Description("A list of regions that can be visited in this version group.")
                .Type<ListType<RegionType>>()
                .Resolver(async (ctx, token) =>
                {
                    var resolver = ctx.Service<LocationResolver>();
                    var resourceTasks = ctx.Parent<VersionGroup>()
                        .Regions
                        .Select(region => resolver.GetRegionAsync(region.Name, token));
                    return await Task.WhenAll(resourceTasks);
                });
            descriptor.Field(x => x.Versions)
                .Description("The versions this version group owns.")
                .Type<ListType<VersionType>>()
                .Resolver(async (ctx, token) =>
                {
                    var resolver = ctx.Service<GameResolver>();
                    var resourceTasks = ctx.Parent<VersionGroup>()
                        .Versions
                        .Select(version => resolver.GetVersionAsync(version.Name, token));
                    return await Task.WhenAll(resourceTasks);
                });
        }
    }
}
