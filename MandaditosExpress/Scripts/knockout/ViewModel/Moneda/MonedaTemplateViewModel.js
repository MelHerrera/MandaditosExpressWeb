function MonedaTemplateViewModel(params) {
    const self = this;
    self.Name = ko.observable(params.Name);
    self.MonedaViewModel = ko.observable(params.MonedaViewModel);
};