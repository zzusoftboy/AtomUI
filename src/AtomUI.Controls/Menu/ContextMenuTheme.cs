﻿using AtomUI.Theme;
using AtomUI.Theme.Styling;
using AtomUI.Utils;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Media;

namespace AtomUI.Controls;

[ControlThemeProvider]
public class ContextMenuTheme : BaseControlTheme
{
   public const string ItemsPresenterPart  = "PART_ItemsPresenter";
   public const string RootContainerPart   = "PART_RootContainer";
   
   public ContextMenuTheme()
      : base(typeof(ContextMenu)) { }

   protected override IControlTemplate BuildControlTemplate()
   {
      return new FuncControlTemplate<ContextMenu>((menu, scope) =>
      {
         var wrapper = new Border()
         {
            Name = RootContainerPart
         };
         TokenResourceBinder.CreateTokenBinding(wrapper, Border.BackgroundProperty, MenuTokenResourceKey.MenuBgColor);
         TokenResourceBinder.CreateTokenBinding(wrapper, Border.MinWidthProperty, MenuTokenResourceKey.MenuPopupMinWidth);
         TokenResourceBinder.CreateTokenBinding(wrapper, Border.MaxWidthProperty, MenuTokenResourceKey.MenuPopupMaxWidth);
         TokenResourceBinder.CreateTokenBinding(wrapper, Border.MinHeightProperty, MenuTokenResourceKey.MenuPopupMinHeight);
         TokenResourceBinder.CreateTokenBinding(wrapper, Border.MaxHeightProperty, MenuTokenResourceKey.MenuPopupMaxHeight);
         TokenResourceBinder.CreateTokenBinding(wrapper, Border.PaddingProperty, MenuTokenResourceKey.MenuPopupContentPadding);
         TokenResourceBinder.CreateTokenBinding(wrapper, Border.CornerRadiusProperty, MenuTokenResourceKey.MenuPopupBorderRadius);

         var scrollViewer = new MenuScrollViewer();
         var itemsPresenter = new ItemsPresenter
         {
            Name = ItemsPresenterPart,
         };
         CreateTemplateParentBinding(itemsPresenter, ItemsPresenter.ItemsPanelProperty, MenuItem.ItemsPanelProperty);
         KeyboardNavigation.SetTabNavigation(itemsPresenter, KeyboardNavigationMode.Continue);
         Grid.SetIsSharedSizeScope(itemsPresenter, true);
         scrollViewer.Content = itemsPresenter;
         wrapper.Child = scrollViewer;
         return wrapper;
      });
   }

   protected override void BuildStyles()
   {
      this.Add(ContextMenu.BackgroundProperty, new SolidColorBrush(Colors.Transparent));
      this.Add(ContextMenu.CornerRadiusProperty, MenuTokenResourceKey.MenuPopupBorderRadius);
   }
}