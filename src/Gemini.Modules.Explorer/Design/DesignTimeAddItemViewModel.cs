using Gemini.Framework.Services;
using Gemini.Modules.Explorer.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace Gemini.Modules.Explorer.Design
{
    public class DesignTimeAddItemViewModel : AddItemViewModel
    {
        public DesignTimeAddItemViewModel() : base(null, null, null)
        {
        }
    }
}
