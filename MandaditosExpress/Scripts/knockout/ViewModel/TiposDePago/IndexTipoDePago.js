﻿
function IndexTipoDePago(TipoDePagoCollection) {
    const self = this;
    self.TiposDePago = ko.observableArray(TipoDePagoCollection || []);
    self.Disable = ko.observable(false);

    self.ModalViewModel = ko.observable(new ModalViewModel({
        ModalId: "tipodepago-modal",
        ModalHeaderViewModel: new ModalHeaderViewModel({ ModalTitle: "Initial", ModalHeaderClass: "bg-secondary" }),
        ModalBodyViewModel: new ModalBodyViewModel(new TemplateViewModel({ Name: "tipodepago-modal-template", Data: new TipoDePagoViewModel({}) })),
        FooterViewModel: new FooterModalViewModel({ ActionName: "Initial", UrlAction: "/Index" })
    }));

    self.ShowModal = function (TipoDePago, event) {

        if (event.currentTarget.id == "btn-edit") {
            self.Disable(false);
            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("Editar la información del método de pago").ModalHeaderClass("bg-success");
            self.ModalViewModel().ModalBodyViewModel().TemplateViewModel({ Name: "tipodepago-modal-template", Data: new TipoDePagoViewModel(ko.toJS(TipoDePago)) });
            self.ModalViewModel().FooterViewModel().ActionName("Editar").UrlAction($(event.currentTarget).attr("href"));
        }
        if (event.currentTarget.id == "btn-del") {
            self.Disable(true);
            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("Eliminar el método de pago").ModalHeaderClass("bg-danger");
            self.ModalViewModel().ModalBodyViewModel().TemplateViewModel({ Name: "tipodepago-modal-template", Data: new TipoDePagoViewModel(ko.toJS(TipoDePago)) });
            self.ModalViewModel().FooterViewModel().ActionName("Eliminar").UrlAction($(event.currentTarget).attr("href"));
        }

        self.ModalViewModel().ShowModal();
    };

    self.MostrarDetalles = function (TipoDePago) {
        $.ajax(
            {
                url: $(event.currentTarget).attr("href"),
                data: {
                    id: ko.toJS(TipoDePago).Id
                },
                success: function (res) {
                    $(".body-content").html(res);
                }
            }
        );
    }

    self.GuardarCambios = function (data, event) {

        let token = $('.modal-form-foot input[name="__RequestVerificationToken"]').val();
        let TipoDePagoSelected = self.ModalViewModel().ModalBodyViewModel().TemplateViewModel().Data;

        $.ajax({
            url: ko.toJS(data).UrlAction,
            type: "Post",
            data: { __RequestVerificationToken: token, tipoDePago: TipoDePagoSelected },
            success: function (res) {
                self.ModalViewModel().HideModal();
                if (res.exito) {
                    setTimeout(function () { location.reload(); }, 2000);

                    $.notify({
                        icon: 'fa fa-check-circle',
                        message: "Se edito la informacion Correctamente"
                    });
                }
                else {
                    $.notify({
                        icon: 'fa fa-exclamation-circle',
                        message: res.message
                    });
                }
            },
            error: function (e) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops... Disculpa',
                    text: 'Ha ocurrido un error procesando tu solicitud!'
                })
            }
        });
    }

    self.mostrarAlertVacio = ko.computed(function () {
        return self.TiposDePago().length <= 0;
    });
}

$(function () {

    var TiposDePago = JSON.parse($("#dt").val());
    $("#dt").remove();

    ko.applyBindings(new IndexTipoDePago(TiposDePago));
});
