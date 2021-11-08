function ServicioViewModel(Servicio) {
    const self = this;
    self.Id = ko.observable(Servicio.Id || -1);
    self.DescripcionDelServicio = ko.observable(Servicio.DescripcionDelServicio || "");
    self.Estado = ko.observable(Servicio.Estado || false);
    self.TipoDeServicioId = ko.observable(Servicio.TipoDeServicioId || -1);
    self.DescripcionTipoDeServicio = ko.observable(Servicio.DescripcionTipoDeServicio ? Servicio.DescripcionTipoDeServicio : Servicio.TipoDeServicio ? Servicio.TipoDeServicio.DescripcionTipoDeServicio : "");
};
