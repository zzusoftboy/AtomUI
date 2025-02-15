﻿
using AtomUI.Theme.Styling;
using AtomUI.Theme;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.Styling;

namespace AtomUI.Controls;

[ControlThemeProvider]
internal class SeparatorTheme : BaseControlTheme
{
   public const string TitlePart = "PART_Title";

   public SeparatorTheme()
      : base(typeof(Separator)) { }

   protected override IControlTemplate? BuildControlTemplate()
   {
      return new FuncControlTemplate<Separator>((separator, scope) =>
      {
         var titleLabel = new Label
         {
            Name = TitlePart,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center,
            Padding = new Thickness(0)
         };
         CreateTemplateParentBinding(titleLabel, Label.ContentProperty, Separator.TitleProperty);
         CreateTemplateParentBinding(titleLabel, Label.FontSizeProperty, Separator.FontSizeProperty);
         CreateTemplateParentBinding(titleLabel, Label.ForegroundProperty, Separator.TitleColorProperty);
         CreateTemplateParentBinding(titleLabel, Label.FontStyleProperty, Separator.FontStyleProperty);
         CreateTemplateParentBinding(titleLabel, Label.FontWeightProperty, Separator.FontWeightProperty);
         titleLabel.RegisterInNameScope(scope);
         return titleLabel;
      });
   }

   protected override void BuildStyles()
   {
      // 默认的一些样式
      this.Add(Separator.TitleColorProperty, GlobalTokenResourceKey.ColorText);
      this.Add(Separator.FontSizeProperty, GlobalTokenResourceKey.FontSize);
      this.Add(Separator.LineColorProperty, GlobalTokenResourceKey.ColorSplit);
      this.Add(Separator.TextPaddingInlineProperty, SeparatorTokenResourceKey.TextPaddingInline);
      this.Add(Separator.OrientationMarginPercentProperty, SeparatorTokenResourceKey.OrientationMarginPercent);
      this.Add(Separator.VerticalMarginInlineProperty, SeparatorTokenResourceKey.VerticalMarginInline);

      var titleSelector = default(Selector).Nesting().Template().OfType<Label>().Name(TitlePart);
      var horizontalStyle =
         new Style(selector => selector.Nesting()
                                       .PropertyEquals(Separator.OrientationProperty, Orientation.Horizontal));
      horizontalStyle.Add(Separator.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
      horizontalStyle.Add(Separator.VerticalAlignmentProperty, VerticalAlignment.Center);
      {
         var titleStyle = new Style(selector => titleSelector);
         titleStyle.Add(Label.IsVisibleProperty, true);
         horizontalStyle.Add(titleStyle);
      }
      Add(horizontalStyle);

      var verticalStyle =
         new Style(selector => selector.Nesting().PropertyEquals(Separator.OrientationProperty, Orientation.Vertical));
      verticalStyle.Add(Separator.HorizontalAlignmentProperty, HorizontalAlignment.Center);
      verticalStyle.Add(Separator.VerticalAlignmentProperty, VerticalAlignment.Center);
      {
         var titleStyle = new Style(selector => titleSelector);
         titleStyle.Add(Label.IsVisibleProperty, false);
         verticalStyle.Add(titleStyle);
      }
      Add(verticalStyle);
   }
}