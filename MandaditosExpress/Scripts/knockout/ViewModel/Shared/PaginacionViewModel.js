﻿function PaginationViewModel(options) {
    const self = this;

    self.PageSize = ko.isObservable(options.pageSize) ? options.pageSize : ko.observable(options.pageSize);
    self.CurrentPage = ko.observable(1);
    self.TotalCount = ko.observable(options.totalCount || 0);
    self.maxPageCount = ko.isObservable(options.maxPageCount) ? options.maxPageCount : ko.observable(options.maxPageCount); // this should be odd number always

    self.PageCount = ko.computed(function () {
        return Math.ceil(self.TotalCount() / self.PageSize());
    });

    self.SetCurrentPage = function (page) {
        if (page < self.FirstPage)
            page = self.FirstPage;

        if (page > self.LastPage())
            page = self.LastPage();

        self.CurrentPage(page);
    };

    self.FirstPage = 1;
    self.LastPage = ko.computed(function () {
        return self.PageCount();
    });

    self.NextPage = ko.computed(function () {
        var next = self.CurrentPage() + 1;
        if (next > self.LastPage())
            return null;
        return next;
    });

    self.PreviousPage = ko.computed(function () {
        var previous = self.CurrentPage() - 1;
        if (previous < self.FirstPage)
            return null;
        return previous;
    });

    self.NeedPaging = ko.computed(function () {
        return self.PageCount() > 1;
    });

    self.NextPageActive = ko.computed(function () {
        return self.NextPage() != null;
    });

    self.PreviousPageActive = ko.computed(function () {
        return self.PreviousPage() != null;
    });

    self.LastPageActive = ko.computed(function () {
        return (self.LastPage() != self.CurrentPage());
    });

    self.FirstPageActive = ko.computed(function () {
        return (self.FirstPage != self.CurrentPage());
    });

    self.generateAllPages = function () {
        var pages = [];
        for (var i = self.FirstPage; i <= self.LastPage(); i++)
            pages.push(i);

        return pages;
    };

    self.generateMaxPage = function () {
        var current = self.CurrentPage();
        var pageCount = self.PageCount();
        var first = self.FirstPage;

        var upperLimit = current + parseInt((self.maxPageCount() - 1) / 2);
        var downLimit = current - parseInt((self.maxPageCount() - 1) / 2);

        while (upperLimit > pageCount) {
            upperLimit--;
            if (downLimit > first)
                downLimit--;
        }

        while (downLimit < first) {
            downLimit++;
            if (upperLimit < pageCount)
                upperLimit++;
        }

        var pages = [];
        for (var i = downLimit; i <= upperLimit; i++) {
            pages.push(i);
        }
        return pages;
    };

    self.GetPages = ko.computed(function () {
        self.CurrentPage();
        self.TotalCount();

        if (self.PageCount() <= self.maxPageCount()) {
            return ko.observableArray(self.generateAllPages());
        } else {
            return ko.observableArray(self.generateMaxPage());
        }
    });

    self.Update = function (e) {
        self.TotalCount(e.TotalCount);
        self.PageSize(e.PageSize);
        self.SetCurrentPage(e.CurrentPage);
    };

    self.GoToPage = function (page) {
        if (page >= self.FirstPage && page <= self.LastPage())
            self.SetCurrentPage(page);
    }

    self.GoToFirst = function () {
        self.SetCurrentPage(self.FirstPage);
    };

    self.GoToPrevious = function () {
        var previous = self.PreviousPage();
        if (previous != null)
            self.SetCurrentPage(previous);
    };

    self.GoToNext = function () {
        var next = self.NextPage();
        if (next != null)
            self.SetCurrentPage(next);
    };

    self.GoToLast = function () {
        self.SetCurrentPage(self.LastPage());
    };
}