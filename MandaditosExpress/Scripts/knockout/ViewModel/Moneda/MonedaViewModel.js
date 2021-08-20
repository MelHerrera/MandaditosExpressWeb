function MonedaViewModel(moneda) {
    const self = this;
    self.Id = ko.observable(moneda.Id);
    self.NombreDeMoneda = ko.observable(moneda.NombreDeMoneda);
    self.Abreviatura = ko.observable(moneda.Abreviatura);
    self.Estado = ko.observable(moneda.Estado);
};