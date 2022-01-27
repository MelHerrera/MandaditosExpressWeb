function CostoGestionBancariaViewModel(Cgbancaria) {
    const self = this;
    self.Id = ko.observable(Cgbancaria.Id);
    self.FechaDeInicio = ko.observable(Cgbancaria.FechaDeInicio || "01/01/1900");
    self.FechaDeInicioFormatted = ko.computed(() => { return formatDate(self.FechaDeInicio()) });
    self.FechaDeFin = ko.observable(Cgbancaria.FechaDeFin || "01/01/1900");
    self.FechaDeFinFormatted = ko.computed(() => { return formatDate(self.FechaDeFin()) });
    self.Descripcion = ko.observable(Cgbancaria.Descripcion || "");
    self.MontoDesde = ko.observable(Cgbancaria.MontoDesde || 0);
    self.MontoHasta = ko.observable(Cgbancaria.MontoHasta || 0);
    self.Estado = ko.observable(Cgbancaria.Estado || false);
    self.Porcentaje = ko.observable(Cgbancaria.Porcentaje || 0);
    self.Valor = ko.observable(Cgbancaria.Valor || 0);
    self.PrecioDeRecargo = ko.observable(Cgbancaria.PrecioDeRecargo || 0);
    self.PrecioDeRegreso = ko.observable(Cgbancaria.PrecioDeRegreso || 0);
    self.TipoDeServicioId = ko.observable(Cgbancaria.TipoDeServicioId || -1);
    self.TipoDeServicioDescripcion = ko.observable(Cgbancaria.TipoDeServicioDescripcion || " - ");
};

function formatDate(dataToFormat) {
    return moment(dataToFormat).format("DD/MM/yyyy hh:mm")
}
