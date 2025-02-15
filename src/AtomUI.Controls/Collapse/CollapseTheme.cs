﻿using AtomUI.Theme;
using AtomUI.Theme.Styling;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Styling;

namespace AtomUI.Controls;

[ControlThemeProvider]
internal class CollapseTheme : BaseControlTheme
{
   public const string FrameDecoratorPart = "PART_FrameDecorator";
   public const string ItemsPresenterPart = "PART_ItemsPresenter";
   
   public CollapseTheme() : base(typeof(Collapse)) {}
   
   
   protected override IControlTemplate BuildControlTemplate()
   {
      return new FuncControlTemplate<Collapse>((collapse, scope) =>
      {
         var frameDecorator = new Border()
         {
            Name = FrameDecoratorPart,
            ClipToBounds = true
         };
         var itemsPresenter = new ItemsPresenter()
         {
            Name = ItemsPresenterPart
         };
         itemsPresenter.RegisterInNameScope(scope);
         frameDecorator.Child = itemsPresenter;
         
         CreateTemplateParentBinding(itemsPresenter, ItemsPresenter.ItemsPanelProperty, Collapse.ItemsPanelProperty);
         CreateTemplateParentBinding(frameDecorator, Border.BorderThicknessProperty, Collapse.EffectiveBorderThicknessProperty);
         CreateTemplateParentBinding(frameDecorator, Border.BorderBrushProperty, Collapse.BorderBrushProperty);
         CreateTemplateParentBinding(frameDecorator, Border.CornerRadiusProperty, Collapse.CornerRadiusProperty);
         
         return frameDecorator;
      });
   }
      
   protected override void BuildStyles()
   {
      var commonStyle = new Style(selector => selector.Nesting());
      commonStyle.Add(Collapse.BorderBrushProperty, GlobalTokenResourceKey.ColorBorder);
      commonStyle.Add(Collapse.CornerRadiusProperty, CollapseTokenResourceKey.CollapsePanelBorderRadius);
      
      Add(commonStyle);
   }
}