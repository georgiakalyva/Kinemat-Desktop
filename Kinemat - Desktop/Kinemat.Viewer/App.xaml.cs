using System;
using System.ComponentModel.Composition.Hosting;
using System.Windows;
using Kinemat.Viewer.ViewModels;

namespace Kinemat.Viewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IDisposable
    {
        #region Private members

        private bool disposed = false;

        /// <summary>
        /// Catalog of exported parts from an assembly
        /// </summary>
        private AssemblyCatalog catalog;

        /// <summary>
        /// Managed Entity Framework composition container used to compose the entity graph
        /// </summary>
        private CompositionContainer compositionContainer;

        /// <summary>
        /// The id of the selected game
        /// </summary>
        internal Guid gameId;



        #endregion

        #region Public methods

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion


        #region Protected methods

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.catalog.Dispose();
                    this.compositionContainer.Dispose();
                }
            }

            this.disposed = true;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Process the command line arguments
            if(e.Args.Length == 1)
                ProcessArguments(e.Args, out gameId);

            // Catalog all exported parts within this assembly
            catalog = new AssemblyCatalog(typeof(App).Assembly);
            compositionContainer = new CompositionContainer(catalog);

            Window window = new MainWindow(compositionContainer.GetExportedValue<KinectController>());
            window.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            this.Dispose();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Extracts the command line arguments passed to the application
        /// </summary>
        /// <param name="args">The command line arguments array</param>
        /// <param name="gameId">The game id</param>
        private void ProcessArguments(string[] args, out Guid gameId)
        {
            if (args == null)
                throw new ArgumentNullException("args");

            
            gameId = new Guid(args[0]);
        }

        #endregion
    }
}
