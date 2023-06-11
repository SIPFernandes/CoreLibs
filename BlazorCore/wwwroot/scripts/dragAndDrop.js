let projectTreeElement;
let dotNetReference;

function unsubscribeToDragAndDrop() {
    projectTreeElement.onmousedown = null;

    projectTreeElement.removeEventListener('dragstart', dragStart);
    projectTreeElement.removeEventListener('dragend', dragEnd);
}

function subscribeToDragAndDrop(dotNetRef, projectTreeEl) {
    dotNetReference = dotNetRef;

    if (projectTreeEl) {
        projectTreeElement = projectTreeEl;

        projectTreeElement.onmousedown = ev => {
            if (ev.target.dataset.isDragHandle) {
                let draggableParent = getDraggableParent(ev.target);

                draggableParent.draggable = true;
            }
        }

        projectTreeElement.addEventListener('dragstart', dragStart);
        projectTreeElement.addEventListener('dragend', dragEnd);
    }
}

function dragStart(e) {
    if (!e.target.draggable) {
        return;
    }

    e.dataTransfer.setData('text/plain', e.target.id);

    setTimeout(() => {
        e.target.classList.add('hide');
    }, 0);

    const boxes = document.querySelectorAll('.qa-task-item--' + e.target.dataset.targetType);

    const parentId = e.target.dataset.parentId;

    boxes.forEach(box => {
        if (box.dataset.hasOwnProperty('canAcceptMovedTask') && parentId != box.id)
        {
            box.addEventListener('dragenter', dragEnter)
            box.addEventListener('dragover', dragOver);
            box.addEventListener('dragleave', dragLeave);
            box.addEventListener('drop', drop);
            box.classList.add("accept-drag");
        }
    });
}

function dragEnd(e) {
    counter = 0;
    e.target.classList.remove('hide');

    const boxes = document.querySelectorAll('.qa-task-item--' + e.target.dataset.targetType);

    boxes.forEach(box => {
        box.removeEventListener('dragenter', dragEnter)
        box.removeEventListener('dragover', dragOver);
        box.removeEventListener('dragleave', dragLeave);
        box.removeEventListener('drop', drop);
        box.classList.remove("accept-drag");
    });
}

var currentTarget = null;

function dragEnter(e) {
    e.preventDefault();
    currentTarget = e.target;
    e.target.classList.add('drag-over');
}

function dragOver(e) {
    e.preventDefault();
    e.target.classList.add('drag-over');
}

function dragLeave(e) {
    if (currentTarget == e.target) {
        e.stopPropagation();
        e.preventDefault();
        e.target.classList.remove('drag-over');
    } 
}

function drop(event) {
    element = getParentById(event.target);
    element.classList.remove('drag-over');

    // get the draggable element
    const id = event.dataTransfer.getData('text/plain');
    const draggableElement = document.getElementById(id);
    draggableElement.draggable = false;
    draggableElement.dataset.parentId = element.id;

    dotNetReference?.invokeMethodAsync('OnTaskMoved', id, element.id);
}

function getParentById(parent) {
    while (parent && !parent.id) {
        parent = parent.parentElement;
    }

    return parent;
}

function getDraggableParent(element) {
    while (element && !element.dataset.isDraggable) {
        element = element.parentElement;
    }

    return element;
}