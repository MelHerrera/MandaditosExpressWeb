
function VelocidadesDeConexionViewModel(VelocidadDeConexionCollection) {
    const self = this;
    self.VelocidadDeConexiones = ko.observableArray(VelocidadDeConexionCollection);
    
    self.ModalViewModel = ko.observable(new ModalViewModel({
        ModalId: "VelocidadDeConexion-modal",
        ModalHeaderViewModel: new ModalHeaderViewModel({ ModalTitle: "Initial", ModalHeaderClass: "bg-secondary" }),
        ModalBodyViewModel: new ModalBodyViewModel(new TemplateViewModel({ Name: "Velocidad-modal-template", Data: new VelocidadDeConexionViewModel({}) })),
        FooterViewModel: new FooterModalViewModel({ ActionName: "Initial", UrlAction: "/Index" })
    }));

    self.ShowModal = function (VelocidadDeConexion, event) {

        if (event.currentTarget.id == "btn-edit") {
            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("Editar la información de Calidad De Conexión").ModalHeaderClass("bg-success");
            self.ModalViewModel().ModalBodyViewModel().TemplateViewModel({ Name: "Velocidad-modal-template", Data: new VelocidadDeConexionViewModel(ko.toJS(VelocidadDeConexion)) });
            self.ModalViewModel().FooterViewModel().ActionName("Editar").UrlAction($(event.currentTarget).attr("href"));
        }
        if (event.currentTarget.id == "btn-del") {
            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("Eliminar la información de Calidad de Conexión").ModalHeaderClass("bg-danger");
            self.ModalViewModel().ModalBodyViewModel().TemplateViewModel({ Name: "Velocidad-modal-template", Data: new VelocidadDeConexionViewModel(ko.toJS(VelocidadDeConexion)) });
            self.ModalViewModel().FooterViewModel().ActionName("Eliminar").UrlAction($(event.currentTarget).attr("href"));
        }

        self.ModalViewModel().ShowModal();
    };

    self.MostrarDetalles = function (VelocidadDeConexion) {
        $.ajax(
            {
                url: $(event.currentTarget).attr("href"),
                data: {
                    id: ko.toJS(VelocidadDeConexion).Id
                },
                success: function (res) {
                    $(".body-content").html(res);
                }
            }
        );
    }

    self.GuardarCambios = function (data, event) {

        let token = $('.modal-form-foot input[name="__RequestVerificationToken"]').val();
        let velocidadDeConexion = self.ModalViewModel().ModalBodyViewModel().TemplateViewModel().Data;

        $.ajax({
            url: ko.toJS(data).UrlAction,
            type: "Post",
            data: { __RequestVerificationToken: token, velocidadDeConexion: velocidadDeConexion },
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
        return self.VelocidadDeConexiones().length <= 0;
    });
}

$(function () {

    var VelocidadDeConexiones = JSON.parse($("#dt").val());

 /*   ko.utils.arrayMap(VelocidadDeConexiones, function (item) { return new VelocidadDeConexionViewModel(item); });*/
    $("#dt").remove();


    ko.applyBindings(new VelocidadesDeConexionViewModel(VelocidadDeConexiones));
  });
