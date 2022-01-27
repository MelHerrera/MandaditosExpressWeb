function CreditoViewModel(credito) {
    const self = this;

    self.Id = ko.observable(credito.Id || -1);
    self.Descripcion = ko.observable(credito.Descripcion || "");
    self.FechaDeInicio = ko.observable(credito.FechaDeInicio || "01/01/1900");
    self.FechaDeInicioFormatted = ko.computed(() => { return formatDate(self.FechaDeInicio()) });
    self.FechaDeVencimiento = ko.observable(credito.FechaDeVencimiento || "01/01/1900");
    self.FechaDeVencimientoFormatted = ko.computed(() => { return formatDate(self.FechaDeVencimiento()) });
    self.FechaDeCancelacion = ko.observable(credito.FechaDeCancelacion || "01/01/1900");
    self.FechaDeCancelacionFormatted = ko.computed(() => { return formatDate(self.FechaDeCancelacion()) });
    self.ClienteId = ko.observable(credito.ClienteId || -1);
    self.NombreCompleto = ko.observable(credito.NombreCompletoCliente ? credito.NombreCompletoCliente : credito.NombreCompleto ? credito.NombreCompleto : "");
    self.EstadoDelCredito = ko.observable(credito.EstadoDelCredito || false);
};

function formatDate(dataToFormat) {
    return moment(dataToFormat).format("DD/MM/yyyy hh:mm")
}