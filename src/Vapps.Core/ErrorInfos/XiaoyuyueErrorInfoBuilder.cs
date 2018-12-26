using Abp.Dependency;
using Abp.Localization;
using Abp.Web.Configuration;
using Abp.Web.Models;
using System;

namespace Vapps.ErrorInfos
{
    /// <inheritdoc/>
    public class VappsErrorInfoBuilder : IErrorInfoBuilder, ISingletonDependency
    {
        private IExceptionToErrorInfoConverter Converter { get; set; }

        /// <inheritdoc/>
        public VappsErrorInfoBuilder(IAbpWebCommonModuleConfiguration configuration, ILocalizationManager localizationManager)
        {
            Converter = new VappsErrorInfoConverter(configuration, localizationManager);
        }

        /// <inheritdoc/>
        public ErrorInfo BuildForException(Exception exception)
        {
            return Converter.Convert(exception);
        }

        /// <summary>
        /// Adds an exception converter that is used by <see cref="BuildForException"/> method.
        /// </summary>
        /// <param name="converter">Converter object</param>
        public void AddExceptionConverter(IExceptionToErrorInfoConverter converter)
        {
            converter.Next = Converter;
            Converter = converter;
        }
    }
}
