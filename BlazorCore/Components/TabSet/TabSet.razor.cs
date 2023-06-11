using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.TabSet
{
    public partial class TabSet : ComponentBase
    {
        [Parameter, EditorRequired]
        public TabSetItem SelectedTab { get; set; } = default!;
        [Parameter, EditorRequired]
        public List<TabSetItem> TabsList { get; set; } = default!;
        [Parameter, EditorRequired]
        public EventCallback<TabSetItem> SelectedTabChanged { get; set; }
        [Parameter]
        public bool DisplayValue { get; set; } = true;

        public void UpdateTabValue(string tab, int value)
        {
            TabsList.Single(x => x.Name == tab).Value = value;

            StateHasChanged();
        }

        private async Task ChangeTab(TabSetItem tab)
        {
            if (SelectedTab.Name != tab.Name)
            {
                SelectedTab = tab;

                await SelectedTabChanged.InvokeAsync(tab);
            }
        }
    }

    public class TabSetItem
    {
        public string Name { get; set; }
        public string? Icon { get; set; }
        public int Value { get; set; }

        public TabSetItem(string name, string? icon = null, int value = 0) 
        { 
            Name = name;
            Icon = icon;
            Value = value;
        }
    }
}