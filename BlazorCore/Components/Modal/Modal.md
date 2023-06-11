# Modal Component

### Parameters

Parameters:
- Header - RenderFragment, usually the title of the modal
- Body - RenderFragment
- Footer - RenderFragment
- Id
- IsMinimized
- Size - sm, md, lg, xl
- Style - css style

### Usage

```html
@using BlazorCore.Components.Modal;

<BaseModal @ref="Modal" Size="Modal.ModalSize.md" IsMinimized="@IsMinimized">
    <Header>
        Modal Title
        <button @onclick="Modal.Close">x</button>
    </Header>
    <Body>
        Body of modal
    </Body>
    <Footer>
        Modal Footer
        <button @onclick="OnConfirm">
            Confirm
        </button>
    </Footer>
</BaseModal>

<button onclick="@UploadModalOpen">Open Modal</button>
```
```c#
private Modal Modal { get; set; }

public void UploadModalOpen()
{
    Modal.Open();
}
public void OnConfirm()
{
    //do something
    Modal.Close();
}
```