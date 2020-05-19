using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Lab05.Views
{
    /// <summary>
    /// Interaction logic for SplitButton.xaml
    /// </summary>
    public partial class SplitButton
    {
        #region Content Property

        public new static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(SplitButton));
        public new object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        #endregion

        #region Command Property

        public static readonly DependencyProperty CommandProperty = ButtonBase.CommandProperty.AddOwner(typeof (SplitButton));
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        #endregion

        #region ItemsSource Property

        public static readonly DependencyProperty ItemsSourceProperty = ItemsControl.ItemsSourceProperty.AddOwner(typeof(SplitButton));
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        #endregion

        #region DisplayMemberPath
        public static readonly DependencyProperty DisplayMemberPathProperty = ItemsControl.DisplayMemberPathProperty.AddOwner(typeof (SplitButton));
        public string DisplayMemberPath
        {
            get { return (string) base.GetValue(DisplayMemberPathProperty); }
            set { base.SetValue(DisplayMemberPathProperty, value); }
        }
        #endregion

        #region SelectedItem
        public static readonly DependencyProperty SelectedItemProperty = Selector.SelectedItemProperty.AddOwner(typeof(SplitButton));
        public object SelectedItem
        {
            get { return base.GetValue(SelectedItemProperty); }
            set { base.SetValue(SelectedItemProperty, value); }
        }
        #endregion

        public SplitButton()
        {
            InitializeComponent();

            button.SetBinding(ButtonBase.CommandProperty, new Binding("Command") {Source = this});
            button.SetBinding(ContentControl.ContentProperty, new Binding("Content") { Source = this });
            items.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("ItemsSource") { Source = this });
            items.SetBinding(ItemsControl.DisplayMemberPathProperty, new Binding("DisplayMemberPath") {Source = this});
            items.SetBinding(Selector.SelectedItemProperty, new Binding("SelectedItem") { Source = this, Mode = BindingMode.TwoWay });
        }
    }
}
