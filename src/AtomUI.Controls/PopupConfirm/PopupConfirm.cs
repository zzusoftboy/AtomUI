﻿using Avalonia;
using Avalonia.Controls.Templates;

namespace AtomUI.Controls;

public enum PopupConfirmStatus
{
   Info,
   Warning,
   Error
}

public class PopupConfirm : FlyoutHost
{
   #region 公共属性属性

   public static readonly StyledProperty<string> OkTextProperty =
      AvaloniaProperty.Register<PopupConfirm, string>(nameof(OkText), "Ok");
   
   public static readonly StyledProperty<string> CancelTextProperty =
      AvaloniaProperty.Register<PopupConfirm, string>(nameof(CancelText), "Cancel");
   
   public static readonly StyledProperty<ButtonType> OkButtonTypeProperty =
      AvaloniaProperty.Register<PopupConfirm, ButtonType>(nameof(OkButtonType), ButtonType.Primary);
   
   public static readonly StyledProperty<bool> IsShowCancelButtonProperty =
      AvaloniaProperty.Register<PopupConfirm, bool>(nameof(IsShowCancelButton), true);
   
   public static readonly StyledProperty<string> TitleProperty =
      AvaloniaProperty.Register<PopupConfirm, string>(nameof(Title));
   
   public static readonly StyledProperty<object?> ConfirmContentProperty =
      AvaloniaProperty.Register<PopupConfirm, object?>(nameof(ConfirmContent));
   
   public static readonly StyledProperty<IDataTemplate?> ConfirmContentTemplateProperty =
      AvaloniaProperty.Register<PopupConfirm, IDataTemplate?>(nameof(ConfirmContentTemplate));
   
   public static readonly StyledProperty<PathIcon?> IconProperty
      = AvaloniaProperty.Register<PopupConfirm, PathIcon?>(nameof(Icon));
   
   public static readonly StyledProperty<PopupConfirmStatus> ConfirmStatusProperty
      = AvaloniaProperty.Register<PopupConfirm, PopupConfirmStatus>(nameof(ConfirmStatus), PopupConfirmStatus.Warning);
   
   public string OkText
   {
      get => GetValue(OkTextProperty);
      set => SetValue(OkTextProperty, value);
   }
   
   public string CancelText
   {
      get => GetValue(CancelTextProperty);
      set => SetValue(CancelTextProperty, value);
   }
   
   public ButtonType OkButtonType
   {
      get => GetValue(OkButtonTypeProperty);
      set => SetValue(OkButtonTypeProperty, value);
   }
   
   public bool IsShowCancelButton
   {
      get => GetValue(IsShowCancelButtonProperty);
      set => SetValue(IsShowCancelButtonProperty, value);
   }
   
   public string Title
   {
      get => GetValue(TitleProperty);
      set => SetValue(TitleProperty, value);
   }
   
   public object? ConfirmContent
   {
      get => GetValue(ConfirmContentProperty);
      set => SetValue(ConfirmContentProperty, value);
   }
   
   public IDataTemplate? ConfirmContentTemplate
   {
      get => GetValue(ConfirmContentTemplateProperty);
      set => SetValue(ConfirmContentTemplateProperty, value);
   }
   
   public PathIcon? Icon
   {
      get => GetValue(IconProperty);
      set => SetValue(IconProperty, value);
   }
   
   public PopupConfirmStatus ConfirmStatus
   {
      get => GetValue(ConfirmStatusProperty);
      set => SetValue(ConfirmStatusProperty, value);
   }

   #endregion
   
   public sealed override void ApplyTemplate()
   {
      if (Flyout is null) {
         Flyout = new PopupConfirmFlyout(this);
      }

      if (Icon is null) {
         Icon = new PathIcon()
         {
            Kind = "ExclamationCircleFilled"
         };
      }
      base.ApplyTemplate();
   }
}