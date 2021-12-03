//custom binding para inicializar el plugin custom switch
ko.bindingHandlers.bsSwitchButton = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        let elementId = element.id;
        let checkedProp = allBindings.get("checked");
        document.getElementById(elementId).switchButton({
            onlabel: '<i class="fa fa-thumbs-up"></i> Sí',
            offlabel: '<i class="fa fa-thumbs-down"></i> No'
        });
        $('#' + elementId).change(function () {
            let value = $(this).prop("checked");
            checkedProp(value);
        });
    }
};

//custom binding para inicializar el plugin select 2
ko.bindingHandlers.Select2 = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        // add destroy callback
        ko.utils.domNodeDisposal.addDisposeCallback(element, () => {
            $(element).select2('destroy');
        });

        var configOptions = ko.unwrap(valueAccessor());
        $(element).select2(configOptions);
    },
    update: (element, valueAccessor, allBindingsAccessor, viewModel) => {
        var allBindings = allBindingsAccessor();

        if (allBindings["value"]) {
            // the value changed
            const value = ko.utils.unwrapObservable(allBindings.value());
            $(element).val(value).trigger("change");
        } else if (allBindings["selectedOptions"]) {
            // the selected options changed
            const value = ko.utils.unwrapObservable(allBindings.selectedOptions());
            $(element).val(value).trigger("change");
        }
    }
};

//expand or collapse text is more that 80 letters
ko.bindingHandlers.expandText = {
    init: function (element, valueAccessor) {
        element.appendChild(document.createElement('span'));
        var toggle = document.createElement('a');
        toggle.appendChild(document.createTextNode("..."));
        toggle.href = "#";
        element.appendChild(toggle);
    },
    update: function (element, valueAccessor) {
        var text = ko.unwrap(valueAccessor());
        var textElement = element.getElementsByTagName('span')[0];
        var toggle = element.getElementsByTagName('a')[0];
        var collapsed = true;
        toggle.onclick = function () {
            collapsed = !collapsed;
            ko.utils.setTextContent(textElement, collapsed ? text.substr(0, 80) : text);
        }
        toggle.style.display = text.length > 80 ? 'inline' : 'none';
        ko.utils.setTextContent(textElement, collapsed ? text.substr(0, 80) : text);
    }
};

//put toggle icon to show/hide input type text
ko.bindingHandlers.toggleValue = {
    init: function (element, valueAccessor) {

        //crear un icono para el input element
        var i = document.createElement('i');
        $(i).css("float", "right");
        $(i).css("margin-top", "-25px");
        $(i).css("margin-right", "10px");

        //agregar el icono correspondiente
        var mostrado = ko.unwrap(valueAccessor()) || false;

        if (mostrado)
            $(i).addClass("fa fa-eye-slash");
        else
            $(i).addClass("fa fa-eye");

        $(element).after(i);
    },
    update: function (element, valueAccessor) {

        var mostrado = ko.unwrap(valueAccessor()) || false;
        var iconNode = element.nextSibling;

        ko.utils.registerEventHandler(iconNode, "click", function (event) {
            //mostrar u ocultar el valor del input finalmente
            console.log(ko.unwrap(valueAccessor()));

            if (mostrado)
                element.type = "text";
            else
                element.type = "password";

            //cambiar el valor del observable
            return valueAccessor(!mostrado);
        });
    }
};