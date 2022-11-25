let onOutsideClickFunctions = {};

window.fstypeahead = {
    registerOutsideClick: function (element, dotnetRef, callback, clearOnFire) {
        if (element === null)
            return;

        let bId = "";//get the blazor internal ID to distinguish different components
        for (let clearCount = 0; clearCount < element.attributes.length; clearCount++) {
            let a = element.attributes[clearCount];
            if (a.name.startsWith('_bl_')) {
                bId = a.name;
                break;
            }
        }

        fstypeahead.onOutsideClickClear(element);

        let outsideClickDetected = function (e) {
            var parent = e.target;
            while (parent != null) {
                if (parent.classList != null && parent.classList.contains('fs-type-ahead')) {
                    var hasSearch = parent.contains(element); //check if this is the same typeahead parent element
                    if (hasSearch) {
                        return; //we're still in the search so don't fire
                    }
                }
                parent = parent.parentNode;
            }

            dotnetRef.invokeMethodAsync(callback);
        }

        onOutsideClickFunctions[bId] = outsideClickDetected;
        window.addEventListener("click", outsideClickDetected);
    },

    onOutsideClickClear: function (element) {
        if (element == null) {
            return;
        }

        let bId = "";
        for (let clearCount = 0; clearCount < element.attributes.length; clearCount++) {
            let a = element.attributes[clearCount];
            if (a.name.startsWith('_bl_')) {
                bId = a.name;
                break;
            }
        }

        let func = onOutsideClickFunctions[bId];
        if (func == null || func == "undefined") {
            return;
        }
        window.removeEventListener("click", func);
        onOutsideClickFunctions[bId] = null;
    }
};