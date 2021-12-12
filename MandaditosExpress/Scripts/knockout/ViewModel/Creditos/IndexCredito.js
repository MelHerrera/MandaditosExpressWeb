
function IndexCredito(creditoCollection) {
    const self = this;
    self.Disable = ko.observable(false);
    self.DisableEver = ko.observable(true);

    self.Creditos = ko.observableArray(creditoCollection ?
        ko.utils.arrayMap(creditoCollection, function (credito) { return new CreditoViewModel(credito) }) : []);

    self.ModalViewModel = ko.observable(new ModalViewModel({
        ModalId: "credito-modal",
        ModalHeaderViewModel: new ModalHeaderViewModel({ ModalTitle: "Initial", ModalHeaderClass: "bg-secondary" }),
        ModalBodyViewModel: new ModalBodyViewModel(new TemplateViewModel({ Name: "credito-modal-template", Data: new CreditoViewModel({}) })),
        FooterViewModel: new FooterModalViewModel({ ActionName: "Initial", UrlAction: "/Index" })
    }));

    self.ShowModal = function (credito, event) {

        if (event.currentTarget.id == "btn-edit") {
            self.Disable(false);

            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("Editar la información del crédito").ModalHeaderClass("bg-success");
            self.ModalViewModel().ModalBodyViewModel().TemplateViewModel({ Name: "credito-modal-template", Data: new CreditoViewModel(ko.toJS(credito)) });
            self.ModalViewModel().FooterViewModel().ActionName("Editar").UrlAction($(event.currentTarget).attr("href"));
        }
        if (event.currentTarget.id == "btn-del") {
            self.Disable(true);

            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("Eliminar el crédito").ModalHeaderClass("bg-danger");
            self.ModalViewModel().ModalBodyViewModel().TemplateViewModel({ Name: "credito-modal-template", Data: new CreditoViewModel(ko.toJS(credito)) });
            self.ModalViewModel().FooterViewModel().ActionName("Eliminar").UrlAction($(event.currentTarget).attr("href"));
        }

        self.ModalViewModel().ShowModal();
    };

    self.MostrarDetalles = function (credito) {
        $.ajax(
            {
                url: $(event.currentTarget).attr("href"),
                data: {
                    id: ko.toJS(credito).Id
                },
                success: function (res) {
                    $(".body-content").html(res);
                }
            }
        );
    }

    self.GuardarCambios = function (data, event) {

        let token = $('.modal-form-foot input[name="__RequestVerificationToken"]').val();
        let CreditoSelected = self.ModalViewModel().ModalBodyViewModel().TemplateViewModel().Data;

        $.ajax({
            url: ko.toJS(data).UrlAction,
            type: "Post",
            data: { __RequestVerificationToken: token, credito: CreditoSelected },
            success: function (res) {
                $("#" + ko.unwrap(self.ModalViewModel().ModalId())).modal('hide');//cerrarla independientemente de si es false o succes

                if (res.exito) {
                    $.notify({
                        icon: 'fa fa-check-circle',
                        message: "Se actualizó la informacion Correctamente"
                    });

                    setTimeout(function () { location.reload(); }, 2000);
                }
                else {
                    $.notify({
                        icon: 'fa fa-check-circle',
                        message: res.message
                    });
                }
            },
            error: function (e) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops... Disculpa',
                    text: 'Ha sucedido un error procesando tu solicitud!'
                })
            }
        });
    }

    self.mostrarAlertVacio = ko.computed(function () {
        return self.Creditos().length <= 0;
    });
}

$(function () {

    var creditos = JSON.parse($("#dt").val());
    $("#dt").remove();

    ko.applyBindings(new IndexCredito(creditos));
});
