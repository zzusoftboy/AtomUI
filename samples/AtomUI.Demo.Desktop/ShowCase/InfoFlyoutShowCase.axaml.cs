using AtomUI.Controls;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace AtomUI.Demo.Desktop.ShowCase;

public partial class InfoFlyoutShowCase : UserControl
{
   public static readonly StyledProperty<bool> ShowArrowProperty =
      AvaloniaProperty.Register<TooltipShowCase, bool>(nameof(ShowArrow), true);
   
   public static readonly StyledProperty<bool> IsPointAtCenterProperty =
      AvaloniaProperty.Register<TooltipShowCase, bool>(nameof(IsPointAtCenter), false);

   private Segmented _segmented;
   
   public bool ShowArrow
   {
      get => GetValue(ShowArrowProperty);
      set => SetValue(ShowArrowProperty, value);
   }
   
   public bool IsPointAtCenter
   {
      get => GetValue(IsPointAtCenterProperty);
      set => SetValue(IsPointAtCenterProperty, value);
   }
   
   public InfoFlyoutShowCase()
   {
      DataContext = this;
      InitializeComponent();
      var control = this as Control;
      _segmented = control.FindControl<Segmented>("ArrowSegmented")!;
      _segmented.CurrentChanged += (sender, args) =>
      {
         if (args.ItemIndex == 0) {
            ShowArrow = true;
            IsPointAtCenter = false;
         } else if (args.ItemIndex == 1) {
            ShowArrow = false;
            IsPointAtCenter = false;
         } else if (args.ItemIndex == 2) {
            IsPointAtCenter = true;
            ShowArrow = true;
         }
      };
   }
}