using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Projet_Yasmine_Fadila
{
    public class Gestionnaire:Salarie
    {
        string pass;
        List<Client> ListeClient;//on met la liste client en static pour qu'il soit partagé entre toute les instance de gestionnaire
        Chauffeur[] chauffeurs;
        Arbre organigrame;

        public Gestionnaire(int numss, string nom, string prenom, string telephone, string rue, string code, string ville, DateTime dateEntree, string poste,double salaire):base(numss, nom, prenom, telephone, rue, code, ville, dateEntree, poste,salaire) {
            this.pass = "motpass";
            organigrame = new Arbre(this);
            ListeClient = new List<Client>();
            chauffeurs= new Chauffeur[0];

        }
        public string Pass
        {
            get { return pass; }
        }
        public Chauffeur[] Chauffeurs { get { return chauffeurs; } }

        public Arbre Organigrame
        {
            get { return organigrame; }
        }
        public List<Client> ListeClients
        {
            get { return ListeClient; }
        }
        


        public  void AjouterClient()
        {
            Console.WriteLine("Entrez un numéro d'identifiant pour le client \n");
            string num = Console.ReadLine();

            Console.WriteLine("\nEntrez le nom du client \n");
            string nom = Console.ReadLine();

            Console.WriteLine("\nEntrez le prénom du client \n");
            string prenom = Console.ReadLine();

            Console.WriteLine("\nEntrez l'adresse du client \n");
            Console.WriteLine("\nRue \n");
            string rue = Console.ReadLine();

            Console.WriteLine("\nCode postal\n");
            string code = Console.ReadLine();

            Console.WriteLine("\nVille\n");
            string ville = Console.ReadLine();

            Console.WriteLine("\nEntrez le numéro de téléphone du client \n");
            string telephone = Console.ReadLine();

            Client Nclient= new Client(int.Parse(num), nom, prenom,telephone,rue,code,ville);
            this.ListeClient.Add(Nclient);

        }

        public void AfficherListeclient()
        {
            foreach(var client in this.ListeClient)
            {
                Console.WriteLine(client.ToString());

            }
        }
        
    //   // nous avons choisit ici d'affuicher les client par montant d'achat cummulé
    //et pour se faire nous avons utilisé les trie par selection pour trier le liste de client
        public  void AfficherClientParAchat()
        {
            for (int i = 0; i < this.ListeClient.Count -1; i++)
            {
                int Indexmin = i;
                for (int j=i+1; j < this.ListeClient.Count; j++)
                {
                    if (this.ListeClient[j].CompareTo(this.ListeClient[Indexmin]) < 0)
                    {
                        Indexmin = j;
                    }
                }
                Client c =  this.ListeClient[i];
               this.ListeClient[i] = this.ListeClient[Indexmin];
                this.ListeClient[Indexmin] = c;

            }
            AfficherListeclient();
            
        }

        //// ici nous utilison la methode comparto qui est predefinie dans la classe string 
        //pour comparer les nom des client
        public  void AfficherClientParOrdreAlpha()
        {
            for (int i = 0;i < this.ListeClient.Count - 1; i++)
            {
                int Indexmin = i;
                for(int j=i+1;j < this.ListeClient.Count; j++)
                {
                    if (this.ListeClient[j].Nom.CompareTo(this.ListeClient[Indexmin].Nom) < 0)
                    {
                        Indexmin=j;
                    }
                }
                Client c = this.ListeClient[i];
                this.ListeClient[i] =  this.ListeClient[Indexmin];
                this.ListeClient[Indexmin] = c;
            }
            AfficherListeclient();
        }

        public  void AfficherClientParVille()
        {
            for (int i = 0; i < this.ListeClient.Count - 1; i++)
            {
                int Indexmin = i;
                for (int j = i + 1; j < this.ListeClient.Count; j++)
                {
                    if (this.ListeClient[j].Adresse.Ville.CompareTo(this.ListeClient[Indexmin].Adresse.Ville) < 0)
                    {
                        Indexmin = j;
                    }
                }
                Client c = this.ListeClient[i];
                this.ListeClient[i] = this.ListeClient[Indexmin];
                this.ListeClient[Indexmin] = c;
            }
            AfficherListeclient();

        }

     

        public  void SupprimerClient(int idclient)
        {
            Client c=null;
            foreach (var client in this.ListeClient)
            {
                if (client.Numss == idclient)
                {
                  c= client; break;
                }


            }
            if (c != null)
            {
                this.ListeClient.Remove(c);
            }
            else
            {
                Console.WriteLine("Client non trouvé");
            }
        }

        public void ModifierClient(int idclient)
        {
            Client c = null;
            foreach (var client in this.ListeClient)
            {
                if (client.Numss == idclient)
                {
                    c = client; break;
                }
            }
            if (c != null)
            {
                string entree;


                Console.WriteLine("\nEntrez le numéro de l'information que vous voulez modifier \n");
                Console.WriteLine("\n1: Numéro de téléphone \n");
                Console.WriteLine("\n2: Adresse\n");
                entree = Console.ReadLine();

                switch (entree)
                {
                    case "1":
                        Console.WriteLine("Entrez le nouveau numéro de téléphone du client");
                        string num = Console.ReadLine();
                        c.Telephone = num;
                        break;

                    case "2":
                        Console.WriteLine("\nEntrez la rue \n");
                        string rue = Console.ReadLine();
                        Console.WriteLine("\nEntrez le code postal\n");
                        string code = Console.ReadLine();
                        Console.WriteLine("\nEntrez la ville \n");
                        string ville = Console.ReadLine();
                        Adresse ad = new Adresse(rue, code, ville);
                        c.Adresse = ad;
                        break;
                }




            }

            else
            {
                Console.WriteLine("\n Client non trouvé\n");
            }
        }

        public  void AssocierChauffeurACommande(int idcommande,Chauffeur chauffeur)

        {
            var lignes = File.ReadAllLines("commandes.txt").ToList();
            int index = -1;
            for (int i = 0; i < lignes.Count; i++)
            {
                var parties = lignes[i].Split(';');
                if (parties[0] == idcommande.ToString())
                {
                    index = i; break;
                }
            }
            if (index != -1)
            {
                var partiecmdtrouve = lignes[index].Split(';');
                string datecmd = partiecmdtrouve[6];
                bool chauffeurDispo = !lignes.Any(ligne =>
                {
                    var p = ligne.Split(';');
                    return p.Length > 10 && p[10] == chauffeur.Numss.ToString() && p[6] == datecmd;
                });
                if(chauffeurDispo )
                {
                    
                    
                    Client c = null;
                    foreach(var client in ListeClient)
                    {
                        if(client.Numss.ToString() == partiecmdtrouve[3])
                        {
                           c= client; break;
                        }
                    }
                  c.TrouverCommande(idcommande).Chauffeur=chauffeur;
                    Console.WriteLine(c.TrouverCommande(idcommande).Chauffeur.ToString());

                    Array.Resize(ref partiecmdtrouve, 12);


                    partiecmdtrouve[10] = chauffeur.Numss.ToString();
                    partiecmdtrouve[11]= c.TrouverCommande(idcommande).Prix.ToString();
                    lignes[index] = string.Join(";", partiecmdtrouve);
                    File.WriteAllLines("commandes.txt", lignes);

                }
                else
                {
                    Console.WriteLine("Le chauffeur n'est pas disponible pour effectuer une livraison à cette date");

                }
            }
            else { Console.WriteLine("Cette commande n'existe pas"); }
          
        }

        public Chauffeur TrouverChauffeur(int idchauffeur)
        {
            Salarie sal = this.organigrame.RetounerSalarie(idchauffeur);
            if (sal is Chauffeur ch)
            {
                return ch;
            }
            else
            {
                return null;
            }
        }

        public void AjouterSalarie()
        {
            string entree;
            Console.WriteLine("\nEntrez une option\n");
            Console.WriteLine("1: Salarié ordinaire\n");
            Console.WriteLine("2: Chauffeur\n");
            entree =Console.ReadLine();
            switch(entree)
            {
                case "1":
                    this.AjouterSalarieOrd();break;
                case "2":
                    this.AjouterChauffeur();break;
            }

        }


        private void AjouterSalarieOrd()
        {
            Console.WriteLine("\nAjouter les informations du salarié\n");
            Console.WriteLine("\nNuméro du salarié\n");
            int num = int.Parse(Console.ReadLine());

            Console.WriteLine("\nNom du salarié\n");
            string nom = Console.ReadLine();

            Console.WriteLine("\nPrénom du salarié\n");
            string prenom = Console.ReadLine();

            Console.WriteLine("\nNuméro de téléphone\n");
            string tel = Console.ReadLine();

            Console.WriteLine("\nRue\n");
            string rue = Console.ReadLine();

            Console.WriteLine("\nCode postal\n");
            string code = Console.ReadLine();

            Console.WriteLine("\nVille\n");
            string ville = Console.ReadLine();

            Console.WriteLine("\nDate d'entrée\n");
            Console.WriteLine("Année");
            int annee = int.Parse(Console.ReadLine());

            Console.WriteLine("Mois");
            int mois = int.Parse(Console.ReadLine());

            Console.WriteLine("Jour");
            int jours = int.Parse(Console.ReadLine());

            Console.WriteLine("\nSalaire\n");
            double salaire = double.Parse(Console.ReadLine());

            Console.WriteLine("\nPoste\n");
         

            string poste =Console.ReadLine();
            Salarie s=new Salarie(num,nom,prenom, tel,rue,code,ville, new DateTime(annee, mois, jours),poste,salaire);

            Console.WriteLine("\nEntrez le numéro du supérieur\n");

            int numss = int.Parse(Console.ReadLine());
            this.organigrame.AjouterSubordonne(numss, s);
            

        }

        private void AjouterChauffeur()
        {

            Console.WriteLine("\nAjouter les informations du salarié\n");
            Console.WriteLine("\nNuméro du salarié\n");
            int num = int.Parse(Console.ReadLine());

            Console.WriteLine("\nNom du salarié\n");
            string nom = Console.ReadLine();

            Console.WriteLine("\nPrénom du salarié\n");
            string prenom = Console.ReadLine();

            Console.WriteLine("\nNuméro de téléphone\n");
            string tel = Console.ReadLine();

            Console.WriteLine("\nRue\n");
            string rue = Console.ReadLine();

            Console.WriteLine("\nCode postal\n");
            string code = Console.ReadLine();

            Console.WriteLine("\nVille\n");
            string ville = Console.ReadLine();

            Console.WriteLine("\nDate d'entrée\n");
            Console.WriteLine("Année");
            int annee = int.Parse(Console.ReadLine());

            Console.WriteLine("Mois");
            int mois = int.Parse(Console.ReadLine());

            Console.WriteLine("Jour");
            int jour = int.Parse(Console.ReadLine());

            Console.WriteLine("\nTarif horaire\n");
            double tarif = double.Parse(Console.ReadLine());

            Salarie chauffeur = new Chauffeur(num, nom, prenom, tel, rue, code, ville, new DateTime(annee, mois, jour), tarif);

            Console.WriteLine("\nEntrez le numéro du supérieur\n");

            int numss = int.Parse(Console.ReadLine());
            this.organigrame.AjouterSubordonne(numss, chauffeur);

        }

        public void SupprimerSalarie()
        {
            Console.WriteLine("\nNuméro du salarié à supprimer\n");

            int num = int.Parse(Console.ReadLine());
            this.organigrame.SupprimerSalarie(num);
        }
      public void ModifierSlarie()
        {
            string entree;
            Console.WriteLine("\n Entrez une option \n");
            Console.WriteLine("\n 1: Salarié ordinaire\n");
            Console.WriteLine("\n 2: Chauffeur\n");
            entree = Console.ReadLine();
            switch (entree)
            {
                case "1":
                    this.ModifierSalarieO(); break;
                case "2":
                    this.ModifierChauffeur(); break;
            }
        }
        private void ModifierSalarieO()
        {
            Console.WriteLine("\n Entrez le numero du salarié\n");
            int num = int.Parse(Console.ReadLine());
            Salarie s=this.organigrame.RetounerSalarie(num);
            if (s != null)
            {
                string entree;
                Console.WriteLine("\nEntrez le numéro de l'information à modifier\n");
                Console.WriteLine("1: Numéro de téléphone\n");
                Console.WriteLine("2: Poste\n");
                Console.WriteLine("3: Salaire\n");
                entree = Console.ReadLine();
                switch(entree)
                {
                    case "1":
                        Console.WriteLine("\n Numero de telephone\n");
                       string telephone=Console.ReadLine();
                        s.Telephone = telephone;break;
                    case "2": Console.WriteLine("\n Poste\n");
                          string p=Console.ReadLine();
                        s.Poste = p;break;
                    case "3": Console.WriteLine("\n Slaire\n");
                        double salaire=double.Parse(Console.ReadLine());
                        s.Salaire = salaire;
                        break;
                }
                
               
            }

            


        }

        private void ModifierChauffeur()
        {
            Console.WriteLine("\n Entrez le numero du salarié\n");
            int num = int.Parse(Console.ReadLine());
            Salarie s = this.organigrame.RetounerSalarie(num);
            if (s != null)
            {
                string entree;
                Console.WriteLine("\n Entrez le numero de l'information à modifier\n");
                Console.WriteLine("\n 1: Numero de telephone\n");
                Console.WriteLine("\n 2: Poste\n");
                Console.WriteLine("\n 3: Tarif horaire\n");
                entree = Console.ReadLine();
                switch (entree)
                {
                    case "1":
                        Console.WriteLine("\n Numero de telephone\n");
                        string telephone = Console.ReadLine();
                        s.Telephone = telephone; break;
                    case "2":
                        Console.WriteLine("\n Poste\n");
                        string p = Console.ReadLine();
                        s.Poste = p; break;
                    case "3":
                        Console.WriteLine("\n Tarif horaire\n");
                        double tarif = double.Parse(Console.ReadLine());
                        Chauffeur c = (Chauffeur)s;
                        c.TarifHoraire = tarif;
                        break;
                }


            }




        }
        public void AfficherOrganig()
        {
            this.organigrame.AfficherArbre();
        }

        


    }
}
