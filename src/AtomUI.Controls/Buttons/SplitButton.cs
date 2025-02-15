﻿using System.Reactive.Disposables;
using System.Windows.Input;
using AtomUI.Data;
using AtomUI.Input;
using AtomUI.Theme.Data;
using AtomUI.Theme.Styling;
using AtomUI.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Diagnostics;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Primitives.PopupPositioning;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Input.Raw;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.Threading;

namespace AtomUI.Controls;

[TemplatePart("PART_PrimaryButton", typeof(Button))]
[TemplatePart("PART_SecondaryButton", typeof(Button))]
public class SplitButton : ContentControl, ICommandSource, ISizeTypeAware
{
   internal const string pcChecked = ":checked";
   internal const string pcPressed = ":pressed";
   internal const string pcFlyoutOpen = ":flyout-open";

   #region 公共属性定义

   public static readonly RoutedEvent<RoutedEventArgs> ClickEvent =
      RoutedEvent.Register<SplitButton, RoutedEventArgs>(
         nameof(Click),
         RoutingStrategies.Bubble);

   public static readonly StyledProperty<ICommand?> CommandProperty =
      Button.CommandProperty.AddOwner<SplitButton>();

   public static readonly StyledProperty<object?> CommandParameterProperty =
      Button.CommandParameterProperty.AddOwner<SplitButton>();

   public static readonly StyledProperty<Flyout?> FlyoutProperty =
      AvaloniaProperty.Register<Button, Flyout?>(nameof(Flyout));

   public static readonly StyledProperty<KeyGesture?> HotKeyProperty =
      Button.HotKeyProperty.AddOwner<SplitButton>();

   public static readonly StyledProperty<FlyoutTriggerType> TriggerTypeProperty =
      AvaloniaProperty.Register<SplitButton, FlyoutTriggerType>(nameof(TriggerType), FlyoutTriggerType.Click);

   public static readonly StyledProperty<bool> IsShowArrowProperty =
      ArrowDecoratedBox.IsShowArrowProperty.AddOwner<SplitButton>();

   public static readonly StyledProperty<bool> IsPointAtCenterProperty =
      MenuFlyout.IsPointAtCenterProperty.AddOwner<SplitButton>();

   public static readonly StyledProperty<PlacementMode> PlacementProperty =
      Popup.PlacementProperty.AddOwner<SplitButton>();

   public static readonly StyledProperty<PopupAnchor> PlacementAnchorProperty =
      Popup.PlacementAnchorProperty.AddOwner<SplitButton>();

   public static readonly StyledProperty<PopupGravity> PlacementGravityProperty =
      Popup.PlacementGravityProperty.AddOwner<SplitButton>();

   public static readonly StyledProperty<double> MarginToAnchorProperty =
      Popup.MarginToAnchorProperty.AddOwner<SplitButton>();

   public static readonly StyledProperty<int> MouseEnterDelayProperty =
      AvaloniaProperty.Register<SplitButton, int>(nameof(MouseEnterDelay), 100);

   public static readonly StyledProperty<int> MouseLeaveDelayProperty =
      AvaloniaProperty.Register<SplitButton, int>(nameof(MouseLeaveDelay), 100);

   public static readonly StyledProperty<bool> IsShowIndicatorProperty =
      AvaloniaProperty.Register<SplitButton, bool>(nameof(IsShowIndicator), true);
   
   public static readonly StyledProperty<SizeType> SizeTypeProperty =
      Button.SizeTypeProperty.AddOwner<SplitButton>();
   
   public static readonly StyledProperty<PathIcon?> IconProperty
      = Button.IconProperty.AddOwner<SplitButton>();
   
   public static readonly StyledProperty<PathIcon?> FlyoutButtonIconProperty
      = AvaloniaProperty.Register<SplitButton, PathIcon?>(nameof(FlyoutButtonIcon));
   
   public static readonly StyledProperty<bool> IsDangerProperty =
      Button.IsDangerProperty.AddOwner<SplitButton>();
   
   public static readonly StyledProperty<bool> IsPrimaryButtonTypeProperty =
      AvaloniaProperty.Register<SplitButton, bool>(nameof(IsPrimaryButtonType), false);

   /// <summary>
   /// Raised when the user presses the primary part of the <see cref="SplitButton"/>.
   /// </summary>
   public event EventHandler<RoutedEventArgs>? Click
   {
      add => AddHandler(ClickEvent, value);
      remove => RemoveHandler(ClickEvent, value);
   }

   public ICommand? Command
   {
      get => GetValue(CommandProperty);
      set => SetValue(CommandProperty, value);
   }

   /// <summary>
   /// Gets or sets a parameter to be passed to the <see cref="Command"/>.
   /// </summary>
   public object? CommandParameter
   {
      get => GetValue(CommandParameterProperty);
      set => SetValue(CommandParameterProperty, value);
   }

   /// <summary>
   /// Gets or sets the <see cref="Flyout"/> that is shown when the secondary part is pressed.
   /// </summary>
   public Flyout? Flyout
   {
      get => GetValue(FlyoutProperty);
      set => SetValue(FlyoutProperty, value);
   }

   /// <summary>
   /// Gets or sets an <see cref="KeyGesture"/> associated with this control
   /// </summary>
   public KeyGesture? HotKey
   {
      get => GetValue(HotKeyProperty);
      set => SetValue(HotKeyProperty, value);
   }

   public FlyoutTriggerType TriggerType
   {
      get => GetValue(TriggerTypeProperty);
      set => SetValue(TriggerTypeProperty, value);
   }

   public bool IsShowArrow
   {
      get => GetValue(IsShowArrowProperty);
      set => SetValue(IsShowArrowProperty, value);
   }

   public bool IsPointAtCenter
   {
      get => GetValue(IsPointAtCenterProperty);
      set => SetValue(IsPointAtCenterProperty, value);
   }

   public PlacementMode Placement
   {
      get => GetValue(PlacementProperty);
      set => SetValue(PlacementProperty, value);
   }

   public PopupGravity PlacementGravity
   {
      get => GetValue(PlacementGravityProperty);
      set => SetValue(PlacementGravityProperty, value);
   }

   public PopupAnchor PlacementAnchor
   {
      get => GetValue(PlacementAnchorProperty);
      set => SetValue(PlacementAnchorProperty, value);
   }

   public double MarginToAnchor
   {
      get => GetValue(MarginToAnchorProperty);
      set => SetValue(MarginToAnchorProperty, value);
   }

   public int MouseEnterDelay
   {
      get => GetValue(MouseEnterDelayProperty);
      set => SetValue(MouseEnterDelayProperty, value);
   }

   public int MouseLeaveDelay
   {
      get => GetValue(MouseLeaveDelayProperty);
      set => SetValue(MouseLeaveDelayProperty, value);
   }

   public bool IsShowIndicator
   {
      get => GetValue(IsShowIndicatorProperty);
      set => SetValue(IsShowIndicatorProperty, value);
   }
   
   public SizeType SizeType
   {
      get => GetValue(SizeTypeProperty);
      set => SetValue(SizeTypeProperty, value);
   }
   
   public PathIcon? Icon
   {
      get => GetValue(IconProperty);
      set => SetValue(IconProperty, value);
   }
   
   public PathIcon? FlyoutButtonIcon
   {
      get => GetValue(FlyoutButtonIconProperty);
      set => SetValue(FlyoutButtonIconProperty, value);
   }
   
   public bool IsDanger
   {
      get => GetValue(IsDangerProperty);
      set => SetValue(IsDangerProperty, value);
   }
   
   public bool IsPrimaryButtonType
   {
      get => GetValue(IsPrimaryButtonTypeProperty);
      set => SetValue(IsPrimaryButtonTypeProperty, value);
   }
   
   #endregion

   #region 内部属性定义

   internal static readonly DirectProperty<SplitButton, CornerRadius> PrimaryButtonCornerRadiusProperty =
      AvaloniaProperty.RegisterDirect<SplitButton, CornerRadius>(nameof(PrimaryButtonCornerRadius),
                                                                 o => o.PrimaryButtonCornerRadius,
                                                                 (o, v) => o.PrimaryButtonCornerRadius = v);

   private CornerRadius _primaryButtonCornerRadius;
   internal CornerRadius PrimaryButtonCornerRadius
   {
      get => _primaryButtonCornerRadius; 
      set => SetAndRaise(PrimaryButtonCornerRadiusProperty, ref _primaryButtonCornerRadius, value); 
   }
   
   internal static readonly DirectProperty<SplitButton, CornerRadius> SecondaryButtonCornerRadiusProperty =
      AvaloniaProperty.RegisterDirect<SplitButton, CornerRadius>(nameof(SecondaryButtonCornerRadius),
                                                                 o => o.SecondaryButtonCornerRadius,
                                                                 (o, v) => o.SecondaryButtonCornerRadius = v);

   private CornerRadius _secondaryButtonCornerRadius;
   internal CornerRadius SecondaryButtonCornerRadius
   {
      get => _secondaryButtonCornerRadius; 
      set => SetAndRaise(SecondaryButtonCornerRadiusProperty, ref _secondaryButtonCornerRadius, value); 
   }
   
   internal static readonly DirectProperty<SplitButton, ButtonType> EffectiveButtonTypeProperty =
      AvaloniaProperty.RegisterDirect<SplitButton, ButtonType>(nameof(EffectiveButtonType),
                                                               o => o.EffectiveButtonType,
                                                               (o, v) => o.EffectiveButtonType = v);

   private ButtonType _effectiveButtonType;
   internal ButtonType EffectiveButtonType
   {
      get => _effectiveButtonType; 
      set => SetAndRaise(EffectiveButtonTypeProperty, ref _effectiveButtonType, value); 
   }

   #endregion

   private Button? _primaryButton = null;
   private Button? _secondaryButton = null;
   private KeyGesture? _hotkey = default;

   private bool _commandCanExecute = true;
   private bool _isAttachedToLogicalTree = false;
   private bool _isFlyoutOpen = false;
   private bool _isKeyboardPressed = false;
   
   private DispatcherTimer? _mouseEnterDelayTimer;
   private DispatcherTimer? _mouseLeaveDelayTimer;
   private CompositeDisposable? _subscriptions;
   private IDisposable? _flyoutCloseDetectDisposable;

   private IDisposable? _flyoutPropertyChangedDisposable;

   static SplitButton()
   {
      PlacementProperty.OverrideDefaultValue<SplitButton>(PlacementMode.BottomEdgeAlignedRight);
      IsShowArrowProperty.OverrideDefaultValue<SplitButton>(false);
      HorizontalAlignmentProperty.OverrideDefaultValue<SplitButton>(HorizontalAlignment.Left);
      VerticalAlignmentProperty.OverrideDefaultValue<SplitButton>(VerticalAlignment.Top);
      AffectsRender<SplitButton>(IsPrimaryButtonTypeProperty, IsDangerProperty);
   }

   internal virtual bool InternalIsChecked => false;

   /// <inheritdoc/>
   protected override bool IsEnabledCore => base.IsEnabledCore && _commandCanExecute;

   /// <inheritdoc/>
   void ICommandSource.CanExecuteChanged(object sender, EventArgs e) => this.CanExecuteChanged(sender, e);

   /// <inheritdoc cref="ICommandSource.CanExecuteChanged"/>
   private void CanExecuteChanged(object? sender, EventArgs e)
   {
      (var command, var parameter) = (Command, CommandParameter);
      CanExecuteChanged(command, parameter);
   }

   private void CanExecuteChanged(ICommand? command, object? parameter)
   {
      if (!((ILogical)this).IsAttachedToLogicalTree) {
         return;
      }

      var canExecute = command is null || command.CanExecute(parameter);

      if (canExecute != _commandCanExecute) {
         _commandCanExecute = canExecute;
         UpdateIsEffectivelyEnabled();
      }
   }

   /// <summary>
   /// Updates the visual state of the control by applying latest PseudoClasses.
   /// </summary>
   protected void UpdatePseudoClasses()
   {
      PseudoClasses.Set(pcFlyoutOpen, _isFlyoutOpen);
      PseudoClasses.Set(pcPressed, _isKeyboardPressed);
      PseudoClasses.Set(pcChecked, InternalIsChecked);
   }

   protected void OpenFlyout()
   {
      if (Flyout is null) {
         return;
      }

      // 防止干扰打开
      _flyoutCloseDetectDisposable?.Dispose();
      StopMouseEnterTimer();
      StopMouseLeaveTimer();
      Flyout.Hide();
      if (MouseEnterDelay == 0) {
         Flyout.ShowAt(this);
      } else {
         StartMouseEnterTimer();
      }
   }

   /// <summary>
   /// Closes the secondary button's flyout.
   /// </summary>
   protected void CloseFlyout()
   {
      if (Flyout is null) {
         return;
      }

      _flyoutCloseDetectDisposable?.Dispose();
      _flyoutCloseDetectDisposable = null;
      StopMouseEnterTimer();

      if (MouseLeaveDelay == 0) {
         Flyout.Hide();
      } else {
         StartMouseLeaveTimer();
      }
   }

   /// <summary>
   /// Registers all flyout events.
   /// </summary>
   /// <param name="flyout">The flyout to connect events to.</param>
   private void RegisterFlyoutEvents(Flyout? flyout)
   {
      if (flyout != null) {
         flyout.Opened += HandleFlyoutOpened;
         flyout.Closed += HandleFlyoutClosed;

         _flyoutPropertyChangedDisposable = flyout.GetPropertyChangedObservable(Popup.PlacementProperty)
                                                  .Subscribe(HandleFlyoutPlacementPropertyChanged);
      }
   }

   /// <summary>
   /// Explicitly unregisters all flyout events.
   /// </summary>
   /// <param name="flyout">The flyout to disconnect events from.</param>
   private void UnregisterFlyoutEvents(Flyout? flyout)
   {
      if (flyout != null) {
         flyout.Opened -= HandleFlyoutOpened;
         flyout.Closed -= HandleFlyoutClosed;

         _flyoutPropertyChangedDisposable?.Dispose();
         _flyoutPropertyChangedDisposable = null;
      }
   }

   /// <summary>
   /// Explicitly unregisters all events related to the two buttons in OnApplyTemplate().
   /// </summary>
   private void UnregisterEvents()
   {
      if (_primaryButton != null) {
         _primaryButton.Click -= HandlePrimaryButtonClick;
      }
   }
   
   protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
   {
      base.OnDetachedFromVisualTree(e);
      StopMouseLeaveTimer();
      StopMouseEnterTimer();
      _subscriptions?.Dispose();
   }

   protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
   {
      base.OnAttachedToVisualTree(e);
      SetupTriggerHandler();
   }
   
   private void SetupTriggerHandler()
   {
      _subscriptions = new CompositeDisposable();
      if (TriggerType == FlyoutTriggerType.Hover) {
         _subscriptions.Add(IsPointerOverProperty.Changed.Subscribe(args =>
         {
            if (args.Sender == _secondaryButton) {
               HandleAnchorTargetHover(args);
            }
         }));
      } else if (TriggerType == FlyoutTriggerType.Click) {
         var inputManager = AvaloniaLocator.Current.GetService<IInputManager>()!;
         _subscriptions.Add(inputManager.Process.Subscribe(HandleAnchorTargetClick));
      }
   }
   
   private void HandleAnchorTargetClick(RawInputEventArgs args)
   {
      if (args is RawPointerEventArgs pointerEventArgs) {
         if (Flyout is null) {
            return;
         }

         if (pointerEventArgs.Type == RawPointerEventType.LeftButtonUp) {
            if (_secondaryButton is not null) {
               if (!Flyout.IsOpen) {
                  var pos = _secondaryButton.TranslatePoint(new Point(0, 0), TopLevel.GetTopLevel(this)!);
                  if (!pos.HasValue) {
                     return;
                  }

                  var bounds = new Rect(pos.Value, _secondaryButton.Bounds.Size);
                  if (bounds.Contains(pointerEventArgs.Position)) {
                     OpenFlyout();
                     args.Handled = true;
                  }
               } else {
                  if (Flyout is IPopupHostProvider popupHostProvider) {
                     if (popupHostProvider.PopupHost != pointerEventArgs.Root) {
                        CloseFlyout();
                        args.Handled = true;
                     }
                  }
               }
            }
         }
      }
   }
   
   private void HandleAnchorTargetHover(AvaloniaPropertyChangedEventArgs<bool> e)
   {
      if (Flyout is not null) {
         if (e.GetNewValue<bool>()) {
            OpenFlyout();
         } else {
            CloseFlyout();
         }
      }
   }

   /// <inheritdoc/>
   protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
   {
      base.OnApplyTemplate(e);

      TokenResourceBinder.CreateGlobalResourceBinding(this, Border.BorderBrushProperty, GlobalTokenResourceKey.ColorBorder);
      TokenResourceBinder.CreateGlobalResourceBinding(this, Border.BorderThicknessProperty, GlobalTokenResourceKey.BorderThickness,
         BindingPriority.Template,
         new RenderScaleAwareThicknessConfigure(this));
      
      UnregisterEvents();
      UnregisterFlyoutEvents(Flyout);

      _primaryButton = e.NameScope.Find<Button>("PART_PrimaryButton");
      _secondaryButton = e.NameScope.Find<Button>("PART_SecondaryButton");

      if (_primaryButton != null) {
         _primaryButton.Click += HandlePrimaryButtonClick;
      }

      if (FlyoutButtonIcon is null) {
         FlyoutButtonIcon = new PathIcon()
         {
            Kind = "EllipsisOutlined"
         };
      }
      SetupEffectiveButtonType();
      SetupFlyoutProperties();
      RegisterFlyoutEvents(Flyout);
      UpdatePseudoClasses();
   }

   private void SetupFlyoutProperties()
   {
      if (Flyout is not null) {
         BindUtils.RelayBind(this, PlacementProperty, Flyout);
         BindUtils.RelayBind(this, PlacementAnchorProperty, Flyout);
         BindUtils.RelayBind(this, PlacementGravityProperty, Flyout);
         BindUtils.RelayBind(this, IsShowArrowProperty, Flyout);
         BindUtils.RelayBind(this, IsPointAtCenterProperty, Flyout);
         BindUtils.RelayBind(this, MarginToAnchorProperty, Flyout);
      }
   }
   
   /// <inheritdoc/>
   protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
   {
      base.OnAttachedToLogicalTree(e);

      // Control attached again, set Hotkey to create a hotkey manager for this control
      SetCurrentValue(HotKeyProperty, _hotkey);

      if (Command != null) {
         Command.CanExecuteChanged += CanExecuteChanged;
         CanExecuteChanged(this, EventArgs.Empty);
      }

      _isAttachedToLogicalTree = true;
   }

   /// <inheritdoc/>
   protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
   {
      base.OnDetachedFromLogicalTree(e);

      // This will cause the hotkey manager to dispose the observer and the reference to this control
      _hotkey = HotKey;
      SetCurrentValue(HotKeyProperty, null);

      if (Command != null) {
         Command.CanExecuteChanged -= CanExecuteChanged;
      }

      _isAttachedToLogicalTree = false;
   }

   /// <inheritdoc/>
   protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
   {
      if (e.Property == CommandProperty) {
         // Must unregister events here while a reference to the old command still exists
         var (oldValue, newValue) = e.GetOldAndNewValue<ICommand?>();

         if (_isAttachedToLogicalTree) {
            if (oldValue is ICommand oldCommand) {
               oldCommand.CanExecuteChanged -= CanExecuteChanged;
            }

            if (newValue is ICommand newCommand) {
               newCommand.CanExecuteChanged += CanExecuteChanged;
            }
         }

         CanExecuteChanged(newValue, CommandParameter);
      } else if (e.Property == CommandParameterProperty && IsLoaded) {
         CanExecuteChanged(Command, e.NewValue);
      } else if (e.Property == FlyoutProperty) {
         var (oldFlyout, newFlyout) = e.GetOldAndNewValue<Flyout?>();

         // If flyout is changed while one is already open, make sure we 
         // close the old one first
         // This is the same behavior as Button
         if (oldFlyout != null && oldFlyout.IsOpen) {
            oldFlyout.Hide();
         }

         // Must unregister events here while a reference to the old flyout still exists
         UnregisterFlyoutEvents(oldFlyout);

         RegisterFlyoutEvents(newFlyout);
         UpdatePseudoClasses();
      } else if (e.Property == CornerRadiusProperty) {
         SetupButtonCornerRadius();
      } else if (e.Property == IsPrimaryButtonTypeProperty) {
         SetupEffectiveButtonType();
      }

      base.OnPropertyChanged(e);
   }

   private void SetupEffectiveButtonType()
   {
      if (IsPrimaryButtonType) {
         EffectiveButtonType = ButtonType.Primary;
      } else {
         EffectiveButtonType = ButtonType.Default;
      }
   }

   private void SetupButtonCornerRadius()
   {
      PrimaryButtonCornerRadius = new CornerRadius(CornerRadius.TopLeft,
                                                   0,
                                                   0, 
                                                   CornerRadius.BottomLeft);
      SecondaryButtonCornerRadius = new CornerRadius(0,
                                                     CornerRadius.TopRight,
                                                     CornerRadius.BottomRight, 
                                                     0);
   }

   /// <inheritdoc/>
   protected override void OnKeyDown(KeyEventArgs e)
   {
      var key = e.Key;

      if (key == Key.Space || key == Key.Enter) {
         _isKeyboardPressed = true;
         UpdatePseudoClasses();
      }

      base.OnKeyDown(e);
   }

   /// <inheritdoc/>
   protected override void OnKeyUp(KeyEventArgs e)
   {
      var key = e.Key;

      if (key == Key.Space || key == Key.Enter) {
         _isKeyboardPressed = false;
         UpdatePseudoClasses();

         // Consider this a click on the primary button
         if (IsEffectivelyEnabled) {
            OnClickPrimary(null);
            e.Handled = true;
         }
      } else if (key == Key.Down && e.KeyModifiers.HasAllFlags(KeyModifiers.Alt) && IsEffectivelyEnabled
                 && !XYFocusHelpers.IsAllowedXYNavigationMode(this, e.KeyDeviceType)) {
         OpenFlyout();
         e.Handled = true;
      } else if (key == Key.F4 && IsEffectivelyEnabled) {
         OpenFlyout();
         e.Handled = true;
      } else if (e.Key == Key.Escape && _isFlyoutOpen) {
         // If Flyout doesn't have focusable content, close the flyout here
         // This is the same behavior as Button
         CloseFlyout();
         e.Handled = true;
      }

      base.OnKeyUp(e);
   }

   /// <summary>
   /// Invokes the <see cref="Click"/> event when the primary button part is clicked.
   /// </summary>
   /// <param name="e">The event args from the internal Click event.</param>
   protected virtual void OnClickPrimary(RoutedEventArgs? e)
   {
      (var command, var parameter) = (Command, CommandParameter);
      // Note: It is not currently required to check enabled status; however, this is a failsafe
      if (IsEffectivelyEnabled) {
         var eventArgs = new RoutedEventArgs(ClickEvent);
         RaiseEvent(eventArgs);

         if (!eventArgs.Handled && command?.CanExecute(parameter) == true) {
            command.Execute(parameter);
            eventArgs.Handled = true;
         }
      }
   }

   /// <summary>
   /// Invoked when the split button's flyout is opened.
   /// </summary>
   protected virtual void OnFlyoutOpened()
   {
      // Available for derived types
   }

   /// <summary>
   /// Invoked when the split button's flyout is closed.
   /// </summary>
   protected virtual void OnFlyoutClosed()
   {
      // Available for derived types
   }

   /// <summary>
   /// Event handler for when the internal primary button part is clicked.
   /// </summary>
   private void HandlePrimaryButtonClick(object? sender, RoutedEventArgs e)
   {
      // Handle internal button click, so it won't bubble outside together with SplitButton.ClickEvent.
      e.Handled = true;
      OnClickPrimary(e);
   }

   /// <summary>
   /// Called when the <see cref="PopupFlyoutBase.Placement"/> property changes.
   /// </summary>
   private void HandleFlyoutPlacementPropertyChanged(AvaloniaPropertyChangedEventArgs e)
   {
      UpdatePseudoClasses();
   }

   /// <summary>
   /// Event handler for when the split button's flyout is opened.
   /// </summary>
   private void HandleFlyoutOpened(object? sender, EventArgs e)
   {
      var flyout = sender as Flyout;

      
      // It is possible to share flyouts among multiple controls including SplitButton.
      // This can cause a problem here since all controls that share a flyout receive
      // the same Opened/Closed events at the same time.
      // For SplitButton that means they all would be updating their pseudoclasses accordingly.
      // In other words, all SplitButtons with a shared Flyout would have the backgrounds changed together.
      // To fix this, only continue here if the Flyout target matches this SplitButton instance.
      if (object.ReferenceEquals(flyout?.Target, this)) {
         _isFlyoutOpen = true;
         UpdatePseudoClasses();

         OnFlyoutOpened();
      }
      
      if (Flyout is IPopupHostProvider popupHostProvider) {
         var host = popupHostProvider.PopupHost;
         if (host is PopupRoot popupRoot) {
            // 这里 PopupRoot 关闭的时候会被关闭，所以这里的事件处理器是不是不需要删除
            popupRoot.PointerMoved += (o, args) =>
            {
               StopMouseLeaveTimer();
               if (_flyoutCloseDetectDisposable is null) {
                  var inputManager = AvaloniaLocator.Current.GetService<IInputManager>()!;
                  _flyoutCloseDetectDisposable = inputManager.Process.Subscribe(DetectWhenToClosePopup);
               }
            };
         }
      }
   }
   
   private void DetectWhenToClosePopup(RawInputEventArgs args)
   {
      if (args is RawPointerEventArgs pointerEventArgs) {
         if (Flyout is null) {
            return;
         }

         if (Flyout.IsOpen) {
            var found = false;
            if (pointerEventArgs.Root is PopupRoot popupRoot) {
               var current = popupRoot.Parent;
               while (current is not null) {
                  if (current == this) {
                     found = true;
                  }

                  current = current.Parent;
               }
            } else if (object.Equals(pointerEventArgs.Root, this)) {
               found = true;
            }

            if (!found) {
               CloseFlyout();
            }
         }
      }
   }
   
   private void StartMouseEnterTimer()
   {
      _mouseEnterDelayTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(MouseEnterDelay), Tag = this };
      _mouseEnterDelayTimer.Tick += (sender, args) =>
      {
         if (_mouseEnterDelayTimer != null) {
            StopMouseEnterTimer();
            if (Flyout is null) {
               return;
            }

            Flyout.ShowAt(this);
         }
      };
      _mouseEnterDelayTimer.Start();
   }

   private void StopMouseEnterTimer()
   {
      _mouseEnterDelayTimer?.Stop();
      _mouseEnterDelayTimer = null;
   }

   private void StopMouseLeaveTimer()
   {
      _mouseLeaveDelayTimer?.Stop();
      _mouseLeaveDelayTimer = null;
   }

   private void StartMouseLeaveTimer()
   {
      _mouseLeaveDelayTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(MouseLeaveDelay), Tag = this };
      _mouseLeaveDelayTimer.Tick += (sender, args) =>
      {
         if (_mouseLeaveDelayTimer != null) {
            StopMouseLeaveTimer();
            if (Flyout is null) {
               return;
            }

            Flyout.Hide();
         }
      };
      _mouseLeaveDelayTimer.Start();
   }

   /// <summary>
   /// Event handler for when the split button's flyout is closed.
   /// </summary>
   private void HandleFlyoutClosed(object? sender, EventArgs e)
   {
      var flyout = sender as Flyout;

      // See comments in HandleFlyoutOpened
      if (object.ReferenceEquals(flyout?.Target, this)) {
         _isFlyoutOpen = false;
         UpdatePseudoClasses();

         OnFlyoutClosed();
      }
   }
   
   private Rect? _originRect;
   
   protected override Size ArrangeOverride(Size finalSize)
   {
      var size = base.ArrangeOverride(finalSize);
      if (_originRect is null) {
         _originRect = _secondaryButton?.Bounds;
      }

      if (!IsPrimaryButtonType) {
         if (_secondaryButton is not null && _originRect.HasValue) {
            _secondaryButton.Arrange(_originRect.Value.Inflate(new Thickness(_secondaryButton.BorderThickness.Left, 0, 0, 0)));
         }
      } else {
         if (_secondaryButton is not null && _originRect.HasValue) {
            _secondaryButton.Arrange(_originRect.Value.Deflate(new Thickness(_secondaryButton.BorderThickness.Left, 0, 0, 0)));
         }
      }

      return size;
   }

   public override void Render(DrawingContext context)
   {
      if (IsPrimaryButtonType) {
         if (_secondaryButton is not null) {
            var offset = _secondaryButton.TranslatePoint(new Point(0, 0), this);
            if (!offset.HasValue) {
               return;
            }
            using var optionState = context.PushRenderOptions(new RenderOptions()
            {
               EdgeMode = EdgeMode.Aliased
            });
            var startPoint = new Point(offset.Value.X, 0);
            var endPoint = new Point(offset.Value.X, Bounds.Height);
            context.DrawLine(new Pen(BorderBrush, BorderThickness.Left), startPoint, endPoint);
         }
      }
   }
}