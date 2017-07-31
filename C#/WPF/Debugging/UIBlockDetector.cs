using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Threading;


///<summary>
///Class used for finding where a UI lock comes from. Breaks once the UI is locked for longer than maxFreezeTimeInMilliseconds
///The thread can then be found in the threads window.
///Found here: https://stackoverflow.com/a/21411656
///</summary>
public class UIBlockDetector
{
    static Timer _Timer;

    public UIBlockDetector(int maxFreezeTimeInMilliseconds = 200)
    {
        var sw = new Stopwatch();

        new DispatcherTimer(TimeSpan.FromMilliseconds(10), DispatcherPriority.Send, (sender, args) =>
        {
            lock (sw)
            {
                sw.Restart();
            }
        }, Application.Current.Dispatcher);

        _Timer = new Timer(state =>
        {
            lock (sw)
            {
                if (sw.ElapsedMilliseconds > maxFreezeTimeInMilliseconds)
                    Debugger.Break();
            }
        }, null, TimeSpan.FromMilliseconds(0), TimeSpan.FromMilliseconds(10));
    }
}