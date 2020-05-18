using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileExplorer.UIEventHub
{

    public class QueryDropEffects
    {
        public QueryDropEffects(DragDropEffects supportedEffects, DragDropEffects preferredEffects)
        {
            SupportedEffects = supportedEffects;
            PreferredEffect = preferredEffects;
        }

        public static QueryDropEffects None = new QueryDropEffects(DragDropEffects.None, DragDropEffects.None);

        public static QueryDropEffects CreateNew(DragDropEffects supportedEffects, DragDropEffects preferredEffects)
        {
            return new QueryDropEffects(supportedEffects, preferredEffects);
        }

        public static QueryDropEffects CreateNew(DragDropEffects supportedEffects)
        {
            return new QueryDropEffects(supportedEffects, supportedEffects);
        }


        public DragDropEffects SupportedEffects { get; set; }
        public DragDropEffects PreferredEffect { get; set; }
    }

  
}
