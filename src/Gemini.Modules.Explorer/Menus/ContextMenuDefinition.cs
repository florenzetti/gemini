using Gemini.Framework.Commands;
using Gemini.Framework.Menus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Gemini.Modules.Explorer.Menus
{
    public class ContextMenuDefinition : MenuDefinitionBase
    {
        public override int SortOrder => 0;

        public override string Text => "";

        public override Uri IconSource => null;

        public override KeyGesture KeyGesture => null;

        public override CommandDefinitionBase CommandDefinition => null;
    }
}