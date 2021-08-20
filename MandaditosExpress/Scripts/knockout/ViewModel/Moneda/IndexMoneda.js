
function MonedasViewModel(monedasCollection) {
    const self = this;
    self.Monedas = ko.observableArray(monedasCollection);

    self.Moneda = ko.observable(new MonedaViewModel({}));

    self.ModalViewModel = ko.observable(new ModalViewModel({
        ModalId: "monedas-modal",
        ModalHeaderViewModel: new ModalHeaderViewModel({ ModalTitle: "Initial", ModalHeaderClass: "bg-secondary" }),
        ModalBodyViewModel: new ModalBodyViewModel({ MonedaViewModel: new MonedaViewModel({}) }),
        FooterViewModel: new FooterModalViewModel({ ActionName: "Initial", UrlAction: "/Index" })
    }));

    self.ShowModal = function (moneda, event) {

        //pasarle la moneda actual a la propiedad moneda
        self.Moneda(ko.toJS(moneda));

        if (event.currentTarget.id == "btn-edit") {
            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("editar la información de la moneda").ModalHeaderClass("bg-success");
            self.ModalViewModel().ModalBodyViewModel().MonedaViewModel(new MonedaViewModel(ko.toJS(self.Moneda)));
            self.ModalViewModel().FooterViewModel().ActionName("Editar").UrlAction($(event.currentTarget).attr("href"));
        }
        if (event.currentTarget.id == "btn-del") {
            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("Eliminar la información de la Moneda").ModalHeaderClass("bg-danger");
            self.ModalViewModel().ModalBodyViewModel().MonedaViewModel(new MonedaViewModel(ko.toJS(self.Moneda)));
            self.ModalViewModel().FooterViewModel().ActionName("Eliminar").UrlAction($(event.currentTarget).attr("href"));
        }

        self.ModalViewModel().ShowModal();
    };

    self.MostrarDetalles = function (moneda) {
        $.ajax(
            {
                url: $(event.currentTarget).attr("href"),
                data: {
                    id: ko.toJS(moneda).Id
                },
                success: function (res) {
                    $(".body-content").html(res);
                }
            }
        );
    }

    self.GuardarCambios = function (data, event) {

        let token = $('.modal-form-foot input[name="__RequestVerificationToken"]').val();
        let moneda = ko.toJS(self.ModalViewModel().ModalBodyViewModel().MonedaViewModel());

        $.ajax({
            url: ko.toJS(data).UrlAction,
            type: "Post",
            data: { __RequestVerificationToken: token, moneda: moneda },
            success: function (res) {
                $(".body-content").html(res);
            },
            error: function (e) {
                alert(e.responseText);
            }
        });
    }

    self.mostrarAlertVacio = ko.computed(function () {
        return self.Monedas().length <= 0;
    });
}


$(function () {

    var monedas = JSON.parse($("#dt").val());

    ko.utils.arrayMap(monedas, function (item) { return new MonedaViewModel(item); });
    $("#dt").remove();


    ko.applyBindings(new MonedasViewModel(monedas));
});