
function MonedasViewModel(monedasCollection) {
    const self = this;
    self.Monedas = ko.observableArray(monedasCollection);

    self.ModalViewModel = ko.observable(new ModalViewModel({
        ModalId: "monedas-modal",
        ModalHeaderViewModel: new ModalHeaderViewModel({ ModalTitle: "Initial", ModalHeaderClass: "bg-secondary" }),
        ModalBodyViewModel: new ModalBodyViewModel( new TemplateViewModel({ Name: "moneda-modal-template", Data: new MonedaViewModel({}) }) ),
        FooterViewModel: new FooterModalViewModel({ ActionName: "Initial", UrlAction: "/Index" })
    }));

    self.ShowModal = function (moneda, event) {

        if (event.currentTarget.id == "btn-edit") {
            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("editar la información de la moneda").ModalHeaderClass("bg-success");
            self.ModalViewModel().ModalBodyViewModel().TemplateViewModel({ Name: "moneda-modal-template", Data: new MonedaViewModel(ko.toJS(moneda)) });
            self.ModalViewModel().FooterViewModel().ActionName("Editar").UrlAction($(event.currentTarget).attr("href"));
        }
        if (event.currentTarget.id == "btn-del") {
            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("Eliminar la información de la Moneda").ModalHeaderClass("bg-danger");
            self.ModalViewModel().ModalBodyViewModel().TemplateViewModel({ Name: "moneda-modal-template", Data: new MonedaViewModel(ko.toJS(moneda)) });
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
        let moneda = self.ModalViewModel().ModalBodyViewModel().TemplateViewModel().Data;

        $.ajax({
            url: ko.toJS(data).UrlAction,
            type: "Post",
            data: { __RequestVerificationToken: token, moneda: moneda },
            success: function (res) {
                if (res.exito) {
                    location.reload();
                    $.notify({
                        icon: 'fa fa-check-circle',
                        message: "Se edito la informacion Correctamente"
                    });
                }
            },
            error: function (e) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops... Disculpa',
                    text: 'Algo salio mal!',
                    footer: 'Contactese con el Administrador del Sistema'
                })
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