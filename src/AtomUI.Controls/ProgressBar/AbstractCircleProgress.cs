using AtomUI.Media;
using AtomUI.Styling;
using AtomUI.Utils;
using Avalonia;
using Avalonia.Layout;
using Math = System.Math;

namespace AtomUI.Controls;

public abstract partial class AbstractCircleProgress : AbstractProgressBar
{
   // 默认的大小推荐，针对 SizeType
   protected const double LARGE_CIRCLE_SIZE           = 120;
   protected const double MIDDLE_CIRCLE_SIZE          = 90;
   protected const double SMALL_CIRCLE_SIZE           = 60;
   protected const double CIRCLE_MIN_STROKE_THICKNESS = 3;
   
   public static readonly StyledProperty<int> StepCountProperty =
      AvaloniaProperty.Register<ProgressBar, int>(nameof(StepCount), 0, coerce:(o, v) => Math.Max(v, 0));
   
   public static readonly StyledProperty<double> StepGapProperty =
      AvaloniaProperty.Register<ProgressBar, double>(nameof(StepGap), 2, coerce:(o, v) => Math.Max(v, 0));
   
   public int StepCount
   {
      get => GetValue(StepCountProperty);
      set => SetValue(StepCountProperty, value);
   }
   
   public double StepGap
   {
      get => GetValue(StepGapProperty);
      set => SetValue(StepGapProperty, value);
   }
   
   internal Dictionary<SizeType, double> _sizeTypeThresholdValue;

   static AbstractCircleProgress()
   {
      HorizontalAlignmentProperty.OverrideDefaultValue<AbstractCircleProgress>(HorizontalAlignment.Left);
      VerticalAlignmentProperty.OverrideDefaultValue<AbstractCircleProgress>(VerticalAlignment.Top);
      AffectsMeasure<AbstractCircleProgress>(StepCountProperty,
                                            StepGapProperty);
   }
   
   public AbstractCircleProgress()
   {
      _sizeTypeThresholdValue = new Dictionary<SizeType, double>();
   }
   
   protected override SizeType CalculateEffectiveSizeType(double size)
   {
      var sizeType = SizeType.Large;
      var largeThresholdValue = _sizeTypeThresholdValue[SizeType.Large];
      var middleThresholdValue = _sizeTypeThresholdValue[SizeType.Middle];
      if (MathUtils.GreaterThanOrClose(size, largeThresholdValue)) {
         sizeType = SizeType.Large;
      } else if (MathUtils.GreaterThanOrClose(size, middleThresholdValue)) {
         sizeType = SizeType.Middle;
      } else {
         sizeType = SizeType.Small;
      }
      return sizeType;
   }
   
   protected override void NotifySetupUi()
   {
      CalculateSizeTypeThresholdValue();
      base.NotifySetupUi();
   }
   
   protected override void NotifyUiStructureReady()
   {
      base.NotifyUiStructureReady();
      _percentageLabel!.HorizontalAlignment = HorizontalAlignment.Center;
      _percentageLabel!.VerticalAlignment = VerticalAlignment.Center;
   }

   private void CalculateSizeTypeThresholdValue()
   {
      _sizeTypeThresholdValue.Add(SizeType.Large, LARGE_CIRCLE_SIZE);
      _sizeTypeThresholdValue.Add(SizeType.Middle, MIDDLE_CIRCLE_SIZE);
      _sizeTypeThresholdValue.Add(SizeType.Small, SMALL_CIRCLE_SIZE);
   }

   // 是否考虑一个最小的值
   protected override Size MeasureOverride(Size availableSize)
   {
      var targetSize = CalculateCircleSize();
      if (!double.IsInfinity(availableSize.Width) && !double.IsInfinity(availableSize.Height)) {
         var minSize = Math.Min(availableSize.Width, availableSize.Height);
         if (minSize < targetSize || IsStretchAlignment()) {
            targetSize = minSize;
         }
      } else if (!double.IsInfinity(availableSize.Width) && double.IsInfinity(availableSize.Height)) {
         if (availableSize.Width < targetSize || IsStretchAlignment()) {
            targetSize = availableSize.Width;
         }
      } else if (!double.IsInfinity(availableSize.Height) && double.IsInfinity(availableSize.Width)) {
         if (availableSize.Height < targetSize || IsStretchAlignment()) {
            targetSize = availableSize.Height;
         }
      }

      if (ShowProgressInfo) {
         _percentageLabel!.Measure(availableSize);
      }
      return new Size(targetSize, targetSize);
   }

   private bool IsStretchAlignment()
   {
      return HorizontalAlignment == HorizontalAlignment.Stretch || VerticalAlignment == VerticalAlignment.Stretch;
   }

   private double CalculateCircleSize()
   {
      var targetSize = 0d;
      var sizeTypeDefaultValue = _sizeTypeThresholdValue[EffectiveSizeType];
      if (double.IsNaN(Width) && double.IsNaN(Height)) {
         targetSize = sizeTypeDefaultValue;
      } else if (double.IsNaN(Width) && !double.IsNaN(Height)) {
         targetSize = Height;
      } else if (!double.IsNaN(Width) && double.IsNaN(Height)) {
         targetSize = Width;
      } else {
         targetSize = Math.Min(Width, Height);
      }

      return targetSize;
   }

   protected override void NotifyHandleExtraInfoVisibility()
   {
      // TODO 可能存在重复计算
      var circleSize = CalculateCircleSize();
      CalculateStrokeThickness();
      var extraInfoSize = circleSize - StrokeThickness - 1; // 写死一个像素的 padding 吧
      var extraInfo = TextUtils.CalculateTextSize(string.Format(ProgressTextFormat, 100), FontFamily, FontSize);
      if (_percentageLabel!.IsVisible) {
         if (extraInfo.Width > extraInfoSize || extraInfo.Height > extraInfoSize) {
            _percentageLabel!.IsVisible = false;
         }
      }

      if (_exceptionCompletedIcon!.IsVisible) {
         var exceptionIconWidth = _exceptionCompletedIcon!.Width;
         var exceptionIconHeight = _exceptionCompletedIcon!.Height;
         if (exceptionIconWidth > extraInfoSize || exceptionIconHeight > extraInfoSize) {
            _exceptionCompletedIcon!.IsVisible = false;
         }
      }

      if (_successCompletedIcon!.IsVisible) {
         var successIconWidth = _successCompletedIcon!.Width;
         var successIconHeight = _successCompletedIcon!.Height;
         if (successIconWidth > extraInfoSize || successIconHeight > extraInfoSize) {
            _successCompletedIcon!.IsVisible = false;
         }
      }

   }
   
   protected override void NotifyPropertyChanged(AvaloniaPropertyChangedEventArgs e)
   {
      base.NotifyPropertyChanged(e);
     
      if (e.Property == WidthProperty || 
          e.Property == HeightProperty) {
         if (_initialized) {
            CalculateStrokeThickness();
            SetupExtraInfoFontSize();
            SetupExtraInfoIconSize();
         }
      }
   }
   
   private void SetupExtraInfoFontSize()
   {
      var circleSize = CalculateCircleSize();
      double fontSize = circleSize * 0.15 + 6;
      if (fontSize < _circleMinimumTextFontSize) {
         fontSize = _circleMinimumTextFontSize;
      }
      FontSize = fontSize;
   }

   private void SetupExtraInfoIconSize()
   {
      var circleSize = CalculateCircleSize();
      var calculatedSize = Math.Max(circleSize / 4.5, _circleMinimumIconSize);
      _exceptionCompletedIcon!.Width = calculatedSize;
      _exceptionCompletedIcon!.Height = calculatedSize;
      
      _successCompletedIcon!.Width = calculatedSize;
      _successCompletedIcon!.Height = calculatedSize;
   }

   protected override Size ArrangeOverride(Size finalSize)
   {
      if (ShowProgressInfo) {
         var extraInfoRect = GetExtraInfoRect(new Rect(new Point(0, 0), DesiredSize));
         if (_percentageLabel!.IsVisible) {
            _percentageLabel.Arrange(extraInfoRect);
         }

         if (_successCompletedIcon != null && _successCompletedIcon.IsVisible) {
            _successCompletedIcon.Arrange(extraInfoRect);
         }
         if (_exceptionCompletedIcon != null && _exceptionCompletedIcon.IsVisible) {
            _exceptionCompletedIcon.Arrange(extraInfoRect);
         }
      }
      return finalSize;
   }

   protected override Rect GetProgressBarRect(Rect controlRect)
   {
      return new Rect(new Point(0, 0), controlRect.Size.Deflate(Margin));
   }

   protected override Rect GetExtraInfoRect(Rect controlRect)
   {
      return GetProgressBarRect(controlRect).Deflate(StrokeThickness);
   }
   
   protected override void CalculateStrokeThickness()
   {
      var circleSize = CalculateCircleSize();
      var calculatedValue = (MIDDLE_STROKE_THICKNESS / MIDDLE_CIRCLE_SIZE) * circleSize;
      calculatedValue = Math.Max(calculatedValue, CIRCLE_MIN_STROKE_THICKNESS);
      if (!double.IsNaN(IndicatorThickness)) {
         calculatedValue = Math.Max(IndicatorThickness, CIRCLE_MIN_STROKE_THICKNESS);
      }

      StrokeThickness = calculatedValue;
   }
   
   protected override void CreateCompletedIcons()
   {
      _exceptionCompletedIcon = new PathIcon
      {
         Kind = "CloseOutlined",
         HorizontalAlignment = HorizontalAlignment.Center,
         VerticalAlignment = VerticalAlignment.Center
      };
      BindUtils.CreateTokenBinding(_exceptionCompletedIcon, PathIcon.NormalFillBrushProperty, GlobalResourceKey.ColorError);
      BindUtils.CreateTokenBinding(_exceptionCompletedIcon, PathIcon.DisabledFilledBrushProperty, GlobalResourceKey.ControlItemBgActiveDisabled);
      _successCompletedIcon = new PathIcon
      {
         Kind = "CheckOutlined",
         HorizontalAlignment = HorizontalAlignment.Center,
         VerticalAlignment = VerticalAlignment.Center
      };
      BindUtils.CreateTokenBinding(_successCompletedIcon, PathIcon.NormalFillBrushProperty, GlobalResourceKey.ColorSuccess);
      BindUtils.CreateTokenBinding(_successCompletedIcon, PathIcon.DisabledFilledBrushProperty, GlobalResourceKey.ControlItemBgActiveDisabled);
      _successCompletedIcon.IsVisible = false;
      _exceptionCompletedIcon.IsVisible = false;
      AddChildControl(_exceptionCompletedIcon);
      AddChildControl(_successCompletedIcon);
   }
   
   protected override void NotifyApplyFixedStyleConfig()
   {
      base.NotifyApplyFixedStyleConfig();
      BindUtils.CreateTokenBinding(this, CircleMinimumTextFontSizeTokenProperty, ProgressBarResourceKey.CircleMinimumTextFontSize);
      BindUtils.CreateTokenBinding(this, CircleMinimumIconSizeTokenProperty, ProgressBarResourceKey.CircleMinimumIconSize);
   }

   protected override void NotifyEffectSizeTypeChanged()
   {
      base.NotifyEffectSizeTypeChanged();
      SetupExtraInfoFontSize();
      SetupExtraInfoIconSize();
   }
}