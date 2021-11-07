function IndexDisponibilidad(disponibilidadCollection) {
    const self = this;
    self.Disponibilidad = ko.observableArray(disponibilidadCollection);

    self.ModalViewModel = ko.observable(new ModalViewModel({
        ModalId: "disponibilidad-modal",
        ModalHeaderViewModel: new ModalHeaderViewModel({ ModalTitle: "Initial", ModalHeaderClass: "bg-secondary" }),
        ModalBodyViewModel: new ModalBodyViewModel(new TemplateViewModel({ Name: "disponibilidad-modal-template", Data: new DisponibilidadViewModel({}) })),
        FooterViewModel: new FooterModalViewModel({ ActionName: "Initial", UrlAction: "/Index" })
    }));

    self.ShowModal = function (disponiblidad, event) {

        if (event.currentTarget.id == "btn-edit") {
            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("editar la información de la disponibilidad").ModalHeaderClass("bg-success");
            self.ModalViewModel().ModalBodyViewModel().TemplateViewModel({ Name: "disponibilidad-modal-template", Data: new DisponibilidadViewModel(ko.toJS(disponiblidad)) });
            self.ModalViewModel().FooterViewModel().ActionName("Editar").UrlAction($(event.currentTarget).attr("href"));
        }
        if (event.currentTarget.id == "btn-del") {
            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("Eliminar la información de disponibilidad").ModalHeaderClass("bg-danger");
            self.ModalViewModel().ModalBodyViewModel().TemplateViewModel({ Name: "disponibilidad-modal-template", Data: new DisponibilidadViewModel(ko.toJS(disponiblidad)) });
            self.ModalViewModel().FooterViewModel().ActionName("Eliminar").UrlAction($(event.currentTarget).attr("href"));
        }

        self.ModalViewModel().ShowModal();
    };

    self.MostrarDetalles = function (disponibilidad) {
        $.ajax(
            {
                url: $(event.currentTarget).attr("href"),
                data: {
                    id: ko.toJS(disponibilidad).Id
                },
                success: function (res) {
                    $(".body-content").html(res);
                }
            }
        );
    }

    self.GuardarCambios = function (data, event) {

        let token = $('.modal-form-foot input[name="__RequestVerificationToken"]').val();
        let disponibilidad = self.ModalViewModel().ModalBodyViewModel().TemplateViewModel().Data;

        $.ajax({
            url: ko.toJS(data).UrlAction,
            type: "Post",
            data: { __RequestVerificationToken: token, disponibilidad: disponibilidad },
            success: function (res) {
                if (res.exito) {

                    $("#" + ko.unwrap(self.ModalViewModel().ModalId())).modal('hide');

                    $.notify({
                        icon: 'fa fa-check-circle',
                        message: "Se actualizó la informacion Correctamente"
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
        return self.Disponibilidad().length <= 0;
    });
}


$(function () {

    var disponibilidad = JSON.parse($("#dt").val());

    /*ko.utils.arrayMap(disponibilidad, function (item) { return new IndexDisponibilidad(item); });*/
    $("#dt").remove();


    ko.applyBindings(new IndexDisponibilidad(disponibilidad));
});