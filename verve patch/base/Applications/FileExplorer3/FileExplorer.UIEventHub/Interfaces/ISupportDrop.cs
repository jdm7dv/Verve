using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileExplorer.UIEventHub
{

    public interface ISupportDropHelper
    {
        ISupportDrop DropHelper { get; }
    }

    /// <summary>
    /// Implemented by ViewModel that support Item Drop.
    /// </summary>
    public interface ISupportDrop : IUIAware
    {
        /// <summary>
        /// Set by UIEventHub, whether an item is dragging over the current item.
        /// </summary>
        bool IsDraggingOver { get;  set; }
        /// <summary>
        /// Whether current item is support drop at this time.
        /// </summary>
        bool IsDroppable { get; }

        /// <summary>
        /// <para>Tooltip when item drop over the current item, e.g. "{MethodLabel} {ItemLabel} to {ISupportDrop.DisplayName}"</para>
        /// <para>Where MethodLabel is DragDropEffects, ItemLabel is the IUIAware.DisplayName of sender model,</para>
        /// <para>ISupportDrop is the current item.</para>
        /// </summary>
        string DropTargetLabel { get; }
        
        /// <summary>
        /// Indicate an item drag over current item, and return possible drop effects.
        /// </summary>
        /// <param name="draggables"></param>
        /// <param name="allowedEffects"></param>
        /// <returns></returns>
        QueryDropEffects QueryDrop(IEnumerable<IDraggable> draggables, DragDropEffects allowedEffects);

        /// <summary>
        /// Indicate user drop an item over current item, and return result effect.
        /// </summary>
        /// <param name="draggables"></param>
        /// <param name="allowedEffects"></param>
        /// <returns></returns>
        DragDropEffects Drop(IEnumerable<IDraggable> draggables, DragDropEffects allowedEffects);
    }

    public class NullSupportDrop : ISupportDrop
    {
        public static ISupportDrop Instance = new NullSupportDrop();
        public bool IsDraggingOver { get; set; }
        public bool IsDroppable { get { return false; }}
        public string DropTargetLabel { get { return null; }}
        public string DisplayName { get; set; }
        public QueryDropEffects QueryDrop(IDataObject da, DragDropEffects allowedEffects)
        {
            return QueryDropEffects.None;
        }


        public DragDropEffects Drop(IEnumerable<IDraggable> draggables, DragDropEffects allowedEffects)
        {
            return DragDropEffects.None;
        }


        public QueryDropEffects QueryDrop(IEnumerable<IDraggable> draggables, DragDropEffects allowedEffects)
        {
            return QueryDropEffects.None;
        }
    }
}
