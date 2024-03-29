﻿function IndexCostoGestionBancaria(CostoGetionBancariaCollection) {
    const self = this;
    self.CostoGetionBancaria = ko.observableArray(CostoGetionBancariaCollection ? ko.utils.arrayMap(CostoGetionBancariaCollection, function (it) { return new CostoGestionBancariaViewModel(it) }) : []);
    self.Disable = ko.observable(false);
    self.DisableEver = ko.observable(true);

    self.ModalViewModel = ko.observable(new ModalViewModel({
        ModalId: "costogestionbancaria-modal",
        ModalHeaderViewModel: new ModalHeaderViewModel({ ModalTitle: "Initial", ModalHeaderClass: "bg-secondary" }),
        ModalBodyViewModel: new ModalBodyViewModel(new TemplateViewModel({ Name: "costogestionbancaria-modal-template", Data: new CostoGestionBancariaViewModel({}) })),
        FooterViewModel: new FooterModalViewModel({ ActionName: "Initial", UrlAction: "/Index" })
    }));

    self.ShowModal = function (costogestionbancaria, event) {

        if (event.currentTarget.id == "btn-edit") {
            self.Disable(false);
            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("Editar la información del costo de gestion Bancaria ").ModalHeaderClass("bg-success");
            self.ModalViewModel().ModalBodyViewModel().TemplateViewModel({ Name: "costogestionbancaria-modal-template", Data: new CostoGestionBancariaViewModel(ko.toJS(costogestionbancaria)) });
            self.ModalViewModel().FooterViewModel().ActionName("Editar").UrlAction($(event.currentTarget).attr("href"));
        }
        if (event.currentTarget.id == "btn-del") {
            self.Disable(true);
            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("Eliminar la información delcosto de gestion Bancariad").ModalHeaderClass("bg-danger");
            self.ModalViewModel().ModalBodyViewModel().TemplateViewModel({ Name: "costogestionbancaria-modal-template", Data: new CostoGestionBancariaViewModel(ko.toJS(costogestionbancaria)) });
            self.ModalViewModel().FooterViewModel().ActionName("Eliminar").UrlAction($(event.currentTarget).attr("href"));
        }

        self.ModalViewModel().ShowModal();
    };

    self.MostrarDetalles = function (costoGestionBancaria) {
        $.ajax(
            {
                url: $(event.currentTarget).attr("href"),
                data: {
                    id: ko.toJS(costoGestionBancaria).Id
                },
                success: function (res) {
                    $(".body-content").html(res);
                }
            }
        );
    }

    self.GuardarCambios = function (data, event) {

        let token = $('.modal-form-foot input[name="__RequestVerificationToken"]').val();
        let costoGestionBancaria = self.ModalViewModel().ModalBodyViewModel().TemplateViewModel().Data;

        $.ajax({
            url: ko.toJS(data).UrlAction,
            type: "Post",
            data: { __RequestVerificationToken: token, costoGestionbancaria: costoGestionBancaria },
            success: function (res) {
                $("#" + ko.unwrap(self.ModalViewModel().ModalId())).modal('hide');

                if (res.exito) {
                    $.notify({
                        icon: 'fa fa-check-circle',
                        message: "Se actualizó la informacion Correctamente"
                    });
                    setTimeout(function () { location.reload(); }, 2000);
                }
                else {
                    $.notify({
                        icon: 'fa fa-triangle-circle',
                        message: res.message
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
        return self.CostoGetionBancaria().length <= 0;
    });
}


$(function () {

    var costogestionbancaria = JSON.parse($("#dt").val());

    /*ko.utils.arrayMap(costogestionbancaria, function (item) { return new Indexcostogestionbancaria(item); });*/
    $("#dt").remove();


    ko.applyBindings(new IndexCostoGestionBancaria(costogestionbancaria));
});