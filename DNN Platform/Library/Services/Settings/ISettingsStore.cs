namespace DotNetNuke.Services.Settings
{
    using System.Runtime.CompilerServices;

    public interface ISettingsStore
    {
        T Get<T>(T @default = default(T), [CallerMemberName] string name = null);
        void Set<T>(T value, [CallerMemberName] string name = null);
        void Save();
    }
}