using BlazorCore.Data.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.Avatar
{
    public partial class Avatar : ComponentBase
    {
        [Parameter]
        public AvatarModel? AvatarModel { get; set; }
        [Parameter]
        public int AvatarSize { set; get; } = 33;
        private const string HeightWidth = "min-width: {0}px; height: {0}px; width: {0}px;";
        private const string FontSize = "font-size: {0}px;";
        private const int MinFontSize = 12;
        private string _style = string.Empty;

        protected override void OnInitialized()
        {
            var fontSize = Math.Max(MinFontSize, AvatarSize / 3);
            
            _style = string.Format(HeightWidth, AvatarSize) + string.Format(FontSize, fontSize);
        }
    }
}
