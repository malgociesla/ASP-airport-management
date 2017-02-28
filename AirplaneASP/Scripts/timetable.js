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
    self.departureDT = formatDateTime(data.DepartureDT);
    self.arrivalDT = formatDateTime(data.ArrivalDT);
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
    self.startDate = ko.observable(getToday());
    self.endDate = ko.observable(getTomorrow(getToday()));
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
        var startDateString = toJSONDate(self.startDate());
        var endDateString = toJSONDate(self.endDate());
        var selectedCityIDsString = self.chosenCities().map(function (city) {
            return city.id;
        });
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
