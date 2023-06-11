using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.Modal
{
    public partial class BaseModal : ComponentBase
    {
        [Parameter]
        public RenderFragment? Header { get; set; }
        [Parameter]
        public RenderFragment? Body { get; set; }
        [Parameter]
        public RenderFragment? Footer { get; set; }
        [Parameter]
        public string Id { get; set; } = string.Empty;
        [Parameter]
        public bool IsMinimized { get; set; }
        [Parameter]
        public ModalSize Size { get; set; } = 0;
        [Parameter]
        public string? Style { get; set; }
        [Parameter]
        public string? ModalClass { get; set; }
        private string Display = "none;";
        private string Class = "";

        public void Open()
        {
            Display = "block;";
            Class = "show";
            StateHasChanged();
        }

        public void Close()
        {
            Display = "none";
            Class = "";
            StateHasChanged();
        }

        public enum ModalSize
        {
            sm,
            md,
            lg,
            xl,
            xxl,
            large
        }
    }
}
