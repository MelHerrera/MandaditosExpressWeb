function IndexAsistente(asistenteCollection) {
    const self = this;

    self.Asistentes = ko.observableArray(ko.utils.arrayMap(asistenteCollection, function (item) { return new AsistenteViewModel(item) }) || []);

    self.ModalViewModel = ko.observable(new ModalViewModel({
        ModalId: "asistente-modal",
        ModalHeaderViewModel: new ModalHeaderViewModel({ ModalTitle: "Initial", ModalHeaderClass: "bg-secondary" }),
        ModalBodyViewModel: new ModalBodyViewModel(new TemplateViewModel({ Name: "asistente-modal-template", Data: new AsistenteViewModel({}) })),
        FooterViewModel: new FooterModalViewModel({ ActionName: "Initial", UrlAction: "/Index" })
    }));

    self.ShowModal = function (asistente, event) {

        if (event.currentTarget.id == "btn-edit") {

            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("Editar la información del Asistente").ModalHeaderClass("bg-success");
            self.ModalViewModel().ModalBodyViewModel().TemplateViewModel({ Name: "asistente-modal-template", Data: new AsistenteViewModel(ko.toJS(asistente)) });
            self.ModalViewModel().FooterViewModel().ActionName("Editar").UrlAction($(event.currentTarget).attr("href"));
        }
        if (event.currentTarget.id == "btn-del") {

            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("Eliminar el Asistente").ModalHeaderClass("bg-danger");
            self.ModalViewModel().ModalBodyViewModel().TemplateViewModel({ Name: "asistente-modal-template", Data: new AsistenteViewModel(ko.toJS(asistente)) });
            self.ModalViewModel().FooterViewModel().ActionName("Eliminar").UrlAction($(event.currentTarget).attr("href"));
        }

        self.ModalViewModel().ShowModal();
    };

    self.MostrarDetalles = function (asistente) {
        $.ajax(
            {
                url: $(event.currentTarget).attr("href"),
                data: {
                    id: ko.toJS(asistente).Id
                },
                success: function (res) {
                    $(".body-content").html(res);
                }
            }
        );
    }

    self.GuardarCambios = function (data, event) {

        let token = $('.modal-form-foot input[name="__RequestVerificationToken"]').val();
        let AsistenteSeleted = self.ModalViewModel().ModalBodyViewModel().TemplateViewModel().Data;

        $.ajax({
            url: ko.toJS(data).UrlAction,
            type: "Post",
            data: { __RequestVerificationToken: token, asistente: AsistenteSeleted },
            success: function (res) {
                if (res.exito) {
                    //location.reload();
                    console.log(self.ModalViewModel());
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
        return self.Asistentes().length <= 0;
    });
}

$(function () {

    var asistentes = JSON.parse($("#dt").val());
    ko.utils.arrayMap(asistentes, function (item) { return new AsistenteViewModel(item); });
    $("#dt").remove();

    ko.applyBindings(new IndexAsistente(asistentes));
});