function PagoViewModel(pago) {
    const self = this;

    self.Id = ko.observable(pago.);
    self.NumeroDePago = ko.observable(pago.NumeroDePago);
    self.FechaDePago = ko.observable(pago.FechaDePago);
    self.MontoDelPago = ko.observable(pago.MontoDelPago);
    self.Cambio = ko.observable(pago.Cambio);
    self.MonedaId = ko.observable(pago.MonedaId || -1);
    self.TipoDePagoId = ko.observable(pago.TipoDePagoId || -1);
    self.EnvioId = ko.observable(pago.EnvioId || -1);
    self.CreditoId = ko.observable(pago.CreditoId || -1);
    self.EstadoDelPago = ko.observable(pago.EstadoDelPago || false);
}