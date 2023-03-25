using Nuke.Common;

namespace CreativeCoders.NukeBuild.Components;

public class BaseNukeBuild : Nuke.Common.NukeBuild
{
    public T From<T>()
        where T : INukeBuild
        => (T) (object) this;
}
