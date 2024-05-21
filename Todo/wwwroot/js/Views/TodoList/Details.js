const doneItemsCheckbox = document.querySelector('#toggleDoneItems');

const toggleTodoItems = () => {
    var ddd = $(".list-group-item-done");
    var fff = doneItemsCheckbox.checked;
    doneItemsCheckbox.checked ? $(".list-group-item-done").hide() : $(".list-group-item-done").show();
        
    }


doneItemsCheckbox.addEventListener('change', toggleTodoItems);