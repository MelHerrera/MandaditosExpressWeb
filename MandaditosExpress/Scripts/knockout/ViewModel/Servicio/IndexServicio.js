function IndexServicio(ServicioCollection) {
    const self = this;
    self.Servicio = ko.observableArray(ServicioCollection ? ko.utils.arrayMap(ServicioCollection, function (it) { return new ServicioViewModel(it) }) : []);
    self.TablePagination = ko.observable(new TablePagintationViewModel({
        PaginationSize: 5,
        Items: self.Servicio,//pasar todo el observable para que la paginacion reaccione a cualquier accion aplicada a este arreglo
        maxPageCount: 7
    }));
    self.Disable = ko.observable(false);
    self.DisableEver = ko.observable(true);

    self.ModalViewModel = ko.observable(new ModalViewModel({
        ModalId: "servicio-modal",
        ModalHeaderViewModel: new ModalHeaderViewModel({ ModalTitle: "Initial", ModalHeaderClass: "bg-secondary" }),
        ModalBodyViewModel: new ModalBodyViewModel(new TemplateViewModel({ Name: "servicio-modal-template", Data: new ServicioViewModel({}) })),
        FooterViewModel: new FooterModalViewModel({ ActionName: "Initial", UrlAction: "/Index" })
    }));

    self.ShowModal = function (Servicio, event) {

        if (event.currentTarget.id == "btn-edit") {
            self.Disable(false);
            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("Editar la información del Servicio").ModalHeaderClass("bg-success");
            self.ModalViewModel().ModalBodyViewModel().TemplateViewModel({ Name: "servicio-modal-template", Data: new ServicioViewModel(ko.toJS(Servicio)) });
            self.ModalViewModel().FooterViewModel().ActionName("Editar").UrlAction($(event.currentTarget).attr("href"));
        }
        if (event.currentTarget.id == "btn-del") {
            self.Disable(true);
            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("Eliminar el servicio").ModalHeaderClass("bg-danger");
            self.ModalViewModel().ModalBodyViewModel().TemplateViewModel({ Name: "servicio-modal-template", Data: new ServicioViewModel(ko.toJS(Servicio)) });
            self.ModalViewModel().FooterViewModel().ActionName("Eliminar").UrlAction($(event.currentTarget).attr("href"));
        }

        self.ModalViewModel().ShowModal();
    };

    self.MostrarDetalles = function (Servicio) {
        $.ajax(
            {
                url: $(event.currentTarget).attr("href"),
                data: {
                    id: ko.toJS(Servicio).Id
                },
                success: function (res) {
                    $(".body-content").html(res);
                }
            }
        );
    }

    self.GuardarCambios = function (data, event) {

        let token = $('.modal-form-foot input[name="__RequestVerificationToken"]').val();
        let servicio = self.ModalViewModel().ModalBodyViewModel().TemplateViewModel().Data;

        $.ajax({
            url: ko.toJS(data).UrlAction,
            type: "Post",
            data: { __RequestVerificationToken: token, servicio: servicio },
            success: function (res) {
                $("#" + ko.unwrap(self.ModalViewModel().ModalId())).modal('hide');
                if (res.exito) {
                    $.notify({
                        icon: 'fa fa-check-circle',
                        message: "Se actualizó la información Correctamente"
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
                    text: 'Ha ocurrido un error procesando tu solicitud!'
                })
            }
        });
    }

    self.mostrarAlertVacio = ko.computed(function () {
        return self.Servicio().length <= 0;
    });
}

$(function () {

    var Servicio = JSON.parse($("#dt").val());
    $("#dt").remove();

    ko.applyBindings(new IndexServicio(Servicio));
});
