using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components
{
    public partial class RedirectTo : ComponentBase
    {
        [Parameter, EditorRequired]
        public string Path { get; set; } = default!;
        [Parameter]
        public bool ForceLoad { get; set; }
        [Inject]
        NavigationManager NavigationManager { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            NavigationManager.NavigateTo(Path, ForceLoad);

            await Task.CompletedTask;
        }
    }
}
