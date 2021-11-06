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