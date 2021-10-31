

function CostoGestionBancariaViewModel(Cgbancaria) {
    const self = this;
    self.Id = ko.observable(Cgbancaria.Id);
    self.FechaDeInicio = ko.observable(Cgbancaria.FechaDeInicio || "01/01/1900");
    self.FechaDeFin = ko.observable(Cgbancaria.FechaDeFin || "01/01/1900");
    self.Descripcion = ko.observable(Cgbancaria.Descripcion);
    self.MontoDesde = ko.observable(Cgbancaria.MontoDesde);
    self.Estado = ko.observable(Cgbancaria.Estado || false);
    self.Porcentaje = ko.observable(Cgbancaria.Porcentaje);
    self.PrecioDeRecargo = ko.observable(Cgbancaria.PrecioDeRecargo);
    self.TipoDeServicioId = ko.observable(Cgbancaria.TipoDeServicioId || -1);
    self.Descripcion = ko.observable(Cgbancaria.Descripcion ? Cgbancaria.Descripcion : Cgbancaria.TipoDeServicio ? Cgbancaria.TipoDeServicio.Descripcion : "");
};
