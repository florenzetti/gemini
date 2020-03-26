using Gemini.Framework.Commands;
using Gemini.Framework.Menus;
using Gemini.Modules.Explorer.Models;
using System;
using System.Windows.Input;

namespace Gemini.Modules.Explorer.Menus
{
    public class ContextMenuDefinition : MenuDefinitionBase
    {
        public override int SortOrder { get; }

        public override string Text => "";

        public override Uri IconSource => null;

        public override KeyGesture KeyGesture => null;

        public override CommandDefinitionBase CommandDefinition => null;

        public EditorFileTemplate[] TargetTypes { get; }

        public ContextMenuDefinition(int sortOrder, params EditorFileTemplate[] targetTypes)
        {
            if (targetTypes == null)
                throw new ArgumentNullException(nameof(TargetTypes));

            SortOrder = sortOrder;
            TargetTypes = targetTypes;
        }
    }
}
