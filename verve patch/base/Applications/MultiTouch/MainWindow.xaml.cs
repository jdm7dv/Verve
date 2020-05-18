using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace TouchRect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            canvas.ManipulationStarting += new EventHandler<ManipulationStartingEventArgs>(image_ManipulationStarting);
            canvas.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(image_ManipulationDelta);
            this.ManipulationBoundaryFeedback += new EventHandler<ManipulationBoundaryFeedbackEventArgs>(rect2_ManipulationBoundaryFeedback);

            //Optional 
            image.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(rect2_ManipulationCompleted);

              
             
            
        }

        void rect2_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Completed"); 
        }

        void rect2_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Mani boundary " + e.BoundaryFeedback.Translation.ToString()); 
        }

void image_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
{   
    //canvas is the parent of the image starting the manipulation;
    //Container does not have to be parent, but that is the most common scenario
    e.ManipulationContainer = canvas; 
    // you could set the mode here too 
    // e.Mode = ManipulationModes.All;             
}

void image_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
{
    //this just gets the source. 
    // I cast it to FE because I wanted to use ActualWidth for Center. You could try RenderSize as alternate
    var element = e.Source as FrameworkElement; 
    if ( element != null ) 
    { 
        //e.DeltaManipulation has the changes 
        // Scale is a delta multiplier; 1.0 is last size,  (so 1.1 == scale 10%, 0.8 = shrink 20%) 
        // Rotate = Rotation, in degrees
        // Pan = Translation, == Translate offset, in Device Independent Pixels 
         

        var deltaManipulation = e.DeltaManipulation; 
        var matrix  = ((MatrixTransform)element.RenderTransform).Matrix;            
        // find the old center; arguaby this could be cached 
        Point center =  new Point ( element.ActualWidth/2, element.ActualHeight/2) ;
        // transform it to take into account transforms from previous manipulations 
        center = matrix.Transform(center); 
        //this will be a Zoom. 
        matrix.ScaleAt(deltaManipulation.Scale.X, deltaManipulation.Scale.Y, center.X, center.Y); 
        // Rotation 
        matrix.RotateAt(e.DeltaManipulation.Rotation, center.X, center.Y);             
        //Translation (pan) 
        matrix.Translate(e.DeltaManipulation.Translation.X, e.DeltaManipulation.Translation.Y);

        ((MatrixTransform)element.RenderTransform).Matrix = matrix; 

        e.Handled = true;

        System.Diagnostics.Debug.WriteLine("Matrix: " + matrix.ToString()); 
    }
}


       
 }


#if FALSE 

    [UidProperty("Uid")]
public class UIElement : Visual, IAnimatable, IInputElement
{
    // Fields
  
    public static readonly DependencyProperty AreAnyTouchesCapturedProperty;
 
    public static readonly DependencyProperty AreAnyTouchesCapturedWithinProperty;
  
    public static readonly DependencyProperty AreAnyTouchesDirectlyOverProperty;
   
    public static readonly DependencyProperty AreAnyTouchesOverProperty;
    
    public static readonly RoutedEvent LostTouchCaptureEvent;
    public static readonly RoutedEvent ManipulationBoundaryFeedbackEvent;
    public static readonly RoutedEvent ManipulationCompletedEvent;
    public static readonly RoutedEvent ManipulationDeltaEvent;
    public static readonly RoutedEvent ManipulationInertiaStartingEvent;
    public static readonly RoutedEvent ManipulationStartedEvent;
    public static readonly RoutedEvent ManipulationStartingEvent;
    
       
    public static readonly RoutedEvent PreviewTouchDownEvent;
    public static readonly RoutedEvent PreviewTouchMoveEvent;
    public static readonly RoutedEvent PreviewTouchUpEvent;
    public static readonly RoutedEvent TouchDownEvent;
    public static readonly RoutedEvent TouchEnterEvent;
    public static readonly RoutedEvent TouchLeaveEvent;
    public static readonly RoutedEvent TouchMoveEvent;
    public static readonly RoutedEvent TouchUpEvent;

    internal static readonly TouchesCapturedWithinProperty TouchesCapturedWithinProperty;
    internal static readonly TouchesOverProperty TouchesOverProperty;
     public static readonly DependencyProperty UidProperty;
    [CommonDependencyProperty]
    public static readonly DependencyProperty VisibilityProperty;

    // Events
    public event DragEventHandler DragEnter;
    public event DragEventHandler DragLeave;
    public event DragEventHandler DragOver;
    public event DragEventHandler Drop;
    public event DependencyPropertyChangedEventHandler FocusableChanged;
    public event GiveFeedbackEventHandler GiveFeedback;
    public event RoutedEventHandler GotFocus;
    public event KeyboardFocusChangedEventHandler GotKeyboardFocus;
    public event MouseEventHandler GotMouseCapture;
    public event StylusEventHandler GotStylusCapture;
    [CustomCategory("Touch_Category")]
    public event EventHandler<TouchEventArgs> GotTouchCapture;
    public event DependencyPropertyChangedEventHandler IsEnabledChanged;
    public event DependencyPropertyChangedEventHandler IsHitTestVisibleChanged;
    public event DependencyPropertyChangedEventHandler IsKeyboardFocusedChanged;
    public event DependencyPropertyChangedEventHandler IsKeyboardFocusWithinChanged;
    public event DependencyPropertyChangedEventHandler IsMouseCapturedChanged;
    public event DependencyPropertyChangedEventHandler IsMouseCaptureWithinChanged;
    public event DependencyPropertyChangedEventHandler IsMouseDirectlyOverChanged;
    public event DependencyPropertyChangedEventHandler IsStylusCapturedChanged;
    public event DependencyPropertyChangedEventHandler IsStylusCaptureWithinChanged;
    public event DependencyPropertyChangedEventHandler IsStylusDirectlyOverChanged;
    public event DependencyPropertyChangedEventHandler IsVisibleChanged;
    public event KeyEventHandler KeyDown;
    public event KeyEventHandler KeyUp;
    public event EventHandler LayoutUpdated;
    public event RoutedEventHandler LostFocus;
    public event KeyboardFocusChangedEventHandler LostKeyboardFocus;
    public event MouseEventHandler LostMouseCapture;
    public event StylusEventHandler LostStylusCapture;
    [CustomCategory("Touch_Category")]
    public event EventHandler<TouchEventArgs> LostTouchCapture;
    [CustomCategory("Touch_Category")]
    public event EventHandler<ManipulationBoundaryFeedbackEventArgs> ManipulationBoundaryFeedback;
    [CustomCategory("Touch_Category")]
    public event EventHandler<ManipulationCompletedEventArgs> ManipulationCompleted;
    [CustomCategory("Touch_Category")]
    public event EventHandler<ManipulationDeltaEventArgs> ManipulationDelta;
    [CustomCategory("Touch_Category")]
    public event EventHandler<ManipulationInertiaStartingEventArgs> ManipulationInertiaStarting;
    [CustomCategory("Touch_Category")]
    public event EventHandler<ManipulationStartedEventArgs> ManipulationStarted;
    [CustomCategory("Touch_Category")]
    public event EventHandler<ManipulationStartingEventArgs> ManipulationStarting;
    public event MouseButtonEventHandler MouseDown;
    public event MouseEventHandler MouseEnter;
    public event MouseEventHandler MouseLeave;
    public event MouseButtonEventHandler MouseLeftButtonDown;
    public event MouseButtonEventHandler MouseLeftButtonUp;
    public event MouseEventHandler MouseMove;
    public event MouseButtonEventHandler MouseRightButtonDown;
    public event MouseButtonEventHandler MouseRightButtonUp;
    public event MouseButtonEventHandler MouseUp;
    public event MouseWheelEventHandler MouseWheel;
    public event DragEventHandler PreviewDragEnter;
    public event DragEventHandler PreviewDragLeave;
    public event DragEventHandler PreviewDragOver;
    public event DragEventHandler PreviewDrop;
    public event GiveFeedbackEventHandler PreviewGiveFeedback;
    public event KeyboardFocusChangedEventHandler PreviewGotKeyboardFocus;
    public event KeyEventHandler PreviewKeyDown;
    public event KeyEventHandler PreviewKeyUp;
    public event KeyboardFocusChangedEventHandler PreviewLostKeyboardFocus;
    public event MouseButtonEventHandler PreviewMouseDown;
    public event MouseButtonEventHandler PreviewMouseLeftButtonDown;
    public event MouseButtonEventHandler PreviewMouseLeftButtonUp;
    public event MouseEventHandler PreviewMouseMove;
    public event MouseButtonEventHandler PreviewMouseRightButtonDown;
    public event MouseButtonEventHandler PreviewMouseRightButtonUp;
    public event MouseButtonEventHandler PreviewMouseUp;
    public event MouseWheelEventHandler PreviewMouseWheel;
    public event QueryContinueDragEventHandler PreviewQueryContinueDrag;
    public event StylusButtonEventHandler PreviewStylusButtonDown;
    public event StylusButtonEventHandler PreviewStylusButtonUp;
    public event StylusDownEventHandler PreviewStylusDown;
    public event StylusEventHandler PreviewStylusInAirMove;
    public event StylusEventHandler PreviewStylusInRange;
    public event StylusEventHandler PreviewStylusMove;
    public event StylusEventHandler PreviewStylusOutOfRange;
    public event StylusSystemGestureEventHandler PreviewStylusSystemGesture;
    public event StylusEventHandler PreviewStylusUp;
    public event TextCompositionEventHandler PreviewTextInput;
    [CustomCategory("Touch_Category")]
    public event EventHandler<TouchEventArgs> PreviewTouchDown;
    [CustomCategory("Touch_Category")]
    public event EventHandler<TouchEventArgs> PreviewTouchMove;
    [CustomCategory("Touch_Category")]
    public event EventHandler<TouchEventArgs> PreviewTouchUp;
    public event QueryContinueDragEventHandler QueryContinueDrag;
    public event QueryCursorEventHandler QueryCursor;
    public event StylusButtonEventHandler StylusButtonDown;
    public event StylusButtonEventHandler StylusButtonUp;
    public event StylusDownEventHandler StylusDown;
    public event StylusEventHandler StylusEnter;
    public event StylusEventHandler StylusInAirMove;
    public event StylusEventHandler StylusInRange;
    public event StylusEventHandler StylusLeave;
    public event StylusEventHandler StylusMove;
    public event StylusEventHandler StylusOutOfRange;
    public event StylusSystemGestureEventHandler StylusSystemGesture;
    public event StylusEventHandler StylusUp;
    public event TextCompositionEventHandler TextInput;
    [CustomCategory("Touch_Category")]
    public event EventHandler<TouchEventArgs> TouchDown;
    [CustomCategory("Touch_Category")]
    public event EventHandler<TouchEventArgs> TouchEnter;
    [CustomCategory("Touch_Category")]
    public event EventHandler<TouchEventArgs> TouchLeave;
    [CustomCategory("Touch_Category")]
    public event EventHandler<TouchEventArgs> TouchMove;
    [CustomCategory("Touch_Category")]
    public event EventHandler<TouchEventArgs> TouchUp;

    // Methods
    [SecurityCritical, SecurityTreatAsSafe]
    static UIElement();
    public UIElement();
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public void AddHandler(RoutedEvent routedEvent, Delegate handler);
    [FriendAccessAllowed]
    internal static void AddHandler(DependencyObject d, RoutedEvent routedEvent, Delegate handler);
    public void AddHandler(RoutedEvent routedEvent, Delegate handler, bool handledEventsToo);
    private void addLayoutUpdatedHandler(EventHandler handler, LayoutEventList.ListItem item);
    internal void AddSynchronizedInputPostOpportunityHandler(EventRoute route, RoutedEventArgs args);
    internal void AddSynchronizedInputPreOpportunityHandler(EventRoute route, RoutedEventArgs args);
    internal virtual void AddSynchronizedInputPreOpportunityHandlerCore(EventRoute route, RoutedEventArgs args);
    public void AddToEventRoute(EventRoute route, RoutedEventArgs e);
    internal virtual void AddToEventRouteCore(EventRoute route, RoutedEventArgs args);
    internal virtual object AdjustEventSource(RoutedEventArgs args);
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public void ApplyAnimationClock(DependencyProperty dp, AnimationClock clock);
    public void ApplyAnimationClock(DependencyProperty dp, AnimationClock clock, HandoffBehavior handoffBehavior);
    public void Arrange(Rect finalRect);
    protected virtual void ArrangeCore(Rect finalRect);
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public void BeginAnimation(DependencyProperty dp, AnimationTimeline animation);
    public void BeginAnimation(DependencyProperty dp, AnimationTimeline animation, HandoffBehavior handoffBehavior);
    private static void BitmapScalingMode_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e);
    internal virtual bool BlockReverseInheritance();
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal void BuildRoute(EventRoute route, RoutedEventArgs args);
    internal virtual bool BuildRouteCore(EventRoute route, RoutedEventArgs args);
    internal static void BuildRouteHelper(DependencyObject e, EventRoute route, RoutedEventArgs args);
    internal void CancelSynchronizedInput();
    public bool CaptureMouse();
    public bool CaptureStylus();
    public bool CaptureTouch(TouchDevice touchDevice);
    private static void ClearTypeHint_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e);
    private static void Clip_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e);
    private static void ClipToBounds_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e);
    private static object CoerceIsEnabled(DependencyObject d, object value);
    private static object CoerceIsHitTestVisible(DependencyObject d, object value);
    private void CoerceStylusProperties();
    private static RoutedEvent CrackMouseButtonEvent(MouseButtonEventArgs e);
    private static void CrackMouseButtonEventAndReRaiseEvent(DependencyObject sender, MouseButtonEventArgs e);
    internal AutomationPeer CreateAutomationPeer();
    internal AutomationPeer CreateGenericRootAutomationPeer();
    private static void EdgeMode_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e);
    private void ensureClip(Size layoutSlotSize);
    [FriendAccessAllowed]
    internal void EnsureEventHandlersStore();
    private void ensureInvisible(bool collapsed);
    private void ensureVisible();
    [UIPermission(SecurityAction.InheritanceDemand, Window=UIPermissionWindow.AllWindows)]
    internal sealed override void EvaluateAnimatedValueCore(DependencyProperty dp, PropertyMetadata metadata, ref EffectiveValueEntry entry);
    private void EventHandlersStoreAdd(EventPrivateKey key, Delegate handler);
    private void EventHandlersStoreRemove(EventPrivateKey key, Delegate handler);
    public bool Focus();
    [SecurityCritical]
    internal override void FreeContent(DUCE.Channel channel);
    public object GetAnimationBaseValue(DependencyProperty dp);
    internal AutomationPeer GetAutomationPeer();
    internal override Rect GetContentBounds();
    internal override DrawingGroup GetDrawing();
    internal override Rect GetHitTestBounds();
    private static object GetIsVisible(DependencyObject d, out BaseValueSourceInternal source);
    protected virtual Geometry GetLayoutClip(Size layoutSlotSize);
    private LayoutEventList.ListItem getLayoutUpdatedHandler(EventHandler d);
    internal DependencyObject GetUIParent();
    internal DependencyObject GetUIParent(bool continuePastVisualTree);
    protected internal virtual DependencyObject GetUIParentCore();
    internal DependencyObject GetUIParentNo3DTraversal();
    internal void GetUIParentOrICH(out UIElement uiParent, out IContentHost ich);
    internal UIElement GetUIParentWithinLayoutIsland();
    protected override GeometryHitTestResult HitTestCore(GeometryHitTestParameters hitTestParameters);
    protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters);
    private void Initialize();
    public IInputElement InputHitTest(Point point);
    internal void InputHitTest(Point pt, out IInputElement enabledHit, out IInputElement rawHit);
    private HitTestFilterBehavior InputHitTestFilterCallback(DependencyObject currentNode);
    public void InvalidateArrange();
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal void InvalidateArrangeInternal();
    internal static void InvalidateForceInheritPropertyOnChildren(Visual v, DependencyProperty property);
    public void InvalidateMeasure();
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal void InvalidateMeasureInternal();
    public void InvalidateVisual();
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal void InvokeAccessKey(AccessKeyEventArgs e);
    private static bool IsDefaultValue(DependencyObject dependencyObject, DependencyProperty dependencyProperty);
    private static void IsFocused_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e);
    private static void IsKeyboardFocused_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e);
    private bool IsKeyboardFocused_ComputeValue();
    private static void IsMouseCaptured_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e);
    private static void IsMouseDirectlyOver_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e);
    private bool IsMouseDirectlyOver_ComputeValue();
    private bool IsRenderable();
    private static bool IsRenderTransformOriginValid(object value);
    private static void IsStylusCaptured_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e);
    private static void IsStylusDirectlyOver_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e);
    private bool IsStylusDirectlyOver_ComputeValue();
    private bool markForSizeChangedIfNeeded(Size oldSize, Size newSize);
    public void Measure(Size availableSize);
    protected virtual Size MeasureCore(Size availableSize);
    public virtual bool MoveFocus(TraversalRequest request);
    protected virtual void OnAccessKey(AccessKeyEventArgs e);
    internal virtual void OnAddHandler(RoutedEvent routedEvent, Delegate handler);
    private static void OnBitmapEffectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e);
    private static void OnBitmapEffectInputChanged(DependencyObject d, DependencyPropertyChangedEventArgs e);
    private static void OnCacheModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e);
    [SecurityCritical]
    private static void OnCanExecuteThunk(object sender, CanExecuteRoutedEventArgs e);
    protected virtual void OnChildDesiredSizeChanged(UIElement child);
    [SecurityCritical]
    private static void OnCommandDeviceThunk(object sender, CommandDeviceEventArgs e);
    protected virtual AutomationPeer OnCreateAutomationPeer();
    protected virtual void OnDragEnter(DragEventArgs e);
    [SecurityCritical]
    private static void OnDragEnterThunk(object sender, DragEventArgs e);
    protected virtual void OnDragLeave(DragEventArgs e);
    [SecurityCritical]
    private static void OnDragLeaveThunk(object sender, DragEventArgs e);
    protected virtual void OnDragOver(DragEventArgs e);
    [SecurityCritical]
    private static void OnDragOverThunk(object sender, DragEventArgs e);
    protected virtual void OnDrop(DragEventArgs e);
    [SecurityCritical]
    private static void OnDropThunk(object sender, DragEventArgs e);
    private static void OnEffectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e);
    [SecurityCritical]
    private static void OnExecutedThunk(object sender, ExecutedRoutedEventArgs e);
    private static void OnFocusableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e);
    protected virtual void OnGiveFeedback(GiveFeedbackEventArgs e);
    [SecurityCritical]
    private static void OnGiveFeedbackThunk(object sender, GiveFeedbackEventArgs e);
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected virtual void OnGotFocus(RoutedEventArgs e);
    protected virtual void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e);
    [SecurityCritical]
    private static void OnGotKeyboardFocusThunk(object sender, KeyboardFocusChangedEventArgs e);
    protected virtual void OnGotMouseCapture(MouseEventArgs e);
    [SecurityCritical]
    private static void OnGotMouseCaptureThunk(object sender, MouseEventArgs e);
    protected virtual void OnGotStylusCapture(StylusEventArgs e);
    [SecurityCritical]
    private static void OnGotStylusCaptureThunk(object sender, StylusEventArgs e);
    protected virtual void OnGotTouchCapture(TouchEventArgs e);
    [SecurityCritical]
    private static void OnGotTouchCaptureThunk(object sender, TouchEventArgs e);
    private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e);
    private static void OnIsHitTestVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e);
    protected virtual void OnIsKeyboardFocusedChanged(DependencyPropertyChangedEventArgs e);
    protected virtual void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e);
    private static void OnIsManipulationEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e);
    protected virtual void OnIsMouseCapturedChanged(DependencyPropertyChangedEventArgs e);
    protected virtual void OnIsMouseCaptureWithinChanged(DependencyPropertyChangedEventArgs e);
    protected virtual void OnIsMouseDirectlyOverChanged(DependencyPropertyChangedEventArgs e);
    protected virtual void OnIsStylusCapturedChanged(DependencyPropertyChangedEventArgs e);
    protected virtual void OnIsStylusCaptureWithinChanged(DependencyPropertyChangedEventArgs e);
    protected virtual void OnIsStylusDirectlyOverChanged(DependencyPropertyChangedEventArgs e);
    private static void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e);
    protected virtual void OnKeyDown(KeyEventArgs e);
    [SecurityCritical]
    private static void OnKeyDownThunk(object sender, KeyEventArgs e);
    protected virtual void OnKeyUp(KeyEventArgs e);
    [SecurityCritical]
    private static void OnKeyUpThunk(object sender, KeyEventArgs e);
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected virtual void OnLostFocus(RoutedEventArgs e);
    protected virtual void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e);
    [SecurityCritical]
    private static void OnLostKeyboardFocusThunk(object sender, KeyboardFocusChangedEventArgs e);
    protected virtual void OnLostMouseCapture(MouseEventArgs e);
    [SecurityCritical]
    private static void OnLostMouseCaptureThunk(object sender, MouseEventArgs e);
    protected virtual void OnLostStylusCapture(StylusEventArgs e);
    [SecurityCritical]
    private static void OnLostStylusCaptureThunk(object sender, StylusEventArgs e);
    protected virtual void OnLostTouchCapture(TouchEventArgs e);
    [SecurityCritical]
    private static void OnLostTouchCaptureThunk(object sender, TouchEventArgs e);
    protected virtual void OnManipulationBoundaryFeedback(ManipulationBoundaryFeedbackEventArgs e);
    private static void OnManipulationBoundaryFeedbackThunk(object sender, ManipulationBoundaryFeedbackEventArgs e);
    protected virtual void OnManipulationCompleted(ManipulationCompletedEventArgs e);
    private static void OnManipulationCompletedThunk(object sender, ManipulationCompletedEventArgs e);
    protected virtual void OnManipulationDelta(ManipulationDeltaEventArgs e);
    private static void OnManipulationDeltaThunk(object sender, ManipulationDeltaEventArgs e);
    protected virtual void OnManipulationInertiaStarting(ManipulationInertiaStartingEventArgs e);
    private static void OnManipulationInertiaStartingThunk(object sender, ManipulationInertiaStartingEventArgs e);
    protected virtual void OnManipulationStarted(ManipulationStartedEventArgs e);
    private static void OnManipulationStartedThunk(object sender, ManipulationStartedEventArgs e);
    protected virtual void OnManipulationStarting(ManipulationStartingEventArgs e);
    private static void OnManipulationStartingThunk(object sender, ManipulationStartingEventArgs e);
    protected virtual void OnMouseDown(MouseButtonEventArgs e);
    [SecurityCritical]
    private static void OnMouseDownThunk(object sender, MouseButtonEventArgs e);
    protected virtual void OnMouseEnter(MouseEventArgs e);
    [SecurityCritical]
    private static void OnMouseEnterThunk(object sender, MouseEventArgs e);
    protected virtual void OnMouseLeave(MouseEventArgs e);
    [SecurityCritical]
    private static void OnMouseLeaveThunk(object sender, MouseEventArgs e);
    protected virtual void OnMouseLeftButtonDown(MouseButtonEventArgs e);
    [SecurityCritical]
    private static void OnMouseLeftButtonDownThunk(object sender, MouseButtonEventArgs e);
    protected virtual void OnMouseLeftButtonUp(MouseButtonEventArgs e);
    [SecurityCritical]
    private static void OnMouseLeftButtonUpThunk(object sender, MouseButtonEventArgs e);
    protected virtual void OnMouseMove(MouseEventArgs e);
    [SecurityCritical]
    private static void OnMouseMoveThunk(object sender, MouseEventArgs e);
    protected virtual void OnMouseRightButtonDown(MouseButtonEventArgs e);
    [SecurityCritical]
    private static void OnMouseRightButtonDownThunk(object sender, MouseButtonEventArgs e);
    protected virtual void OnMouseRightButtonUp(MouseButtonEventArgs e);
    [SecurityCritical]
    private static void OnMouseRightButtonUpThunk(object sender, MouseButtonEventArgs e);
    protected virtual void OnMouseUp(MouseButtonEventArgs e);
    [SecurityCritical]
    private static void OnMouseUpThunk(object sender, MouseButtonEventArgs e);
    protected virtual void OnMouseWheel(MouseWheelEventArgs e);
    [SecurityCritical]
    private static void OnMouseWheelThunk(object sender, MouseWheelEventArgs e);
    internal virtual void OnPresentationSourceChanged(bool attached);
    [SecurityCritical]
    private static void OnPreviewCanExecuteThunk(object sender, CanExecuteRoutedEventArgs e);
    protected virtual void OnPreviewDragEnter(DragEventArgs e);
    [SecurityCritical]
    private static void OnPreviewDragEnterThunk(object sender, DragEventArgs e);
    protected virtual void OnPreviewDragLeave(DragEventArgs e);
    [SecurityCritical]
    private static void OnPreviewDragLeaveThunk(object sender, DragEventArgs e);
    protected virtual void OnPreviewDragOver(DragEventArgs e);
    [SecurityCritical]
    private static void OnPreviewDragOverThunk(object sender, DragEventArgs e);
    protected virtual void OnPreviewDrop(DragEventArgs e);
    [SecurityCritical]
    private static void OnPreviewDropThunk(object sender, DragEventArgs e);
    [SecurityCritical]
    private static void OnPreviewExecutedThunk(object sender, ExecutedRoutedEventArgs e);
    protected virtual void OnPreviewGiveFeedback(GiveFeedbackEventArgs e);
    [SecurityCritical]
    private static void OnPreviewGiveFeedbackThunk(object sender, GiveFeedbackEventArgs e);
    protected virtual void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e);
    [SecurityCritical]
    private static void OnPreviewGotKeyboardFocusThunk(object sender, KeyboardFocusChangedEventArgs e);
    protected virtual void OnPreviewKeyDown(KeyEventArgs e);
    [SecurityCritical]
    private static void OnPreviewKeyDownThunk(object sender, KeyEventArgs e);
    protected virtual void OnPreviewKeyUp(KeyEventArgs e);
    [SecurityCritical]
    private static void OnPreviewKeyUpThunk(object sender, KeyEventArgs e);
    protected virtual void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e);
    [SecurityCritical]
    private static void OnPreviewLostKeyboardFocusThunk(object sender, KeyboardFocusChangedEventArgs e);
    protected virtual void OnPreviewMouseDown(MouseButtonEventArgs e);
    [SecurityCritical]
    private static void OnPreviewMouseDownThunk(object sender, MouseButtonEventArgs e);
    protected virtual void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e);
    [SecurityCritical]
    private static void OnPreviewMouseLeftButtonDownThunk(object sender, MouseButtonEventArgs e);
    protected virtual void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e);
    [SecurityCritical]
    private static void OnPreviewMouseLeftButtonUpThunk(object sender, MouseButtonEventArgs e);
    protected virtual void OnPreviewMouseMove(MouseEventArgs e);
    [SecurityCritical]
    private static void OnPreviewMouseMoveThunk(object sender, MouseEventArgs e);
    protected virtual void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e);
    [SecurityCritical]
    private static void OnPreviewMouseRightButtonDownThunk(object sender, MouseButtonEventArgs e);
    protected virtual void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e);
    [SecurityCritical]
    private static void OnPreviewMouseRightButtonUpThunk(object sender, MouseButtonEventArgs e);
    protected virtual void OnPreviewMouseUp(MouseButtonEventArgs e);
    [SecurityCritical]
    private static void OnPreviewMouseUpThunk(object sender, MouseButtonEventArgs e);
    protected virtual void OnPreviewMouseWheel(MouseWheelEventArgs e);
    [SecurityCritical]
    private static void OnPreviewMouseWheelThunk(object sender, MouseWheelEventArgs e);
    protected virtual void OnPreviewQueryContinueDrag(QueryContinueDragEventArgs e);
    [SecurityCritical]
    private static void OnPreviewQueryContinueDragThunk(object sender, QueryContinueDragEventArgs e);
    protected virtual void OnPreviewStylusButtonDown(StylusButtonEventArgs e);
    [SecurityCritical]
    private static void OnPreviewStylusButtonDownThunk(object sender, StylusButtonEventArgs e);
    protected virtual void OnPreviewStylusButtonUp(StylusButtonEventArgs e);
    [SecurityCritical]
    private static void OnPreviewStylusButtonUpThunk(object sender, StylusButtonEventArgs e);
    protected virtual void OnPreviewStylusDown(StylusDownEventArgs e);
    [SecurityCritical]
    private static void OnPreviewStylusDownThunk(object sender, StylusDownEventArgs e);
    protected virtual void OnPreviewStylusInAirMove(StylusEventArgs e);
    [SecurityCritical]
    private static void OnPreviewStylusInAirMoveThunk(object sender, StylusEventArgs e);
    protected virtual void OnPreviewStylusInRange(StylusEventArgs e);
    [SecurityCritical]
    private static void OnPreviewStylusInRangeThunk(object sender, StylusEventArgs e);
    protected virtual void OnPreviewStylusMove(StylusEventArgs e);
    [SecurityCritical]
    private static void OnPreviewStylusMoveThunk(object sender, StylusEventArgs e);
    protected virtual void OnPreviewStylusOutOfRange(StylusEventArgs e);
    [SecurityCritical]
    private static void OnPreviewStylusOutOfRangeThunk(object sender, StylusEventArgs e);
    protected virtual void OnPreviewStylusSystemGesture(StylusSystemGestureEventArgs e);
    [SecurityCritical]
    private static void OnPreviewStylusSystemGestureThunk(object sender, StylusSystemGestureEventArgs e);
    protected virtual void OnPreviewStylusUp(StylusEventArgs e);
    [SecurityCritical]
    private static void OnPreviewStylusUpThunk(object sender, StylusEventArgs e);
    protected virtual void OnPreviewTextInput(TextCompositionEventArgs e);
    [SecurityCritical]
    private static void OnPreviewTextInputThunk(object sender, TextCompositionEventArgs e);
    protected virtual void OnPreviewTouchDown(TouchEventArgs e);
    [SecurityCritical]
    private static void OnPreviewTouchDownThunk(object sender, TouchEventArgs e);
    protected virtual void OnPreviewTouchMove(TouchEventArgs e);
    [SecurityCritical]
    private static void OnPreviewTouchMoveThunk(object sender, TouchEventArgs e);
    protected virtual void OnPreviewTouchUp(TouchEventArgs e);
    [SecurityCritical]
    private static void OnPreviewTouchUpThunk(object sender, TouchEventArgs e);
    protected virtual void OnQueryContinueDrag(QueryContinueDragEventArgs e);
    [SecurityCritical]
    private static void OnQueryContinueDragThunk(object sender, QueryContinueDragEventArgs e);
    protected virtual void OnQueryCursor(QueryCursorEventArgs e);
    [SecurityCritical]
    private static void OnQueryCursorThunk(object sender, QueryCursorEventArgs e);
    internal virtual void OnRemoveHandler(RoutedEvent routedEvent, Delegate handler);
    protected virtual void OnRender(DrawingContext drawingContext);
    protected internal virtual void OnRenderSizeChanged(SizeChangedInfo info);
    protected virtual void OnStylusButtonDown(StylusButtonEventArgs e);
    [SecurityCritical]
    private static void OnStylusButtonDownThunk(object sender, StylusButtonEventArgs e);
    protected virtual void OnStylusButtonUp(StylusButtonEventArgs e);
    [SecurityCritical]
    private static void OnStylusButtonUpThunk(object sender, StylusButtonEventArgs e);
    protected virtual void OnStylusDown(StylusDownEventArgs e);
    [SecurityCritical]
    private static void OnStylusDownThunk(object sender, StylusDownEventArgs e);
    protected virtual void OnStylusEnter(StylusEventArgs e);
    [SecurityCritical]
    private static void OnStylusEnterThunk(object sender, StylusEventArgs e);
    protected virtual void OnStylusInAirMove(StylusEventArgs e);
    [SecurityCritical]
    private static void OnStylusInAirMoveThunk(object sender, StylusEventArgs e);
    protected virtual void OnStylusInRange(StylusEventArgs e);
    [SecurityCritical]
    private static void OnStylusInRangeThunk(object sender, StylusEventArgs e);
    protected virtual void OnStylusLeave(StylusEventArgs e);
    [SecurityCritical]
    private static void OnStylusLeaveThunk(object sender, StylusEventArgs e);
    protected virtual void OnStylusMove(StylusEventArgs e);
    [SecurityCritical]
    private static void OnStylusMoveThunk(object sender, StylusEventArgs e);
    protected virtual void OnStylusOutOfRange(StylusEventArgs e);
    [SecurityCritical]
    private static void OnStylusOutOfRangeThunk(object sender, StylusEventArgs e);
    protected virtual void OnStylusSystemGesture(StylusSystemGestureEventArgs e);
    [SecurityCritical]
    private static void OnStylusSystemGestureThunk(object sender, StylusSystemGestureEventArgs e);
    protected virtual void OnStylusUp(StylusEventArgs e);
    [SecurityCritical]
    private static void OnStylusUpThunk(object sender, StylusEventArgs e);
    protected virtual void OnTextInput(TextCompositionEventArgs e);
    [SecurityCritical]
    private static void OnTextInputThunk(object sender, TextCompositionEventArgs e);
    protected virtual void OnTouchDown(TouchEventArgs e);
    [SecurityCritical]
    private static void OnTouchDownThunk(object sender, TouchEventArgs e);
    protected virtual void OnTouchEnter(TouchEventArgs e);
    [SecurityCritical]
    private static void OnTouchEnterThunk(object sender, TouchEventArgs e);
    protected virtual void OnTouchLeave(TouchEventArgs e);
    [SecurityCritical]
    private static void OnTouchLeaveThunk(object sender, TouchEventArgs e);
    protected virtual void OnTouchMove(TouchEventArgs e);
    [SecurityCritical]
    private static void OnTouchMoveThunk(object sender, TouchEventArgs e);
    protected virtual void OnTouchUp(TouchEventArgs e);
    [SecurityCritical]
    private static void OnTouchUpThunk(object sender, TouchEventArgs e);
    private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e);
    internal void OnVisualAncestorChanged(object sender, AncestorChangedEventArgs e);
    private void OnVisualAncestorChanged_ForceInherit(object sender, AncestorChangedEventArgs e);
    protected internal override void OnVisualParentChanged(DependencyObject oldParent);
    private static void Opacity_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e);
    private static void OpacityMask_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e);
    public virtual DependencyObject PredictFocus(FocusNavigationDirection direction);
    internal static void PropagateResumeLayout(Visual parent, Visual v);
    internal static void PropagateSuspendLayout(Visual v);
    private void pushBitmapEffect();
    private void pushBitmapEffectInput(BitmapEffectInput newValue);
    private void pushBitmapScalingMode();
    private void pushCacheMode();
    private void pushClearTypeHint();
    private void pushEdgeMode();
    private void pushEffect();
    private void pushOpacity();
    private void pushOpacityMask();
    private void pushTextHintingMode();
    private void pushTextRenderingMode();
    private void pushVisualEffects();
    private void RaiseDependencyPropertyChanged(EventPrivateKey key, DependencyPropertyChangedEventArgs args);
    public void RaiseEvent(RoutedEventArgs e);
    [SecurityCritical]
    internal void RaiseEvent(RoutedEventArgs args, bool trusted);
    internal static void RaiseEventImpl(DependencyObject sender, RoutedEventArgs args);
    private void RaiseIsKeyboardFocusedChanged(DependencyPropertyChangedEventArgs args);
    internal void RaiseIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs args);
    private void RaiseIsMouseCapturedChanged(DependencyPropertyChangedEventArgs args);
    internal void RaiseIsMouseCaptureWithinChanged(DependencyPropertyChangedEventArgs args);
    private void RaiseIsMouseDirectlyOverChanged(DependencyPropertyChangedEventArgs args);
    private void RaiseIsStylusCapturedChanged(DependencyPropertyChangedEventArgs args);
    internal void RaiseIsStylusCaptureWithinChanged(DependencyPropertyChangedEventArgs args);
    private void RaiseIsStylusDirectlyOverChanged(DependencyPropertyChangedEventArgs args);
    internal bool ReadFlag(CoreFlags field);
    [SecurityTreatAsSafe, SecurityCritical]
    internal static void RegisterEvents(Type type);
    public void ReleaseAllTouchCaptures();
    public void ReleaseMouseCapture();
    public void ReleaseStylusCapture();
    public bool ReleaseTouchCapture(TouchDevice touchDevice);
    public void RemoveHandler(RoutedEvent routedEvent, Delegate handler);
    [FriendAccessAllowed]
    internal static void RemoveHandler(DependencyObject d, RoutedEvent routedEvent, Delegate handler);
    private void removeLayoutUpdatedHandler(EventHandler d);
    internal override void RenderClose(IDrawingContent newContent);
    internal override void RenderContent(RenderContext ctx, bool isOnChannel);
    [FriendAccessAllowed]
    internal DrawingContext RenderOpen();
    private static void RenderTransform_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e);
    private static void RenderTransformOrigin_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e);
    private static void ReRaiseEventAs(DependencyObject sender, RoutedEventArgs args, RoutedEvent newEvent);
    [SecurityTreatAsSafe, SecurityCritical]
    internal Rect RoundRect(Rect rect);
    [FriendAccessAllowed]
    internal void SetPersistId(int value);
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool ShouldSerializeCommandBindings();
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool ShouldSerializeInputBindings();
    private void signalDesiredSizeChange();
    private static void SnapsToDevicePixels_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e);
    internal bool StartListeningSynchronizedInput(SynchronizedInputType inputType);
    private void switchVisibilityIfNeeded(Visibility visibility);
    internal void SynchronizedInputPostOpportunityHandler(object sender, RoutedEventArgs args);
    internal void SynchronizedInputPreOpportunityHandler(object sender, RoutedEventArgs args);
    internal static void SynchronizeForceInheritProperties(UIElement uiElement, ContentElement contentElement, UIElement3D uiElement3D, DependencyObject parent);
    [FriendAccessAllowed]
    internal void SynchronizeReverseInheritPropertyFlags(DependencyObject oldParent, bool isCoreParent);
    private static void TextHintingMode_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e);
    private static void TextRenderingMode_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e);
    public Point TranslatePoint(Point point, UIElement relativeTo);
    [SecurityTreatAsSafe, SecurityCritical]
    internal void UpdateIsVisibleCache();
    public void UpdateLayout();
    private void updatePixelSnappingGuidelines();
    private static bool ValidateVisibility(object o);
    internal void WalkContent(DrawingContextWalker walker);
    internal void WriteFlag(CoreFlags field, bool value);

    // Properties
    public bool AllowDrop { get; set; }
    public bool AreAnyTouchesCaptured { get; }
    public bool AreAnyTouchesCapturedWithin { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; }
    public bool AreAnyTouchesDirectlyOver { get; }
    public bool AreAnyTouchesOver { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; }
    internal bool AreTransformsClean { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set; }
    internal bool ArrangeDirty { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set; }
    internal bool ArrangeInProgress { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set; }
    [Obsolete("BitmapEffects are deprecated and no longer function.  Consider using Effects where appropriate instead.")]
    public BitmapEffect BitmapEffect { get; set; }
    [Obsolete("BitmapEffects are deprecated and no longer function.  Consider using Effects where appropriate instead.")]
    public BitmapEffectInput BitmapEffectInput { get; set; }
    public CacheMode CacheMode { get; set; }
    public Geometry Clip { get; set; }
    public bool ClipToBounds { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; set; }
    internal bool ClipToBoundsCache { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set; }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public CommandBindingCollection CommandBindings { get; }
    internal CommandBindingCollection CommandBindingsInternal { get; }
    public Size DesiredSize { get; }
    public Effect Effect { get; set; }
    internal EventHandlersStore EventHandlersStore { [FriendAccessAllowed] get; }
    public bool Focusable { get; set; }
    public bool HasAnimatedProperties { get; }
    private bool HasAutomationPeer { get; set; }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public InputBindingCollection InputBindings { get; }
    internal InputBindingCollection InputBindingsInternal { get; }
    public bool IsArrangeValid { get; }
    public bool IsEnabled { get; set; }
    protected virtual bool IsEnabledCore { get; }
    public bool IsFocused { get; }
    public bool IsHitTestVisible { get; set; }
    public bool IsInputMethodEnabled { get; }
    public bool IsKeyboardFocused { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; }
    public bool IsKeyboardFocusWithin { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; }
    [CustomCategory("Touch_Category")]
    public bool IsManipulationEnabled { get; set; }
    public bool IsMeasureValid { get; }
    public bool IsMouseCaptured { get; }
    public bool IsMouseCaptureWithin { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; }
    public bool IsMouseDirectlyOver { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; }
    public bool IsMouseOver { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; }
    public bool IsStylusCaptured { get; }
    public bool IsStylusCaptureWithin { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; }
    public bool IsStylusDirectlyOver { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; }
    public bool IsStylusOver { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; }
    public bool IsVisible { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; }
    internal MeasureData MeasureData { get; set; }
    internal bool MeasureDirty { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set; }
    internal bool MeasureDuringArrange { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set; }
    internal bool MeasureInProgress { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set; }
    internal bool NeverArranged { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set; }
    internal bool NeverMeasured { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set; }
    [Localizability(LocalizationCategory.None, Readability=Readability.Unreadable)]
    public double Opacity { get; set; }
    public Brush OpacityMask { get; set; }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("PersistId is an obsolete property and may be removed in a future release.  The value of this property is not defined.")]
    public int PersistId { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; }
    internal Rect PreviousArrangeRect { [FriendAccessAllowed, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; }
    internal Size PreviousConstraint { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; }
    internal MeasureData PreviousMeasureData { get; private set; }
    private bool RenderingInvalidated { get; set; }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Size RenderSize { get; set; }
    public Transform RenderTransform { get; set; }
    public Point RenderTransformOrigin { get; set; }
    public bool SnapsToDevicePixels { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; set; }
    internal bool SnapsToDevicePixelsCache { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set; }
    protected StylusPlugInCollection StylusPlugIns { get; }
    public IEnumerable<TouchDevice> TouchesCaptured { get; }
    public IEnumerable<TouchDevice> TouchesCapturedWithin { get; }
    public IEnumerable<TouchDevice> TouchesDirectlyOver { get; }
    public IEnumerable<TouchDevice> TouchesOver { get; }
    public string Uid { get; set; }
    [Localizability(LocalizationCategory.None, Readability=Readability.Unreadable)]
    public Visibility Visibility { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; set; }
    private Visibility VisibilityCache { get; set; }

    // Nested Types
    private class InputHitTestResult
    {
        // Fields
        private HitTestResult _result;

        // Methods
        public InputHitTestResult();
        public HitTestResultBehavior InputHitTestResultCallback(HitTestResult result);

        // Properties
        public DependencyObject Result { get; }
    }
}
#endif 

 
}
