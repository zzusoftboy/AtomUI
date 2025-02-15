﻿using AtomUI.Theme;
using AtomUI.Theme.Styling;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.Styling;

namespace AtomUI.Controls;

[ControlThemeProvider]
public class OptionButtonGroupTheme : BaseControlTheme
{
   public const string MainContainerPart = "PART_MainContainer";
   
   public OptionButtonGroupTheme()
      : base(typeof(OptionButtonGroup))
   {
   }

   protected override IControlTemplate? BuildControlTemplate()
   {
      return new FuncControlTemplate<OptionButtonGroup>((group, scope) =>
      {
         var layout = new StackPanel
         {
            Name = MainContainerPart,
            Orientation = Orientation.Horizontal,
            ClipToBounds = true,
         };
         layout.RegisterInNameScope(scope);
         return layout;
      });
   }

   protected override void BuildStyles()
   {
      var largeSizeStyle =
         new Style(selector => selector.Nesting().PropertyEquals(OptionButtonGroup.SizeTypeProperty, SizeType.Large));
      largeSizeStyle.Add(OptionButtonGroup.CornerRadiusProperty, GlobalTokenResourceKey.BorderRadiusLG);
      largeSizeStyle.Add(OptionButtonGroup.MaxHeightProperty, GlobalTokenResourceKey.ControlHeightLG);
      Add(largeSizeStyle);
      
      var middleSizeStyle =
         new Style(selector => selector.Nesting().PropertyEquals(OptionButtonGroup.SizeTypeProperty, SizeType.Middle));
      middleSizeStyle.Add(OptionButtonGroup.CornerRadiusProperty, GlobalTokenResourceKey.BorderRadius);
      middleSizeStyle.Add(OptionButtonGroup.MaxHeightProperty, GlobalTokenResourceKey.ControlHeight);
      Add(middleSizeStyle);
      
      var smallSizeStyle =
         new Style(selector => selector.Nesting().PropertyEquals(OptionButtonGroup.SizeTypeProperty, SizeType.Small));
      smallSizeStyle.Add(OptionButtonGroup.CornerRadiusProperty, GlobalTokenResourceKey.BorderRadiusSM);
      smallSizeStyle.Add(OptionButtonGroup.MaxHeightProperty, GlobalTokenResourceKey.ControlHeightSM);
      Add(smallSizeStyle);

      this.Add(OptionButtonGroup.BorderBrushProperty, GlobalTokenResourceKey.ColorBorder);
      this.Add(OptionButtonGroup.SelectedOptionBorderColorProperty, GlobalTokenResourceKey.ColorPrimary);
      this.Add(OptionButtonGroup.BorderThicknessProperty, GlobalTokenResourceKey.BorderThickness);
   }
}