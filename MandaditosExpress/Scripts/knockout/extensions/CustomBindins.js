ko.bindingHandlers.bsSwitchButton = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        let elementId = element.id;
        let checkedProp = allBindings.get("checked");
        document.getElementById(elementId).switchButton();
        $('#' + elementId).change(function () {
            let value = $(this).prop("checked");
            checkedProp(value);
        });
    }
};