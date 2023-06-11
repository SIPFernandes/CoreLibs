using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.Modal;

public partial class ModalConfirm : ComponentBase
{
    [Parameter]
    public string? ConfirmTitle { get; set; }
    [Parameter]
    public bool ConfirmDisabled { get; set; }
    [Parameter]
    public bool DontCloseOnConfirm { get; set; }
    [Parameter]
    public string Class { get; set; } = string.Empty;
    [Parameter]
    public RenderFragment? Header { get; set; }
    [Parameter]
    public RenderFragment? Body { get; set; }
    [Parameter]
    public BaseModal.ModalSize Size { get; set; } = BaseModal.ModalSize.md;
    [Parameter, EditorRequired]
    public EventCallback OnConfirmation { get; set; }
    [Parameter]
    public EventCallback OnCancel { get; set; }    
    private BaseModal? ModalRef { get; set; } = default!;
    private bool Submitting;

    public void CloseModal()
    {
        ModalRef?.Close();
    }

    public void OpenModal()
    {
        ModalRef!.Open();
    }
    
    private async Task OnConfirm(bool confirmation)
    {
        if (!Submitting)
        {
            Submitting = true;
            
            if (confirmation)
            {
                if (!DontCloseOnConfirm) 
                {
                    ModalRef!.Close();
                }

                await OnConfirmation.InvokeAsync();
            }
            else
            {
                ModalRef!.Close();

                await OnCancel.InvokeAsync();
            }

            Submitting = false;
        }
    }
}