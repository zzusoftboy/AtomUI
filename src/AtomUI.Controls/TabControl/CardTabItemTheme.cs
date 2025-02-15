﻿using AtomUI.Media;
using AtomUI.Theme.Styling;
using AtomUI.Theme.Utils;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Styling;

namespace AtomUI.Controls;

[ControlThemeProvider]
internal class CardTabItemTheme : BaseTabItemTheme
{
   public const string ID = "CardTabItem";
   
   public CardTabItemTheme() : base(typeof(TabItem)) { }
   
   public override string ThemeResourceKey()
   {
      return ID;
   }
   
   protected override void NotifyBuildControlTemplate(TabItem tabItem, INameScope scope, Border container)
   {
      base.NotifyBuildControlTemplate(tabItem, scope, container);

      if (container.Transitions is null) {
         var transitions = new Transitions();
         transitions.Add(AnimationUtils.CreateTransition<SolidColorBrushTransition>(Border.BackgroundProperty));
         container.Transitions = transitions;
      }
      CreateTemplateParentBinding(container, Border.BorderThicknessProperty, TabItem.BorderThicknessProperty);
      CreateTemplateParentBinding(container, Border.CornerRadiusProperty, TabItem.CornerRadiusProperty);
   }
   
   protected override void BuildStyles()
   {
      base.BuildStyles();
      
      var commonStyle = new Style(selector => selector.Nesting());

      {
         var decoratorStyle = new Style(selector => selector.Nesting().Template().Name(DecoratorPart));
         decoratorStyle.Add(Border.MarginProperty, TabControlTokenResourceKey.HorizontalItemMargin);
         decoratorStyle.Add(Border.BackgroundProperty, TabControlTokenResourceKey.CardBg);
         decoratorStyle.Add(Border.BorderBrushProperty, GlobalTokenResourceKey.ColorBorderSecondary);
         commonStyle.Add(decoratorStyle);
      }
      
      // 选中
      var selectedStyle = new Style(selector => selector.Nesting().Class(StdPseudoClass.Selected));
      {
         var decoratorStyle = new Style(selector => selector.Nesting().Template().Name(DecoratorPart));
         decoratorStyle.Add(Border.BackgroundProperty, GlobalTokenResourceKey.ColorBgContainer);
         selectedStyle.Add(decoratorStyle);
      }
      commonStyle.Add(selectedStyle);
      
      Add(commonStyle);
      
      BuildSizeTypeStyle();
      BuildPlacementStyle();
      BuildDisabledStyle();
   }

   protected void BuildSizeTypeStyle()
   {
      var topOrBottomStyle = new Style(selector => Selectors.Or(selector.Nesting().PropertyEquals(TabItem.TabStripPlacementProperty, Dock.Top),
                                                                selector.Nesting().PropertyEquals(TabItem.TabStripPlacementProperty, Dock.Bottom)));
      {
         var largeSizeStyle = new Style(selector => selector.Nesting().PropertyEquals(TabItem.SizeTypeProperty, SizeType.Large));
         {
            var decoratorStyle = new Style(selector => selector.Nesting().Template().Name(DecoratorPart));
            decoratorStyle.Add(Border.PaddingProperty, TabControlTokenResourceKey.CardPaddingLG);
            largeSizeStyle.Add(decoratorStyle);
         }
         topOrBottomStyle.Add(largeSizeStyle);

         var middleSizeStyle = new Style(selector => selector.Nesting().PropertyEquals(TabItem.SizeTypeProperty, SizeType.Middle));
         {
            var decoratorStyle = new Style(selector => selector.Nesting().Template().Name(DecoratorPart));
            decoratorStyle.Add(Border.PaddingProperty, TabControlTokenResourceKey.CardPadding);
            middleSizeStyle.Add(decoratorStyle);
         }
         topOrBottomStyle.Add(middleSizeStyle);

         var smallSizeType = new Style(selector => selector.Nesting().PropertyEquals(TabItem.SizeTypeProperty, SizeType.Small));
         {
            var decoratorStyle = new Style(selector => selector.Nesting().Template().Name(DecoratorPart));
            decoratorStyle.Add(Border.PaddingProperty, TabControlTokenResourceKey.CardPaddingSM);
            smallSizeType.Add(decoratorStyle);
         }
    
         topOrBottomStyle.Add(smallSizeType);
      }
      Add(topOrBottomStyle);
      
      var leftOrRightStyle = new Style(selector => Selectors.Or(selector.Nesting().PropertyEquals(TabItem.TabStripPlacementProperty, Dock.Left),
                                                                selector.Nesting().PropertyEquals(TabItem.TabStripPlacementProperty, Dock.Right)));
      {
         var largeSizeStyle = new Style(selector => selector.Nesting().PropertyEquals(TabItem.SizeTypeProperty, SizeType.Large));
         {
            var decoratorStyle = new Style(selector => selector.Nesting().Template().Name(DecoratorPart));
            decoratorStyle.Add(Border.PaddingProperty, TabControlTokenResourceKey.VerticalItemPadding);
            largeSizeStyle.Add(decoratorStyle);
         }
         leftOrRightStyle.Add(largeSizeStyle);

         var middleSizeStyle = new Style(selector => selector.Nesting().PropertyEquals(TabItem.SizeTypeProperty, SizeType.Middle));
         {
            var decoratorStyle = new Style(selector => selector.Nesting().Template().Name(DecoratorPart));
            decoratorStyle.Add(Border.PaddingProperty, TabControlTokenResourceKey.VerticalItemPadding);
            middleSizeStyle.Add(decoratorStyle);
         }
         leftOrRightStyle.Add(middleSizeStyle);

         var smallSizeType = new Style(selector => selector.Nesting().PropertyEquals(TabItem.SizeTypeProperty, SizeType.Small));
         {
            var decoratorStyle = new Style(selector => selector.Nesting().Template().Name(DecoratorPart));
            decoratorStyle.Add(Border.PaddingProperty, TabControlTokenResourceKey.VerticalItemPadding);
            smallSizeType.Add(decoratorStyle);
         }
    
         leftOrRightStyle.Add(smallSizeType);
      }
      Add(leftOrRightStyle);
   }

   private void BuildPlacementStyle()
   {
      // 设置 items presenter 面板样式
      // 分为上、右、下、左
      {
         // 上
         var topStyle = new Style(selector => selector.Nesting().PropertyEquals(TabItem.TabStripPlacementProperty, Dock.Top));
         var iconStyle = new Style(selector => selector.Nesting().Template().OfType<PathIcon>());
         iconStyle.Add(PathIcon.VerticalAlignmentProperty, VerticalAlignment.Center);
         topStyle.Add(iconStyle);
         
         var decoratorStyle = new Style(selector => selector.Nesting().Template().Name(DecoratorPart));
         decoratorStyle.Add(Border.BorderBrushProperty, GlobalTokenResourceKey.ColorBorderSecondary);
         
         Add(topStyle);
      }

      {
         // 右
         var rightStyle = new Style(selector => selector.Nesting().PropertyEquals(TabItem.TabStripPlacementProperty, Dock.Right));
         var iconStyle = new Style(selector => selector.Nesting().Template().OfType<PathIcon>());
         iconStyle.Add(PathIcon.HorizontalAlignmentProperty, HorizontalAlignment.Center);
         rightStyle.Add(iconStyle);
         Add(rightStyle);
      }
      {
         // 下
         var bottomStyle = new Style(selector => selector.Nesting().PropertyEquals(TabItem.TabStripPlacementProperty, Dock.Bottom));
         
         var iconStyle = new Style(selector => selector.Nesting().Template().OfType<PathIcon>());
         iconStyle.Add(PathIcon.VerticalAlignmentProperty, VerticalAlignment.Center);
         bottomStyle.Add(iconStyle);
         Add(bottomStyle);
      }
      {
         // 左
         var leftStyle = new Style(selector => selector.Nesting().PropertyEquals(TabItem.TabStripPlacementProperty, Dock.Left));
         var iconStyle = new Style(selector => selector.Nesting().Template().OfType<PathIcon>());
         iconStyle.Add(PathIcon.HorizontalAlignmentProperty, HorizontalAlignment.Center);
         leftStyle.Add(iconStyle);
         Add(leftStyle);
      }
   }

   private void BuildDisabledStyle()
   {
      var disabledStyle = new Style(selector => selector.Nesting().Class(StdPseudoClass.Disabled));
      var decoratorStyle = new Style(selector => selector.Nesting().Template().Name(DecoratorPart));
      decoratorStyle.Add(Border.BackgroundProperty, GlobalTokenResourceKey.ColorBgContainerDisabled);
      disabledStyle.Add(decoratorStyle);
      Add(disabledStyle);
   }
}