using Abp.Reflection.Extensions;
using System.CodeDom.Compiler;
using System.ComponentModel;

namespace Vapps
{
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), System.Diagnostics.DebuggerNonUserCode, System.Runtime.CompilerServices.CompilerGenerated]
    internal class Strings
    {
        private static System.Resources.ResourceManager resourceMan;
        private static System.Globalization.CultureInfo resourceCulture;
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(Strings.resourceMan, null))
                {
                    System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager("Vapps.Strings", typeof(Strings).GetAssembly());
                    Strings.resourceMan = resourceManager;
                }
                return Strings.resourceMan;
            }
        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Globalization.CultureInfo Culture
        {
            get
            {
                return Strings.resourceCulture;
            }
            set
            {
                Strings.resourceCulture = value;
            }
        }
        internal static string ConfigurationTypeMustBePublic
        {
            get
            {
                return Strings.ResourceManager.GetString("ConfigurationTypeMustBePublic", Strings.resourceCulture);
            }
        }
        internal static string ConfigurationXamlReferenceRequiresHttpContext
        {
            get
            {
                return Strings.ResourceManager.GetString("ConfigurationXamlReferenceRequiresHttpContext", Strings.resourceCulture);
            }
        }
        internal static string EmbeddedResourceUrlProviderRequired
        {
            get
            {
                return Strings.ResourceManager.GetString("EmbeddedResourceUrlProviderRequired", Strings.resourceCulture);
            }
        }
        internal static string EmptyStringNotAllowed
        {
            get
            {
                return Strings.ResourceManager.GetString("EmptyStringNotAllowed", Strings.resourceCulture);
            }
        }
        internal static string InvalidArgument
        {
            get
            {
                return Strings.ResourceManager.GetString("InvalidArgument", Strings.resourceCulture);
            }
        }
        internal static string RequiredPropertyNotYetPreset
        {
            get
            {
                return Strings.ResourceManager.GetString("RequiredPropertyNotYetPreset", Strings.resourceCulture);
            }
        }
        internal static string ResponseBodyNotSupported
        {
            get
            {
                return Strings.ResourceManager.GetString("ResponseBodyNotSupported", Strings.resourceCulture);
            }
        }
        internal static string StoreRequiredWhenNoHttpContextAvailable
        {
            get
            {
                return Strings.ResourceManager.GetString("StoreRequiredWhenNoHttpContextAvailable", Strings.resourceCulture);
            }
        }
        internal Strings()
        {
        }
    }
}
