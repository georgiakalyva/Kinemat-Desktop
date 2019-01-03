using System;

namespace Kinemat.Core
{
    /// <summary>
    /// Interface for an IStoryBook exported part
    /// </summary>
    public interface IExportStoryBookMetadata
    {
        /// <summary>
        /// Unique identifier for the exported book template
        /// </summary>
        Guid TemplateId { get; }
    }
}
