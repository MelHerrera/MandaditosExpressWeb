﻿
function IndexTipoDePago(TipoDePagoCollection) {
    const self = this;
    self.TiposDePago = ko.observableArray(TipoDePagoCollection || []);

    self.ModalViewModel = ko.observable(new ModalViewModel({
        ModalId: "tipodepago-modal",
        ModalHeaderViewModel: new ModalHeaderViewModel({ ModalTitle: "Initial", ModalHeaderClass: "bg-secondary" }),
        ModalBodyViewModel: new ModalBodyViewModel(new TemplateViewModel({ Name: "tipodepago-modal-template", Data: new TipoDePagoViewModel({}) })),
        FooterViewModel: new FooterModalViewModel({ ActionName: "Initial", UrlAction: "/Index" })
    }));

    self.ShowModal = function (TipoDePago, event) {

        if (event.currentTarget.id == "btn-edit") {
            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("Editar la información del tipo de pago").ModalHeaderClass("bg-success");
            self.ModalViewModel().ModalBodyViewModel().TemplateViewModel({ Name: "tipodepago-modal-template", Data: new TipoDePagoViewModel(ko.toJS(TipoDePago)) });
            self.ModalViewModel().FooterViewModel().ActionName("Editar").UrlAction($(event.currentTarget).attr("href"));
        }
        if (event.currentTarget.id == "btn-del") {
            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("Eliminar el tipo de pago").ModalHeaderClass("bg-danger");
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
        return self.TiposDePago().length <= 0;
    });
}

$(function () {

    var TiposDePago = JSON.parse($("#dt").val());
    $("#dt").remove();

    ko.applyBindings(new IndexTipoDePago(TiposDePago));
});