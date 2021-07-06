using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System;
using OpenQA.Selenium.Firefox;

namespace PS5_Finder
{
    public class Autobuyer
    {
        public bool autobuyAmazonDE(string homeURL, string username, string password, int piepen)
        {
            IWebDriver driver;

            for (int i = 0; i < piepen; i++)
            {
                Console.Beep();
            }

            bool ex = false;

            driver = new FirefoxDriver();
            //driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(homeURL);
            WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
            //wait.Until(driver => driver.FindElement(By.XPath("//a[@href='/images']")));
            //IWebElement element = driver.FindElement(By.XPath("//a[@href='/images']"));
            IWebElement element;

            // ACCEPT COOKIES
            try
            {
                Thread.Sleep(2000);
                ex = false;
                driver.FindElement(By.Id("sp-cc-accept"));
            }
            catch (Exception) { ex = true; }
            if (!ex)
            {
                wait.Until(driver => driver.FindElement(By.Id("sp-cc-accept")));
                element = driver.FindElement(By.Id("sp-cc-accept"));
                try
                {
                    element.Click();
                }
                catch (Exception) { ex = true; }
            }

            // USE BUY NOW BUTTON IF POSSIBLE
            bool usingBuyNow = false;
            try
            {
                ex = false;
                driver.FindElement(By.Id("buy-now-button"));
            }
            catch (Exception) { ex = true; }
            if (!ex)
            {
                wait.Until(driver => driver.FindElement(By.Id("buy-now-button")));
                element = driver.FindElement(By.Id("buy-now-button"));
                try
                {
                    element.Click();
                    usingBuyNow = true;
                }
                catch (Exception) { ex = true; }
            }
            if (usingBuyNow)
            {
                try
                {
                    ex = false;
                    driver.FindElement(By.Id("turbo-checkout-pyo-button"));
                }
                catch (Exception) { ex = true; }
                if (!ex)
                {
                    wait.Until(driver => driver.FindElement(By.Id("turbo-checkout-pyo-button")));
                    element = driver.FindElement(By.Id("turbo-checkout-pyo-button"));
                    try
                    {
                        element.Click();
                    }
                    catch (Exception) { ex = true; }
                }
            }

            // ADD TO CART
            try
            {
                ex = false;
                driver.FindElement(By.Id("add-to-cart-button"));
            }
            catch (Exception) { ex = true; }
            if (!ex)
            {
                wait.Until(driver => driver.FindElement(By.Id("add-to-cart-button")));
                element = driver.FindElement(By.Id("add-to-cart-button"));
                try
                {
                    element.Click();
                }
                catch (Exception) { ex = true; }
            }

            // DISMIS VoRGESCHLAGENE GERAETE
            //Thread.Sleep(2000);
            //try
            //{
            //    ex = false;
            //    driver.FindElement(By.ClassName("a-button-input"));
            //}
            //catch (Exception) { ex = true; }
            //if (!ex)
            //{
            //    wait.Until(driver => driver.FindElement(By.ClassName("a-button-input")));
            //    element = driver.FindElement(By.ClassName("a-button-input"));
            //    element.SendKeys(Keys.Enter);
            //}
            //Thread.Sleep(1000);

            // GO TO BASKET
            try
            {
                ex = false;
                driver.FindElement(By.Id("hlb-ptc-btn-native"));
            }
            catch (Exception) { ex = true; }
            if (!ex)
            {
                wait.Until(driver => driver.FindElement(By.Id("hlb-ptc-btn-native")));
                element = driver.FindElement(By.Id("hlb-ptc-btn-native"));
                try
                {
                    element.Click();
                }
                catch (Exception) { ex = true; }
            }

            Thread.Sleep(2000);

            // ACCEPT ATTACH SIDESHEET
            try
            {
                ex = false;
                driver.FindElement(By.Id("attach-sidesheet-checkout-button"));
            }
            catch (Exception) { ex = true; }
            if (!ex)
            {
                wait.Until(driver => driver.FindElement(By.Id("attach-sidesheet-checkout-button")));
                element = driver.FindElement(By.Id("attach-sidesheet-checkout-button"));
                try
                {
                    element.Click();
                }
                catch (Exception) { ex = true; }
            }

            // ACCEPT BASKET
            try
            {
                ex = false;
                driver.FindElement(By.Id("a-button-primary"));
            }
            catch (Exception) { ex = true; }
            if (!ex)
            {
                wait.Until(driver => driver.FindElement(By.Id("a-button-primary")));
                element = driver.FindElement(By.Id("a-button-primary"));
                try
                {
                    element.Click();
                }
                catch (Exception) { ex = true; }
            }

            // LOGIN USERNAME

            wait.Until(driver => driver.FindElement(By.Id("ap_email")));
            element = driver.FindElement(By.Id("ap_email"));
            element.SendKeys(username);
            wait.Until(driver => driver.FindElement(By.Id("continue")));
            element = driver.FindElement(By.Id("continue"));
            try
            {
                element.Click();
            }
            catch (Exception) { ex = true; }


            // LOGIN PASSWORD

            wait.Until(driver => driver.FindElement(By.Id("ap_password")));
            element = driver.FindElement(By.Id("ap_password"));
            element.SendKeys(password);
            wait.Until(driver => driver.FindElement(By.Id("signInSubmit")));
            element = driver.FindElement(By.Id("signInSubmit"));
            try
            {
                element.Click();
            }
            catch (Exception) { ex = true; }

            Thread.Sleep(2000);

            // ACCEPT SHIPPING ADDRESS
            try
            {
                ex = false;
                driver.FindElement(By.XPath("//*[@id='address-book-entry-0']/div[2]/span/a"));
            }
            catch (Exception) { ex = true; }
            if (!ex)
            {
                wait.Until(driver => driver.FindElement(By.XPath("//*[@id='address-book-entry-0']/div[2]/span/a")));
                element = driver.FindElement(By.XPath("//*[@id='address-book-entry-0']/div[2]/span/a"));
                try
                {
                    element.Click();
                }
                catch (Exception) { ex = true; }
            }

            // ACCEPT SHIPPING METHOD
            try
            {
                ex = false;
                driver.FindElement(By.ClassName("a-button-text"));
            }
            catch (Exception) { ex = true; }
            if (!ex)
            {
                wait.Until(driver => driver.FindElement(By.ClassName("a-button-text")));
                element = driver.FindElement(By.ClassName("a-button-text"));
                try
                {
                    element.Click();
                }
                catch (Exception) { ex = true; }
            }

            // SELECT PAYMENT METHOD
            try
            {
                ex = false;
                driver.FindElement(By.XPath("/html/body/div[5]/div/div[2]/div[2]/div/div[2]/div/form/div/div/div/div[3]/div[2]/div/div/div/div/div/div/span/div/label/input"));
            }
            catch (Exception) { ex = true; }
            if (!ex)
            {
                wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[5]/div/div[2]/div[2]/div/div[2]/div/form/div/div/div/div[3]/div[2]/div/div/div/div/div/div/span/div/label/input")));
                element = driver.FindElement(By.XPath("/html/body/div[5]/div/div[2]/div[2]/div/div[2]/div/form/div/div/div/div[3]/div[2]/div/div/div/div/div/div/span/div/label/input"));
                try
                {
                    element.Click();
                }
                catch (Exception) { ex = true; }
            }
            try
            {
                ex = false;
                driver.FindElement(By.XPath("html/body/div[5]/div/div[2]/div[2]/div/div[2]/div/form/div/div/div/div[3]/div[2]/div/div/div/div/div/div/span/div/label/input"));
            }
            catch (Exception) { ex = true; }
            if (!ex)
            {
                wait.Until(driver => driver.FindElement(By.XPath("html/body/div[5]/div/div[2]/div[2]/div/div[2]/div/form/div/div/div/div[3]/div[2]/div/div/div/div/div/div/span/div/label/input")));
                element = driver.FindElement(By.XPath("html/body/div[5]/div/div[2]/div[2]/div/div[2]/div/form/div/div/div/div[3]/div[2]/div/div/div/div/div/div/span/div/label/input"));
                try
                {
                    element.Click();
                }
                catch (Exception) { ex = true; }
            }
            try
            {
                ex = false;
                driver.FindElement(By.ClassName("a-button-text"));
            }
            catch (Exception) { ex = true; }
            if (!ex)
            {
                wait.Until(driver => driver.FindElement(By.ClassName("a-button-text")));
                element = driver.FindElement(By.ClassName("a-button-text"));
                try
                {
                    element.Click();
                }
                catch (Exception) { ex = true; }
            }

            // CONFIRM PURCHASE
            try
            {
                ex = false;
                driver.FindElement(By.Id("submitOrderButtonId"));
            }
            catch (Exception) { ex = true; }
            if (!ex)
            {
                wait.Until(driver => driver.FindElement(By.Id("submitOrderButtonId")));
                element = driver.FindElement(By.Id("submitOrderButtonId"));
                try
                {
                    element.Click();
                }
                catch (Exception) { ex = true; }
            }

            // CHECK PURCHASE SUCCESS
            bool checkPurchase = true;
            string elementText = "";
            try
            {
                driver.FindElement(By.ClassName("a-alert-heading"));
                checkPurchase = true;
            }
            catch (Exception) { checkPurchase = false; }
            if (checkPurchase)
            {
                element = driver.FindElement(By.ClassName("a-alert-heading"));
                elementText = element.Text;
            }
            else
            {
                try
                {
                    driver.FindElement(By.ClassName("a-color-success"));
                    checkPurchase = true;
                }
                catch (Exception) { checkPurchase = false; }
                if (checkPurchase)
                {
                    element = driver.FindElement(By.ClassName("a-color-success"));
                    elementText = element.Text;
                }
            }


            // CLOSE DRIVER AND WINDOW
            driver.Close();
            driver.Quit();

            // RETURN TRUE OR FALSE
            if (elementText.Equals("Bestellungaufgegeben,danke!"))
            {
                return true;
            }
            else if (elementText.Equals("VielenDank,IhreBestellungwirdbearbeitet."))
            {
                return true;
            }
            else if (ex)
            {
                return false;
            }
            else
            {
                return false;
            }
        }

        public bool autobuyMediaMarktSaturnDE(string homeURL, string username, string password, int piepen)
        {
            IWebDriver driver;
            for (int i = 0; i < piepen; i++)
            {
                Console.Beep();
            }

            bool ex = false;

            try
            {
                driver = new ChromeDriver();
            }
            catch (Exception)
            {
                try
                {
                    var options = new EdgeOptions();
                    options.UseChromium = true;
                    options.BinaryLocation = @"C:\Program Files (x86)\Microsoft\Edge Beta\Application\msedge.exe";
                    driver = new EdgeDriver();
                }
                catch (Exception) { goto ende; }
            }

            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(homeURL);
            WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
            //wait.Until(driver => driver.FindElement(By.XPath("//a[@href='/images']")));
            //IWebElement element = driver.FindElement(By.XPath("//a[@href='/images']"));
            IWebElement element;

            // ADD TO BASKET
            try
            {
                ex = false;
                driver.FindElement(By.Id("pdp-add-to-cart"));
            }
            catch (Exception) { ex = true; }
            if (!ex)
            {
                wait.Until(driver => driver.FindElement(By.Id("pdp-add-to-cart")));
                element = driver.FindElement(By.Id("pdp-add-to-cart"));
                try
                {
                    element.Click();
                }
                catch (Exception) { ex = true; }
            }

            // GO THROU FLYOUT CART
            try
            {
                ex = false;
                driver.FindElement(By.ClassName("Buttonstyled__StyledButton-sc-140xkaw-1"));
            }
            catch (Exception) { ex = true; }
            if (!ex)
            {
                wait.Until(driver => driver.FindElement(By.ClassName("Buttonstyled__StyledButton-sc-140xkaw-1")));
                element = driver.FindElement(By.ClassName("Buttonstyled__StyledButton-sc-140xkaw-1"));
                try
                {
                    element.Click();
                }
                catch (Exception) { ex = true; }
            }

            // PROCEED TO CHECKOUT
            try
            {
                ex = false;
                driver.FindElement(By.ClassName("Buttonstyled__StyledButton-sc-140xkaw-1"));
            }
            catch (Exception) { ex = true; }
            if (!ex)
            {
                wait.Until(driver => driver.FindElement(By.ClassName("Buttonstyled__StyledButton-sc-140xkaw-1")));
                element = driver.FindElement(By.ClassName("Buttonstyled__StyledButton-sc-140xkaw-1"));
                try
                {
                    element.Click();
                }
                catch (Exception) { ex = true; }
            }

            // LOGIN
            // USERNAME
            try
            {
                ex = false;
                driver.FindElement(By.Id("mms-login-form__email"));
            }
            catch (Exception) { ex = true; }
            if (!ex)
            {
                wait.Until(driver => driver.FindElement(By.Id("mms-login-form__email")));
                element = driver.FindElement(By.Id("mms-login-form__email"));
                element.SendKeys(username);
            }
            // PASSWORD
            try
            {
                ex = false;
                driver.FindElement(By.Id("mms-login-form__password"));
            }
            catch (Exception) { ex = true; }
            if (!ex)
            {
                wait.Until(driver => driver.FindElement(By.Id("mms-login-form__password")));
                element = driver.FindElement(By.Id("mms-login-form__password"));
                element.SendKeys(password);
            }
            // CONTINUE
            try
            {
                ex = false;
                driver.FindElement(By.Id("mms-login-form__login-button"));
            }
            catch (Exception) { ex = true; }
            if (!ex)
            {
                wait.Until(driver => driver.FindElement(By.Id("mms-login-form__login-button")));
                element = driver.FindElement(By.Id("mms-login-form__login-button"));
                try
                {
                    element.Click();
                }
                catch (Exception) { ex = true; }
            }

            // AGAIN PROCEED TO CHECKOUT
            try
            {
                ex = false;
                driver.FindElement(By.ClassName("Buttonstyled__StyledButton-sc-140xkaw-1"));
            }
            catch (Exception) { ex = true; }
            if (!ex)
            {
                wait.Until(driver => driver.FindElement(By.ClassName("Buttonstyled__StyledButton-sc-140xkaw-1")));
                element = driver.FindElement(By.ClassName("Buttonstyled__StyledButton-sc-140xkaw-1"));
                try
                {
                    element.Click();
                }
                catch (Exception) { ex = true; }
            }

            // CONTINUE
            try
            {
                ex = false;
                driver.FindElement(By.ClassName("Buttonstyled__StyledButton-sc-140xkaw-1"));
            }
            catch (Exception) { ex = true; }
            if (!ex)
            {
                wait.Until(driver => driver.FindElement(By.ClassName("Buttonstyled__StyledButton-sc-140xkaw-1")));
                element = driver.FindElement(By.ClassName("Buttonstyled__StyledButton-sc-140xkaw-1"));
                try
                {
                    element.Click();
                }
                catch (Exception) { ex = true; }
            }

            // PROCEED AND PAY
            try
            {
                ex = false;
                driver.FindElement(By.ClassName("Buttonstyled__StyledButton-sc-140xkaw-1"));
            }
            catch (Exception) { ex = true; }
            if (!ex)
            {
                wait.Until(driver => driver.FindElement(By.ClassName("Buttonstyled__StyledButton-sc-140xkaw-1")));
                element = driver.FindElement(By.ClassName("Buttonstyled__StyledButton-sc-140xkaw-1"));
                try
                {
                    element.Click();
                }
                catch (Exception) { ex = true; }
            }




            // CHECK PURCHASE SUCCESS
            bool checkPurchase = true;
            string elementText = "";
            try
            {
                driver.FindElement(By.ClassName("a-alert-heading"));
                checkPurchase = true;
            }
            catch (Exception) { checkPurchase = false; }
            if (checkPurchase)
            {
                element = driver.FindElement(By.ClassName("a-alert-heading"));
                elementText = element.Text;
            }
            else
            {
                try
                {
                    driver.FindElement(By.ClassName("a-color-success"));
                    checkPurchase = true;
                }
                catch (Exception) { checkPurchase = false; }
                if (checkPurchase)
                {
                    element = driver.FindElement(By.ClassName("a-color-success"));
                    elementText = element.Text;
                }
            }

            // CLOSE DRIVER AND WINDOW
            driver.Close();
            driver.Quit();

            // RETURN TRUE OR FALSE
            if (elementText.Equals("Bestellungaufgegeben,danke!"))
            {
                return true;
            }
            else if (elementText.Equals("VielenDank,IhreBestellungwirdbearbeitet."))
            {
                return true;
            }
            else if (ex)
            {
                return false;
            }
            else
            {
                return false;
            }

        ende:
            return false;
        }
    }
}


//# ACCEPT COOKIES
//WDW(driver, 10).until(
//    EC.presence_of_element_located((By.CLASS_NAME, 'gdpr-cookie-layer__btn--submit--all'))).click()
//        # LOGIN
//        driver.execute_script("document.getElementById('pdp-add-to-cart').click()")
//        WDW(driver, 10).until(EC.presence_of_element_located((By.ID, 'basket-flyout-cart'))).click()
//        WDW(driver, 10).until(EC.presence_of_element_located((By.CLASS_NAME, 'cobutton-next'))).click()
//        WDW(driver, 10).until(EC.presence_of_element_located((By.ID, 'login-email'))).send_keys(settings.get("email"))
//        WDW(driver, 10).until(EC.presence_of_element_located((By.ID, 'loginForm-password'))).send_keys(
//            settings.get("mediamarkt_password"))
//        WDW(driver, 10).until(EC.presence_of_element_located((By.NAME, 'loginForm'))).find_element(By.CLASS_NAME,
//                                                                                                   'cobutton-next').click()

//        # CHECK CART FOR OTHER ITEMS AND DELETE THESE
//basket = WDW(driver, 10).until(EC.presence_of_all_elements_located((By.CLASS_NAME, 'cart-product-table')))
//        length_basket = len(basket)
//        if length_basket > 1:
//            while length_basket > 1:
//                basket = driver.find_elements(By.CLASS_NAME, 'cart-product-table')
//                for item in basket:
//                    try:
//                        title = item.find_element(By.CLASS_NAME, 'cproduct-heading').get_attribute('innerHTML')
//                    except(SE.NoSuchElementException, SE.StaleElementReferenceException) as e:
//                        continue
//                    if "playstation" not in str.lower(title) or "ps5" not in str.lower(title):
//                        try:
//                            options = item.find_element_by_class_name('js-cartitem-qty')
//                        except(SE.NoSuchElementException, SE.StaleElementReferenceException) as e:
//                            continue
//                        for option in options.find_elements_by_tag_name('option'):
//                            if option.text == 'Verwijder':
//                                option.click()
//                            length_basket -= 1

//        # PROCEED TO PAYMENT
//    delivery_form = WDW(driver, 10).until(EC.presence_of_element_located((By.CLASS_NAME, 'deliveryForm')))
//        delivery_form.find_element(By.CLASS_NAME, 'cobutton-next').click()
//        # PAYPAL
//        WDW(driver, 10).until(EC.presence_of_element_located((By.CLASS_NAME, 'paypal__xpay'))).click()
//        driver.execute_script("document.getElementsByClassName('cobutton-next')[1].click()")
//        WDW(driver, 10).until(EC.presence_of_element_located(
//            (By.XPATH, '/html/body/div[4]/form/checkout-footer/div/div[1]/div/div[2]/button'))).click()
//        email_input = WDW(driver, 10).until(EC.presence_of_element_located((By.ID, 'email')))
//        email_input.clear()
//        email_input.send_keys(settings.get("email"))
//        WDW(driver, 10).until(EC.presence_of_element_located((By.ID, 'password'))).send_keys(
//            settings.get("paypal_password"))
//        WDW(driver, 10).until(EC.presence_of_element_located((By.ID, 'btnLogin'))).click()
//        if in_production:
//            WDW(driver, 10).until(EC.presence_of_element_located((By.ID, 'confirmButtonTop'))).click()
//        else:
// 