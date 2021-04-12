using System;
using System.Collections.Generic;
using System.Text;

namespace VozovyPark
{
    class UzivatelPrikazy : MujSystem
    {
        public static void VlozeniRezervace(string username)
        {
            string odpoved = "";
            bool dale = false;
            User proKoho = new User(username, "s");
            Rezervace rezervace = new Rezervace();

            dale = false;
            do
            {
                Console.Write("Rezervovat auto od (např. 08.04.2021 21:34): ");
                odpoved = Console.ReadLine();
                if (DateTime.TryParse(odpoved, out DateTime od) && DateTime.Compare(od, DateTime.Now) > 0)
                {
                    dale = true;
                    rezervace.Od = od;
                }
                else if (odpoved == "!z")
                    return;
                else
                    Console.WriteLine("~~ Neodpověděli jste správným způsobem, zkuste to znovu.  Přidávání rezervace můžete kdykoliv zrušit příkazem !z ~~");
            } while (dale == false);

            dale = false;
            do
            {
                Console.Write("Rezervovat auto do (např. 09.04.2021 21:34): ");
                odpoved = Console.ReadLine();
                if (DateTime.TryParse(odpoved, out DateTime doo) && DateTime.Compare(doo, DateTime.Now) > 0)
                {
                    dale = true;
                    rezervace.Do = doo;
                }
                else if (odpoved == "!z")
                    return;
                else
                    Console.WriteLine("~~ Neodpověděli jste správným způsobem, zkuste to znovu.  Přidávání rezervace můžete kdykoliv zrušit příkazem !z ~~");
            } while (dale == false);

            dale = false;
            do
            {
                MujSystem.VypisDostupnaAuta(DostupnaAuta(rezervace.Od, rezervace.Do));
                if (DostupnaAuta(rezervace.Od, rezervace.Do).Count > 0)
                    Console.WriteLine("Napiste id auta ktere si chcete pronajmout: ");
                else
                    return;
                List<Auto> dostupnaAuta = DostupnaAuta(rezervace.Od, rezervace.Do);
                odpoved = Console.ReadLine();

                foreach (Auto auto in dostupnaAuta)
                {
                    if (auto.Id.ToString() == odpoved)
                    {
                        dale = true;
                        rezervace.IdAuta = int.Parse(odpoved);
                    }
                }
                if (odpoved == "!z")
                    return;
                else if (dale == false)
                    Console.WriteLine("~~ Neodpověděli jste správným id, zkuste to znovu.  Přidávání rezervace můžete kdykoliv zrušit příkazem !z ~~");
            } while (dale == false);

            rezervace.Uzivatel = proKoho.Username;
            rezervace.Aktivni = true;

            rezervace.Id = RandomInt(4);


            MujSystem.rezervaceData.Add(rezervace);

            Console.WriteLine("Rezervace úspěšně přidána.");
            Console.ReadKey();
        }
        public static void ZobrazRezervace(string username)
        {
            bool maRezervaci = false;
            int sirka = 40;
            Console.WriteLine();

            if(currentUser.JeAdmin)
            {
                foreach (Rezervace rezervace in rezervaceData)
                {
                    if (rezervace.Uzivatel == username)
                    {
                        if (!maRezervaci)
                            Console.WriteLine(CenterText("Id", sirka) + CenterText("IdAuta", sirka) + CenterText("Od", sirka) + CenterText("Do", sirka));

                        Console.WriteLine(CenterText(rezervace.Id.ToString(), sirka) + CenterText(rezervace.IdAuta.ToString(), sirka) +
                            CenterText(rezervace.Od.ToString(), sirka) + CenterText(rezervace.Do.ToString(), sirka));
                        maRezervaci = true;
                    }
                }
                if (!maRezervaci)
                    Console.WriteLine("Uživatel nemá žádné rezervace. ");
            }
            else
            {
                foreach (Rezervace rezervace in rezervaceData)
                {
                    if (rezervace.Uzivatel == username && rezervace.Aktivni)
                    {
                        if (!maRezervaci)
                            Console.WriteLine(CenterText("Id", sirka) + CenterText("IdAuta", sirka) + CenterText("Od", sirka) + CenterText("Do", sirka));

                        Console.WriteLine(CenterText(rezervace.Id.ToString(), sirka) + CenterText(rezervace.IdAuta.ToString(), sirka) +
                            CenterText(rezervace.Od.ToString(), sirka) + CenterText(rezervace.Do.ToString(), sirka));
                        maRezervaci = true;
                    }
                }
                if (!maRezervaci)
                    Console.WriteLine("Nemáte žádné aktivní rezervace.");
            }
                

            Console.ReadKey();

        }

        public static void ZobrazRezervaceAuta()
        {
            bool maRezervaci = false;
            int sirka = 40;

            VypisDostupnaAuta(autaData);
            Console.WriteLine();
            Console.WriteLine("Napište id auta jehož rezervace chcete vidět: ");
            string odpoved = Console.ReadLine();

            if (currentUser.JeAdmin)
            {
                foreach (Rezervace rezervace in rezervaceData)
                {
                    if (rezervace.IdAuta.ToString() == odpoved)
                    {
                        if (!maRezervaci)
                            Console.WriteLine(CenterText("Id", 10) + CenterText("Id Auta", sirka) + CenterText("Uzivatel", sirka) + CenterText("Od", sirka) + CenterText("Do", sirka));

                        Console.WriteLine(CenterText(rezervace.Id.ToString(), 10) + CenterText(rezervace.IdAuta.ToString(), sirka) + CenterText(rezervace.Uzivatel, sirka) +
                            CenterText(rezervace.Od.ToString(), sirka) + CenterText(rezervace.Do.ToString(), sirka));
                        maRezervaci = true;
                    }
                }
                if (!maRezervaci)
                    Console.WriteLine("Auto nemá žádné rezervace. ");
            }
            else
            {
                foreach (Rezervace rezervace in rezervaceData)
                {
                    if (rezervace.IdAuta.ToString() == odpoved)
                    {
                        if (!maRezervaci)
                            Console.WriteLine(CenterText("Id", sirka) + CenterText("Id Auta", sirka) + CenterText("Od", sirka) + CenterText("Do", sirka));

                        Console.WriteLine(CenterText(rezervace.Id.ToString(), sirka) + CenterText(rezervace.IdAuta.ToString(), sirka) +
                            CenterText(rezervace.Od.ToString(), sirka) + CenterText(rezervace.Do.ToString(), sirka));
                        maRezervaci = true;
                    }
                }
                if (!maRezervaci)
                    Console.WriteLine("Auto nemá žádné rezervace. ");
            }


            Console.ReadKey();
        }
        public static void ZrusitRezervaci(string username)
        {
            bool maRezervaci = false;
            int sirka = 40;
            foreach (Rezervace rezervace in rezervaceData)
            {
                if (rezervace.Uzivatel == username)
                {
                    if (!maRezervaci)
                        Console.WriteLine(CenterText("Id", sirka) + CenterText("IdAuta", sirka) + CenterText("Od", sirka) + CenterText("Do", sirka));

                    Console.WriteLine(CenterText(rezervace.Id.ToString(), sirka) + CenterText(rezervace.IdAuta.ToString(), sirka) +
                        CenterText(rezervace.Od.ToString(), sirka) + CenterText(rezervace.Do.ToString(), sirka));
                    maRezervaci = true;
                }
            }
            if (!maRezervaci)
            {
                Console.WriteLine("Uživatel nemá žádné rezervace. ");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Napište id rezervace kterou chcete zrušit.");
            string odpoved = Console.ReadLine();
            bool necoOdstraneno = false;

            for(int i = 0;i<rezervaceData.Count;i++)
            {
                if(rezervaceData[i].Id.ToString() == odpoved && username == rezervaceData[i].Uzivatel)
                {
                    rezervaceData[i].Aktivni = false;
                    Console.WriteLine("Rezervace číslo " + odpoved + " byla úspěšně smazána.");
                    Console.ReadKey();
                    necoOdstraneno = true;
                }

            }
            if(!necoOdstraneno)
            {
                Console.WriteLine("Spatne id zadáno, rezervace nebyla odstraněna.");
                Console.ReadKey();
            }
        }

        public static void ZmenitHeslo(string username)
        {
            Console.WriteLine("Zadejte nové heslo: ");
            string odpoved = Console.ReadLine();

            if (odpoved.Length > 0)
            {
                if (currentUser.Username == username)
                    currentUser.HesloHash = Enkryptor(odpoved);
                else
                    for (int i = 0; i < usersData.Count; i++)
                    {
                        if (usersData[i].Username == username)
                            usersData[i].HesloHash = Enkryptor(odpoved);
                    }
                Console.WriteLine("Změna hesla byla provedena úspěšně.");
            }
            else
                Console.WriteLine("Heslo nemůže být prázdný pole. Změna hesla nebyla provedena.");
            Console.ReadKey();
        }

        public static void OdhlasitSe()
        {
            Konec();
            currentUser = null;
            odhlasitVypnout = 1;
        }
    }

    class AdminPrikazy : MujSystem
    {
        public static void PridatUzivatele()
        {
            User newUser = new User();
            string odpoved = "";
            bool dale = false;



            odpoved = MujSystem.AskUser("Bude to admin? (a/n).  ", "a", "n");
            if (odpoved == "a")
            {
                dale = true;
                newUser.JeAdmin = true;
            }
            else if (odpoved == "n")
            {
                dale = true;
                newUser.JeAdmin = false;
            }
            else if (odpoved == "!z")
                return;


            dale = false;
            do
            {
                Console.Write("Username: ");
                odpoved = Console.ReadLine();
                if (odpoved != null && odpoved != "" && odpoved != "!z")
                {
                    dale = true;
                    if (MujSystem.ExistujeUzivatel(odpoved))
                    {
                        dale = false;
                    }
                    if (dale == false)
                        Console.WriteLine("Už existuje uživatel se stejným uživatelským jménem.");
                    else
                        newUser.Username = odpoved;
                }
                else if (odpoved == "!z")
                    return;
                else
                    Console.WriteLine("~~ Neodpověděli jste správným způsobem, zkuste to znovu. Přidávání uživatele můžete kdykoliv zrušit příkazem !z ~~");
            } while (dale == false);

            dale = false;
            do
            {
                Console.Write("Heslo: ");
                odpoved = Console.ReadLine();
                if (odpoved != null && odpoved != "" && odpoved != "!z")
                {
                    dale = true;
                    newUser.HesloHash = MujSystem.Enkryptor(odpoved);
                }
                else if (odpoved == "!z")
                    return;
                else
                    Console.WriteLine("~~ Neodpověděli jste správným způsobem, zkuste to znovu. Přidávání uživatele můžete kdykoliv zrušit příkazem !z ~~");
            } while (dale == false);

            dale = false;
            do
            {
                Console.Write("Jméno: ");
                odpoved = Console.ReadLine();
                if (odpoved != null && odpoved != "" && odpoved != "!z")
                {
                    dale = true;
                    newUser.Jmeno = odpoved;
                }
                else if (odpoved == "!z")
                    return;
                else
                    Console.WriteLine("~~ Neodpověděli jste správným způsobem, zkuste to znovu. Přidávání uživatele můžete kdykoliv zrušit příkazem !z ~~");
            } while (dale == false);

            dale = false;
            do
            {
                Console.Write("Příjmení: ");
                odpoved = Console.ReadLine();
                if (odpoved != null && odpoved != "" && odpoved != "!z")
                {
                    dale = true;
                    newUser.Prijmeni = odpoved;
                }
                else if (odpoved == "!z")
                    return;
                else
                    Console.WriteLine("~~ Neodpověděli jste správným způsobem, zkuste to znovu. Přidávání uživatele můžete kdykoliv zrušit příkazem !z ~~");
            } while (dale == false);

            MujSystem.usersData.Add(newUser);
            Console.WriteLine("Uživatel úspěšně přidán.");
            Console.ReadKey();
        }

        public static void SmazatUzivatele()
        {
            Console.Write("Napište username uživatele kterého chcete smazat: ");
            string odpoved = Console.ReadLine();
            bool smazano = false;

            foreach(User user in usersData)
            {
                if(user.Username == odpoved && user.Smazan == false)
                {
                    user.Username = RandomString(8);
                    string newUsername = user.Username;
                    user.Jmeno = "SMAZANO";
                    user.Prijmeni = "SMAZANO";
                    user.LastLogin = DateTime.Now;
                    user.JeAdmin = false;
                    user.HesloHash = Enkryptor(RandomString(6));
                    user.ChciNoveHeslo = false;
                    user.Smazan = true;
                    smazano = true;
                    Console.WriteLine("Uživatel " + odpoved + " byl úspěšně vymazán ze systému.");

                    foreach(Rezervace rezervace in rezervaceData)
                    {
                        if(rezervace.Uzivatel == odpoved)
                        {
                            rezervace.Uzivatel = newUsername;
                            rezervace.Aktivni = false;
                            Console.WriteLine("Rezervace " + rezervace.Id + " od tohoto uživatele též smazána.");
                        }
                    }
                    Console.ReadKey();
                }
            }
            if(!smazano)
            {
                Console.WriteLine("Uživatel " + odpoved + " nebyl nalezen v systému.");
                Console.ReadKey();
            }

        }

        public static void ZobrazitUzivatele()
        {

            if (usersData.Count - 1 < 1)
            {
                Console.WriteLine("Není zaregistrován žádný uživatel kromě Vás.");
                Console.ReadKey();
                return;
            }
            int sirka = 40;

            Console.WriteLine(CenterText("Admin", sirka) + CenterText("Username", sirka) + CenterText("Jmeno", sirka) + CenterText("Prijmeni", sirka) + CenterText("Posledni prihlaseni", sirka));
            foreach (User user in usersData)
            {
                if (!user.Smazan && user.Username != currentUser.Username)
                {
                    if (user.JeAdmin == true)
                        Console.Write(CenterText("admin", sirka));
                    else
                        Console.Write(CenterText("user", sirka));

                    Console.WriteLine(CenterText(user.Username, sirka) + CenterText(user.Jmeno, sirka) + CenterText(user.Prijmeni, sirka) + CenterText(user.LastLogin.ToString(), sirka));
                    
                }
            }

            Console.ReadKey();
        }

        public static void ZobrazitSmazaneUzivatele()
        {

            if (usersData.Count - 1 < 1)
            {
                Console.WriteLine("Není zaregistrován žádný uživatel kromě Vás.");
                Console.ReadKey();
                return;
            }
            int sirka = 40;

            Console.WriteLine(CenterText("Admin", sirka) + CenterText("Username", sirka) + CenterText("Jmeno", sirka) + CenterText("Prijmeni", sirka) + CenterText("Posledni prihlaseni", sirka));
            foreach (User user in usersData)
            {
                if (user.Smazan && user.Username != currentUser.Username)
                {
                    if (user.JeAdmin == true)
                        Console.Write(CenterText("admin", sirka));
                    else
                        Console.Write(CenterText("user", sirka));

                    Console.WriteLine(CenterText(user.Username, sirka) + CenterText(user.Jmeno, sirka) + CenterText(user.Prijmeni, sirka) + CenterText(user.LastLogin.ToString(), sirka));

                }
            }

            Console.ReadKey();
        }
        public static void PridatAuto()
        {
            Auto auto = new Auto();
            string odpoved;

            auto.Id = RandomInt(4);

            Console.WriteLine("Napište značku auta: ");
            odpoved = Console.ReadLine();
            auto.Znacka = odpoved;

            Console.WriteLine("Model auta: ");
            odpoved = Console.ReadLine();
            auto.Model = odpoved;

            odpoved = AskUser("Je to auto osobní? (a/n)", "a", "n");
            if (odpoved == "a")
                auto.JeOsobni = true;
            else if (odpoved == "n")
                auto.JeOsobni = false;

            Console.WriteLine("Jaká je spotřeba auta v litrech na 100 km? např. 4,5. ");
            odpoved = Console.ReadLine();
            if (float.TryParse(odpoved, out float spotreba))
                auto.Spotreba = spotreba;
            else
            {
                Console.WriteLine("Spotřeba vozidla byla zadána špatně. Nepovedlo se přidat vozidlo.");
                return;
            }

            autaData.Add(auto);
            Console.WriteLine("Auto úspěšně přidáno.");
            Console.ReadKey();
        }

        public static void SmazatAuto()
        {
            if (autaData.Count < 1)
            {
                Console.WriteLine("Není dostupné žádné auto ke smazání.");
                Console.ReadKey();
                return;
            }
            int sirka = 20;

            Console.WriteLine(CenterText("Id", sirka) + CenterText("Znacka", sirka) + CenterText("Model", sirka) + CenterText("Osobní/jiné", sirka));
            foreach (Auto auto in autaData)
            {
                Console.Write(CenterText(auto.Id.ToString(), sirka) + CenterText(auto.Znacka, sirka) + CenterText(auto.Model, sirka));
                if (auto.JeOsobni == true)
                    Console.WriteLine(CenterText("osobní", sirka));
                else
                    Console.WriteLine(CenterText("jiné", sirka));

            }

            Console.WriteLine("Napište id auta které chcete smazat.");
            string odpoved = Console.ReadLine();
            for(int i = 0; i < autaData.Count; i++)
            {
                if (autaData[i].Id.ToString() == odpoved)
                {
                    autaData.RemoveAt(i);
                    Console.WriteLine("Auto s id " + odpoved + " bylo úspěšně smazáno.");
                    Console.ReadKey();
                    return;
                }
            }
            Console.WriteLine("Auto s id " + odpoved + " nebylo nalezeno.");
            Console.ReadKey();
            return;
        }

        public static void PridatVyzadaniZmeny()
        {
            string odpoved = "";
            Console.WriteLine("Napište username uživatele kterýmu chcete přidat vyžádání změny hesla: ");
            odpoved = Console.ReadLine();
            if (ExistujeUzivatel(odpoved))
            {
                int i = 0;
                foreach (User user in usersData)
                {
                    if (user.Username == odpoved && user.JeAdmin == true)
                        Console.WriteLine("Uživatel je administrátor, nelze vyžádát změnu hesla.");
                    else if (user.Username == odpoved && user.JeAdmin == false)
                    {
                        usersData[i].ChciNoveHeslo = true;
                    }
                    i++;
                }
            }
            else
                Console.WriteLine("Uživatel neexistuje");

        }

        public static void ZobrazNeaktivniRezervace()
        {
            bool maRezervaci = false;
            int sirka = 40;

            foreach (Rezervace rezervace in rezervaceData)
            {
                if (!rezervace.Aktivni)
                {
                    if (!maRezervaci)
                        Console.WriteLine(CenterText("Id", sirka) + CenterText("IdAuta", sirka) + CenterText("Od", sirka) + CenterText("Do", sirka));

                    Console.WriteLine(CenterText(rezervace.Id.ToString(), sirka) + CenterText(rezervace.IdAuta.ToString(), sirka) +
                        CenterText(rezervace.Od.ToString(), sirka) + CenterText(rezervace.Do.ToString(), sirka));
                    maRezervaci = true;
                }
            }
            if (!maRezervaci)
                Console.WriteLine("Nemáme k dispozici žádné záznamy neaktivních rezervací.");

            Console.ReadKey();
        }

        public static void PridatServis()
        {
            ServisAuto servis = new ServisAuto();
            VypisDostupnaAuta(autaData);
            Console.WriteLine();
            Console.WriteLine("Servisní úkon přidávejte opatrně, v budoucnu nepůjde smazat.");
            bool existuje = false;

            do
            {
                servis.Id = RandomInt(4);
                foreach (ServisAuto serviss in servisData)
                {
                    if (serviss.Id == servis.Id)
                        existuje = true;
                }
                if (!existuje)
                    break;
            } while (true);

            Console.WriteLine("Napište id auta ke kterému chcete zapsat servisní úkon:");
            string odpoved = Console.ReadLine();
            existuje = false;
            foreach(Auto auto in autaData)
            {
                if (auto.Id.ToString() == odpoved)
                {
                    servis.IdAuto = int.Parse(odpoved);
                    existuje = true;
                }
            }
            if(!existuje)
            {
                Console.WriteLine("Špatně zadané id auta.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Napište kdy byl proveden servisní úkon: (např. 12.04.2021)");
            odpoved = Console.ReadLine();
            if (DateTime.TryParse(odpoved, out DateTime kdy))
                servis.Kdy = kdy;
            else
            {
                Console.WriteLine("Špatně zadané datum.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Napište popis servisniho ukonu: ");
            odpoved = Console.ReadLine();
            servis.UkonPopis = odpoved;

            Console.WriteLine("Napište cenu servisniho ukonu v korunach: (napr. 250,90)");
            odpoved = Console.ReadLine();
            if (decimal.TryParse(odpoved, out decimal cena))
                servis.CenaUkonu = cena;
            else
            {
                Console.WriteLine("Špatně zadaná cena.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Napište cislo faktury: ");
            odpoved = Console.ReadLine();
            if (int.TryParse(odpoved, out int faktura))
                servis.Faktura = faktura;
            else
            {
                Console.WriteLine("Špatně zadané číslo faktury.");
                Console.ReadKey();
                return;
            }

            servisData.Add(servis);
            Console.WriteLine("Servisní úkon byl úspěšně zapsán.");
            Console.ReadKey();
        }

        public static void VypsatServisAuta()
        {
            VypisDostupnaAuta(autaData);
            Console.WriteLine();
            Console.WriteLine("Napište id auta jehož servisní knížku byste chtěli vidět: ");
            string odpoved = Console.ReadLine();
            int sirka = 40;

            bool existuje = false;
            foreach(Auto auto in autaData)
            {
                if(odpoved == auto.Id.ToString())
                {
                    foreach(ServisAuto servis in servisData)
                    {
                        if (existuje == false)
                        {
                            Console.WriteLine("  Faktura" + CenterText("Cena", sirka) + "    Popis");
                            Console.WriteLine("  "+servis.Faktura.ToString() + CenterText(servis.CenaUkonu.ToString(), sirka) + CenterText(servis.UkonPopis, sirka));
                        } 
                        else
                            Console.WriteLine(CenterText(servis.Faktura.ToString(), sirka) + CenterText(servis.CenaUkonu.ToString(), sirka) + "   " + servis.UkonPopis);
                        existuje = true;
                    }
                }
            }
            if(!existuje)
            {
                Console.WriteLine("Servisní knížka tohoto auta neexistuje.");
            }
            Console.ReadKey();
        }
    }
}
