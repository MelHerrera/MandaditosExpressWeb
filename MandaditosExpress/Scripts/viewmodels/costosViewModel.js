////var costos = function (data) {
////    var self = this;

////    //properties
////    self.Id = ko.observable(data.Id);
////    self.FechaDeInicio = ko.observable(data.FechaDeInicio);
////    self.FechaDeFin = ko.observable(data.FechaDeFin);
////    self.CostoDeGasolina = ko.observable(data.CostoDeGasolina);
////    self.CostoDeAsistencia = ko.observable(data.CostoDeAsistencia);
////    self.CostoDeMotorizado = ko.observable(data.CostoDeMotorizado);
////    self.DistanciaBase = ko.observable(data.DistanciaBase);
////    self.PrecioPorKm = ko.observable(data.PrecioPorKm);
////    self.EstadoDelCosto = ko.observable(data.EstadoDelCosto);
////    self.PrecioBaseGestionBancaria = ko.observable(data.PrecioBaseGestionBancaria);
////    self.PorcentajeBaseGestionBancaria = ko.observable(data.PorcentajeBaseGestionBancaria);
////    self.Gestiones = ko.observable(data.Gestiones);
////}

var costos = function (data) {
    
    var self = this;

    //properties
    self.costos = ko.observableArray(data);
  
    self.show = function () {
        console.log(self.costos);
    }
}