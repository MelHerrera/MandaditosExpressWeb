function AsistenteViewModel(asistente) {
    const self = this;


    self.Id = ko.observable(asistente.Id || -1);
    self.FechaIngreso = ko.observable(asistente.FechaIngreso || "01/01/1900");
    self.EstadoDeAsistente = ko.observable(asistente.EstadoDeAsistente || false);
    self.FechaDeBaja = ko.observable(asistente.FechaDeBaja || "01/01/1900");
    self.CorreoElectronico = ko.observable(asistente.CorreoElectronico || "");
    self.NombreCompleto = ko.observable(asistente.NombreCompleto || "");
  
    self.Telefono = ko.observable(asistente.Telefono || "");
    self.Foto = ko.observable(asistente.Foto || "");
    self.Sexo = ko.observable(asistente.Sexo || "");
    self.Direccion = ko.observable(asistente.Direccion || "");

    self.Cedula = ko.observable(asistente.Cedula || "");
   
};