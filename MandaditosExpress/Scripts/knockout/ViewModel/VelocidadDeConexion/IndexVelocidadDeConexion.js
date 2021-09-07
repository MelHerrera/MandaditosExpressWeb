
function VelocidadesDeConexionViewModel(VelocidadDeConexionCollection) {
    const self = this;
    self.VelocidadDeConexiones = ko.observableArray(VelocidadDeConexionCollection);
    
    self.VelocidadDeConexion = ko.observable(new VelocidadDeConexionViewModel({}));

    self.ModalViewModel = ko.observable(new ModalViewModel({
        ModalId: "VelocidadDeConexion-modal",
        ModalHeaderViewModel: new ModalHeaderViewModel({ ModalTitle: "Initial", ModalHeaderClass: "bg-secondary" }),
        ModalBodyViewModel: new ModalBodyViewModel({ VelocidadDeConexionTemplateViewModel: new VelocidadDeConexionTemplateViewModel({ Name: "Velocidad-modal-template", VelocidadDeConexionViewModel: new VelocidadDeConexionViewModel({}) }) }),
        FooterViewModel: new FooterModalViewModel({ ActionName: "Initial", UrlAction: "/Index" })
    }));

    self.ShowModal = function (VelocidadDeConexion, event) {

        //pasarle la Velocidad  actual a la propiedad velocidades
        self.VelocidadDeConexion(ko.toJS(VelocidadDeConexion));

        if (event.currentTarget.id == "btn-edit") {
            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("editar la información de Velocidad De Conexion").ModalHeaderClass("bg-success");
            self.ModalViewModel().ModalBodyViewModel().TemplateViewModel().VelocidadDeConexionTemplateViewModel().Name("Velocidad-modal-template").VelocidadDeConexionViewModel(new VelocidadDeConexionViewModel(ko.toJS(self.VelocidadDeConexion)));
            self.ModalViewModel().FooterViewModel().ActionName("Editar").UrlAction($(event.currentTarget).attr("href"));
        }
        if (event.currentTarget.id == "btn-del") {
            self.ModalViewModel().ModalHeaderViewModel().ModalTitle("Eliminar la información de Velocidad De Conexion").ModalHeaderClass("bg-danger");
            self.ModalViewModel().ModalBodyViewModel().VelocidadDeConexionTemplateViewModel().Name("Velocidad-modal-template").VelocidadDeConexionViewModel(new VelocidadDeConexionViewModel(ko.toJS(self.VelocidadDeConexion)));
            self.ModalViewModel().FooterViewModel().ActionName("Eliminar").UrlAction($(event.currentTarget).attr("href"));
        }

        self.ModalViewModel().ShowModal();
    };

    self.MostrarDetalles = function (VelocidadDeConexion) {
        $.ajax(
            {
                url: $(event.currentTarget).attr("href"),
                data: {
                    id: ko.toJS(moneda).Id
                },
                success: function (res) {
                    $(".body-content").html(res);
                }
            }
        );
    }

    self.GuardarCambios = function (data, event) {

        let token = $('.modal-form-foot input[name="__RequestVerificationToken"]').val();
        let moneda = ko.toJS(self.ModalViewModel().ModalBodyViewModel().VelocidadDeConexionTemplateViewModel().VelocidadDeConexionViewModel());

        $.ajax({
            url: ko.toJS(data).UrlAction,
            type: "Post",
            data: { __RequestVerificationToken: token, moneda: moneda },
            success: function (res) {
                $(".body-content").html(res);
            },
            error: function (e) {
                alert(e.responseText);
            }
        });
    }

    self.mostrarAlertVacio = ko.computed(function () {
        return self.VelocidadDeConexion().length <= 0;
    });
}

$(function () {

    var VelocidadDeConexiones = JSON.parse($("#dt").val());

 /*   ko.utils.arrayMap(VelocidadDeConexiones, function (item) { return new VelocidadDeConexionViewModel(item); });*/
    $("#dt").remove();


    ko.applyBindings(new VelocidadesDeConexionViewModel(VelocidadDeConexiones));
  });
