// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using JetBrains.Annotations;
using Miyu.Events;

namespace Miyu.Attributes;

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Method)]
public class EventListenerAttribute : Attribute
{
    public EventType TargetType { get; }

    public EventListenerAttribute(EventType targetType)
    {
        TargetType = targetType;
    }
}
