function CostoViewModel(costo) {
    const self = this;
    self.Id = ko.observable(costo.Id);
    self.FechaDeInicio = ko.observable(costo.FechaDeInicio || "01/01/1900");
    self.FechaDeInicioFormatted = ko.computed(() => { return formatDate(self.FechaDeInicio()) });
    self.FechaDeFin = ko.observable(costo.FechaDeFin || "01/01/1900");
    self.FechaDeFinFormatted = ko.computed(() => { return formatDate(self.FechaDeFin()) });
    self.Descripcion = ko.observable(costo.Descripcion || "");
    self.CostoDeGasolina = ko.observable(costo.CostoDeGasolina || 0);
    self.CostoDeAsistencia = ko.observable(costo.CostoDeAsistencia || 0);
    self.CostoDeMotorizado = ko.observable(costo.CostoDeMotorizado || 0);
    self.DistanciaBase = ko.observable(costo.DistanciaBase || 0);
    self.PrecioPorKm = ko.observable(costo.PrecioPorKm || 0);
    self.TipoDeServicioId = ko.observable(costo.TipoDeServicioId || -1);
    self.EstadoDelCosto = ko.observable(costo.EstadoDelCosto || false);
    self.PrecioDeRecargo = ko.observable(costo.PrecioDeRecargo || 0);
    self.PrecioDeRegreso = ko.observable(costo.PrecioDeRegreso || 0);
    self.TipoDeServicioDescripcion = ko.observable(costo.TipoDeServicioDescripcion || "");
};

function formatDate(dataToFormat) {
    return moment(dataToFormat).format("DD/MM/yyyy hh:mm")
}