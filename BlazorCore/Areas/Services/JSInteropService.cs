using BlazorCore.Areas.Interfaces;
using BlazorCore.Data.Models;
using DotNetCore.Consts;
using DotNetCore.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using static BlazorCore.Areas.Helpers.OfficeHelper.ExcelExtension;

namespace BlazorCore.Areas.Services
{
    public class JSInteropService : IJSInteropService
    {
        private readonly IJSRuntime _jS;                            

        public JSInteropService(IJSRuntime js)
        {
            _jS = js;                        
        }

        public async Task TriggerElement(ElementReference? element)
        {
            await _jS.InvokeVoidAsync("triggerElement", element);
        }

        public async Task ExportExcel(string projectName, List<ExcelCell> cells)
        {
            using var stream = new MemoryStream();

            DownloadExcel(stream, cells);

            stream.Seek(0, SeekOrigin.Begin);

            using var streamRef = new DotNetStreamReference(stream: stream);

            await _jS.InvokeVoidAsync("downloadFileFromStream", $"FazerTree-{projectName}-{DateTime.UtcNow.Date}.xlsx", streamRef);
        }

        public async Task ExportAttachment(FileBase file)
        {            
            using var stream = new MemoryStream(file.Content);

            stream.Seek(0, SeekOrigin.Begin);

            using var streamRef = new DotNetStreamReference(stream: stream);

            await _jS.InvokeVoidAsync("downloadFileFromStream", file.Name, streamRef);
        }

        public async Task NavigateToNewTab(string path)
        {
            await _jS.InvokeAsync<object>("open", path, "_blank");
        }

        public async Task ScrollBottomListener<T>(DotNetObjectReference<T> objectReference,
            ElementReference? elementReference = null) where T : class
        {
            await _jS.InvokeVoidAsync("scrollBottomListener", objectReference, elementReference);
        }

        public async Task ScrollTopListener<T>(DotNetObjectReference<T> objectReference,
            ElementReference? elementReference = null) where T : class
        {
            await _jS.InvokeVoidAsync("scrollTopListener", objectReference, elementReference);
        }

        public async Task ScrollTopBottomListener<T>(DotNetObjectReference<T> objectReference,
            ElementReference? elementReference = null) where T : class
        {
            await _jS.InvokeVoidAsync("scrollTopBottomListener", objectReference, elementReference);
        }

        public async Task OnElementClickOutside<T>(DotNetObjectReference<T> objectReference, ElementReference? elementReference = null) where T : class
        {
            await _jS.InvokeVoidAsync("onElementClickOutside", objectReference, elementReference);
        }

        public async Task<bool> HasScroll(ElementReference? scrollElement)
        {
            return await _jS.InvokeAsync<bool>("hasScroll", scrollElement);
        }

        public async Task NotifiyChildOnTop<T>(DotNetObjectReference<T> objectReference,
            ElementReference? scrollElement, string childSelector, string resultSelector) where T : class
        {
            await _jS.InvokeVoidAsync("notifyChildOnTop", objectReference, scrollElement, childSelector, resultSelector);
        }

        public async Task OnWindowResize<T>(DotNetObjectReference<T> objectReference) where T : class
        {
            await _jS.InvokeVoidAsync("onWindowResize", objectReference);
        }

        public async Task<DomRectModel> GetElementDomRect(ElementReference reference)
        {
            return await _jS.InvokeAsync<DomRectModel>("getElementDomRect", reference);
        }

        public async Task<bool> CanScroll(ElementReference reference)
        {
            return await _jS.InvokeAsync<bool>("canScroll", reference);
        }

        public async Task<bool> OnWindowResizeRefreshHorizontalSliderScroll(ElementReference reference)
        {
            return await _jS.InvokeAsync<bool>("onWindowResizeRefreshHorizontalSliderScroll", reference);
        }

        public async Task<bool> ScrollSide(ElementReference reference, bool left, int itemsPerSlide)
        {
            return await _jS.InvokeAsync<bool>("scrollSide", reference, left, itemsPerSlide);
        }

        public async Task ScrollToElementWithClass(ElementReference containerReference, string classToGoTo)
        {
            await _jS.InvokeVoidAsync("scrollToElementWithClass", containerReference, classToGoTo);
        }

        public async Task ScrollToElement(ElementReference reference, string block = "start", string inline = "start")
        {
            await _jS.InvokeVoidAsync("scrollToElement", reference, block, inline);
        }

        public async Task ScrollToElementById(string elementId)
        {
            await _jS.InvokeVoidAsync("scrollToElementById", elementId);
        }

        public async Task SubscribeToArrowsMovement<T>(DotNetObjectReference<T> reference,
            ElementReference element, string selector, string selectedColor) where T : class
        {
            await _jS.InvokeVoidAsync("subscribeToArrowsMovement", reference, element, selector, selectedColor);
        }

        public async Task SetOnWindowResizeListener<T>(DotNetObjectReference<T> reference, string invokeMethodName) where T : class
        {
            await _jS.InvokeVoidAsync("setOnWindowResizeListener", reference, invokeMethodName);
        }

        public async Task AddClassOnScroll(ElementReference scrollElement, string elementId, string className)
        {
            await _jS.InvokeVoidAsync("addClassOnScroll", scrollElement, elementId, className);
        }

        public async Task CopyToClipBoard(string value)
        {
            await _jS.InvokeVoidAsync("clipboardCopy.copyText", value);
        }

        public async Task ToggleFullscreen()
        {
            await _jS.InvokeVoidAsync("toggleFullScreen");
        }

        public async Task<DomRectModel> GetWindowDimensions()
        {
            return await _jS.InvokeAsync<DomRectModel>("getWindowDimensions");
        }

        public async Task RemoveClickOutsideListener()
        {
            await _jS.InvokeVoidAsync("removeClickOutsideListener");
        }

        public async Task RemoveClickOutsideListenerAndPopStack()
        {
            await _jS.InvokeVoidAsync("removeClickOutsideListenerAndPopStack");
        }

        public async Task SetOnCloseListener<T>(ElementReference elementReference,
            DotNetObjectReference<T> objectReference) where T : class
        {
            await _jS.InvokeVoidAsync("addOnDropdownCloseListener", elementReference, objectReference);
        }

        public async Task<string> GetInnerHTMLElement(ElementReference reference)
        {
            return await _jS.InvokeAsync<string>("getInnerHTMLElement", reference);
        }

        public async Task<string> GetInnerTextElement(ElementReference reference)
        {
            return await _jS.InvokeAsync<string>("getInnerTextElement", reference);
        }

        public async Task ExportInnerHTMLElement(ElementReference reference, string filename)
        {
            await _jS.InvokeVoidAsync("exportInnerHTMLElement", reference, filename);
        }

        public async Task ExportInnerHTMLElementById(string referenceId, string filename)
        {
            await _jS.InvokeVoidAsync("exportInnerHTMLElementById", referenceId, filename);
        }

        public async Task<string> GetInnerHTMLElementById(string elementId)
        {
            return await _jS.InvokeAsync<string>("getInnerHTMLElementById", elementId);
        }

        public async Task SelectText(ElementReference reference)
        {
            await _jS.InvokeVoidAsync("selectText", reference);
        }

        public async Task SetContentEditableFocusEnd(ElementReference reference)
        {
            await _jS.InvokeVoidAsync("setContentEditableFocusEnd", reference);
        }

        public async Task FocusElement(ElementReference reference)
        {
            await _jS.InvokeVoidAsync("focusElement", reference);
        }

        public async Task BlurElement(ElementReference reference)
        {
            await _jS.InvokeVoidAsync("blurElement", reference);
        }

        public async Task SetInnerHTMLElement(ElementReference reference, string? content)
        {
            await _jS.InvokeVoidAsync("setInnerHTMLElement", reference, content);
        }

        public async Task AppendToInnerHTML(ElementReference reference, string content)
        {
            await _jS.InvokeVoidAsync("appendToInnerHTML", reference, content);
        }

        public async Task ReplaceInnerHTML(ElementReference reference, string contentToReplace, string content)
        {
            await _jS.InvokeVoidAsync("replaceInnerHTML", reference, contentToReplace, content);
        }

        public async Task EditText(string command)
        {
            await _jS.InvokeVoidAsync("editText", "bold");
        }

        public async Task<string> GetTimezoneValue()
        {
            return await _jS.InvokeAsync<string>("getTimezoneValue");
        }

        public async Task SubscribeToTextAfterCharacter<T>(DotNetObjectReference<T> objectReference, ElementReference reference, string triggerChar) where T : class
        {
            await _jS.InvokeVoidAsync("subscribeToTextAfterCharacter", objectReference, reference, triggerChar);
        }

        public async Task UnsubscribeToTextAfterCharacter(ElementReference reference)
        {
            await _jS.InvokeVoidAsync("unsubscribeToTextAfterCharacter", reference);
        }

        public async Task SubscribeToTreeKeyboardControls<T>(DotNetObjectReference<T> objectReference,
            ElementReference reference) where T : class
        {
            await _jS.InvokeVoidAsync("subscribeToTreeKeyboardControls", objectReference, reference);
        }

        public async Task SubscribeToDragAndDrop<T>(DotNetObjectReference<T> objectReference,
            ElementReference reference) where T : class
        {
            await _jS.InvokeVoidAsync("subscribeToDragAndDrop", objectReference, reference);
        }

        public async Task UnsubscribeToTreeKeyboardControls()
        {
            await _jS.InvokeVoidAsync("unsubscribeToTreeKeyboardControls");
        }

        public async Task UnsubscribeToDragAndDrop()
        {
            await _jS.InvokeVoidAsync("unsubscribeToDragAndDrop");
        }

        public async Task PlayGifOnHover(ElementReference reference)
        {
            await _jS.InvokeVoidAsync("playGifOnHover", reference);
        }

        public async Task SetSliderByCardNumber(ElementReference reference, int cardsPerSlide)
        {
            await _jS.InvokeVoidAsync("setSliderByCardNumber", reference, cardsPerSlide);
        }

        public async Task<bool> IsElementOverflown(ElementReference element)
        {
            return await _jS.InvokeAsync<bool>("isElementOverflown", element);
        }

        public async Task ValidatePasswordById(string inputValue, string matchPasswordId)
        {
            await _jS.InvokeVoidAsync("validatePasswordById", inputValue, matchPasswordId,
                AppConst.PasswordMinLength);
        }

        public async Task ValidatePasswordMatch(string inputValue, string passwordId)
        {
            await _jS.InvokeVoidAsync("validatePasswordMatchById", inputValue, passwordId);
        }
    }
}
