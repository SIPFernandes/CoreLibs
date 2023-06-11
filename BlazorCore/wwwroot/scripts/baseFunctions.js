function triggerElementById(id) {
    if (id) {
        document.getElementById(id).click();
    }
}

function triggerElement(element) {
    if (element) {
        element.click();
    }
}

function editText(command) {

    document.execCommand(command, false, null);
}

function focusElement(element) {
    if (element) {
        element.focus();
    }
}

function blurElement(element) {
    if (element) {
        element.blur();
    }
}

function preventNewLine(event) {
    if (event.keyCode === 13) {
        event.preventDefault();
    }
}

function selectText(element) {
    if (element) {
        element.select();
    }
}

function getInnerHTMLElement(element) {
    if (element) {
        return element.innerHTML;
    }
    return null;
}

function exportInnerHTMLElement(element, filename) {
    if (element) {
        downloadFileFromString(filename, element.innerHTML)
    }
}

function exportInnerHTMLElementById(elementId, filename) {
    let element = document.getElementById(elementId);

    exportInnerHTMLElement(element, filename);
}

function getInnerTextElement(element) {
    if (element) {
        return element.textContent;
    }
    return null;
}

function getInnerHTMLElementById(elementId) {
    let element = document.getElementById(elementId);

    if (element) {
        return element.innerHTML;
    }
    return null;
}

function setInnerHTMLElement(element, content) {
    if (element) {
        element.innerHTML = content;
    }
}

function appendToInnerHTML(element, content) {
    if (element) {
        element.innerHTML += content;
    }
}

function replaceInnerHTML(element, contentToReplace, content) {
    if (element) {
        let newInnerHtml = element.innerHTML.substring(0, element.innerHTML.length - contentToReplace.length).trim() + content;

        element.innerHTML = newInnerHtml; 

        setContentEditableFocusEnd(element);
    }
}

function subscribeToTextAfterCharacter(dotNetObjectRef, element, triggerChar) {
    if (element) {
        element.oninput = function () {
            let text = element.innerText;

            const afterCharText = text.substring(text.lastIndexOf(triggerChar) + 1);

            dotNetObjectRef?.invokeMethodAsync('OnInputChange', afterCharText);
        }
    }
}

function unsubscribeToTextAfterCharacter(element) {
    if (element) {
        element.oninput = null;
    }
}

function setContentEditableFocusEnd(contentEditableElement) {
    if (contentEditableElement) {
        let range = document.createRange();//Create a range (a range is a like the selection but invisible)
        range.selectNodeContents(contentEditableElement);//Select the entire contents of the element with the range
        range.collapse(false);//collapse the range to the end point. false means collapse to end rather than the start

        let selection = window.getSelection();//get the selection object (allows you to change selection)
        selection.removeAllRanges();//remove any selections already made
        selection.addRange(range);//make the range you have just created the visible selection
    }
}

function subscribeToArrowsMovement(dotNetObjectRef, element, selector, selectedColor) {
    if (element) {
        let selectedElements = element.getElementsByClassName(selector);
        let currentIndex = 0;
        let currentSelectedElement = selectedElements[0];

        window.addEventListener("keyup", event => {
            if (selectedElements.length > 0) {
                if (event.code == 'ArrowDown') {
                    event.preventDefault();

                    currentIndex = currentIndex + 1 < selectedElements.length
                        ? currentIndex + 1
                        : 0;

                    currentSelectedElement.style.backgroundColor = 'transparent';
                }
                else if (event.code == 'ArrowUp') {
                    event.preventDefault();

                    currentIndex = currentIndex - 1 < 0
                        ? selectedElements.length - 1
                        : currentIndex - 1;

                    currentSelectedElement.style.backgroundColor = 'transparent';
                }
                else {
                    selectedElements = element.getElementsByClassName(selector);

                    currentIndex = 0;
                }

                currentSelectedElement = selectedElements[currentIndex];

                currentSelectedElement.style.backgroundColor = selectedColor;

                dotNetObjectRef?.invokeMethodAsync('OnSelectedIndexChanged', currentIndex);
            } else {
                currentIndex = 0;
            }
        });
    }
}

function canScroll(element) {
    if (element) {
        return element.scrollWidth > element.clientWidth;
    }
}

function scrollSide(element, scrollLeft, itemsPerSlide) {

    if (element) {
        let childs = element.children;

        let isEnd = false;

        if (childs.length <= 1) {
            isEnd = scrollSideBySlide(element, scrollLeft);
        } else {
            isEnd = scrollSideByItem(element, scrollLeft, itemsPerSlide)
        }

        return isEnd;
    }
}

function scrollSideBySlide(element, scrollLeft) {

    if (element) {
        let isEnd = false;

        if (scrollLeft) {
            element.scrollLeft -= element.offsetWidth;

            let scrollCount = element.scrollLeft - 2 * element.offsetWidth;

            isEnd = scrollCount <= -element.offsetWidth;
        }
        else {
            element.scrollLeft += element.offsetWidth;

            let scrollCount = element.scrollLeft + 2 * element.offsetWidth;

            isEnd = scrollCount >= element.scrollWidth;
        }

        return isEnd;
    }
}

function scrollSideByItem(element, scrollLeft, itemsPerSlide) {

    if (element) {
        let childs = element.children;

        let scrollWrapperRect = element.getBoundingClientRect();

        let firstCardOutOfView;

        let isEnd = false;

        if (scrollLeft) {
            for (var i = 0; i < childs.length; i++) {
                let isInTheView = isCardInTheView(scrollWrapperRect, childs[i]);

                if (isInTheView) {
                    firstCardOutOfView = childs[i - 1];

                    scrollToElement(firstCardOutOfView, "end", "end")

                    isEnd = i - itemsPerSlide <= 0;

                    break;
                }
            }
        } else {
            let hadOneInView;

            for (var i = 0; i < childs.length; i++) {
                let isInTheView = isCardInTheView(scrollWrapperRect, childs[i]);

                if (!hadOneInView && isInTheView) {
                    hadOneInView = true;
                }

                if (hadOneInView && !isInTheView) {
                    firstCardOutOfView = childs[i];

                    scrollToElement(firstCardOutOfView, "end", "start");

                    isEnd = i + itemsPerSlide >= childs.length;

                    break;
                }
            }
        }

        return isEnd;
    }
}

function onWindowResizeRefreshHorizontalSliderScroll(element) {
    if (element) {
        let childs = element.children;

        let scrollWrapperRect = element.getBoundingClientRect();

        for (var i = 0; i < childs.length; i++) {
            let isInTheView = isCardInTheView(scrollWrapperRect, childs[i]);

            if (isInTheView) {
                let child = childs[i - 1];

                scrollToElement(child, "end", "start");

                break;
            }
        }

        return canScroll(element);
    }
}

function isCardInTheView(scrollWrapperRect, element) {
    if (scrollWrapperRect && element) {
        let offGridX = Math.ceil(scrollWrapperRect.x + scrollWrapperRect.width);

        let elementRect = element.getBoundingClientRect();

        let elementX = Math.ceil(elementRect.x)

        let scrollWrapperRectX = Math.ceil(scrollWrapperRect.x);
        
        return elementX >= scrollWrapperRectX && elementX < offGridX;
    }
}

function scrollToElementWithClass(elementContainer, classToGoTo){
    let element = elementContainer.querySelector('.' + classToGoTo);
    
    if (element != null) {
        elementContainer.scrollTop = element.offsetTop - elementContainer.offsetTop;
    }
}

function scrollToElement(element, block, inline, behavior) {
    if (element) {
        element.scrollIntoView({
            block: block,
            inline: inline,
            behavior: behavior ?? "smooth"
        });
    }
}

function scrollToElementById(elementId) {
    let element = document.getElementById(elementId);
    scrollToElement(element, "start", "start", "auto");
}

function addClassOnScroll(scrollElement, elementId, className) {
    let element = document.getElementById(elementId);

    if (scrollElement && element) {
        if (!hasScroll(scrollElement)) {
            window.onwheel = (event) => {
                if (window.innerWidth >= 1200) {
                    if (event.deltaY >= 0) {
                        element.classList.add(className);
                    } else {
                        element.classList.remove(className);
                    }
                }
            };
        } else {
            window.onwheel = null;
        }
    }
}

function scrollBottomListener(dotNetObjectRef, scrollElement) {
    let scrollBottomLimit = 30;    

    if (!scrollElement) {
        //Scroll content
        scrollElement = document.getElementById("content");
    }

    if (scrollElement) {        
        scrollElement.addEventListener('scroll', () => {            
            if ((scrollElement.clientHeight + scrollElement.scrollTop) >= scrollElement.scrollHeight - scrollBottomLimit) {               
                dotNetObjectRef?.invokeMethodAsync('OnScrollBottom');
            }
        });
    }
}

function scrollTopListener(dotNetObjectRef, scrollElement) {
    let scrollTopLimit = 30;

    if (!scrollElement) {
        //Scroll content
        scrollElement = document.getElementById("content");
    }

    if (scrollElement) {
        scrollElement.addEventListener('scroll', () => {
            let topScroll = scrollElement.scrollHeight - scrollElement.clientHeight;

            if ((topScroll + scrollElement.scrollTop) <= scrollTopLimit) {
                dotNetObjectRef?.invokeMethodAsync('OnScrollTop');
            }
        });
    }
}

function scrollTopBottomListener(dotNetObjectRef, scrollElement) {
    let scrollLimit = 30;

    if (!scrollElement) {
        //Scroll content
        scrollElement = document.getElementById("content");
    }

    if (scrollElement) {
        scrollElement.addEventListener('scroll', () => {
            if ((scrollElement.clientHeight + scrollElement.scrollTop) >= scrollElement.scrollHeight - scrollLimit) {
                dotNetObjectRef?.invokeMethodAsync('OnScrollBottom');
            }

            if (scrollElement.scrollTop == 0) {
                dotNetObjectRef?.invokeMethodAsync('OnScrollTop');
            }
        });
    }
}

function hasScroll(scrollElement) {

    if (!scrollElement) {
        //Scroll content
        scrollElement = document.getElementById("content");
    }

    if (scrollElement) {
        return scrollElement.scrollHeight > scrollElement.clientHeight;
    }
}

let currentDate = null;

function notifyChildOnTop(dotNetObjectRef, scrollElement, childSelector, infoSelector) {
    if (scrollElement) {

        scrollElement.addEventListener('scroll', () => {
            let children = scrollElement.querySelectorAll(childSelector);

            let elementsOnTop = getElementsOnTop(children, scrollElement);

            if (elementsOnTop) {
                let elementOnTop = elementsOnTop.pop();

                if (elementOnTop) {
                    let topElementDate = elementOnTop.querySelector(infoSelector).innerText;
                    let id = topElementDate.replace(/\s/g, "")
                    elementOnTop.setAttribute('id', id);

                    if (currentDate != topElementDate) {
                        currentDate = topElementDate;

                        dotNetObjectRef?.invokeMethodAsync('OnTopElementChange', topElementDate, id);
                    }
                }
            }
        });
    }
}

function getElementsOnTop(elements, scrollElement) {
    let elementsOnTop = [];
    elements.forEach(el => {
        if (scrollElement.scrollTop >= el.offsetTop) {
            elementsOnTop.push(el);
        }
    });
    return elementsOnTop;
}

function setOnWindowResizeListener(dotNetObjectRef, invokeMethodName) {
    let resizeFunction = () => {
        dotNetObjectRef?.invokeMethodAsync(invokeMethodName).catch(error => {
            window.removeEventListener('resize', resizeFunction);
        });
    }

    window.addEventListener('resize', resizeFunction);
}

let dropdownStack = [];

function addOnDropdownCloseListener(element, dotNetObjectRef) {
    let dropdownModel = {
        dropdown: element,
        reference: dotNetObjectRef
    }

    dropdownStack.push(dropdownModel);

    document.addEventListener("click", clickedOutside);
}

function clickedOutside(event) {
    let clickedElement = event.target;

    updateDropdownStack(clickedElement);
}

function updateDropdownStack(clickedElement) {
    if (dropdownStack.length > 0) {

        let dropdownModel = dropdownStack[dropdownStack.length - 1]

        if (dropdownModel.dropdown != clickedElement && !dropdownModel.dropdown.contains(clickedElement)) {
            dropdownStack.pop();

            dropdownModel.reference?.invokeMethodAsync('OnClickOutside');

            updateDropdownStack(clickedElement);
        }
    }
}

function removeClickOutsideListener() {
    document.removeEventListener("click", clickedOutside);

    if (dropdownStack.length > 0) {
        document.addEventListener("click", clickedOutside);
    }
}

function removeClickOutsideListenerAndPopStack() {
    dropdownStack.pop();

    removeClickOutsideListener();
}

function onWindowResize(dotNetObjectRef) {
    window.addEventListener('resize', function () {
        dotNetObjectRef?.invokeMethodAsync('OnWindowResized');
    }, true);
}

function getWindowDimensions() {
    return {
        height: window.innerHeight,
        width: window.innerWidth
    };
}

function getElementDomRect(element) {
    if (element) {
        let domRect = element.getBoundingClientRect();

        return {
            height : domRect.height,
            width : domRect.width,
            scrollWidth : element.scrollWidth,
            scrollHeight : element.scrollHeight,
            scrollY: element.scrollTop,
            scrollX: element.scrollLeft,
            x : domRect.x,
            y : domRect.y
        };
    }
}

window.clipboardCopy = {
    copyText: function (text) {
        navigator.clipboard.writeText(text);
    }
};

function onKeyValidateRegex(e, regex) {
    var validChars = new RegExp(regex);

    return validChars.test(e.key);
}

function showHidePassword(input) {
    if (input.type === "password")
    {
        input.type = "text";
    }
    else
    {
        input.type = "password";
    }
}

function toggleFullScreen() {
    //checks if fullscreen is active (verifying different browsers)
    if ((document.fullScreenElement !== undefined && document.fullScreenElement === null)
        || (document.msFullscreenElement !== undefined && document.msFullscreenElement === null)
        || (document.mozFullScreen !== undefined && !document.mozFullScreen)
        || (document.webkitIsFullScreen !== undefined && !document.webkitIsFullScreen))
    {
        if (document.body.requestFullScreen) {
            document.body.requestFullScreen();
        } else if (document.body.mozRequestFullScreen) {
            document.body.mozRequestFullScreen();
        } else if (document.body.webkitRequestFullScreen) {
            document.body.webkitRequestFullScreen(Element.ALLOW_KEYBOARD_INPUT);
        } else if (document.body.msRequestFullscreen) {
            document.body.msRequestFullscreen();
        }
    } else {
        if (document.cancelFullScreen) {
            document.cancelFullScreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitCancelFullScreen) {
            document.webkitCancelFullScreen();
        } else if (document.msExitFullscreen) {
            document.msExitFullscreen();
        }
    }
}

function validatePasswordById(password, idMatchPassword, minLengthRequired) {
    const matchPassword = document.getElementById(idMatchPassword).value;
    
    const length = document.getElementById("length");
    const lowerupper = document.getElementById("lowerupper");
    const numbercharacter = document.getElementById("numbercharacter");

    // Validate length
    if (password.length >= minLengthRequired) {
        length.classList.remove("invalid");
    } else {
        length.classList.add("invalid");
    }

    // Validate lowercase and capital letters
    const lowerUpperCaseLetters = /(?=.*[a-z])(?=.*[A-Z])/g;
    if (password.match(lowerUpperCaseLetters)) {
        lowerupper.classList.remove("invalid");
    } else {
        lowerupper.classList.add("invalid");
    }

    // Validate numbers and special characters
    let numbersSpchars = /(?=.*[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]+)(?=.*[0-9])/g;
    if (password.match(numbersSpchars)) {
        numbercharacter.classList.remove("invalid");
    } else {
        numbercharacter.classList.add("invalid");
    }
    
    validatePasswordMatch(password, matchPassword);
}

function validatePasswordMatchById(password, idMatchPassword) {
    const matchPasswordValue = document.getElementById(idMatchPassword).value;

    validatePasswordMatch(password, matchPasswordValue);
}

function validatePasswordMatch(password, matchPasswordValue) {
    document.getElementById("errorslist").classList.remove("invalid-initial");
    document.getElementById("errorslist").classList.remove("init");
    
    const passwordMatch = document.getElementById("passwordmatch");

    // Validate password match
    if (password.trim().length !== 0 && password === matchPasswordValue) {
        passwordMatch.classList.remove("invalid");
    } else {
        passwordMatch.classList.add("invalid");
    }
}

function enableSubmitBtn(submitBtn){
    const btnSubmit = document.getElementById(submitBtn);
    btnSubmit.disabled = !$('#account').valid();
}

function onElementClickOutside(dotNetReference, element) {
    if (element && !document.onmousedown) {
        document.onmousedown = event => {
            let clickedElement = event.target;

            if (element != clickedElement && !element.contains(clickedElement)) {
                document.onmousedown = null;
                dotNetReference?.invokeMethodAsync('OnClickOutside');
            }
        };
    }
}

function getTimezoneValue() {
    return Intl.DateTimeFormat().resolvedOptions().timeZone;
}

function playGifOnHover(element) {
    if (element) {
        element.addEventListener("mouseover", () => {
            element.play();
        });

        element.addEventListener("mouseleave", () => {
            element.stop();
        });
    }
}

function setSliderByCardNumber(sliderWrapper, cardNumber) {
    if (sliderWrapper) {
        var resizeObserver = new ResizeObserver(entries => {
            let sliderRect = sliderWrapper.getBoundingClientRect();

            let cards = sliderWrapper.getElementsByClassName('card-overview');

            let cardWidth = (sliderRect.width - 48) / cardNumber;

            if (window.innerWidth >= 1596) {
                cardWidth = cardWidth > 172 ? 172 : cardWidth;
            }

            for (let i = 0; i < cards.length; i++) {
                let card = cards[i];

                card.style.minWidth = cardWidth + 'px';
                card.style.height = cardWidth + 'px';
            }
        }).observe(sliderWrapper);
    }
}

function isElementOverflown(element) {
    let overflown = false;

    if (element) {
        overflown = element.scrollHeight > element.clientHeight || element.scrollWidth > element.clientWidth;
    }

    return overflown;
}

function disableSubmitButton(element) {
    element.find(":submit").prop('disabled',true);
}