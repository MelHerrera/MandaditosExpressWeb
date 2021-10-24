function TipoDePagoViewModel(TipoDePago) {
    const self = this;


    self.Id = ko.observable(TipoDePago.Id || -1);
    self.Descripcion = ko.observable(TipoDePago.Descripcion || "");
    self.EstadoTipoDePago = ko.observable(TipoDePago.EstadoTipoDePago || false);
};