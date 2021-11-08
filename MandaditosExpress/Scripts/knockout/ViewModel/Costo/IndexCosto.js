function IndexCosto(CostoCollection) {
    const self = this;
    self.Costo = ko.observableArray(CostoCollection);
    self.Disable = ko.observable(false);
    self.DisableEver = ko.observable(true);

    self.ModalViewModel = ko.observable(new ModalViewModel({
        ModalId: "costo-modal",
        ModalHeaderViewModel: new ModalHeaderViewModel({ ModalTitle: "Initial", ModalHeaderClass: "bg-secondary" }),
        ModalBodyViewModel: new ModalBodyViewModel(new TemplateViewModel({ Name: "costo-modal-template", Data: new CostoViewModel({}) })),
        FooterViewModel: new FooterModalViewModel({ ActionName: "Initial", UrlAction: "/Index" })
    }));

    self.ShowModal = function (costo, event) {

        if (event.currentTarget.id == "btn-edit") {
            self.Disable(false);
            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("Editar la información del costo ").ModalHeaderClass("bg-success");
            self.ModalViewModel().ModalBodyViewModel().TemplateViewModel({ Name: "costo-modal-template", Data: new CostoViewModel(ko.toJS(costo)) });
            self.ModalViewModel().FooterViewModel().ActionName("Editar").UrlAction($(event.currentTarget).attr("href"));
        }
        if (event.currentTarget.id == "btn-del") {
            self.Disable(true);
            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("Eliminar la información de costo ").ModalHeaderClass("bg-danger");
            self.ModalViewModel().ModalBodyViewModel().TemplateViewModel({ Name: "costo-modal-template", Data: new CostoViewModel(ko.toJS(costo)) });
            self.ModalViewModel().FooterViewModel().ActionName("Eliminar").UrlAction($(event.currentTarget).attr("href"));
        }

        self.ModalViewModel().ShowModal();
    };

    self.MostrarDetalles = function (costo) {
        $.ajax(
            {
                url: $(event.currentTarget).attr("href"),
                data: {
                    id: ko.toJS(costo).Id
                },
                success: function (res) {
                    $(".body-content").html(res);
                }
            }
        );
    }

    self.GuardarCambios = function (data, event) {

        let token = $('.modal-form-foot input[name="__RequestVerificationToken"]').val();
        let costo = self.ModalViewModel().ModalBodyViewModel().TemplateViewModel().Data;

        $.ajax({
            url: ko.toJS(data).UrlAction,
            type: "Post",
            data: { __RequestVerificationToken: token, costo: costo},
            success: function (res) {
                if (res.exito) {

                    $("#" + ko.unwrap(self.ModalViewModel().ModalId())).modal('hide');

                    $.notify({
                        icon: 'fa fa-check-circle',
                        message: "Se actualizó la información Correctamente"
                    });
                    setTimeout(function () { location.reload(); }, 2000);
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
        return self.Costo().length <= 0;
    });
}


$(function () {

    var costo = JSON.parse($("#dt").val());

    /*ko.utils.arrayMap(costo, function (item) { return new Indexcosto(item); });*/
    $("#dt").remove();


    ko.applyBindings(new IndexCosto(costo));
});