using System;
using System.Collections.Generic;
using System.Text;

namespace VozovyPark
{
    public class User
    {
        public bool JeAdmin { get; set; }

        private string username;
        public string Username
        {
            get { return this.username; }
            set
            {
                if (value != null)
                {
                    if (value.Length == 0)
                        Console.WriteLine("Tady musíte něco napsat.");
                    else
                        username = value;
                }
                else
                    throw new ArgumentOutOfRangeException();
            }
        }
        private string hesloHash;
        public string HesloHash
        {
            get { return this.hesloHash; }
            set
            {
                if (value != null)
                {
                    if (value.Length == 0)
                        Console.WriteLine("Heslo nemůže být prázdné pole.");
                    else
                        hesloHash = value;
                }
                else
                    Console.WriteLine("Heslo nemůže být prázdné pole.");
            }
        }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public DateTime LastLogin { get; set; }
        public bool ChciNoveHeslo { get; set; }

        public bool Smazan { get; set; }

        public User()
        {

        }
        public User(string username, string hesloHash)
        {
            this.Username = username;
            this.HesloHash = hesloHash;
        }

        public User(bool jeAdmin, string username, string hesloHash, string jmeno, string prijmeni, DateTime lastLogin)
        {
            this.JeAdmin = jeAdmin;
            this.Username = username;
            this.HesloHash = hesloHash;
            this.Jmeno = jmeno;
            this.Prijmeni = prijmeni;
            this.LastLogin = lastLogin;
        }

        public override string ToString()
        {
            const string oddelovac = MujSystem.oddelovac;
            string s = JeAdmin.ToString() + oddelovac + Username + oddelovac + HesloHash + oddelovac + Jmeno + oddelovac + Prijmeni + oddelovac + LastLogin.ToString() + oddelovac + ChciNoveHeslo.ToString();
            return s;
        }

    }

    public class Rezervace
    {
        private int id;
        public int Id
        {
            get { return this.id; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();
                else
                    id = value;
            }
        }
        public string Uzivatel { get; set; }

        public int IdAuta { get; set; }
        public DateTime Od { get; set; }
        public DateTime Do { get; set; }
        public bool Aktivni { get; set; }

        public override string ToString()
        {
            string oddelovac = MujSystem.oddelovac;
            string s = Id.ToString() + oddelovac + Uzivatel + oddelovac + Od.ToString() + oddelovac + Do.ToString() + oddelovac + Aktivni.ToString();
            return s;
        }
    }

    public class Auto
    {
        private int id;
        public int Id
        {
            get { return this.id; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();
                else
                    id = value;
            }
        }
        public string Znacka { get; set; }
        public string Model { get; set; }
        public bool JeOsobni { get; set; }
        public float Spotreba { get; set; }

        public override string ToString()
        {
            const string oddelovac = MujSystem.oddelovac;
            string s = Id.ToString() + oddelovac + Znacka + oddelovac + Model + oddelovac + JeOsobni.ToString() + oddelovac + Spotreba.ToString();
            return s;
        }

    }

    public class ServisAuto
    {
        private int id;
        public int Id
        {
            get { return this.id; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();
                else
                    id = value;
            }
        }
        public int IdAuto { get; set; }
        public DateTime Kdy { get; set; }
        public string UkonPopis { get; set; }
        public decimal CenaUkonu { get; set; }

        public int Faktura { get; set; }


        public override string ToString()
        {
            string oddelovac = MujSystem.oddelovac;
            string s = Id.ToString() + oddelovac + IdAuto.ToString() + oddelovac + Kdy.ToString() + oddelovac + UkonPopis + oddelovac + CenaUkonu.ToString() + oddelovac + Faktura.ToString();
            return s;
        }
    }
}

