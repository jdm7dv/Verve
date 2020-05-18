using System.Data;
using System.Data.SqlClient;

namespace Arachnode.DataSource.Next.ArachnodeNextDataSetTableAdapters
{
    public partial class CrawlRequestsTableAdapter
    {
        public IDbCommand[] SqlCommands
        {
            get { return CommandCollection; }
        }
    }

    public partial class DiscoveriesTableAdapter
    {
        public IDbCommand[] SqlCommands
        {
            get { return CommandCollection; }
        }
    }

    public partial class QueriesTableAdapter
    {
        public IDbCommand[] SqlCommands
        {
            get { return CommandCollection; }
        }
    }
}
namespace Arachnode.DataSource.Next
{
    
    
    public partial class ArachnodeNextDataSet {
    }
}
