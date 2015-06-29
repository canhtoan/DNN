namespace DotNetNuke.Services.Settings
{
    using System.Collections;

    using DotNetNuke.Entities.Modules;

    public class ModuleScopedSettings : StringBasedSettings
    {
        public ModuleScopedSettings(int moduleId, Hashtable moduleSettings)
            : base(
                name => moduleSettings[name] as string,
                (name, value) => new ModuleController().UpdateModuleSetting(moduleId, name, value)
                ) { }
    }
}