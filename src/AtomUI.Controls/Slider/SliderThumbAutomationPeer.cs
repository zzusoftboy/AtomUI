﻿using Avalonia.Automation.Peers;

namespace AtomUI.Controls;

public class SliderThumbAutomationPeer : ControlAutomationPeer
{
   public SliderThumbAutomationPeer(SliderThumb owner) : base(owner) { }
   protected override AutomationControlType GetAutomationControlTypeCore() => AutomationControlType.Thumb;
   protected override bool IsContentElementCore() => false;
}