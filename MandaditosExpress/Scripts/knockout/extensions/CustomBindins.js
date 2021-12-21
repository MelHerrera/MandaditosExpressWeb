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
        $(i).css("position", "relative");
        $(i).css("z-index", "2");
        $(i).addClass("fa fa-eye");

        //valor del observable
        var toggleObservable = valueAccessor() || false;

        //add event lister for click event for each click in icon toggle.
        ko.utils.registerEventHandler(i, "click", function (event) {
            //mostrar u ocultar el valor del input finalmente

            toggleObservable(!toggleObservable());

            if (toggleObservable()) {
                element.type = "text";
                $(i).addClass("fa-eye-slash");
                $(i).removeClass("fa-eye");
            }
            else {
                element.type = "password";
                $(i).removeClass("fa-eye-slash");
                $(i).addClass("fa-eye");
            }

            //cambiar el valor del observable
            return toggleObservable();
        });

        $(element).after(i);
    }
};


//custom binding para inicializar el plugin slimscroll in element
ko.bindingHandlers.slimScroll = {
    init: function (element, valueAccessor) {
        // add destroy callback
        ko.utils.domNodeDisposal.addDisposeCallback(element, () => {
            $('#' + element.id).slimscroll('destroy');
        });

        var configOptions = ko.unwrap(valueAccessor());
        $('#' + element.id).slimscroll(configOptions)
    }
};

//custom binding para inicializar el plugin de graficos sparkline
ko.bindingHandlers.defaultSparkline = {
    init: function (element, valueAccessor) {

        var values = [5, 6, 7, 5, 13, 7, 6, 7, 6, 4, 7];

        var range_map = $.range_map({
            '7:': 'green',
        });

        // Draw a sparkline for the #sparkline element
        $('.sparkline').sparkline(values, {
            type: "bar",
            barWidth: 10,
            height: 60,
            barColor: '#0083CD',
            colorMap: range_map,
            chartRangeMax: 12
        });
    }
};