using Microsoft.Extensions.DependencyInjection;

namespace Tally.App
{
    public abstract class BaseServiceObject
    {
        protected static T GetInstance<T>() => Dependcy.Provider.GetService<T>();
    }
}
