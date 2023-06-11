using Microsoft.AspNetCore.Components;
using System.Timers;
using Timer = System.Timers.Timer;

namespace BlazorCore.Components.Carousel;

public partial class Carousel<TItem> : ComponentBase, IDisposable
{
    [Parameter, EditorRequired]
    public RenderFragment<TItem> ItemTemplate { get; set; } = default!;

    [Parameter, EditorRequired]
    public IReadOnlyList<TItem> Items { get; set; } = default!;

    private int _activeIndex = 0;
    private readonly Timer _timer = new Timer(10000); // change every 10 seconds

    protected override void OnInitialized()
    {
        _timer.Elapsed += OnTimerElapsed;
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }

    private async void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        Next();

        await InvokeAsync(StateHasChanged);
    }

    private void Next()
    {
        _activeIndex = (_activeIndex + 1) % Items.Count;
    }

    private void Prev()
    {
        _activeIndex = (_activeIndex - 1 + Items.Count) % Items.Count;
    }
    
    private void SetActiveIndex(int index)
    {
        _activeIndex = index;
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}