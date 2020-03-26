using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gemini.Modules.Explorer.Models
{
    public static class UniqueNameExtension
    {
        public static string GetUniqueName(this IEnumerable<TreeItem> children, string name)
        {
            int i = 1;
            string newName = name;
            while (children.Any(o => o.Name == newName))
            {
                newName = $"{newName} {i}";
                i++;
            }
            return newName;
        }
    }
}
