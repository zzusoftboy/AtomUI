﻿using AtomUI.Styling;
using AtomUI.Utils;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.Media;

namespace AtomUI.Controls;

[ControlThemeProvider]
internal class ToolTipTheme : ControlTheme
{
   public const string ToolTipContainerPart = "PART_ToolTipContainer";
   
   public ToolTipTheme()
      : base(typeof(ToolTip))
   {
   }
   
   protected override IControlTemplate? BuildControlTemplate()
   {
      return new FuncControlTemplate<ToolTip>((tip, scope) =>
      {
         var arrowDecoratedBox = new ArrowDecoratedBox()
         {
            Name = ToolTipContainerPart,
         };
         if (tip.Content is string text) {
            arrowDecoratedBox.Child = new TextBlock
            {
               Text = text,
               VerticalAlignment = VerticalAlignment.Center,
               HorizontalAlignment = HorizontalAlignment.Center,
               TextWrapping = TextWrapping.Wrap,
            };
         } else if (tip.Content is Control control) {
            arrowDecoratedBox.Child = control;
         }
         
         CreateTemplateParentBinding(arrowDecoratedBox, ArrowDecoratedBox.IsShowArrowProperty, ToolTip.IsShowArrowEffectiveProperty);
         arrowDecoratedBox.RegisterInNameScope(scope);
         return arrowDecoratedBox;
      });
   }
}