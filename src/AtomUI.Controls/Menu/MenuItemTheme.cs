﻿using AtomUI.Media;
using AtomUI.Theme;
using AtomUI.Theme.Styling;
using AtomUI.Theme.Utils;
using AtomUI.Utils;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;

namespace AtomUI.Controls;

[ControlThemeProvider]
internal class MenuItemTheme : BaseControlTheme
{
   public const string ItemDecoratorPart     = "PART_ItemDecorator";
   public const string MainContainerPart     = "PART_MainContainer";
   public const string TogglePresenterPart   = "PART_TogglePresenter";
   public const string ItemIconPresenterPart = "PART_ItemIconPresenter";
   public const string ItemTextPresenterPart = "PART_ItemTextPresenter";
   public const string InputGestureTextPart  = "PART_InputGestureText";
   public const string MenuIndicatorIconPart = "PART_MenuIndicatorIcon";
   public const string PopupPart             = "PART_Popup";
   public const string ItemsPresenterPart    = "PART_ItemsPresenter";
   
   public MenuItemTheme()
      : base(typeof(MenuItem))
   {
   }

   protected override IControlTemplate BuildControlTemplate()
   {
      return new FuncControlTemplate<MenuItem>((item, scope) =>
      {
         // 仅仅为了把 Popup 包进来，没有其他什么作用
         var layoutWrapper = new Panel();
         var container = new Border()
         {
            Name = ItemDecoratorPart
         };
         
         var transitions = new Transitions();
         transitions.Add(AnimationUtils.CreateTransition<SolidColorBrushTransition>(Border.BackgroundProperty));
         container.Transitions = transitions;
         
         var layout = new Grid()
         {
            Name = MainContainerPart,
            ColumnDefinitions =
            {
               new ColumnDefinition(GridLength.Auto)
               {
                  SharedSizeGroup = "TogglePresenter"
               },
               new ColumnDefinition(GridLength.Auto)
               {
                  SharedSizeGroup = "IconPresenter"
               },
               new ColumnDefinition(GridLength.Star),
               new ColumnDefinition(GridLength.Auto)
               {
                  SharedSizeGroup = "InputGestureText"
               },
               new ColumnDefinition(GridLength.Auto)
               {
                  SharedSizeGroup = "MenuIndicatorIcon"
               }
            }
         };
         layout.RegisterInNameScope(scope);

         var togglePresenter = new ContentControl
         {
            Name = TogglePresenterPart,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            IsVisible = false
         };
         CreateTemplateParentBinding(togglePresenter, ContentControl.IsEnabledProperty, MenuItem.IsEnabledProperty);
         Grid.SetColumn(togglePresenter, 0);
         togglePresenter.RegisterInNameScope(scope);

         var iconPresenter = new Viewbox
         {
            Name = ItemIconPresenterPart,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Stretch = Stretch.Uniform
         };
         
         Grid.SetColumn(iconPresenter, 1);
         iconPresenter.RegisterInNameScope(scope);
         CreateTemplateParentBinding(iconPresenter, Viewbox.ChildProperty, MenuItem.IconProperty);
         TokenResourceBinder.CreateTokenBinding(iconPresenter, Viewbox.MarginProperty, MenuTokenResourceKey.ItemMargin);
         TokenResourceBinder.CreateGlobalTokenBinding(iconPresenter, Viewbox.WidthProperty, MenuTokenResourceKey.ItemIconSize);
         TokenResourceBinder.CreateGlobalTokenBinding(iconPresenter, Viewbox.HeightProperty, MenuTokenResourceKey.ItemIconSize);

         var itemTextPresenter = new ContentPresenter
         {
            Name = ItemTextPresenterPart,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Center,
            RecognizesAccessKey = true,
            IsHitTestVisible = false
         };
         Grid.SetColumn(itemTextPresenter, 2);
         TokenResourceBinder.CreateTokenBinding(itemTextPresenter, ContentPresenter.MarginProperty, MenuTokenResourceKey.ItemMargin);
         CreateTemplateParentBinding(itemTextPresenter, ContentPresenter.ContentProperty, MenuItem.HeaderProperty);
         CreateTemplateParentBinding(itemTextPresenter, ContentPresenter.ContentTemplateProperty, MenuItem.HeaderTemplateProperty);

         itemTextPresenter.RegisterInNameScope(scope);

         var inputGestureText = new TextBlock
         {
            Name = InputGestureTextPart,
            HorizontalAlignment = HorizontalAlignment.Right,
            TextAlignment = TextAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
         };
         Grid.SetColumn(inputGestureText, 3);
         TokenResourceBinder.CreateTokenBinding(inputGestureText, ContentPresenter.MarginProperty, MenuTokenResourceKey.ItemMargin);
         CreateTemplateParentBinding(inputGestureText, 
                                     TextBlock.TextProperty, 
                                     MenuItem.InputGestureProperty,
                                     BindingMode.Default,
                                     MenuItem.KeyGestureConverter);
         
         inputGestureText.RegisterInNameScope(scope);

         var menuIndicatorIcon = new PathIcon
         {
            Name = MenuIndicatorIconPart,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
            Kind = "RightOutlined"
         };
         TokenResourceBinder.CreateGlobalTokenBinding(menuIndicatorIcon, PathIcon.WidthProperty, GlobalTokenResourceKey.IconSizeXS);
         TokenResourceBinder.CreateGlobalTokenBinding(menuIndicatorIcon, PathIcon.HeightProperty, GlobalTokenResourceKey.IconSizeXS);
         Grid.SetColumn(menuIndicatorIcon, 4);
         menuIndicatorIcon.RegisterInNameScope(scope);

         var popup = CreateMenuPopup();
         popup.RegisterInNameScope(scope);

         layout.Children.Add(togglePresenter);
         layout.Children.Add(iconPresenter);
         layout.Children.Add(itemTextPresenter);
         layout.Children.Add(inputGestureText);
         layout.Children.Add(menuIndicatorIcon);
         layout.Children.Add(popup);
         
         container.Child = layout;
         layoutWrapper.Children.Add(container);
         return layoutWrapper;
      });
   }

   private Popup CreateMenuPopup()
   {
      var popup = new Popup()
      {
         Name = PopupPart,
         WindowManagerAddShadowHint = false,
         IsLightDismissEnabled = false,
         Placement = PlacementMode.RightEdgeAlignedTop,
      };
      
      var border = new Border();
      TokenResourceBinder.CreateTokenBinding(border, Border.BackgroundProperty, GlobalTokenResourceKey.ColorBgContainer);
      TokenResourceBinder.CreateTokenBinding(border, Border.CornerRadiusProperty, MenuTokenResourceKey.MenuPopupBorderRadius);
      TokenResourceBinder.CreateTokenBinding(border, Border.MinWidthProperty, MenuTokenResourceKey.MenuPopupMinWidth);
      TokenResourceBinder.CreateTokenBinding(border, Border.MaxWidthProperty, MenuTokenResourceKey.MenuPopupMaxWidth);
      TokenResourceBinder.CreateTokenBinding(border, Border.MinHeightProperty, MenuTokenResourceKey.MenuPopupMinHeight);
      TokenResourceBinder.CreateTokenBinding(border, Border.MaxHeightProperty, MenuTokenResourceKey.MenuPopupMaxHeight);
      TokenResourceBinder.CreateTokenBinding(border, Border.PaddingProperty, MenuTokenResourceKey.MenuPopupContentPadding);
      
      var scrollViewer = new MenuScrollViewer();
      var itemsPresenter = new ItemsPresenter
      {
         Name = ItemsPresenterPart,
      };
      CreateTemplateParentBinding(itemsPresenter, ItemsPresenter.ItemsPanelProperty, MenuItem.ItemsPanelProperty);
      Grid.SetIsSharedSizeScope(itemsPresenter, true);
      scrollViewer.Content = itemsPresenter;
      border.Child = scrollViewer;
      
      popup.Child = border;

      TokenResourceBinder.CreateTokenBinding(popup, Popup.MarginToAnchorProperty, MenuTokenResourceKey.TopLevelItemPopupMarginToAnchor);
      TokenResourceBinder.CreateTokenBinding(popup, Popup.MaskShadowsProperty, MenuTokenResourceKey.MenuPopupBoxShadows);
      CreateTemplateParentBinding(popup, Popup.IsOpenProperty, MenuItem.IsSubMenuOpenProperty, BindingMode.TwoWay);
      
      return popup;
   }

   protected override void BuildStyles()
   {
      var commonStyle = new Style(selector => selector.Nesting());
      BuildCommonStyle(commonStyle);
      BuildMenuIndicatorStyle();
      BuildMenuIconStyle();
      Add(commonStyle);
      BuildDisabledStyle();
   }

   private void BuildCommonStyle(Style commonStyle)
   {
      commonStyle.Add(MenuItem.ForegroundProperty, MenuTokenResourceKey.ItemColor);
      {
         var keyGestureStyle = new Style(selector => selector.Nesting().Template().Name(InputGestureTextPart));
         keyGestureStyle.Add(TextBlock.ForegroundProperty, MenuTokenResourceKey.KeyGestureColor);
         commonStyle.Add(keyGestureStyle);
      }
      {
         var borderStyle = new Style(selector => selector.Nesting().Template().Name(ItemDecoratorPart));
         borderStyle.Add(Border.MinHeightProperty, MenuTokenResourceKey.ItemHeight);
         borderStyle.Add(Border.PaddingProperty, MenuTokenResourceKey.ItemPaddingInline);
         borderStyle.Add(Border.BackgroundProperty, MenuTokenResourceKey.ItemBg);
         borderStyle.Add(Border.CornerRadiusProperty, MenuTokenResourceKey.ItemBorderRadius);
         commonStyle.Add(borderStyle);
      }
      
      // Hover 状态
      var hoverStyle = new Style(selector => selector.Nesting().Class(StdPseudoClass.PointerOver));
      hoverStyle.Add(MenuItem.ForegroundProperty, MenuTokenResourceKey.ItemHoverColor);
      {
         var borderStyle = new Style(selector => selector.Nesting().Template().Name(ItemDecoratorPart));
         borderStyle.Add(Border.BackgroundProperty, MenuTokenResourceKey.ItemHoverBg);
         hoverStyle.Add(borderStyle);
      }
      commonStyle.Add(hoverStyle);
   }

   private void BuildMenuIndicatorStyle()
   {
      {
         var menuIndicatorStyle = new Style(selector => selector.Nesting().Template().Name(MenuIndicatorIconPart));
         menuIndicatorStyle.Add(PathIcon.IsVisibleProperty, true);
         Add(menuIndicatorStyle);
      }
      var hasSubMenuStyle = new Style(selector => selector.Nesting().Class(StdPseudoClass.Empty));
      {
         var menuIndicatorStyle = new Style(selector => selector.Nesting().Template().Name(MenuIndicatorIconPart));
         menuIndicatorStyle.Add(PathIcon.IsVisibleProperty, false);
         hasSubMenuStyle.Add(menuIndicatorStyle);
      }
      Add(hasSubMenuStyle);
   }

   private void BuildMenuIconStyle()
   {
      {
         var iconViewBoxStyle = new Style(selector => selector.Nesting().Template().Name(ItemIconPresenterPart));
         iconViewBoxStyle.Add(Viewbox.IsVisibleProperty, false);
         Add(iconViewBoxStyle);
      }

      var hasIconStyle = new Style(selector => selector.Nesting().Class(":icon"));
      {
         var iconViewBoxStyle = new Style(selector => selector.Nesting().Template().Name(ItemIconPresenterPart));
         iconViewBoxStyle.Add(Viewbox.IsVisibleProperty, true);
         hasIconStyle.Add(iconViewBoxStyle);
      }
      Add(hasIconStyle);
   }

   private void BuildDisabledStyle()
   {
      var disabledStyle = new Style(selector => selector.Nesting().Class(StdPseudoClass.Disabled));
      disabledStyle.Add(MenuItem.ForegroundProperty, MenuTokenResourceKey.ItemDisabledColor);
      Add(disabledStyle);
   }
}