function  (costo) {
    const self = this;
    self.Id = ko.observable(costo.Id);
    self.FechaDeInicio = ko.observable(costo.FechaDeInicio || "01/01/1900");
    self.FechaDeFin = ko.observable(costo.FechaDeFin || "01/01/1900");
    self.Descripcion = ko.observable(costo.Descripcion);
    self.CostoDeGasolina = ko.observable(costo.CostoDeGasolina);
    self.CostoDeMotorizado = ko.observable(costo.CostoDeMotorizado);
    self.DistanciaBase = ko.observable(costo.DistanciaBase);
    self.PrecioPorKm = ko.observable(costo.PrecioPorKm);
    self.TipoDeServicioId = ko.observable(costo.TipoDeServicioId || -1);
    self.Estado = ko.observable(costo.Estado || false);
    self.PrecioDeRecargo = ko.observable(costo.PrecioPorKm);
    self.Descripcion = ko.observable(costo.Descripcion ? costo.Descripcion : costo.TipoDeServicio ? costo.TipoDeServicio.Descripcion : "");
};