function TablePagintationViewModel(data) {
    const self = this;

    self.PaginationSize = ko.observable(data.PaginationSize || 5);
    self.Items = ko.isObservable(data.Items) ? data.Items : ko.observable(data.Items);
    self.maxPageCount = ko.observable(data.maxPageCount || 5);//max number link shown in pagination

    //Paginacion
    self.Pagination = ko.observable(new PaginationViewModel({
        pageSize: self.PaginationSize,
        totalCount: self.Items().length,
        maxPageCount: self.maxPageCount
    }));

    self.ItemsInPage = ko.computed(function () {
        var startIndex = (self.Pagination().CurrentPage() - 1) * self.Pagination().PageSize();
        var endIndex = self.Pagination().CurrentPage() * self.Pagination().PageSize();

        return ko.unwrap(self.Items).slice(startIndex, endIndex);
    });
}