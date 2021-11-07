function IndexTipoDeServicio(TipoDeServicioCollection) {
    const self = this;
    self.TipoDeServicio = ko.observableArray(TipoDeServicioCollection || []);

    self.ModalViewModel = ko.observable(new ModalViewModel({
        ModalId: "tipodeservicio-modal", 
        ModalHeaderViewModel: new ModalHeaderViewModel({ ModalTitle: "Initial", ModalHeaderClass: "bg-secondary" }),
        ModalBodyViewModel: new ModalBodyViewModel(new TemplateViewModel({ Name: "tipodeservicio-modal-template", Data: new TipoDeServicioViewModel({}) })),
        FooterViewModel: new FooterModalViewModel({ ActionName: "Initial", UrlAction: "/Index" })
    }));

    self.ShowModal = function (TipoDeServicio, event) {

        if (event.currentTarget.id == "btn-edit") {
            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("Editar la información del tipo de Servicio").ModalHeaderClass("bg-success");
            self.ModalViewModel().ModalBodyViewModel().TemplateViewModel({ Name: "tipodeservicio-modal-template", Data: new TipoDeServicioViewModel(ko.toJS(TipoDeServicio)) });
            self.ModalViewModel().FooterViewModel().ActionName("Editar").UrlAction($(event.currentTarget).attr("href"));
        }
        if (event.currentTarget.id == "btn-del") {
            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("Eliminar el tipo de servicio").ModalHeaderClass("bg-danger");
            self.ModalViewModel().ModalBodyViewModel().TemplateViewModel({ Name: "tipodeservicio-modal-template", Data: new TipoDeServicioViewModel(ko.toJS(TipoDeServicio)) });
            self.ModalViewModel().FooterViewModel().ActionName("Eliminar").UrlAction($(event.currentTarget).attr("href"));
        }

        self.ModalViewModel().ShowModal();
    };

    self.MostrarDetalles = function (TipoDeServicio) {
        $.ajax(
            {
                url: $(event.currentTarget).attr("href"),
                data: {
                    id: ko.toJS(TipoDeServicio).Id
                },
                success: function (res) {
                    $(".body-content").html(res);
                }
            }
        );
    }

    self.GuardarCambios = function (data, event) {

        let token = $('.modal-form-foot input[name="__RequestVerificationToken"]').val();
        let tipoDeServicio = self.ModalViewModel().ModalBodyViewModel().TemplateViewModel().Data;

        $.ajax({
            url: ko.toJS(data).UrlAction,
            type: "Post",
            data: { __RequestVerificationToken: token, tipoDeServicio: tipoDeServicio },
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
        return self.TipoDeServicio().length <= 0;
    });
}

$(function () {

    var TipoDeServicio = JSON.parse($("#dt").val());
    $("#dt").remove();

    ko.applyBindings(new IndexTipoDeServicio(TipoDeServicio));
});