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