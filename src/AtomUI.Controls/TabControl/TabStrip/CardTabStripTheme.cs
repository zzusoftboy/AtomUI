﻿using AtomUI.Theme.Styling;
using AtomUI.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.Styling;

namespace AtomUI.Controls;

[ControlThemeProvider]
internal class CardTabStripTheme : BaseTabStripTheme
{
   public const string AddTabButtonPart             = "PART_AddTabButton";
   public const string CardTabStripContainerPart    = "PART_CardTabStripContainer";
   public const string CardTabStripScrollViewerPart = "PART_CardTabStripScrollViewer";
   
   public CardTabStripTheme() : base(typeof(CardTabStrip)) { }

   protected override void NotifyBuildControlTemplate(BaseTabStrip baseTabStrip, INameScope scope, Border container)
   {
      var alignWrapper = new Panel()
      {
         Name = AlignWrapperPart
      };
      var cardTabStripContainer = new TabsContainerPanel()
      {
         Name = TabsContainerPart,
      };
      cardTabStripContainer.RegisterInNameScope(scope);
      CreateTemplateParentBinding(cardTabStripContainer, TabsContainerPanel.TabStripPlacementProperty, TabStrip.TabStripPlacementProperty);
      
      var tabScrollViewer = new TabStripScrollViewer()
      {
         Name = CardTabStripScrollViewerPart
      };
      tabScrollViewer.RegisterInNameScope(scope);
      CreateTemplateParentBinding(tabScrollViewer, BaseTabScrollViewer.TabStripPlacementProperty, TabStrip.TabStripPlacementProperty);
      var contentPanel = CreateTabStripContentPanel(scope);
      tabScrollViewer.Content = contentPanel;
      tabScrollViewer.TabStrip = baseTabStrip;

      var addTabIcon = new PathIcon()
      {
         Kind = "PlusOutlined"
      };

      TokenResourceBinder.CreateTokenBinding(addTabIcon, PathIcon.NormalFilledBrushProperty, TabControlTokenResourceKey.ItemColor);
      TokenResourceBinder.CreateTokenBinding(addTabIcon, PathIcon.ActiveFilledBrushProperty, TabControlTokenResourceKey.ItemHoverColor);
      TokenResourceBinder.CreateTokenBinding(addTabIcon, PathIcon.DisabledFilledBrushProperty, GlobalTokenResourceKey.ColorTextDisabled);
      
      TokenResourceBinder.CreateGlobalResourceBinding(addTabIcon, PathIcon.WidthProperty, GlobalTokenResourceKey.IconSize);
      TokenResourceBinder.CreateGlobalResourceBinding(addTabIcon, PathIcon.HeightProperty, GlobalTokenResourceKey.IconSize);

      var addTabButton = new IconButton
      {
         Name = AddTabButtonPart,
         BorderThickness = new Thickness(1),
         Icon = addTabIcon
      };
      
      CreateTemplateParentBinding(addTabButton, IconButton.BorderThicknessProperty, CardTabStrip.CardBorderThicknessProperty);
      CreateTemplateParentBinding(addTabButton, IconButton.CornerRadiusProperty, CardTabStrip.CardBorderRadiusProperty);
      CreateTemplateParentBinding(addTabButton, IconButton.IsVisibleProperty, CardTabStrip.IsShowAddTabButtonProperty);
      
      TokenResourceBinder.CreateGlobalResourceBinding(addTabButton, IconButton.BorderBrushProperty, GlobalTokenResourceKey.ColorBorderSecondary);
      
      addTabButton.RegisterInNameScope(scope);

      cardTabStripContainer.TabScrollViewer = tabScrollViewer;
      cardTabStripContainer.AddTabButton = addTabButton;
      
      alignWrapper.Children.Add(cardTabStripContainer);
      
      container.Child = alignWrapper;
   }
   
   private ItemsPresenter CreateTabStripContentPanel(INameScope scope)
   {
      var itemsPresenter = new ItemsPresenter
      {
         Name = ItemsPresenterPart,
      };
      itemsPresenter.RegisterInNameScope(scope);
      
      CreateTemplateParentBinding(itemsPresenter, ItemsPresenter.ItemsPanelProperty, TabStrip.ItemsPanelProperty);
      return itemsPresenter;
   }

   protected override void BuildStyles()
   {
      base.BuildStyles();
      var commonStyle = new Style(selector => selector.Nesting());
      
      // 设置 items presenter 面板样式
      // 分为上、右、下、左
      {
         // 上
         var topStyle = new Style(selector => selector.Nesting().Class(BaseTabStrip.TopPC));
         var containerStyle = new Style(selector => selector.Nesting().Template().Name(CardTabStripContainerPart));
         topStyle.Add(containerStyle);
         
         var itemPresenterPanelStyle = new Style(selector => selector.Nesting().Template().Name(ItemsPresenterPart).Child().OfType<StackPanel>());
         itemPresenterPanelStyle.Add(StackPanel.OrientationProperty, Orientation.Horizontal);
         itemPresenterPanelStyle.Add(StackPanel.SpacingProperty, TabControlTokenResourceKey.CardGutter);

         var addTabButtonStyle = new Style(selector => selector.Nesting().Template().Name(AddTabButtonPart));
         addTabButtonStyle.Add(IconButton.MarginProperty, TabControlTokenResourceKey.AddTabButtonMarginHorizontal);
         topStyle.Add(addTabButtonStyle);
         
         topStyle.Add(itemPresenterPanelStyle);
         commonStyle.Add(topStyle);
      }

      {
         // 右
         var rightStyle = new Style(selector => selector.Nesting().Class(BaseTabStrip.RightPC));
         
         var containerStyle = new Style(selector => selector.Nesting().Template().Name(CardTabStripContainerPart));
         containerStyle.Add(StackPanel.OrientationProperty, Orientation.Vertical);
         rightStyle.Add(containerStyle);
         
         var itemPresenterPanelStyle = new Style(selector => selector.Nesting().Template().Name(ItemsPresenterPart).Child().OfType<StackPanel>());
         itemPresenterPanelStyle.Add(StackPanel.OrientationProperty, Orientation.Vertical);
         itemPresenterPanelStyle.Add(StackPanel.SpacingProperty, TabControlTokenResourceKey.CardGutter);
         rightStyle.Add(itemPresenterPanelStyle);
         
         var addTabButtonStyle = new Style(selector => selector.Nesting().Template().Name(AddTabButtonPart));
         addTabButtonStyle.Add(IconButton.MarginProperty, TabControlTokenResourceKey.AddTabButtonMarginVertical);
         rightStyle.Add(addTabButtonStyle);
         
         commonStyle.Add(rightStyle);
      }
      {
         // 下
         var bottomStyle = new Style(selector => selector.Nesting().Class(BaseTabStrip.BottomPC));
         
         var containerStyle = new Style(selector => selector.Nesting().Template().Name(CardTabStripContainerPart));
         containerStyle.Add(StackPanel.OrientationProperty, Orientation.Horizontal);
         bottomStyle.Add(containerStyle);
         
         var itemPresenterPanelStyle = new Style(selector => selector.Nesting().Template().Name(ItemsPresenterPart).Child().OfType<StackPanel>());
         itemPresenterPanelStyle.Add(StackPanel.OrientationProperty, Orientation.Horizontal);
         itemPresenterPanelStyle.Add(StackPanel.SpacingProperty, TabControlTokenResourceKey.CardGutter);
         bottomStyle.Add(itemPresenterPanelStyle);
         
         var addTabButtonStyle = new Style(selector => selector.Nesting().Template().Name(AddTabButtonPart));
         addTabButtonStyle.Add(IconButton.MarginProperty, TabControlTokenResourceKey.AddTabButtonMarginHorizontal);
         bottomStyle.Add(addTabButtonStyle);
         
         commonStyle.Add(bottomStyle);
      }
      {
         // 左
         var leftStyle = new Style(selector => selector.Nesting().Class(BaseTabStrip.LeftPC));
         
         var containerStyle = new Style(selector => selector.Nesting().Template().Name(CardTabStripContainerPart));
         containerStyle.Add(StackPanel.OrientationProperty, Orientation.Vertical);
         leftStyle.Add(containerStyle);
         
         var itemPresenterPanelStyle = new Style(selector => selector.Nesting().Template().Name(ItemsPresenterPart).Child().OfType<StackPanel>());
         itemPresenterPanelStyle.Add(StackPanel.OrientationProperty, Orientation.Vertical);
         itemPresenterPanelStyle.Add(StackPanel.SpacingProperty, TabControlTokenResourceKey.CardGutter);
         leftStyle.Add(itemPresenterPanelStyle);
         
         var addTabButtonStyle = new Style(selector => selector.Nesting().Template().Name(AddTabButtonPart));
         addTabButtonStyle.Add(IconButton.MarginProperty, TabControlTokenResourceKey.AddTabButtonMarginVertical);
         leftStyle.Add(addTabButtonStyle);
         
         commonStyle.Add(leftStyle);
      }

      Add(commonStyle);
   }
}