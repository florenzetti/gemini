using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Gemini.Modules.Explorer.Models
{
    public interface ISearchable
    {
        Visibility Visibility { get; }
        void Search(string term);
    }
}
