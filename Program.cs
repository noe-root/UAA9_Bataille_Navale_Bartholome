namespace Tirer_Joueur
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool rejouer; // NEW: permet de rejouer une partie

            do
            {
                // Message de bienvenue
                Console.WriteLine("Bienvenu dans mon programme !");

                // ===== DÉCLARATION ET INITIALISATION DES VARIABLES =====
                int largeur; // Nombre de colonnes de la grille (A-J = 10)
                int hauteur; // Nombre de lignes de la grille (1-10 = 10)
                largeur = 10;
                hauteur = 10;

                // ===== CRÉATION DES GRILLES DE JEU =====
                // Grilles contenant les bateaux de chaque joueur
                string[,] grilleJoueur1 = new string[hauteur, largeur]; // Grille des bateaux du joueur 1
                string[,] grilleJoueur2 = new string[hauteur, largeur]; // Grille des bateaux du joueur 2

                // Grilles pour tracer les tirs de chaque joueur (ce qu'ils voient de l'adversaire)
                string[,] grilleTirsJoueur1 = new string[hauteur, largeur]; // Joueur 1 tire sur joueur 2
                string[,] grilleTirsJoueur2 = new string[hauteur, largeur]; // Joueur 2 tire sur joueur 1

                // ===== PHASE DE PLACEMENT - JOUEUR 1 =====
                MethodeDeProjet.RemplirGrille(grilleJoueur1);
                MethodeDeProjet.RemplirGrille(grilleTirsJoueur1);
                MethodeDeProjet.AfficherInfosBateaux();
                MethodeDeProjet.PlacerTousLesBateaux(grilleJoueur1, "JOUEUR 1");

                // ===== TRANSITION ENTRE LES JOUEURS =====
                Console.WriteLine("\n\n=== Le Joueur 1 a terminé ! ===");
                Console.WriteLine("Appuyez sur une touche pour que le Joueur 2 commence...");
                Console.ReadKey();
                Console.Clear();

                // ===== PHASE DE PLACEMENT - JOUEUR 2 =====
                MethodeDeProjet.RemplirGrille(grilleJoueur2);
                MethodeDeProjet.RemplirGrille(grilleTirsJoueur2);
                MethodeDeProjet.AfficherInfosBateaux();
                MethodeDeProjet.PlacerTousLesBateaux(grilleJoueur2, "JOUEUR 2");

                // ===== DÉBUT DE LA BATAILLE =====
                Console.Clear();
                Console.WriteLine("=== Que la bataille commence ! ===\n");
                Console.WriteLine("Appuyez sur une touche pour commencer...");
                Console.ReadKey();

                // Lancer la boucle de jeu (tirs alternés jusqu'à ce qu'un joueur gagne)
                MethodeDeProjet.JouerBataille(grilleJoueur1, grilleJoueur2, grilleTirsJoueur1, grilleTirsJoueur2, largeur, hauteur);

                // ===== DEMANDER SI ON REJOUE =====
                Console.Write("\nVoulez-vous rejouer ? (O/N) : ");
                string reponse = Console.ReadLine().Trim().ToUpper();
                rejouer = (reponse == "O" || reponse == "OUI");

                Console.Clear();

            } while (rejouer);
        }
    }
}
        
