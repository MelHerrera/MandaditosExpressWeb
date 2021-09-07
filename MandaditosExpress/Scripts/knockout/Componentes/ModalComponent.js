﻿function ModalHeaderViewModel(params) {
    const self = this;
    self.ModalTitle = ko.observable(params.ModalTitle);
    self.ModalHeaderClass = ko.observable(params.ModalHeaderClass);
};


function ModalBodyViewModel(params) {
    const self = this;
    console.log(params);
    let data = ko.toJS(params);'io;m '
    console.log(Object.keys(data[0]));
    self.TemplateViewModel = ko.observable(params);
};

//viewmodel for footer modal components
function FooterModalViewModel(params) {
    var self = this;
    self.ActionName = ko.isObservable(params.ActionName) ? params.ActionName : ko.observable(params.ActionName);
    self.UrlAction = ko.isObservable(params.UrlAction) ? params.UrlAction : ko.observable(params.UrlAction);
};

//main viewmodel for modal components
function ModalViewModel(params) {
    var self = this;
    self.ModalId = ko.observable(params.ModalId || "#my-modal");
    self.ModalHeaderViewModel = ko.observable(params.ModalHeaderViewModel);
    self.ModalBodyViewModel = ko.observable(params.ModalBodyViewModel);
    self.FooterViewModel = ko.observable(params.FooterViewModel);

    self.ShowModal = function () {
        let modal = document.getElementById(ko.unwrap(self.ModalId));
        $(modal).modal('show');
    };
};

ko.components.register('modal-header', {
    viewModel: function (params) {
        return params.ModalHeaderViewModel
    },
    template: { element: 'modal-template-header' },
});


ko.components.register('modal-body', {
    viewModel: function (params) {
        return params.ModalBodyViewModel
    },
    template: { element: 'modal-body-template' }
});


//modal footer componet
//necesita un viewmodel aparte porque contiene varias propiedades
ko.components.register('modal-footer', {
    viewModel: function (params) {
        return params.FooterViewModel
    },
    template: { element: 'modal-template-footer' },
});

//main modal component
ko.components.register('modal', {
    viewModel: function (params) {
        return params.ModalViewModel
    },
    template: { element: 'modal-template' }
});

