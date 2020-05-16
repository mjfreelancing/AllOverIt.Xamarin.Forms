using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AllOverIt.XamarinForms.Tests.Scaffolding
{
  internal class MockDeviceInfo : DeviceInfo
  {
    public override Size PixelScreenSize => throw new NotImplementedException();
    public override Size ScaledScreenSize => throw new NotImplementedException();
    public override double ScalingFactor => throw new NotImplementedException();
  }
}