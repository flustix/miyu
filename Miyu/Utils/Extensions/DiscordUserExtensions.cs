// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Guilds.Members;
using Miyu.Models.Guilds.Roles;

namespace Miyu.Utils.Extensions;

public static class DiscordUserExtensions
{
    public static IEnumerable<DiscordRole> GetRolesSorted(this DiscordMember member)
    {
        var roles = member.RoleIDs.Select(x => member.Client.Roles.Find(x)).OfType<DiscordRole>().ToList();
        roles.Sort((a, b) =>
        {
            var result = a.Position.CompareTo(b.Position);
            return result != 0 ? result : a.ID.CompareTo(b.ID);
        });
        roles.Reverse();
        return roles;
    }

    public static DiscordRole? GetTopRoleWithColor(this DiscordMember member)
    {
        var roles = member.GetRolesSorted();
        return roles.FirstOrDefault(x => x.Color != 0);
    }

    public static DiscordRole? GetTopRoleWithIcon(this DiscordMember member)
    {
        var roles = member.GetRolesSorted();
        return roles.FirstOrDefault(x => x.Icon != null);
    }
}
