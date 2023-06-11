using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.Loading
{
    public partial class LoadingBar : ComponentBase
    {
        [Parameter]
        public int CompletedPercentage { set; get; }
        [Parameter]
        public bool IsCompleted { set; get; }
        [Parameter]
        public string BaseColor { set; get; } = "black";
        [Parameter]
        public string CompletedColor { set; get; } = "black";
        [Parameter]
        public int StrokeWidth { set; get; } = 2;
        [Parameter]
        public string Style { set; get; } = string.Empty;
    }
}
