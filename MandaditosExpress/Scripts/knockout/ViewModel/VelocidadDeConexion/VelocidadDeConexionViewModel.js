function VelocidadDeConexionViewModel(VelocidadDeConexion) {
    const self = this;
    self.Id = ko.observable(VelocidadDeConexion.Id);
    self.Descripcion = ko.observable(VelocidadDeConexion.Descripcion);
    self.Estado = ko.observable(VelocidadDeConexion.Estado);
};