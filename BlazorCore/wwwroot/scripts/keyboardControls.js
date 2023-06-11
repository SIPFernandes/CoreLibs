function unsubscribeToTreeKeyboardControls() {
    window.onkeyup = null;

    window.onkeydown = null;
}

function subscribeToTreeKeyboardControls(dotNetObjectRef, element) {
    if (element) {
        let elements = element.getElementsByClassName("task-items-wrapper");

        let currentElement = null;

        let isControlDown = false;

        window.onkeyup = event => {
            if (event.code == 'ControlLeft') {
                isControlDown = false;
            }
        };

        window.onkeydown = event => {
            if (currentElement == null) {
                if (document.activeElement) {
                    currentElement = getParentElementBySelector(document.activeElement, '.task-items-wrapper');
                }
                else {
                    currentElement = elements[0];
                }
            }

            if (document.activeElement) {
                currentElement = getParentElementBySelector(document.activeElement, '.task-items-wrapper');
            }

            if (event.code == 'KeyD' && isControlDown) {
                event.preventDefault();

                dotNetObjectRef?.invokeMethodAsync('OnCtrlDPress', currentElement.id);
            }

            if (event.code == 'KeyM' && isControlDown) {
                event.preventDefault();

                dotNetObjectRef?.invokeMethodAsync('OnCtrlMPress', currentElement.id);
            }

            if (event.code == 'Enter') {
                event.preventDefault();

                dotNetObjectRef?.invokeMethodAsync('OnEnterPress', currentElement.id);
            }

            if (event.code === 'Tab') {
                event.preventDefault();

                dotNetObjectRef?.invokeMethodAsync('OnTabPress', currentElement.id);
            }

            if (event.code == 'ControlLeft') {
                event.preventDefault();

                isControlDown = true;
            }

            let newElement;

            if (event.code == 'ArrowDown') {
                event.preventDefault();

                newElement = downKeyPressed(currentElement);

                currentElement = newElement ? newElement : currentElement;

                focusTreeItemElement(currentElement);
            }

            if (event.code == 'ArrowUp') {
                event.preventDefault();

                newElement = upKeyPressed(currentElement);

                currentElement = newElement ? newElement : currentElement;

                focusTreeItemElement(currentElement);
            }

            if (event.code == 'ArrowRight') {
                event.preventDefault();

                newElement = currentElement.getElementsByClassName("task-items-wrapper")[0];

                currentElement = newElement ? newElement : currentElement;

                focusTreeItemElement(currentElement);
            }

            if (event.code == 'ArrowLeft') {
                event.preventDefault();

                newElement = getParentElementBySelector(currentElement, '.task-items-wrapper');

                currentElement = newElement ? newElement : currentElement;

                focusTreeItemElement(currentElement);
            }
        };
    }
}

function focusTreeItemElement(element) {
    let inputs = element.getElementsByTagName('input');
    inputs[0].focus();

    inputs[0].scrollIntoView({ block: "center", behavior: "smooth", inline: "center" });
}

function downKeyPressed(currentElement) {
    let previousElement = currentElement;

    currentElement = getNextSiblingBySelector(currentElement, '.task-items-wrapper');

    if (!currentElement) {
        let counter = 0;

        while (!currentElement) {
            currentElement = getParentElementBySelector(previousElement, '.task-items-wrapper');

            if (!currentElement) {
                return null;
            }

            previousElement = currentElement;

            currentElement = getNextSiblingBySelector(currentElement, '.task-items-wrapper');

            counter++;
        }

        for (var i = 0; i < counter; i++) {
            currentElement = currentElement.getElementsByClassName("task-items-wrapper")[0];
        }
    }
    
    return currentElement;
}

function upKeyPressed(currentElement) {
    let previousElement = currentElement;

    currentElement = getPreviousSiblingBySelector(currentElement, '.task-items-wrapper');

    if (!currentElement) {
        let counter = 0;

        while (!currentElement) {
            currentElement = getParentElementBySelector(previousElement, '.task-items-wrapper');

            if (!currentElement) {
                return null;
            }

            previousElement = currentElement;

            currentElement = getPreviousSiblingBySelector(currentElement, '.task-items-wrapper');

            counter++;
        }

        for (var i = 0; i < counter; i++) {
            currentElement = currentElement.getElementsByClassName("task-items-wrapper")[0];

            let sibling = getNextSiblingBySelector(currentElement, '.task-items-wrapper');

            while (sibling) {
                currentElement = sibling;

                sibling = getNextSiblingBySelector(currentElement, '.task-items-wrapper');
            }
        }
    }

    return currentElement;
}

function getNextSiblingBySelector(element, selector) {

    var sibling = element.nextElementSibling;

    while (sibling) {
        if (sibling.matches(selector)) {
            return sibling;
        }

        sibling = sibling.nextElementSibling
    }
};

function getPreviousSiblingBySelector(element, selector) {

    var sibling = element.previousElementSibling;

    while (sibling) {
        if (sibling.matches(selector)) {
            return sibling;
        }

        sibling = sibling.previousElementSibling
    }
};

function getParentElementBySelector(element, selector) {

    var parent = element.parentElement;

    while (parent) {
        if (parent.matches(selector)) {
            return parent;
        }

        parent = parent.parentElement
    }
}