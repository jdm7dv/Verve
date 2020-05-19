using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.VersionControl.Client;
using TFSLocalBuildAgentLib.Properties;

namespace TFSLocalBuildAgentLib
{
    /// <summary>
    /// Handles interactions with the TFS server.
    /// </summary>
    public class TFSController : IDisposable
    {
        #region Fields
        /// <summary>
        /// An instance of the TFS configuration server.
        /// </summary>
        private TfsConfigurationServer configurationServer;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the TFS server URI.
        /// </summary>
        public Uri ServerUri { get; private set; }

        /// <summary>
        /// Gets or sets the name of the team project collection.
        /// </summary>
        public string TeamProjectCollectionName { get; private set; }

        /// <summary>
        /// Gets or sets the TFS source control.
        /// </summary>
        public VersionControlServer SourceControl { get; private set; }

        /// <summary>
        /// Gets or sets the collection of projects in the team project collection.
        /// </summary>
        public ReadOnlyCollection<TeamProject> TeamProjects { get; private set; }

        /// <summary>
        /// Gets the latest changeset number from the TFS server.
        /// </summary>
        public int LatestChangeSetNumber
        {
            get
            {
                return SourceControl.GetLatestChangesetId();
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the TFS controller.
        /// </summary>
        /// <param name="serverUri">The server URI.</param>
        /// <param name="teamProjectCollectionName">The team project collection name.</param>
        public TFSController(Uri serverUri, string teamProjectCollectionName)
        {
            this.ServerUri = serverUri;
            this.TeamProjectCollectionName = teamProjectCollectionName;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Connects to the TFS server.
        /// </summary>
        public void Connect()
        {
            configurationServer = new TfsConfigurationServer(ServerUri, System.Net.CredentialCache.DefaultCredentials);

            // Get the catalog of team project collections
            ReadOnlyCollection<CatalogNode> collectionNodes = configurationServer.CatalogNode.QueryChildren(
                new[] { CatalogResourceTypes.ProjectCollection },
                false, CatalogQueryOptions.None);

            // List the team project collections
            foreach (CatalogNode collectionNode in collectionNodes)
            {
                // Use the InstanceId property to get the team project collection
                Guid collectionId = new Guid(collectionNode.Resource.Properties["InstanceId"]);
                TfsTeamProjectCollection teamProjectCollection = configurationServer.GetTeamProjectCollection(collectionId);

                // Find the requested team project collection
                if (teamProjectCollection.Name.ToUpper(CultureInfo.InvariantCulture).EndsWith(this.TeamProjectCollectionName.ToUpper(CultureInfo.InvariantCulture), StringComparison.Ordinal))
                {
                    SourceControl = (VersionControlServer)teamProjectCollection.GetService(typeof(VersionControlServer));

                    this.TeamProjects = new ReadOnlyCollection<TeamProject>(SourceControl.GetAllTeamProjects(false));

                    break; // We got what we want, now exit the loop
                }
            }
        }

        /// <summary>
        /// Downloads the source for the specified changeset.
        /// </summary>
        /// <param name="projectSourcePath">The project source path.</param>
        /// <param name="changesetID">The ID of the changeset to be downloaded.</param>
        /// <param name="workspaceName">The name of the local workspace.</param>
        /// <param name="localWorkspacePath">The local workspace path.</param>
        public void DownloadSource(string projectSourcePath, int changesetID,string workspaceName, string localWorkspacePath)
        {
            if (string.IsNullOrEmpty(projectSourcePath))
            {
                throw new ArgumentNullException(projectSourcePath);
            }

            Workspace workspace = null;

            try
            {
                workspace = SourceControl.GetWorkspace(workspaceName,
                                                        SourceControl.AuthorizedUser);
            }
            catch (WorkspaceNotFoundException)
            {
                workspace = SourceControl.CreateWorkspace(workspaceName,
                                                           SourceControl.AuthorizedUser);
            }

            var serverFolder = String.Format(CultureInfo.InvariantCulture, "$/{0}/", projectSourcePath.TrimEnd('/'));
            var workingFolder = new WorkingFolder(serverFolder, localWorkspacePath);

            // Create a workspace mapping
            workspace.CreateMapping(workingFolder);
            if (!workspace.HasReadPermission)
            {
                throw new SecurityException(
                    String.Format(CultureInfo.InvariantCulture, Resources.ReadPermissionException,
                                  SourceControl.AuthorizedUser, serverFolder));
            }

            workspace.Get(new ChangesetVersionSpec(changesetID), GetOptions.GetAll);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Implements dispose to supress GC finalize.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose of all resources held.
        /// </summary>
        /// <param name="disposeManaged">If disposeManaged equals true, clean up all resources.</param>
        protected virtual void Dispose(bool disposeManaged)
        {
            if (disposeManaged)
            {
                if (configurationServer != null)
                {
                    configurationServer.Dispose();
                    configurationServer = null;
                }
            }
        }
        #endregion
    }
}
