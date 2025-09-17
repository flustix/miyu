// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Miyu.Models;

namespace Miyu.Caches;

public class SnowflakeCache<T>
    where T : Snowflake
{
    private MiyuClient client { get; }
    private ConcurrentDictionary<ulong, T> items { get; } = new();

    public IReadOnlyList<T> Items => items.Values.ToList();

    public SnowflakeCache(MiyuClient client)
    {
        this.client = client;
    }

    internal T AddOrUpdate(T item)
    {
        if (item is Snowflake impl)
            impl.Client = client;

        var existing = Find(item.ID);

        if (existing is null)
        {
            items[item.ID] = item;
            return item;
        }

        copyChanges(item, existing);
        return existing;
    }

    public T? Find(ulong id)
    {
        items.TryGetValue(id, out var item);
        return item;
    }

    public bool TryFind(ulong id, [NotNullWhen(true)] out T? item)
    {
        item = Find(id);
        return item != null;
    }

    #region AutoMapper

    private readonly IMapper mapper = new MapperConfiguration(c =>
    {
        c.ShouldMapField = f => false;
        c.ShouldMapProperty = p =>
        {
            var ignore = p.GetCustomAttribute<IgnoreAttribute>();
            if (ignore != null) return false;

            return p.GetMethod?.IsPublic == true && p.SetMethod != null;
        };

        c.CreateMap<T, T>().ForAllMembers(opt => opt.Condition((s, d, mem) =>
        {
            if (mem is null)
                return false;

            if (mem is IEnumerable enu)
                return enu.Cast<object>().Any();

            return true;
        }));
    }).CreateMapper();

    private void copyChanges(T source, T dest)
    {
        mapper.Map(source, dest);
    }

    #endregion
}
