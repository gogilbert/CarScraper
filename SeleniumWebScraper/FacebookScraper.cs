using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Cookie = OpenQA.Selenium.Cookie;

IWebDriver driver;
driver = new FirefoxDriver();

string filename = "cookie.json";
var cookies = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(filename));

driver.Navigate().GoToUrl("https://www.facebook.com/marketplace/108043585884666/vehicles?maxPrice=10000&maxMileage=150000&minYear=2000&sortBy=creation_time_descend&topLevelVehicleType=car_truck&transmissionType=manual&exact=false");

foreach (var c in cookies) {
    DateTime? expiry = null;
    if (c.expiry != null) {
        var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds((long) c.expiry);
        expiry = dateTimeOffset.DateTime;
        Console.WriteLine(expiry);
    }
    string name = c.name;
    string value = c.value;
    string domain = c.domain;
    string path = c.path;
    bool secure = (bool) c.secure;
    bool isHttpOnly = (bool) c.httpOnly;
    string sameSite = c.sameSite;    

    var newCookie = new Cookie(name, value, domain, path, expiry, secure, isHttpOnly, sameSite);
    driver.Manage().Cookies.AddCookie(newCookie);
}

driver.Navigate().Refresh();