using System.Collections;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
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

driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

List<IWebElement> carPrices = driver.FindElements(By.CssSelector(".x3ct3a4 > a > div > div.x9f619 > div:nth-of-type(1) > span > div > span")).ToList();
List<IWebElement> carNames = driver.FindElements(By.CssSelector(".x3ct3a4 > a > div > div.x9f619 > div:nth-of-type(2) > span > div > span > span")).ToList();
List<IWebElement> carLocations = driver.FindElements(By.CssSelector(".x3ct3a4 > a > div > div.x9f619 > div:nth-of-type(3) > span > div > span > span")).ToList();
List<IWebElement> carKms = driver.FindElements(By.CssSelector(".x3ct3a4 > a > div > div.x9f619 > div > div > span > span")).ToList();

List<Car> foundCars = new List<Car>();

for(int i = 0; i < 15; i++){
    Car newCar = new Car();
    
    newCar.price = carPrices[i].Text;
    newCar.name = carNames[i].Text;
    newCar.location = carLocations[i].Text;
    newCar.kilometers = carKms[i].Text;

    foundCars.Add(newCar);
}

foreach (Car cur in foundCars){
    Console.WriteLine("A Found Car: ");
    Console.WriteLine("Name: "+cur.name);
    Console.WriteLine("Price: "+cur.price);
    Console.WriteLine("Location: "+cur.location);
    Console.WriteLine("Kilometers: "+cur.kilometers);
    Console.WriteLine();
}
driver.Quit();