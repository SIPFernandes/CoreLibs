using BlazorCore.Data.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.Avatar;

public partial class AvatarGroup : ComponentBase
{
    [Parameter, EditorRequired]
    public IEnumerable<string> UsersId { get; set; } = default!;
    [Parameter]
    public int Size { get; set; } = 32;
    [Parameter]
    public int? MoreItems { get; set; }
    private AvatarModel AvatarMoreItems { get; set; } = default!;

    protected override void OnInitialized()
    {                
        if (MoreItems.HasValue)
        {
            AvatarMoreItems = new AvatarModel
            {
                Placeholder = "+" + MoreItems.Value
            };
        }
    }    
}