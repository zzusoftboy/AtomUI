﻿using AtomUI.Icon;
using AtomUI.Theme;
using AtomUI.Theme.Styling;
using AtomUI.Utils;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Styling;

namespace AtomUI.Controls;

internal class BaseTabStripItemTheme : BaseControlTheme
{
   public const string DecoratorPart = "PART_Decorator";
   public const string ContentLayoutPart = "PART_ContentLayout";
   public const string ContentPresenterPart = "PART_ContentPresenter";
   public const string ItemIconPart = "PART_ItemIcon";
   public const string ItemCloseButtonPart = "PART_ItemCloseButton";
   
   public BaseTabStripItemTheme(Type targetType) : base(targetType) { }
   
   protected override IControlTemplate BuildControlTemplate()
   {
      return new FuncControlTemplate<TabStripItem>((strip, scope) =>
      {
         // 做边框
         var decorator = new Border()
         {
            Name = DecoratorPart
         };
         decorator.RegisterInNameScope(scope);
         NotifyBuildControlTemplate(strip, scope, decorator);
         return decorator;
      });
   }

   protected virtual void NotifyBuildControlTemplate(TabStripItem stripItem, INameScope scope, Border container)
   {
      var containerLayout = new StackPanel()
      {
         Name = ContentLayoutPart,
         Orientation = Orientation.Horizontal
      };
      containerLayout.RegisterInNameScope(scope);
      
      var contentPresenter = new ContentPresenter()
      {
         Name = ContentPresenterPart
      };
      containerLayout.Children.Add(contentPresenter);
      CreateTemplateParentBinding(contentPresenter, ContentPresenter.ContentProperty, TabStripItem.ContentProperty);
      CreateTemplateParentBinding(contentPresenter, ContentPresenter.ContentTemplateProperty, TabStripItem.ContentTemplateProperty);

      var iconButton = new IconButton()
      {
         Name = ItemCloseButtonPart
      };
      iconButton.RegisterInNameScope(scope);
      TokenResourceBinder.CreateTokenBinding(iconButton, IconButton.MarginProperty, TabControlTokenResourceKey.CloseIconMargin);
      
      CreateTemplateParentBinding(iconButton, IconButton.IconProperty, TabStripItem.CloseIconProperty);
      CreateTemplateParentBinding(iconButton, IconButton.IsVisibleProperty, TabStripItem.IsClosableProperty);
      
      containerLayout.Children.Add(iconButton);
      container.Child = containerLayout;
   }

   protected override void BuildStyles()
   {
      var commonStyle = new Style(selector => selector.Nesting());
      commonStyle.Add(TabStripItem.CursorProperty, new Cursor(StandardCursorType.Hand));
      commonStyle.Add(TabStripItem.ForegroundProperty, TabControlTokenResourceKey.ItemColor);
      
      // Icon 一些通用属性
      {
         var iconStyle = new Style(selector => selector.Nesting().Template().Name(ItemIconPart));
         iconStyle.Add(PathIcon.MarginProperty, TabControlTokenResourceKey.ItemIconMargin);
         commonStyle.Add(iconStyle);
      }
      
      // hover
      var hoverStyle = new Style(selector => selector.Nesting().Class(StdPseudoClass.PointerOver));
      hoverStyle.Add(TabStripItem.ForegroundProperty, TabControlTokenResourceKey.ItemHoverColor);
      {
         var iconStyle = new Style(selector => selector.Nesting().Template().Name(ItemIconPart));
         iconStyle.Add(PathIcon.IconModeProperty, IconMode.Active);
         hoverStyle.Add(iconStyle);
      }
   
      commonStyle.Add(hoverStyle);
      
      // 选中
      var selectedStyle = new Style(selector => selector.Nesting().Class(StdPseudoClass.Selected));
      selectedStyle.Add(TabStripItem.ForegroundProperty, TabControlTokenResourceKey.ItemSelectedColor);
      {
         var iconStyle = new Style(selector => selector.Nesting().Template().Name(ItemIconPart));
         iconStyle.Add(PathIcon.IconModeProperty, IconMode.Selected);
         selectedStyle.Add(iconStyle);
      }
      commonStyle.Add(selectedStyle);
      Add(commonStyle);
      BuildSizeTypeStyle();
      BuildPlacementStyle();
      BuildDisabledStyle();
   }

   private void BuildSizeTypeStyle()
   {
      var largeSizeStyle = new Style(selector => selector.Nesting().PropertyEquals(TabStripItem.SizeTypeProperty, SizeType.Large));
      
      largeSizeStyle.Add(TabStripItem.FontSizeProperty, TabControlTokenResourceKey.TitleFontSizeLG);
      {
         // Icon
         var iconStyle = new Style(selector => selector.Nesting().Template().Name(ItemIconPart));
         iconStyle.Add(PathIcon.WidthProperty, GlobalTokenResourceKey.IconSize);
         iconStyle.Add(PathIcon.HeightProperty, GlobalTokenResourceKey.IconSize);
         largeSizeStyle.Add(iconStyle);
      }
      
      Add(largeSizeStyle);

      var middleSizeStyle = new Style(selector => selector.Nesting().PropertyEquals(TabStripItem.SizeTypeProperty, SizeType.Middle));
      {
         // Icon
         var iconStyle = new Style(selector => selector.Nesting().Template().Name(ItemIconPart));
         iconStyle.Add(PathIcon.WidthProperty, GlobalTokenResourceKey.IconSize);
         iconStyle.Add(PathIcon.HeightProperty, GlobalTokenResourceKey.IconSize);
         middleSizeStyle.Add(iconStyle);
      }
      middleSizeStyle.Add(TabStripItem.FontSizeProperty, TabControlTokenResourceKey.TitleFontSize);
      Add(middleSizeStyle);

      var smallSizeType = new Style(selector => selector.Nesting().PropertyEquals(TabStripItem.SizeTypeProperty, SizeType.Small));
      
      {
         // Icon
         var iconStyle = new Style(selector => selector.Nesting().Template().Name(ItemIconPart));
         iconStyle.Add(PathIcon.WidthProperty, GlobalTokenResourceKey.IconSizeSM);
         iconStyle.Add(PathIcon.HeightProperty, GlobalTokenResourceKey.IconSizeSM);
         smallSizeType.Add(iconStyle);
      }
      
      smallSizeType.Add(TabStripItem.FontSizeProperty, TabControlTokenResourceKey.TitleFontSizeSM);
      Add(smallSizeType);
   }
   
   private void BuildPlacementStyle()
   {
      // 设置 items presenter 面板样式
      // 分为上、右、下、左
      {
         // 上
         var topStyle = new Style(selector => selector.Nesting().PropertyEquals(TabStripItem.TabStripPlacementProperty, Dock.Top));
         var iconStyle = new Style(selector => selector.Nesting().Template().OfType<PathIcon>());
         iconStyle.Add(PathIcon.VerticalAlignmentProperty, VerticalAlignment.Center);
         topStyle.Add(iconStyle);
         Add(topStyle);
      }

      {
         // 右
         var rightStyle = new Style(selector => selector.Nesting().PropertyEquals(TabStripItem.TabStripPlacementProperty, Dock.Right));
         var iconStyle = new Style(selector => selector.Nesting().Template().OfType<PathIcon>());
         iconStyle.Add(PathIcon.HorizontalAlignmentProperty, HorizontalAlignment.Left);
         iconStyle.Add(PathIcon.VerticalAlignmentProperty, VerticalAlignment.Center);
         rightStyle.Add(iconStyle);
         Add(rightStyle);
      }
      {
         // 下
         var bottomStyle = new Style(selector => selector.Nesting().PropertyEquals(TabStripItem.TabStripPlacementProperty, Dock.Bottom));
         
         var iconStyle = new Style(selector => selector.Nesting().Template().OfType<PathIcon>());
         iconStyle.Add(PathIcon.VerticalAlignmentProperty, VerticalAlignment.Center);
         bottomStyle.Add(iconStyle);
         Add(bottomStyle);
      }
      {
         // 左
         var leftStyle = new Style(selector => selector.Nesting().PropertyEquals(TabStripItem.TabStripPlacementProperty, Dock.Left));
         var iconStyle = new Style(selector => selector.Nesting().Template().OfType<PathIcon>());
         iconStyle.Add(PathIcon.VerticalAlignmentProperty, VerticalAlignment.Center);
         iconStyle.Add(PathIcon.HorizontalAlignmentProperty, HorizontalAlignment.Left);
         leftStyle.Add(iconStyle);
         Add(leftStyle);
      }
   }

   private void BuildDisabledStyle()
   {
      var disabledStyle = new Style(selector => selector.Nesting().Class(StdPseudoClass.Disabled));
      disabledStyle.Add(TabStripItem.ForegroundProperty, GlobalTokenResourceKey.ColorTextDisabled);
      Add(disabledStyle);
   }
}