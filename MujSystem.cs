using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace VozovyPark
{
    public class MujSystem
    {
        public static User currentUser = null;
        public static List<User> usersData = new List<User>();
        public static List<Rezervace> rezervaceData = new List<Rezervace>();
        public static List<Auto> autaData = new List<Auto>();
        public static List<ServisAuto> servisData = new List<ServisAuto>();
        public const string oddelovac = "-;-";
        public static int s = Console.WindowWidth;
        protected static int odhlasitVypnout = 0;

        public static void Start()
        {
            Console.SetWindowSize(150, 40);
            Console.SetBufferSize(150, 40);

            MujSystem.Nacist("uzivatele.json", "uzivatele");
            MujSystem.Nacist("rezervace.json", "rezervace");
            MujSystem.Nacist("servis.json", "servis");
            MujSystem.Nacist("auta.json", "auta");
        }

        public static void Konec()
        {
            for (int i = 0; i < usersData.Count; i++)
            {
                if (usersData[i].Username == currentUser.Username)
                    usersData[i] = currentUser;
            }
            MujSystem.Ulozit("uzivatele.json", "uzivatele");
            MujSystem.Ulozit("rezervace.json", "rezervace");
            MujSystem.Ulozit("servis.json", "servis");
            MujSystem.Ulozit("auta.json", "auta");
        }

        protected static string Enkryptor(string heslo)
        {
            byte[] hesloBytes = Encoding.UTF8.GetBytes(heslo);
            SHA256Managed sHA256 = new SHA256Managed();
            byte[] resultBytes = sHA256.ComputeHash(hesloBytes);
            string hesloHashed = "";
            foreach (byte x in resultBytes)
            {
                hesloHashed += String.Format("{0:x2}", x);
            }
            return hesloHashed;
        }

        public static void Ulozit(string cesta, string co)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = "";

            if (co == "uzivatele")
                json = JsonSerializer.Serialize(usersData, options);
            else if (co == "auta")
                json = JsonSerializer.Serialize(autaData, options);
            else if (co == "rezervace")
                json = JsonSerializer.Serialize(rezervaceData, options);
            else if (co == "servis")
                json = JsonSerializer.Serialize(servisData, options);

            File.WriteAllText(cesta, json);


            
        }

        public static void Nacist(string cesta, string co)
        {
            if (File.Exists(cesta))
            {
                string json = File.ReadAllText(cesta);
                try
                {
                    if (co == "uzivatele")
                        usersData = JsonSerializer.Deserialize<List<User>>(json);
                    else if (co == "rezervace")
                        rezervaceData = JsonSerializer.Deserialize<List<Rezervace>>(json);
                    else if (co == "auta")
                        autaData = JsonSerializer.Deserialize<List<Auto>>(json);
                    else if (co == "servis")
                        servisData = JsonSerializer.Deserialize<List<ServisAuto>>(json);
                    else
                    {
                        Console.WriteLine("Error, položka \"co\" je neplatná.");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                }
                catch (System.Text.Json.JsonException)
                {
                    Console.WriteLine("Soubor " + cesta + " není ve json formátu.");
                    Console.ReadKey();
                }
            }
            else
            {
                if (co == "uzivatele")//jestli neexistuje soubor uzivatele.json, vytvori se admin drinkwater:monke.
                {
                    usersData.Add(new User(true, "drinkwater", Enkryptor("monke"), "Drink", "Water", DateTime.Now));
                    Console.WriteLine("Není dostupný soubor v adresáři " + Directory.GetCurrentDirectory() + cesta + ". " +
                    "Program přidal do systému admina. Když to čteš tak oznam to adminovi, jestli jseš admin tak přihlašovací údaje znáš ;)");
                }
                else
                    Console.WriteLine("Není dostupný soubor v adresáři " + Directory.GetCurrentDirectory() + cesta + ". " +
                    "Program bude pracovat s prázdným seznamem položek " + co);
            }



            //StreamReader file = null;
            //try
            //{
            //    file = new StreamReader(cesta);
            //}
            //catch(System.IO.FileNotFoundException e)
            //{
            //    Console.WriteLine("Není dostupný soubor v adresáři " + Directory.GetCurrentDirectory() + cesta + ". " +
            //        "Program bude pracovat s prázdným seznamem položek " + co);
            //    Console.ReadKey();
            //    Environment.Exit(0);
            //}
            //string line;
            //while ((line = file.ReadLine()) != null)
            //{
            //    if (co == "uzivatele")
            //    {
            //        string[] arr = line.Split(oddelovac);
            //        User user = new User();

            //        if (bool.TryParse(arr[0], out bool admin))
            //            user.JeAdmin = admin;
            //        user.Username = arr[1];
            //        user.HesloHash = arr[2];
            //        user.Jmeno = arr[3];
            //        user.Prijmeni = arr[4];
            //        if (DateTime.TryParse(arr[5], out DateTime lastl))
            //            user.LastLogin = lastl;
            //        if (bool.TryParse(arr[6], out bool noveHeslo))
            //            user.ChciNoveHeslo = noveHeslo;

            //        usersData.Add(user);
            //    }
            //    else if (co == "rezervace")
            //    {
            //        string[] arr = line.Split(oddelovac);
            //        Rezervace rezervace = new Rezervace();

            //        rezervace.Uzivatel = arr[0];
            //        if (int.TryParse(arr[1], out int id))
            //        {
            //            rezervace.Id = id;
            //        }
            //        if (DateTime.TryParse(arr[2], out DateTime od))
            //        {
            //            rezervace.Od = od;
            //        }
            //        if (DateTime.TryParse(arr[3], out DateTime Do))
            //        {
            //            rezervace.Do = Do;
            //        }
            //        if (bool.TryParse(arr[0], out bool aktivni))
            //        {
            //            rezervace.Aktivni = aktivni;
            //        }

            //        rezervaceData.Add(rezervace);
            //    }
            //    else if (co == "auta")
            //    {
            //        Nacist("ServisniUkony.txt", "servis");
            //        string[] arr = line.Split(oddelovac);
            //        Auto auto = new Auto();

            //        if (int.TryParse(arr[0], out int id))
            //            auto.Id = id;
            //        auto.Znacka = arr[1];
            //        auto.Model = arr[2];
            //        if (bool.TryParse(arr[3], out bool jeosobni))
            //            auto.JeOsobni = jeosobni;
            //        if (float.TryParse(arr[4], out float spotreba))
            //            auto.Spotreba = spotreba;

            //        autaData.Add(auto);
            //    }
            //    else if (co == "servis")
            //    {
            //        string[] arr = line.Split(oddelovac);
            //        ServisAuto servis = new ServisAuto();

            //        if (int.TryParse(arr[0], out int idUkonu))
            //            servis.Id = idUkonu;
            //        if (int.TryParse(arr[1], out int idAuta))
            //            servis.IdAuto = idAuta;
            //        if (DateTime.TryParse(arr[2], out DateTime kdy))
            //            servis.Kdy = kdy;
            //        servis.UkonPopis = arr[3];
            //        if (decimal.TryParse(arr[4], out decimal cena))
            //            servis.CenaUkonu = cena;
            //        if (int.TryParse(arr[5], out int faktura))
            //            servis.Faktura = faktura;

            //        servisData.Add(servis);
            //    }
            //}

            //file.Close();
        }

        protected static string RandomString(int delka)
        {
            var znaky = "0123456789abcdefghijklmnopqrstuvwxyz";
            var stringZnaky = new char[delka];
            var random = new Random();

            for (int i = 0; i < stringZnaky.Length; i++)
            {
                stringZnaky[i] = znaky[random.Next(znaky.Length)];
            }

            return new String(stringZnaky);
        }

        protected static int RandomInt(int delka)
        {
            var znaky = "0123456789";
            var stringZnaky = new char[delka];
            var random = new Random();

            for (int i = 0; i < stringZnaky.Length; i++)
            {
                stringZnaky[i] = znaky[random.Next(znaky.Length)];
            }

            return int.Parse(stringZnaky);
        }

        protected static bool ExistujeUzivatel(string username)
        {
            foreach (User user in usersData)
            {
                if (username == user.Username)
                    return true;
            }
            return false;
        }

        public static void Prihlaseni()
        {
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                currentUser = new User();
                bool ukoncit = false;

                VypisCenterText("------------------------------------------------------------------------------------------------------------------------------------------------", s);
                VypisCenterText("RezervujSiAutoATak    DrinkWater Inc.", s);
                VypisCenterText("------------------------------------------------------------------------------------------------------------------------------------------------", s);
                VypisCenterText("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~  AUTORIZACE  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~", s);
                Console.WriteLine("\n\n");
                VypisCenterText("Zadejte prosím přihlašovací údaje", s);
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("Username: ");
                currentUser.Username = Console.ReadLine();
                Console.Write("Heslo: ");
                currentUser.HesloHash = Enkryptor(Console.ReadLine());

                foreach (User user in usersData)
                {
                    if (user.Username == currentUser.Username && user.HesloHash == currentUser.HesloHash)
                    {
                        currentUser = user;
                        ukoncit = true;
                        Console.Write("Úspěšně přihlášen.");
                        Console.ReadKey();
                    }
                }
                if (ukoncit && !VyzadatZmenu())
                {
                    ukoncit = false;
                }

                if (ukoncit == true)
                    break;

                if (currentUser.ChciNoveHeslo)
                    Console.WriteLine("Povinná změna hesla nebyla provedena. Zkuste se přihlásit znovu.");
                else
                    Console.Write("Username nebo heslo neodpovídá zápisům v naší databázi.");
                currentUser = null;
                Console.ReadKey();
                Console.WriteLine();
            } while (true);
            currentUser.LastLogin = DateTime.Now;
            Konec();
        }

        public static void VypisCenterText(string text, int sirka)
        {
            int posunuti = (sirka / 2) + text.Length / 2;
            Console.WriteLine(String.Format("{0," + posunuti + "}", text));
        }

        public static string CenterText(string text, int sirka)
        {
            int posunuti = (sirka / 2) + text.Length / 2;
            return String.Format("{0," + posunuti + "}", text);
        }

        protected static string AskUser(string otazka, string moznaOdpoved1, string moznaOdpoved2)
        {
            string odpoved = "";
            do
            {
                Console.WriteLine(otazka);
                odpoved = Console.ReadLine();

                if (odpoved == "!z" || odpoved == moznaOdpoved1 || odpoved == moznaOdpoved2)
                    return odpoved;
                else
                {
                    Console.WriteLine("~~ Neodpověděli jste " + moznaOdpoved1 + " ani " + moznaOdpoved2 + ", zkuste to znovu. Tento proces můžete kdykoliv zrušit příkazem !z ~~");
                    Console.ReadKey();
                }


            } while (true);
        }

        public static void HlavniStranka()
        {
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                VypisCenterText("------------------------------------------------------------------------------------------------------------------------------------------------", s);
                VypisCenterText("RezervujSiAutoATak    DrinkWater Inc.", s);
                VypisCenterText("------------------------------------------------------------------------------------------------------------------------------------------------", s);
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.DarkGreen;

                VypisCenterText("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -", s);
                Console.WriteLine("Uživatel:             " + currentUser.Username);
                Console.WriteLine("Jméno a příjmení:     " + currentUser.Jmeno + " " + currentUser.Prijmeni);
                Console.WriteLine("Poslední přihlášení:  " + currentUser.LastLogin);
                VypisCenterText("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -", s);
                

                Console.WriteLine("________________________________________");
                Console.WriteLine("[1] --- Zobrazit rezervace             |");
                Console.WriteLine("[2] --- Vložit novou rezervaci         |");
                Console.WriteLine("[3] --- Zrušit rezervaci               |");
                Console.WriteLine("[4] --- Změnit heslo                   |");
                Console.WriteLine("[5] --- Odhlásit se                    |");
                if(currentUser.JeAdmin)
                {
                    Console.WriteLine("[6] --- Přidat nového uživatele        |");
                    Console.WriteLine("[7] --- Smazat uživatele               |");
                    Console.WriteLine("[8] --- Přidat nové auto               |");
                    Console.WriteLine("[9] --- Smazat auto                    |");
                    Console.WriteLine("[10] -- Zobrazit uživatele             |");
                    Console.WriteLine("[11] -- Zobrazit smazané uživatele     |");
                    Console.WriteLine("[12] -- Zobrazit rezervace auta        |");
                    Console.WriteLine("[13] -- Zobrazit neplatné rezervace    |");
                    Console.WriteLine("[14] -- Přidat vyžádání změny hesla    |");
                    Console.WriteLine("[15] -- Přidat servisní úkon auta      |");
                    Console.WriteLine("[16] -- Ukázat servisní knížku auta    |");
                }
                Console.WriteLine("[99] -- Ukončit program                |");
                Console.WriteLine("¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯");


                string username = "";
                string odpoved = "";
                if (int.TryParse(Console.ReadLine(), out int prikaz))
                {
                    switch (prikaz)
                    {
                        case 1:
                            if (currentUser.JeAdmin == true)
                            {
                                odpoved = AskUser("Chcete vidět rezervace jiného uživatele? (a/n):", "a", "n");
                                if (odpoved == "a")
                                {
                                    Console.WriteLine("Napište jméno uživatele: ");
                                    username = Console.ReadLine();
                                    if (ExistujeUzivatel(username))
                                    {
                                        UzivatelPrikazy.ZobrazRezervace(username);
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Uživatel neexistuje.");
                                        Console.ReadKey();
                                        break;
                                    }
                                }
                                else
                                {
                                    UzivatelPrikazy.ZobrazRezervace(currentUser.Username);
                                    break;
                                }
                            }

                            if (currentUser.JeAdmin == false)
                            {
                                UzivatelPrikazy.ZobrazRezervace(currentUser.Username);
                                break;
                            }
                            break;
                        case 2:
                            if (currentUser.JeAdmin == true)
                            {
                                odpoved = AskUser("Chcete vložit rezervaci pro jiného uživatele? (a/n):", "a", "n");
                                if (odpoved == "a")
                                {
                                    Console.WriteLine("Napište jméno uživatele: ");
                                    username = Console.ReadLine();
                                    if (ExistujeUzivatel(username))
                                    {
                                        UzivatelPrikazy.VlozeniRezervace(username);
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Uživatel neexistuje.");
                                        Console.ReadKey();
                                        break;
                                    }
                                }
                                else
                                {
                                    UzivatelPrikazy.VlozeniRezervace(currentUser.Username);
                                    break;
                                }
                            }

                            if (currentUser.JeAdmin == false)
                            {
                                UzivatelPrikazy.VlozeniRezervace(currentUser.Username);
                                break;
                            }
                            break;
                        case 3:
                            if (currentUser.JeAdmin == true)
                            {
                                odpoved = AskUser("Chcete zrušit rezervaci pro jiného uživatele? (a/n):", "a", "n");
                                if (odpoved == "a")
                                {
                                    Console.WriteLine("Napište jméno uživatele: ");
                                    username = Console.ReadLine();
                                    if (ExistujeUzivatel(username))
                                    {
                                        UzivatelPrikazy.ZrusitRezervaci(username);
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Uživatel neexistuje.");
                                        Console.ReadKey();
                                        break;
                                    }
                                }
                                else
                                {
                                    UzivatelPrikazy.ZrusitRezervaci(currentUser.Username);
                                    break;
                                }
                            }

                            if (currentUser.JeAdmin == false)
                            {
                                UzivatelPrikazy.ZrusitRezervaci(currentUser.Username);
                                break;
                            }
                            break;
                        case 4:
                            if (currentUser.JeAdmin == true)
                            {
                                odpoved = AskUser("Chcete změnit heslo pro jiného uživatele? (a/n):", "a", "n");
                                if (odpoved == "a")
                                {
                                    Console.WriteLine("Napište jméno uživatele: ");
                                    username = Console.ReadLine();
                                    if (ExistujeUzivatel(username))
                                    {
                                        UzivatelPrikazy.ZmenitHeslo(username);
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Uživatel neexistuje.");
                                        Console.ReadKey();
                                        break;
                                    }
                                }
                                else
                                {
                                    UzivatelPrikazy.ZmenitHeslo(currentUser.Username);
                                    break;
                                }
                            }

                            if (currentUser.JeAdmin == false)
                            {
                                UzivatelPrikazy.ZmenitHeslo(currentUser.Username);
                                break;
                            }
                            break;
                        case 5:
                            UzivatelPrikazy.OdhlasitSe();
                            break;
                        case 6:
                            if (currentUser.JeAdmin == true)
                                AdminPrikazy.PridatUzivatele();
                            break;
                        case 7:
                            if (currentUser.JeAdmin == true)
                                AdminPrikazy.SmazatUzivatele();
                            break;
                        case 8:
                            if (currentUser.JeAdmin == true)
                                AdminPrikazy.PridatAuto();
                            break;
                        case 9:
                            if (currentUser.JeAdmin == true)
                                AdminPrikazy.SmazatAuto();
                            break;
                        case 10:
                            if (currentUser.JeAdmin == true)
                                AdminPrikazy.ZobrazitUzivatele();
                            break;
                        case 11:
                            if (currentUser.JeAdmin == true)
                                AdminPrikazy.ZobrazitSmazaneUzivatele();
                            break;
                        case 12:
                            UzivatelPrikazy.ZobrazRezervaceAuta();
                            break;
                        case 13:
                            if (currentUser.JeAdmin == true)
                                AdminPrikazy.ZobrazNeaktivniRezervace();
                            break;
                        case 14:
                            if (currentUser.JeAdmin == true)
                                AdminPrikazy.PridatVyzadaniZmeny();
                            break;
                        case 15:
                            if (currentUser.JeAdmin == true)
                                AdminPrikazy.PridatServis();
                            break;
                        case 16:
                            if (currentUser.JeAdmin == true)
                                AdminPrikazy.VypsatServisAuta();
                            break;
                        case 99:
                            odhlasitVypnout = 2;
                            break;
                        default:
                            break;
                    }
                    if (odhlasitVypnout == 1)
                    {
                        odhlasitVypnout = 0;
                        break;
                    } 
                    else if(odhlasitVypnout == 2)
                    {
                        odhlasitVypnout = 0;
                        Konec();
                        Console.WriteLine("Hezký zbytek dne.");
                        System.Threading.Thread.Sleep(2000);
                        Environment.Exit(0);
                    }
                    Konec();
                }

            } while (true);
        }

        public static void VypisDostupnaAuta(List<Auto> dostupnaAuta)
        {
            
            if(dostupnaAuta.Count < 1)
            {
                Console.WriteLine("V tomto termínu není dostupné žádné auto.");
                Console.ReadKey();
                return;
            }    
            int sirka = 20;

            Console.WriteLine(CenterText("Id", sirka) + CenterText("Znacka", sirka) + CenterText("Model", sirka) + CenterText("Osobní/jiné", sirka));
            foreach (Auto auto in dostupnaAuta)
            {
                Console.Write(CenterText(auto.Id.ToString(), sirka) + CenterText(auto.Znacka, sirka) + CenterText(auto.Model, sirka));
                if (auto.JeOsobni == true)
                    Console.WriteLine(CenterText("osobní", sirka));
                else
                    Console.WriteLine(CenterText("jiné", sirka));

            }
        }

        public static List<Auto> DostupnaAuta(DateTime od, DateTime doo)
        {
            bool jeAutoDostupne = false;
            List<Auto> dostupnaAuta = new List<Auto>();

            foreach (Auto auto in autaData)
            {
                jeAutoDostupne = true;
                foreach (Rezervace rezervace in rezervaceData)
                {
                    if (rezervace.IdAuta == auto.Id && rezervace.Aktivni == true)
                    {
                        if (rezervace.Od.Ticks > od.Ticks && rezervace.Od.Ticks < doo.Ticks)
                            jeAutoDostupne = false;
                        else if (rezervace.Do.Ticks >= od.Ticks && rezervace.Do.Ticks <= doo.Ticks) // od je v terminu od-do jiné rezervace, do je po terminu od-do jiné rezervace
                            jeAutoDostupne = false;
                        else if (rezervace.Od.Ticks <= od.Ticks && rezervace.Do.Ticks >= od.Ticks) //od je v terminu od-do jiné rezervace
                            jeAutoDostupne = false;
                        else if (rezervace.Od.Ticks <= doo.Ticks && rezervace.Do.Ticks >= doo.Ticks) // do je v termínu od-do jiné rezervace
                            jeAutoDostupne = false;
                    }
                }

                if (jeAutoDostupne == true)
                {
                    dostupnaAuta.Add(auto);
                }
            }
            return dostupnaAuta;
        }
        protected static bool VyzadatZmenu()
        {
            if (currentUser.ChciNoveHeslo == true)
            {
                string odpoved;
                Console.WriteLine("Vaše heslo je zastaralé, musíme ho změnit. Napište prosím své nové heslo (!z pro vrácení se k příhlašování): ");
                do
                {
                    odpoved = Console.ReadLine();
                    if (odpoved == "!z")
                        return false;
                    else if (odpoved != "" && Enkryptor(odpoved) != currentUser.HesloHash)
                    {
                        currentUser.HesloHash = Enkryptor(odpoved);
                        currentUser.ChciNoveHeslo = false;
                        for (int i = 0; i < usersData.Count; i++)
                        {
                            if (usersData[i].Username == currentUser.Username)
                            {
                                usersData[i] = currentUser;
                                Console.WriteLine("Heslo úspěšně změněno.");
                                Console.ReadKey();
                                return true;
                            }
                        }
                    }
                    else
                        Console.WriteLine("Nové heslo se musí lišit od starého hesla. Zkuste znovu nebo napište !z pro vrácení se na přihlašování.");
                } while (true);
            }
            else
                return true;

        }
    }
}
