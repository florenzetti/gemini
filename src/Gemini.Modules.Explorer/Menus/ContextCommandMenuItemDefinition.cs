using Caliburn.Micro;
using Gemini.Framework.Commands;
using Gemini.Framework.Menus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Gemini.Modules.Explorer.Menus
{
    public class ContextCommandMenuItemDefinition<TCommandDefinition> : ContextMenuItemDefinition
        where TCommandDefinition : CommandDefinitionBase
    {
        private readonly CommandDefinitionBase _commandDefinition;
        private readonly KeyGesture _keyGesture;

        public override string Text
        {
            get { return _commandDefinition.Text; }
        }

        public override Uri IconSource
        {
            get { return _commandDefinition.IconSource; }
        }

        public override KeyGesture KeyGesture
        {
            get { return _keyGesture; }
        }

        public override CommandDefinitionBase CommandDefinition
        {
            get { return _commandDefinition; }
        }

        public ContextCommandMenuItemDefinition(ContextMenuItemGroupDefinition group, int sortOrder)
            : base(group, sortOrder)
        {
            _commandDefinition = IoC.Get<ICommandService>().GetCommandDefinition(typeof(TCommandDefinition));
            _keyGesture = IoC.Get<ICommandKeyGestureService>().GetPrimaryKeyGesture(_commandDefinition);
        }
    }
}
