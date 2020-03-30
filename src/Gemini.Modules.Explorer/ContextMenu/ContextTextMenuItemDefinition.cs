using Gemini.Framework.Commands;
using Gemini.Framework.Menus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Gemini.Modules.Explorer.ContextMenu
{
    public class ContextTextMenuItemDefinition : ContextMenuItemDefinition
    {
        private readonly string _text;
        private readonly Uri _iconSource;

        public override string Text
        {
            get { return _text; }
        }

        public override Uri IconSource
        {
            get { return _iconSource; }
        }

        public override KeyGesture KeyGesture
        {
            get { return null; }
        }

        public override CommandDefinitionBase CommandDefinition
        {
            get { return null; }
        }

        public ContextTextMenuItemDefinition(ContextMenuGroupDefinition contextMenu, int sortOrder, string text, Uri iconSource = null)
            : base(contextMenu, sortOrder)
        {
            _text = text;
            _iconSource = iconSource;
        }
    }
}
