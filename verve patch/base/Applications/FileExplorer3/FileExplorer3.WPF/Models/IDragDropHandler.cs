using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FileExplorer.WPF.ViewModels.Helpers;
using FileExplorer.Models;
using FileExplorer.UIEventHub;

namespace FileExplorer.WPF.Models
{
    /// <summary>
    /// Owned by IProfile, for drag drop handling.
    /// </summary>
    public interface IDragDropHandler
    {
        Task<IDataObject> GetDataObject(IEnumerable<IEntryModel> entries);

        DragDropEffects QueryDrag(IEnumerable<IEntryModel> entries);

        void OnDragCompleted(IEnumerable<IEntryModel> entries, IDataObject da, DragDropEffects effect);

        IEnumerable<IEntryModel> GetEntryModels(IDataObject dataObject);

        bool QueryCanDrop(IEntryModel dest);
        QueryDropEffects QueryDrop(IEnumerable<IEntryModel> entries, IEntryModel dest, DragDropEffects allowedEffects);
        DragDropEffects OnDropCompleted(IEnumerable<IEntryModel> entries, IDataObject da, IEntryModel dest, DragDropEffects allowedEffects);

    }
}
