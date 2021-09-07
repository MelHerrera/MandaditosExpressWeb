function VelocidadDeConexionTemplateViewModel(params) {
    const self = this;
    self.Name = ko.observable(params.Name);
    self.VelocidadDeConexionViewModel = ko.observable(params.VelocidadDeConexionViewModel);
};