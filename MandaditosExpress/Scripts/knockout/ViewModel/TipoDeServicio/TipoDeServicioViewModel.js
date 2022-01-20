function TipoDeServicioViewModel(TipoDeServicio) {
    const self = this;
    self.Id = ko.observable(TipoDeServicio.Id || -1);
    self.DescripcionTipoDeServicio = ko.observable(TipoDeServicio.DescripcionTipoDeServicio || "");
    self.EstadoTipoDeServicio = ko.observable(TipoDeServicio.EstadoTipoDeServicio || false);
};