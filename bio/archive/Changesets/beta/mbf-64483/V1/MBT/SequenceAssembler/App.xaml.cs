// -----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//  Contains the Application class.
// </summary>
// -----------------------------------------------------------------------
namespace SequenceAssembler
{
    #region -- Using directives --

    using System.Windows;
    using System.Linq;
    #endregion -- Using directives --

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// This event will gets file names from the command line and sets in to application properties.
        /// </summary>
        /// <param name="e">Startup event args.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args != null && e.Args.Count() > 0)
            {
                // Set the property this will be accessed in OnLoad of SequenceAssembly class.
                this.Properties["FilesToLoad"] = e.Args;
            }

            base.OnStartup(e);
        }
    }
}
