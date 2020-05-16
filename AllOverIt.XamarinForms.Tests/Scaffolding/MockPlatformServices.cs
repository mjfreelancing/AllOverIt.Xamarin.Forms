using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AllOverIt.XamarinForms.Tests.Scaffolding
{
  internal class MockPlatformServices : IPlatformServices
  {
    private readonly Action<Action> _invokeOnMainThread;
    private readonly Action<Uri> _openUriAction;
    private readonly Func<Uri, CancellationToken, Task<Stream>> _getStreamAsync;

    public bool IsInvokeRequired => false;
    public string RuntimePlatform { get; set; }

    public MockPlatformServices(Action<Action> invokeOnMainThread = null, Action<Uri> openUriAction = null,
      Func<Uri, CancellationToken, Task<Stream>> getStreamAsync = null)
    {
      _invokeOnMainThread = invokeOnMainThread;
      _openUriAction = openUriAction;
      _getStreamAsync = getStreamAsync;
    }

    public void BeginInvokeOnMainThread(Action action)
    {
      if (_invokeOnMainThread == null)
        action.Invoke();
      else
        _invokeOnMainThread(action);
    }

    public Ticker CreateTicker()
    {
      return new MockTicker();
    }

    public Assembly[] GetAssemblies()
    {
      return AppDomain.CurrentDomain.GetAssemblies(); //new Assembly[0];
    }

    public string GetMD5Hash(string input)
    {
      throw new NotImplementedException();
    }

    public double GetNamedSize(NamedSize size, Type targetElement, bool useOldSizes)
    {
      return size switch
      {
        NamedSize.Default => 10,
        NamedSize.Micro => 4,
        NamedSize.Small => 8,
        NamedSize.Medium => 12,
        NamedSize.Large => 16,
        NamedSize.Body => 10,
        NamedSize.Header => 12,
        NamedSize.Title => 16,
        NamedSize.Subtitle => 14,
        NamedSize.Caption => 12,

        _ => throw new ArgumentOutOfRangeException(nameof(size)),
      };
    }

    public Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken)
    {
      if (_getStreamAsync == null)
      {
        throw new NotImplementedException();
      }

      return _getStreamAsync.Invoke(uri, cancellationToken);
    }

    public IIsolatedStorageFile GetUserStoreForApplication()
    {
      throw new NotImplementedException();
    }

    public void OpenUriAction(Uri uri)
    {
      if (_openUriAction != null)
      {
        _openUriAction.Invoke(uri);
      }
      else
      {
        throw new NotImplementedException();
      }
    }

    public void StartTimer(TimeSpan interval, Func<bool> callback)
    {
      Timer timer = null;

      void OnTimeout(object o)
      {
        BeginInvokeOnMainThread(() =>
        {
          if (callback.Invoke())
          {
            return;
          }

          timer.Dispose();
        });
      }

      timer = new Timer(OnTimeout, null, interval, interval);
    }

    public void QuitApplication()
    {
      throw new NotImplementedException();
    }

    public SizeRequest GetNativeSize(VisualElement view, double widthConstraint, double heightConstraint)
    {
      throw new NotImplementedException();
    }
  }
}