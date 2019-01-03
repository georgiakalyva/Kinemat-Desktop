using Kinemat.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinemat.Core.Book
{
    /// <summary>
    /// MEF export attribute that defines a contract that the exported part 
    /// is of type IInteractiveStoryBook and exposes a unique template id.
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class StoryBookTemplateAttribute : Attribute, IExportStoryBookMetadata
    {
        #region Constructor

        //public StoryBookTemplateAttribute()
        //    : base(typeof(IInteractiveStoryBook))
        //{
        
        //}

        #endregion
        #region IExportStoryBookMetadata Members

        public Guid TemplateId
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
