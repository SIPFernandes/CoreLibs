using BlazorCore.Data.Models;
using DotNetCore.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using static BlazorCore.Areas.Helpers.OfficeHelper.ExcelExtension;

namespace BlazorCore.Areas.Interfaces
{
    public interface IJSInteropService
    {
        public Task TriggerElement(ElementReference? element);
        public Task ExportExcel(string projectName, List<ExcelCell> cells);
        public Task ExportAttachment(FileBase file);
        public Task NavigateToNewTab(string path);
        public Task ScrollBottomListener<T>(DotNetObjectReference<T> objectReference, ElementReference? elementReference = null) where T : class;
        public Task ScrollTopListener<T>(DotNetObjectReference<T> objectReference, ElementReference? elementReference = null) where T : class;
        public Task ScrollTopBottomListener<T>(DotNetObjectReference<T> objectReference, ElementReference? elementReference = null) where T : class;
        public Task OnElementClickOutside<T>(DotNetObjectReference<T> objectReference, ElementReference? elementReference = null) where T : class;
        public Task OnWindowResize<T>(DotNetObjectReference<T> objectReference) where T : class;
        public Task<bool> HasScroll(ElementReference? scrollElement);
        public Task NotifiyChildOnTop<T>(DotNetObjectReference<T> objectReference, ElementReference? scrollElement, string childSelector, string resultSelector) where T : class;
        public Task<DomRectModel> GetElementDomRect(ElementReference reference);
        public Task<DomRectModel> GetWindowDimensions();
        public Task<bool> CanScroll(ElementReference reference);
        public Task<bool> OnWindowResizeRefreshHorizontalSliderScroll(ElementReference reference);
        public Task<bool> ScrollSide(ElementReference reference, bool left, int itemsPerSlide);
        public Task ScrollToElementWithClass(ElementReference containerReference, string classToGoTo);
        public Task ScrollToElement(ElementReference reference, string block = "start", string inline = "start");
        public Task ScrollToElementById(string elementId);
        public Task SubscribeToArrowsMovement<T>(DotNetObjectReference<T> reference, ElementReference element, string selector, string selectedColor) where T : class;
        public Task SetOnWindowResizeListener<T>(DotNetObjectReference<T> reference, string invokeMethodName) where T : class;
        public Task AddClassOnScroll(ElementReference scrollElement, string elementId, string className);
        public Task CopyToClipBoard(string value);
        public Task ToggleFullscreen();
        public Task RemoveClickOutsideListener();
        public Task RemoveClickOutsideListenerAndPopStack();
        public Task SetOnCloseListener<T>(ElementReference elementReference, DotNetObjectReference<T> objectReference) where T : class;
        public Task<string> GetInnerHTMLElement(ElementReference reference);
        public Task<string> GetInnerTextElement(ElementReference reference);
        public Task<string> GetInnerHTMLElementById(string elementId);
        public Task ExportInnerHTMLElement(ElementReference reference, string filename);
        public Task ExportInnerHTMLElementById(string referenceId, string filename);
        public Task SetInnerHTMLElement(ElementReference reference, string? content);
        public Task AppendToInnerHTML(ElementReference reference, string content);
        public Task ReplaceInnerHTML(ElementReference reference, string contentToReplace, string content);
        public Task SubscribeToTextAfterCharacter<T>(DotNetObjectReference<T> objectReference, ElementReference reference, string triggerChar) where T : class;
        public Task UnsubscribeToTextAfterCharacter(ElementReference reference);
        public Task SelectText(ElementReference reference);
        public Task SetContentEditableFocusEnd(ElementReference reference);
        public Task FocusElement(ElementReference reference);
        public Task BlurElement(ElementReference reference);
        public Task EditText(string command);
        public Task<string> GetTimezoneValue();
        public Task SubscribeToTreeKeyboardControls<T>(DotNetObjectReference<T> objectReference,
            ElementReference reference) where T : class;
        public Task SubscribeToDragAndDrop<T>(DotNetObjectReference<T> objectReference,
            ElementReference reference) where T : class;
        public Task UnsubscribeToTreeKeyboardControls();
        public Task UnsubscribeToDragAndDrop();
        public Task PlayGifOnHover(ElementReference reference);
        public Task SetSliderByCardNumber(ElementReference reference, int cardsPerSlide);
        public Task<bool> IsElementOverflown(ElementReference element);
        public Task ValidatePasswordById(string inputValue, string matchPasswordId);
        public Task ValidatePasswordMatch(string inputValue, string passwordId);
    }
}
