﻿using AtomUI.Data;
using AtomUI.Media;
using AtomUI.Theme;
using AtomUI.Theme.Styling;
using AtomUI.Theme.Utils;
using AtomUI.Utils;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Input.GestureRecognizers;
using Avalonia.Layout;
using Avalonia.Styling;

namespace AtomUI.Controls;

[ControlThemeProvider]
internal class MenuScrollViewerTheme : BaseControlTheme
{
   public const string ScrollUpButtonPart = "PART_ScrollUpButton";
   public const string ScrollDownButtonPart = "PART_ScrollDownButton";
   public const string ScrollViewContentPart = "PART_ContentPresenter";
   public const string MainContainerPart = "PART_MainContainer";

   public MenuScrollViewerTheme()
      : base(typeof(MenuScrollViewer)) { }

   protected override IControlTemplate BuildControlTemplate()
   {
      return new FuncControlTemplate<MenuScrollViewer>((viewer, scope) =>
      {
         var transitions = new Transitions();
         transitions.Add(AnimationUtils.CreateTransition<SolidColorBrushTransition>(IconButton.BackgroundProperty));
         var dockPanel = new DockPanel();
         var scrollUpButton = new IconButton()
         {
            Name = ScrollUpButtonPart,
            Icon = new PathIcon()
            {
               Kind = "UpOutlined"
            },
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Transitions = transitions,
            RenderTransform = null
         };
         CreateTemplateParentBinding(scrollUpButton, IconButton.CommandProperty, nameof(MenuScrollViewer.LineUp));
         TokenResourceBinder.CreateTokenBinding(scrollUpButton.Icon, PathIcon.WidthProperty, MenuTokenResourceKey.ScrollButtonIconSize);
         TokenResourceBinder.CreateTokenBinding(scrollUpButton.Icon, PathIcon.HeightProperty, MenuTokenResourceKey.ScrollButtonIconSize);
         DockPanel.SetDock(scrollUpButton, Dock.Top);
         var scrollDownButton = new IconButton()
         {
            Name = ScrollDownButtonPart,
            Icon = new PathIcon()
            {
               Kind = "DownOutlined"
            },
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Transitions = transitions,
            RenderTransform = null
         };
         CreateTemplateParentBinding(scrollDownButton, IconButton.CommandProperty, nameof(MenuScrollViewer.LineDown));
         TokenResourceBinder.CreateTokenBinding(scrollDownButton.Icon, PathIcon.WidthProperty, MenuTokenResourceKey.ScrollButtonIconSize);
         TokenResourceBinder.CreateTokenBinding(scrollDownButton.Icon, PathIcon.HeightProperty, MenuTokenResourceKey.ScrollButtonIconSize);
         DockPanel.SetDock(scrollDownButton, Dock.Bottom);

         var scrollViewContent = CreateScrollContentPresenter(viewer);

         dockPanel.Children.Add(scrollUpButton);
         dockPanel.Children.Add(scrollDownButton);
         dockPanel.Children.Add(scrollViewContent);
         scrollUpButton.RegisterInNameScope(scope);
         scrollDownButton.RegisterInNameScope(scope);
         scrollViewContent.RegisterInNameScope(scope);
         return dockPanel;
      });
   }

   private ScrollContentPresenter CreateScrollContentPresenter(MenuScrollViewer viewer)
   {
      var scrollViewContent = new ScrollContentPresenter()
      {
         Name = ScrollViewContentPart
      };
      CreateTemplateParentBinding(scrollViewContent, ScrollContentPresenter.MarginProperty,
                                  MenuScrollViewer.PaddingProperty);
      CreateTemplateParentBinding(scrollViewContent, ScrollContentPresenter.HorizontalSnapPointsAlignmentProperty,
                                  MenuScrollViewer.HorizontalSnapPointsAlignmentProperty);
      CreateTemplateParentBinding(scrollViewContent, ScrollContentPresenter.HorizontalSnapPointsTypeProperty,
                                  MenuScrollViewer.HorizontalSnapPointsTypeProperty);
      CreateTemplateParentBinding(scrollViewContent, ScrollContentPresenter.VerticalSnapPointsAlignmentProperty,
                                  MenuScrollViewer.VerticalSnapPointsAlignmentProperty);
      CreateTemplateParentBinding(scrollViewContent, ScrollContentPresenter.VerticalSnapPointsTypeProperty,
                                  MenuScrollViewer.VerticalSnapPointsTypeProperty);
      var scrollGestureRecognizer = new ScrollGestureRecognizer();
      BindUtils.RelayBind(scrollViewContent, ScrollContentPresenter.CanHorizontallyScrollProperty, scrollGestureRecognizer,
                          ScrollGestureRecognizer.CanHorizontallyScrollProperty);
      BindUtils.RelayBind(scrollViewContent, ScrollContentPresenter.CanVerticallyScrollProperty, scrollGestureRecognizer,
                          ScrollGestureRecognizer.CanVerticallyScrollProperty);

      CreateTemplateParentBinding(scrollGestureRecognizer, ScrollGestureRecognizer.IsScrollInertiaEnabledProperty, 
                                  MenuScrollViewer.IsScrollInertiaEnabledProperty);
      scrollViewContent.GestureRecognizers.Add(scrollGestureRecognizer);

      return scrollViewContent;
   }

   protected override void BuildStyles()
   {
      {
         var iconButtonStyle = new Style(selector => selector.Nesting().Template().OfType<IconButton>());
         iconButtonStyle.Add(IconButton.PaddingProperty, MenuTokenResourceKey.ScrollButtonPadding);
         iconButtonStyle.Add(IconButton.MarginProperty, MenuTokenResourceKey.ScrollButtonMargin);
         iconButtonStyle.Add(IconButton.BackgroundProperty, GlobalTokenResourceKey.ColorTransparent);
         Add(iconButtonStyle);
      }
      
      {
         var iconButtonStyle = new Style(selector => selector.Nesting().Template().OfType<IconButton>().Class(StdPseudoClass.PointerOver));
         iconButtonStyle.Add(IconButton.BackgroundProperty, MenuTokenResourceKey.ItemHoverBg);
         Add(iconButtonStyle);
      }

   }
}