using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileExplorer.UIEventHub
{

    public interface ISupportDragHelper
    {
        ISupportDrag DragHelper { get; }
    }

    public interface ISupportDrag
    {        
        bool HasDraggables { get; }
        bool IsDraggingFrom { get; set; }
        IEnumerable<IDraggable> GetDraggables();
        DragDropEffects QueryDrag(IEnumerable<IDraggable> draggables);
        void OnDragCompleted(IEnumerable<IDraggable> draggables, DragDropEffects effect);
    }

    public class NullSupportDrag : ISupportDrag
    {
        public static NullSupportDrag Instance = new NullSupportDrag();
        public bool HasDraggables { get { return false; } }
        public bool IsDraggingFrom { get; set; }

        public IEnumerable<IDraggable> GetDraggables()
        {
            return new List<IDraggable>();
        }

        public DragDropEffects QueryDrag(IEnumerable<IDraggable> draggables)
        {
            return DragDropEffects.None;
        }

        public IDataObject GetDataObject(IEnumerable<IDraggable> draggables)
        {
            return null;
        }

        public void OnDragCompleted(IEnumerable<IDraggable> draggables, DragDropEffects effect)
        {
           
        }
    }
}
