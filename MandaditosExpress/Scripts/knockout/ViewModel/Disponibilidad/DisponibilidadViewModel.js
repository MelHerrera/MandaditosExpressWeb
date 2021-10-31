function DisponibilidadViewModel(disponibilidad) {
    const self = this;
    self.Id = ko.observable(disponibilidad.Id);
    self.Descripcion = ko.observable(disponibilidad.Descripcion);
    self.EstadoDeLaDisponibilidad = ko.observable(disponibilidad.EstadoDeLaDisponibilidad);
    };
