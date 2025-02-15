﻿using AtomUI.Theme;
using AtomUI.Theme.Styling;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Styling;

namespace AtomUI.Controls;

[ControlThemeProvider]
public class MenuTheme : BaseControlTheme
{
   public const string ItemsPresenterPart = "PART_ItemsPresenter";
   public MenuTheme()
      : base(typeof(Menu))
   {
   }

   protected override IControlTemplate? BuildControlTemplate()
   {
      return new FuncControlTemplate<Menu>((menu, scope) =>
      {
         var itemPresenter = new ItemsPresenter()
         {
            Name = ItemsPresenterPart,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Center,
            ItemsPanel = new FuncTemplate<Panel?>(() => new StackPanel()
            {
               Orientation = Orientation.Horizontal
            })
         };

         KeyboardNavigation.SetTabNavigation(itemPresenter, KeyboardNavigationMode.Continue);
         
         var border = new Border()
         {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Child = itemPresenter,
         };
         CreateTemplateParentBinding(border, Border.PaddingProperty, Menu.PaddingProperty);
         CreateTemplateParentBinding(border, Border.BackgroundProperty, Menu.BackgroundProperty);
         CreateTemplateParentBinding(border, Border.BackgroundSizingProperty, Menu.BackgroundSizingProperty);
         CreateTemplateParentBinding(border, Border.BorderThicknessProperty, Menu.BorderThicknessProperty);
         CreateTemplateParentBinding(border, Border.BorderBrushProperty, Menu.BorderBrushProperty);
         CreateTemplateParentBinding(border, Border.CornerRadiusProperty, Menu.CornerRadiusProperty);
         return border;
      });
   }

   protected override void BuildStyles()
   {
      var commonStyle = new Style(selector => selector.Nesting());
      commonStyle.Add(Menu.HorizontalAlignmentProperty, HorizontalAlignment.Left);
      commonStyle.Add(Menu.BackgroundProperty, MenuTokenResourceKey.MenuBgColor);
      commonStyle.Add(Menu.BorderBrushProperty, GlobalTokenResourceKey.ColorBorder);
      var largeSizeType = new Style(selector => selector.Nesting().PropertyEquals(Menu.SizeTypeProperty, SizeType.Large));
      largeSizeType.Add(Menu.MinHeightProperty, GlobalTokenResourceKey.ControlHeightLG);
      largeSizeType.Add(Menu.CornerRadiusProperty, GlobalTokenResourceKey.BorderRadius);
      commonStyle.Add(largeSizeType);
      
      var middleSizeType = new Style(selector => selector.Nesting().PropertyEquals(Menu.SizeTypeProperty, SizeType.Middle));
      middleSizeType.Add(Menu.MinHeightProperty, GlobalTokenResourceKey.ControlHeight);
      middleSizeType.Add(Menu.CornerRadiusProperty, GlobalTokenResourceKey.BorderRadius);
      commonStyle.Add(middleSizeType);
      
      var smallSizeType = new Style(selector => selector.Nesting().PropertyEquals(Menu.SizeTypeProperty, SizeType.Small));
      smallSizeType.Add(Menu.MinHeightProperty, GlobalTokenResourceKey.ControlHeightSM);
      smallSizeType.Add(Menu.CornerRadiusProperty, GlobalTokenResourceKey.BorderRadiusSM);
      commonStyle.Add(smallSizeType);
      Add(commonStyle);
   }
}