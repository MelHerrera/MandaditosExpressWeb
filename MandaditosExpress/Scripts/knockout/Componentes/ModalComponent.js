function ModalHeaderViewModel(params) {
    const self = this;
    self.ModalTitle = ko.observable(params.ModalTitle || "");
    self.ModalHeaderClass = ko.observable(params.ModalHeaderClass || "bg-secondary");
};


function ModalBodyViewModel(params) {
    const self = this;
    self.TemplateViewModel = ko.observable(new TemplateViewModel({ Name: params.Name, Data: params.Data }));
};

function TemplateViewModel(params) {
    const self = this;
    self.Name = ko.observable(ko.isObservable(params.Name) ? params.Name : ko.observable(params.Name) || "");
    self.Data = ko.observable(params.Data || "");
};

//viewmodel for footer modal components
function FooterModalViewModel(params) {
    var self = this;
    self.ActionName = ko.isObservable(params.ActionName || "") ? params.ActionName : ko.observable(params.ActionName || "");
    self.UrlAction = ko.isObservable(params.UrlAction || "") ? params.UrlAction : ko.observable(params.UrlAction || "");
};

//main viewmodel for modal components
function ModalViewModel(params) {
    var self = this;
    self.ModalId = ko.observable(params.ModalId || "#my-modal");
    self.ModalHeaderViewModel = ko.observable(params.ModalHeaderViewModel || new ModalHeaderViewModel({}));
    self.ModalBodyViewModel = ko.observable(params.ModalBodyViewModel || new ModalBodyViewModel({}));
    self.FooterViewModel = ko.observable(params.FooterViewModel || new FooterModalViewModel({}) );

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