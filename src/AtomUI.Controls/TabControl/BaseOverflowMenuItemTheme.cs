﻿using AtomUI.Media;
using AtomUI.Theme;
using AtomUI.Theme.Styling;
using AtomUI.Theme.Utils;
using AtomUI.Utils;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.Styling;

namespace AtomUI.Controls;

[ControlThemeProvider]
internal class BaseOverflowMenuItemTheme : BaseControlTheme
{
   public const string ItemDecoratorPart     = "PART_ItemDecorator";
   public const string MainContainerPart     = "PART_MainContainer";
   public const string ItemTextPresenterPart = "PART_ItemTextPresenter";
   public const string ItemCloseButtonPart = "PART_ItemCloseIcon";
   
   public BaseOverflowMenuItemTheme()
      : base(typeof(BaseOverflowMenuItem))
   {
   }

   protected override IControlTemplate BuildControlTemplate()
   {
      return new FuncControlTemplate<BaseOverflowMenuItem>((item, scope) =>
      {
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
               new ColumnDefinition(GridLength.Star),
               new ColumnDefinition(GridLength.Auto)
               {
                  SharedSizeGroup = "MenuCloseIcon"
               }
            }
         };
         
         var itemTextPresenter = new ContentPresenter
         {
            Name = ItemTextPresenterPart,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Center,
            RecognizesAccessKey = true,
            IsHitTestVisible = false
         };
         
         Grid.SetColumn(itemTextPresenter, 0);
         TokenResourceBinder.CreateTokenBinding(itemTextPresenter, ContentPresenter.MarginProperty, MenuTokenResourceKey.ItemMargin);
         CreateTemplateParentBinding(itemTextPresenter, ContentPresenter.ContentProperty, BaseOverflowMenuItem.HeaderProperty);
         CreateTemplateParentBinding(itemTextPresenter, ContentPresenter.ContentTemplateProperty, BaseOverflowMenuItem.HeaderTemplateProperty);

         itemTextPresenter.RegisterInNameScope(scope);
         
         var menuCloseIcon = new PathIcon
         {
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
            Kind = "CloseOutlined"
         };

         var closeButton = new IconButton()
         {
            Name = ItemCloseButtonPart,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
            Icon = menuCloseIcon
         };


         CreateTemplateParentBinding(closeButton, IconButton.IsVisibleProperty, BaseOverflowMenuItem.IsClosableProperty);
         TokenResourceBinder.CreateGlobalTokenBinding(menuCloseIcon, PathIcon.NormalFilledBrushProperty, GlobalTokenResourceKey.ColorIcon);
         TokenResourceBinder.CreateGlobalTokenBinding(menuCloseIcon, PathIcon.ActiveFilledBrushProperty, GlobalTokenResourceKey.ColorIconHover);
         
         TokenResourceBinder.CreateGlobalTokenBinding(menuCloseIcon, PathIcon.WidthProperty, GlobalTokenResourceKey.IconSizeSM);
         TokenResourceBinder.CreateGlobalTokenBinding(menuCloseIcon, PathIcon.HeightProperty, GlobalTokenResourceKey.IconSizeSM);
         
         Grid.SetColumn(menuCloseIcon, 4);
         closeButton.RegisterInNameScope(scope);
         
         layout.Children.Add(itemTextPresenter);
         layout.Children.Add(closeButton);
         container.Child = layout;
         
         return container;
      });
   }
   
   protected override void BuildStyles()
   {
      var commonStyle = new Style(selector => selector.Nesting());
      BuildCommonStyle(commonStyle);
      BuildDisabledStyle();
      Add(commonStyle);
   }

   private void BuildCommonStyle(Style commonStyle)
   {
      commonStyle.Add(BaseOverflowMenuItem.ForegroundProperty, MenuTokenResourceKey.ItemColor);
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
      hoverStyle.Add(BaseOverflowMenuItem.ForegroundProperty, MenuTokenResourceKey.ItemHoverColor);
      {
         var borderStyle = new Style(selector => selector.Nesting().Template().Name(ItemDecoratorPart));
         borderStyle.Add(Border.BackgroundProperty, MenuTokenResourceKey.ItemHoverBg);
         hoverStyle.Add(borderStyle);
      }
      commonStyle.Add(hoverStyle);
   }
   
   private void BuildDisabledStyle()
   {
      var disabledStyle = new Style(selector => selector.Nesting().Class(StdPseudoClass.Disabled));
      disabledStyle.Add(BaseOverflowMenuItem.ForegroundProperty, MenuTokenResourceKey.ItemDisabledColor);
      Add(disabledStyle);
   }
   
}