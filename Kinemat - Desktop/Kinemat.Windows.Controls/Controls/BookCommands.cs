// Type: Telerik.Windows.Controls.BookCommands
// Assembly: Telerik.Windows.Controls.Navigation, Version=2013.1.403.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7
// Assembly location: W:\Developer\Telerik\Binaries\WPF45\Telerik.Windows.Controls.Navigation.dll

using System.Windows.Input;

namespace Kinemat.Windows.Controls
{
    /// <summary>
    /// Provides a standard set of book related commands.
    /// 
    /// </summary>
    public static class BookCommands
    {
        private static RoutedUICommand[] internalCommands = new RoutedUICommand[4];

        /// <summary/>
        public static RoutedUICommand FirstPage
        {
            get
            {
                return BookCommands.EnsureCommand(BookCommands.CommandId.FirstPage);
            }
        }

        /// <summary/>
        public static RoutedUICommand PreviousPage
        {
            get
            {
                return BookCommands.EnsureCommand(BookCommands.CommandId.PreviousPage);
            }
        }

        /// <summary/>
        public static RoutedUICommand NextPage
        {
            get
            {
                return BookCommands.EnsureCommand(BookCommands.CommandId.NextPage);
            }
        }

        /// <summary/>
        public static RoutedUICommand LastPage
        {
            get
            {
                return BookCommands.EnsureCommand(BookCommands.CommandId.LastPage);
            }
        }

        static BookCommands()
        {
        }

        internal static void EnsureCommands()
        {
            for (byte index = (byte)0; (int)index <= 3; ++index)
                BookCommands.EnsureCommand((BookCommands.CommandId)index);
        }

        private static RoutedUICommand EnsureCommand(BookCommands.CommandId commandId)
        {
            if (commandId < BookCommands.CommandId.FirstPage || commandId > BookCommands.CommandId.LastPage)
                return (RoutedUICommand)null;
            if (BookCommands.internalCommands[(int)commandId] == null)
                BookCommands.internalCommands[(int)commandId] = new RoutedUICommand(BookCommands.GetPropertyName(commandId), BookCommands.GetPropertyName(commandId), typeof(BookCommands));
            return BookCommands.internalCommands[(int)commandId];
        }

        private static string GetPropertyName(BookCommands.CommandId commandId)
        {
            return ((object)commandId).ToString();
        }

        private enum CommandId : byte
        {
            FirstPage,
            PreviousPage,
            NextPage,
            LastPage,
        }
    }
}