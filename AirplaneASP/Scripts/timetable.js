
ko.bindingHandlers.dialog = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var options = ko.utils.unwrapObservable(valueAccessor()) || {};
        //do in a setTimeout, so the applyBindings doesn't bind twice from element being copied and moved to bottom
        setTimeout(function () {
            options.close = function () {
                allBindingsAccessor().dialogVisible(false);
            };

            $(element).dialog(options);
        }, 0);

        //handle disposal (not strictly necessary in this scenario)
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).dialog("destroy");
        });
    },
    update: function (element, valueAccessor, allBindingsAccessor) {
        var shouldBeOpen = ko.utils.unwrapObservable(allBindingsAccessor().dialogVisible),
            $el = $(element),
            dialog = $el.data("uiDialog") || $el.data("dialog");

        //don't call open/close before initilization
        if (dialog) {
            $el.dialog(shouldBeOpen ? "open" : "close");
        }
    }
};

var webApiUrl = "http://localhost:4993/api";

function City(data) {
    self = this;
    self.id = data.ID;
    self.countryID = data.CountryID;
    self.name = data.Name;
    self.isChecked = ko.observable(false);
};

function Schedule(data) {
    self = this;
    self.id = data.ID;
    self.flightID = data.FlightID;
    self.flightStateID = data.FlightStateID;
    self.departureDT = data.DepartureDT;
    self.arrivalDT = data.ArrivalDT;
    self.comment = data.Comment;
    self.cityDeparture = data.CityDeparture;
    self.countryDeparture = data.CountryDeparture;
    self.cityArrival = data.CityArrival;
    self.countryArrival = data.CountryArrival;
    self.company = data.Company;
    self.from = function () {
        return self.cityDeparture + " (" + self.countryDeparture + ")";
    };
    self.to = function () {
        return self.cityArrival + " (" + self.countryArrival + ")";
    };
};

function CityListModel() {
    var self = this;
    self.startDate = ko.observable(moment(Date.now()).format("L"));
    self.endDate = ko.observable(moment(Date.now()).add(24, 'hours').format("L"));
    self.cities = ko.observableArray([]);
    self.chosenCities = function () {
        return ko.utils.arrayFilter(self.cities(), function (city) {
            return city.isChecked();
        })
    };
    self.schedules = ko.observableArray([]);
    self.chosenSchedules = function () {
        return ko.utils.arrayFilter(self.schedules(), function (schedule) {
            return ko.utils.arrayFirst(self.chosenCities(), function (chosenCity) {
                return (chosenCity.name == schedule.cityArrival) || (chosenCity.name == schedule.cityDeparture);
            });
        });
    };
    self.loadCities = function () {
        $.getJSON(webApiUrl + "/City/CityDTOs", function (data) {
            var mappedCities = $.map(data, function (item) {
                var city = new City(item);
                if (self.chosenCities().length > 0) {
                    var match = ko.utils.arrayFirst(self.chosenCities(), function (oldItem) {
                        return oldItem.id == item.ID;
                    });
                    if (match) {
                        city.isChecked = ko.observable(true);
                    }
                }
                return city;
            });
            self.cities(mappedCities);
        });
    };
    self.loadSchedules = function () {
        var startDateString = moment(self.startDate()).toJSON();
        var endDateString = moment(self.endDate()).toJSON();
        var selectedCityIDsString = self.chosenCities().map(function (city) { return city.id; });
        $.getJSON(webApiUrl +
                    "/Schedule/schedules", {
                        startdate: startDateString,
                        enddate: endDateString,
                        guid: selectedCityIDsString
                    },
                    function (data) {
                        var mappedCities = $.map(data, function (item) {
                            return new Schedule(item);
                        });
                        self.schedules(mappedCities);
                    });
    };
    self.isOpen = ko.observable(false);
    self.open = function () {
        self.loadCities();
        self.isOpen(true);
    };
    self.save = function () {
        self.loadSchedules();
        self.isOpen(false);
    };
}

ko.applyBindings(new CityListModel());
